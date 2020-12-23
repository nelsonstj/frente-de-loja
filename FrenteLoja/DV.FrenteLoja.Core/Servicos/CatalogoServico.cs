using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Core.Util;
using Neo4jClient;
using System.Data.Entity;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CatalogoServico : ICatalogoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly GraphClient _graphClient;
        private readonly IRepositorio<Catalogo> _repositorioCatalogo;
        private readonly IRepositorio<CatalogoArquivo> _repositorioCatalogoArquivo;
        private readonly IRepositorio<CatalogoCargaLog> _repositorioCargaCatalogoLog;
        private readonly IRepositorio<CatalogoProdutosCorrelacionados> _repositorioCatalogoProdutosRelaciondos;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IGrupoProdutoServico _grupoProdutoServico;

        public CatalogoServico(IRepositorioEscopo escopo, GraphClient graphClient, IGrupoProdutoServico grupoProdutoServico)
        {
            _escopo = escopo;
            _graphClient = graphClient;
            _repositorioCatalogo = escopo.GetRepositorio<Catalogo>();
            _repositorioCatalogoArquivo = escopo.GetRepositorio<CatalogoArquivo>();
            _repositorioCargaCatalogoLog = escopo.GetRepositorio<CatalogoCargaLog>();
            _repositorioCatalogoProdutosRelaciondos = escopo.GetRepositorio<CatalogoProdutosCorrelacionados>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _grupoProdutoServico = grupoProdutoServico;

        }
        public void Atualizar(CargaCatalogoDto catalogoDto)
        {
            try
            {
                var entidadeCatalogo = Mapper.Map<Catalogo>(catalogoDto);

                _repositorioCatalogo.Update(entidadeCatalogo);
                _escopo.Finalizar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UploadArquivoCatalogo(CargaCatalogoDto catalogoDto, ICatalogoProtheusApi catalogoProtheusApi)
        {
            var logCargaCatalogo = new CatalogoCargaLog { StatusIntegracao = StatusIntegracao.Sucesso, NomeArquivo = catalogoDto.Nome };

            try
            {
                // Lista de objetos extraidos do arquivo de importação. 
                var catalogoList = new List<Catalogo>();

                // Lista de produtos do banco ja atualizada a ser enviada para o protheus.
                List<Catalogo> catalogoListUpdated = new List<Catalogo>();

                var contadorLinhas = 0;
                Stream fileInputStream = new MemoryStream(catalogoDto.Arquivo);
                if (fileInputStream.Length == 0)
                {
                    catalogoDto.LogImportacao += "O arquivo não pode ser vazio ou nulo. \n";
                }

                else
                {
                    /*comentado pois duas cargas rapidas daria erro e nao é necessario salvar o arquivo 
                     *_repositorioCatalogoArquivo.Add(new CatalogoArquivo
                     {
                         Nome = catalogoDto.Nome,
                         Arquivo = catalogoDto.Arquivo,
                         DataAtualizacao = DateTime.Now,
                         UsuarioAtualizacao = HttpContext.Current.User.Identity.GetName()
                     });*/

                    var firstRow = true;

                    using (var reader = new StreamReader(fileInputStream))
                    {

                        var builder = new System.Text.StringBuilder();
                        builder.Append(catalogoDto.LogImportacao);

                        while (!reader.EndOfStream)
                        {
                            try
                            {
                                var line = reader.ReadLine();
                                if (!firstRow)
                                {
                                    var data = line.Split(';');
                                    var codigoFabricante = data[0] ?? string.Empty;

                                    if (codigoFabricante != string.Empty)
                                    {
                                        var catalogo = new Catalogo()
                                        {
                                            CodigoFabricante = codigoFabricante,
                                            Descricao = data[1],
                                            InformacoesComplementares = data[2],
                                            MarcaVeiculo = data[4].ToUpper(),
                                            ModeloVeiculo = data[5].ToUpper(),
                                            VersaoVeiculo = data[6].ToUpper(),
                                            //FabricantePeca = catalogoDto.Nome.Split('.')[0],
                                            FabricantePeca = data[10].ToUpper(),
                                            UsuarioAtualizacao = HttpContext.Current.User.Identity.GetName()
                                        };


                                        var anoInicial = data[7];
                                        catalogo.AnoInicial = anoInicial.ToUpper() == "ATUAL" ? DateTime.Now.Year : Int32.Parse(anoInicial);

                                        var anoFinal = data[8];
                                        catalogo.AnoFinal = anoFinal.ToUpper() == "ATUAL" ? DateTime.Now.Year : Int32.Parse(anoFinal);



                                        //Chave única do catalogo é o Codigo fabricante,Marca. Modelo, Versao, AnoDe e AnoAte
                                        var entidadeCatalogo = _repositorioCatalogo.Get(a => a.CodigoFabricante == catalogo.CodigoFabricante && a.MarcaVeiculo == catalogo.MarcaVeiculo
                                        && a.ModeloVeiculo == catalogo.ModeloVeiculo && a.VersaoVeiculo == catalogo.VersaoVeiculo && a.AnoInicial == catalogo.AnoInicial && a.AnoFinal == catalogo.AnoFinal).FirstOrDefault();

                                        if (entidadeCatalogo == null)
                                        {
                                            entidadeCatalogo = _repositorioCatalogo.Add(catalogo);
                                        }
                                        else
                                        {
                                            entidadeCatalogo.FabricantePeca = catalogo.FabricantePeca;
                                            entidadeCatalogo.InformacoesComplementares = catalogo.InformacoesComplementares;
                                        }

                                        foreach (var item in data[9].Split(','))
                                        {
                                            var itemCorrelacionado = _repositorioCatalogoProdutosRelaciondos.Get(a => a.IdCatalogo == entidadeCatalogo.Id && a.CodigoFabricante == item).FirstOrDefault();

                                            if (itemCorrelacionado == null)
                                            {
                                                var produto = new CatalogoProdutosCorrelacionados()
                                                {
                                                    CodigoFabricante = item,
                                                    IdCatalogo = entidadeCatalogo.Id,
                                                    UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper()
                                                };

                                                // Persistindo no SQL Server 
                                                _repositorioCatalogoProdutosRelaciondos.Add(produto);
                                            }

                                        }
                                        catalogoList.Add(catalogo);
                                    }
                                }
                                else
                                {
                                    firstRow = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                builder.Append(ex.Message);

                            }
                            finally
                            {
                                contadorLinhas++;
                            }

                        }

                        catalogoDto.LogImportacao = builder.ToString();

                        /* Envio para o Protheus e atualização no Sql Sever do código Dellavia. */
                        try
                        {
                            string erros = String.Empty;
                            var result = catalogoProtheusApi.PostCatalogo(catalogoList, out erros);
                            if (!string.IsNullOrEmpty(erros))
                            {
                                catalogoDto.LogImportacao += erros;
                            }


                            foreach (var c in result)
                            {
                                foreach (var catalogoItem in _repositorioCatalogo.Get(x => x.CodigoFabricante == c.CodigoFabricante))
                                {
                                    catalogoItem.CodigoDellavia = c.CodigoDellavia;
                                    _repositorioCatalogo.Update(catalogoItem);
                                    catalogoList.Where(a => a.CodigoFabricante == catalogoItem.CodigoFabricante && a.MarcaVeiculo == catalogoItem.MarcaVeiculo
                                    && a.ModeloVeiculo == catalogoItem.ModeloVeiculo && a.VersaoVeiculo == catalogoItem.VersaoVeiculo && a.AnoInicial == catalogoItem.AnoInicial && a.AnoFinal == catalogoItem.AnoFinal).Select(d => d.CodigoDellavia = c.CodigoDellavia);
                                }
                            }

                            EnviarElasticsearch(catalogoList);

                        }
                        catch (Exception e)
                        {
                            catalogoDto.LogImportacao += $"Erro no método de envio para Protheus. Descrição: {e} ";
                            logCargaCatalogo.StatusIntegracao = StatusIntegracao.Erro;
                        }

                        catalogoDto.LogImportacao += "Importação finalizada em: " + DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss") + " \n";
                        catalogoDto.LogImportacao += "\n----- Resultado da importação ----- \n";
                        catalogoDto.LogImportacao += $" {contadorLinhas} linhas processadas.  \n";
                        //catalogoDto.LogImportacao += $" {contadorLinhas - (contadorLinhaErro + contadorLinhasIgnoradas)} linhas com sucesso.  \n";
                        //catalogoDto.LogImportacao += $" {contadorLinhasIgnoradas} linhas ignoradas (#N/A).  \n";
                        //catalogoDto.LogImportacao += $" {contadorLinhaErro} linhas com erro.  \n";
                        catalogoDto.LogImportacao += "-----------------------------------";
                    }

                    logCargaCatalogo.LogImportacao = catalogoDto.LogImportacao;



                    _repositorioCargaCatalogoLog.Add(logCargaCatalogo);

                    _escopo.Finalizar();

                }
            }
            catch (Exception ex)
            {
                logCargaCatalogo.StatusIntegracao = StatusIntegracao.Sucesso;
                throw ex;
            }
        }

        public string ProcessaCatalogo(List<Catalogo> catalogoList, ICatalogoProtheusApi catalogoProtheusApi)
        {
            // Lista de produtos do banco ja atualizada a ser enviada para o protheus.
            List<Catalogo> catalogoListUpdated = new List<Catalogo>();

            /* Envio para o Protheus e atualização no Sql Sever do código Dellavia. */
            string erros = String.Empty;
            try
            {

                var result = catalogoProtheusApi.PostCatalogo(catalogoList, out erros);

                foreach (var c in result)
                {
                    var catalogoUpdate = _repositorioCatalogo.GetSingle(x => x.CodigoFabricante == c.CodigoFabricante);
                    if (catalogoUpdate != null)
                    {
                        catalogoUpdate.CodigoDellavia = c.CodigoDellavia;
                        catalogoListUpdated.Add(catalogoUpdate);
                    }
                }

                EnviarElasticsearch(catalogoListUpdated);
                return String.Empty;
            }
            catch (Exception)
            {
                return erros;
            }
        }

        public void ProcessaCatalogoHistoricoProtheus()
        {
            try
            {
                List<Catalogo> catalogoList = _repositorioCatalogo.Get(x => x.FabricantePeca == "CARGA CATALOGO").ToList();


                EnviarElasticsearch(catalogoList);
            }
            catch (Exception e)
            {
                throw new Exception("Carga catalogo historica: " + e.Message + " " + e.InnerException?.Message, e);
            }
        }
        private void EnviarElasticsearch(List<Catalogo> catalogList)
        {
            var elasticsearchItemDtos = new List<ElasticsearchItemDto>();

            foreach (var c in catalogList)
            {
                Produto produto = null;

                if (!string.IsNullOrEmpty(c.CodigoFabricante))
                {
                    produto = _produtoRepositorio.GetSingle(x => x.CodigoFabricante == c.CodigoFabricante);
                }

                var elasticsearchItemDto = new ElasticsearchItemDto();
                if (produto != null)
                {
                    elasticsearchItemDto.ProdutoCodFabricante = produto.CodigoFabricante;
                    elasticsearchItemDto.ProdutoDescricao = produto.Descricao;
                    elasticsearchItemDto.ProdutoCodGrupo = _grupoProdutoServico.BuscaTipoProdutoElasticSearch(produto.GrupoProduto.CampoCodigo);
                    elasticsearchItemDto.ProdutoFabricantePeca = produto.FabricantePeca ?? string.Empty;
                    elasticsearchItemDto.PrioridadeOrdenacao = produto.GrupoProduto.CampoCodigo;
                }
                else
                {
                    elasticsearchItemDto.ProdutoCodGrupo = TipoProdutoElasticSearch.Pneus;
                    elasticsearchItemDto.ProdutoCodFabricante = c.CodigoFabricante;
                    elasticsearchItemDto.ProdutoDescricao = c.Descricao;
                    elasticsearchItemDto.ProdutoFabricantePeca = c.FabricantePeca ?? string.Empty;
                }
                elasticsearchItemDto.VeiculoModelo = c.ModeloVeiculo ?? string.Empty;
                elasticsearchItemDto.VeiculoMarca = c.MarcaVeiculo ?? string.Empty;
                elasticsearchItemDto.VeiculoVersao = c.VersaoVeiculo ?? string.Empty;
                elasticsearchItemDto.VeiculoAnoInicial = c.AnoInicial;
                elasticsearchItemDto.VeiculoAnoFinal = c.AnoFinal;
                elasticsearchItemDto.ProdutoCodDellavia = c.CodigoDellavia ?? string.Empty; //Nulo Quebra no cypher
                elasticsearchItemDto.ProdutoInformacaoComplementar = c.InformacoesComplementares ?? string.Empty;
                elasticsearchItemDtos.Add(elasticsearchItemDto);

            }

            new Grafo.Conversores.SQLToGrafoCatalogo().Importar(elasticsearchItemDtos, _graphClient, OrigemCarga.Catalogo);
        }

        public void Excluir(long id)
        {
            try
            {
                var obj = _repositorioCatalogo.FindByKey(id);

                _repositorioCatalogo.Remove(obj);
                _escopo.Finalizar();
            }
            catch (Exception e)
            {
                throw new Exception("Erro não tratado", e);
            }
        }

        public IQueryable<CatalogoDto> Obter()
        {
            return _repositorioCatalogo.GetAll().OrderBy(a => a.Id).ProjectTo<CatalogoDto>();
        }

        public IQueryable<string> ObterFabricantePecaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCatalogoQueryFabricantePeca(termoBusca)
                .Select(x => x.FabricantePeca)
                .Distinct();
        }

        public int TamanhoTermoFabricantePeca(string termoBusca)
        {
            return ObterCatalogoQueryFabricantePeca(termoBusca)
                .Select(x => x.FabricantePeca)
                .Distinct()
                .Count();
        }

        private IQueryable<Catalogo> ObterCatalogoQueryFabricantePeca(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from catalogo in _repositorioCatalogo.GetAll()
                   where DbFunctions.Like(catalogo.FabricantePeca, termoBusca)
                   select catalogo;
        }

        public CargaCatalogoDto ObterPorId(long id)
        {
            try
            {
                var objDto = Mapper.Map<CargaCatalogoDto>(_repositorioCatalogo.FindByKey(id));

                return objDto;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
