using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Grafo;
using DV.FrenteLoja.Core.Util;
using DV.FrenteLoja.Core.Servicos.Comuns;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaProdutoServico : ICargaProdutoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly ICargaCadastrosProtheusSyncApi _protheusSyncApi;
        private readonly ConfigGrafo _configGrafo;
        private readonly IRepositorio<GrupoProduto> _grupoProdutoRepositorio;
        private readonly IRepositorio<GrupoServicoAgregadoProduto> _grupoServicoAgregadosProdutosRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<ProdutoComplemento> _produtoComplementoRepositorio;
        private readonly IRepositorio<Catalogo> _catalogoRepositorio;
        private readonly IRepositorio<VeiculoMedidasPneus> _veiculoMedidasPneusRepositorio;
        private readonly IRepositorio<Veiculo> _veiculoRepositorio;
        private readonly IRepositorio<GrupoSubGrupo> _grupoSubGrupoRepositorio;

        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";
        public CargaProdutoServico(IRepositorioEscopo escopo, ICargaCadastrosProtheusSyncApi protheusSyncApi, ConfigGrafo configGrafo)
        {
            _escopo = escopo;
            _protheusSyncApi = protheusSyncApi;
            _configGrafo = configGrafo;
            _grupoProdutoRepositorio = escopo.GetRepositorio<GrupoProduto>();
            _grupoServicoAgregadosProdutosRepositorio = escopo.GetRepositorio<GrupoServicoAgregadoProduto>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _produtoComplementoRepositorio = escopo.GetRepositorio<ProdutoComplemento>();
            _catalogoRepositorio = escopo.GetRepositorio<Catalogo>();
            _veiculoMedidasPneusRepositorio = escopo.GetRepositorio<VeiculoMedidasPneus>();
            _veiculoRepositorio = escopo.GetRepositorio<Veiculo>();
            _grupoSubGrupoRepositorio = escopo.GetRepositorio<GrupoSubGrupo>();
        }

        public async Task SyncGrupoProduto(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cadastro, TipoTabelaProtheus.SBM);
            var grupoProdutos = ConvertJsonToObject<GrupoProduto>(jArray);
            foreach (var grupoProduto in grupoProdutos)
            {
                GrupoProduto v = null;
                try
                {
                    v = isFirstLoad ? null : _grupoProdutoRepositorio.GetSingle(x => x.CampoCodigo == grupoProduto.CampoCodigo);

                    if (v != null)
                    {
                        v.Descricao = grupoProduto.Descricao;
                        v.RegistroInativo = grupoProduto.RegistroInativo;
                        v.DataAtualizacao = DateTime.Now;
                        v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                    }
                    else
                    {
                        grupoProduto.DataAtualizacao = DateTime.Now;
                        grupoProduto.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        long subGrupo = GetIdSubGrupo(grupoProduto.CampoCodigo);
                        if (subGrupo > 0) {
                            grupoProduto.IdGrupoSubGrupo = subGrupo.ToString();
                        }

                        try
                        {
                            _grupoProdutoRepositorio.Add(grupoProduto);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Grupo Produtos na base de dados. Erro {e}.");
                        }
                    }
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncGrupoProduto)} : {e}. Item: {v?.ToStringLog()}");
                }
            }
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Grupo Produto: {errosBuilder}.");
        }
        public long GetIdSubGrupo(string campoCodigo)
        {
            if (UtilIntegracao.TipoProdutoPneus.Contains(campoCodigo))
            {
                long id = (from sg in _grupoSubGrupoRepositorio.GetAll()
                           where sg.Grupo == TipoProdutoElasticSearch.Pneus
                           select sg.Id).First();
                return id;
            }
            else if (UtilIntegracao.TipoProdutoFreio.Contains(campoCodigo))
            {
                long id = (from sg in _grupoSubGrupoRepositorio.GetAll()
                           where sg.Grupo == TipoProdutoElasticSearch.Freio
                           select sg.Id).First();
                return id;
            }
            else if (UtilIntegracao.TipoProdutoLubrificantes.Contains(campoCodigo))
            {
                long id = (from sg in _grupoSubGrupoRepositorio.GetAll()
                           where sg.Grupo == TipoProdutoElasticSearch.Lubrificantes
                           select sg.Id).First();
                return id;
            }
            else if (UtilIntegracao.TipoProdutoServicos.Contains(campoCodigo))
            {
                long id = (from sg in _grupoSubGrupoRepositorio.GetAll()
                           where sg.Grupo == TipoProdutoElasticSearch.Servicos
                           select sg.Id).First();
                return id;
            }
            else if (UtilIntegracao.TipoProdutoAcessorios.Contains(campoCodigo))
            {
                long id = (from sg in _grupoSubGrupoRepositorio.GetAll()
                           where sg.Grupo == TipoProdutoElasticSearch.Acessorios
                           select sg.Id).First();
                return id;
            }
            else if (UtilIntegracao.TipoProdutoSuspensao.Contains(campoCodigo))
            {
                long id = (from sg in _grupoSubGrupoRepositorio.GetAll()
                           where sg.Grupo == TipoProdutoElasticSearch.Suspensao
                           select sg.Id).First();
                return id;
            }
            return 0;
        }

        public async Task SyncGrupoServicoAgregadosProdutos(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();

            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.PA3);
          
            do
            {
                var servicoAgregadoProdutos = new List<GrupoServicoAgregadoProduto>();
                foreach (var jObj in jArray)
                {
                    var agregadoProduto = new GrupoServicoAgregadoProduto();

                    try
                    {
                        var produtoIdProtheus = jObj["Produto_Id"].ToString();

                        Produto produto = null;

                        if (!produtoIdProtheus.IsNullOrEmpty())
                        {
                            produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == produtoIdProtheus);
                        }

                        if (produto != null)
                        {
                            agregadoProduto.IdGrupoServicoAgregado = jObj["GrupoServicoAgregados_Id"].ToString();
                            agregadoProduto.Item = jObj["Item"].ToString();
                            agregadoProduto.PermiteAlterarQuantidade = jObj["PermiteAlterarQuantidade"].ToString().ToUpper() == "S";
                            agregadoProduto.Quantidade = Convert.ToDecimal(jObj["Quantidade"].ToString());
                            agregadoProduto.CampoCodigo = jObj["Id"].ToString();
                            agregadoProduto.IdProduto = produto.Id.ToString();
                            agregadoProduto.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                            servicoAgregadoProdutos.Add(agregadoProduto);
                        }
                        else
                        {
                            errosBuilder.AppendLine($"Produto id {produtoIdProtheus} não encontrado.");
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncGrupoServicoAgregadosProdutos)} : {e}. Item: {agregadoProduto.ToStringLog()}");
                    }

                }
                
                foreach (var grupoServicoAgregadoProduto in servicoAgregadoProdutos)
                {
                    GrupoServicoAgregadoProduto v = null;
                    try
                    {
                        v = isFirstLoad ? null : _grupoServicoAgregadosProdutosRepositorio.GetSingle(x => x.CampoCodigo == grupoServicoAgregadoProduto.CampoCodigo);

                        if (v != null)
                        {
                            v.RegistroInativo = grupoServicoAgregadoProduto.RegistroInativo;
                            v.DataAtualizacao = DateTime.Now;
                            v.Quantidade = grupoServicoAgregadoProduto.Quantidade;
                            v.Item = grupoServicoAgregadoProduto.Item;
                            v.PermiteAlterarQuantidade = grupoServicoAgregadoProduto.PermiteAlterarQuantidade;
                            v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            grupoServicoAgregadoProduto.DataAtualizacao = DateTime.Now;
                            grupoServicoAgregadoProduto.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                            try
                            {
                                _grupoServicoAgregadosProdutosRepositorio.Add(grupoServicoAgregadoProduto);
                            }
                            catch (Exception e)
                            {
                                errosBuilder.AppendLine($"Erro ao persistir Grupo Servico Agregado na base de dados. Erro {e}.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncGrupoServicoAgregadosProdutos)} : {e}. Item: {v.ToStringLog()}");
                    }
                }
                _escopo.Finalizar();

                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.PA3);

            } while (jArray.Count > 0);            

            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Grupo Servico Agregado Produto: {errosBuilder}.");
        }

        public async Task SyncProduto(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SB1);

            do
            {
                var produtos = new List<Produto>();

                foreach (var jObj in jArray)
                {
                    var produto = new Produto();
                    try
                    {
                        var grupoProdutoId = jObj["GrupoProduto_Id"].ToString();
                        var grupo = _grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == grupoProdutoId);

                        if (grupo != null)
                        {
                            produto.IdGrupoProduto = grupo.Id.ToString();
                            produto.GrupoProduto = grupo;

                            produto.CampoCodigo = jObj["Id"].ToString();
                            produto.Descricao = jObj["Descricao"].ToString();
                            produto.CodigoFabricante = jObj["CodigoFabricante"].ToString();
                            produto.FabricantePeca = jObj["NomeFabricante"].ToString();
                            produto.IdGrupoServicoAgregado = jObj["GrupoServicoAgregados_Id"].ToString();
                            produto.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                            produtos.Add(produto);
                        }
                        else
                        {
                            errosBuilder.AppendLine($"Grupo Id {grupoProdutoId} não encontrado.");
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncProduto)} : {e}. Item: {produto.ToStringLog()}");
                    }
                }

                foreach (var produto in produtos)
                {

                    try
                    {
                        var v = isFirstLoad ? null : _produtoRepositorio.GetSingle(x => x.CampoCodigo == produto.CampoCodigo);

                        if (v != null)
                        {
                            v.CodigoFabricante = produto.CodigoFabricante;
                            v.Descricao = produto.Descricao;
                            v.RegistroInativo = produto.RegistroInativo;
                            v.DataAtualizacao = DateTime.Now;
                            v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            produto.DataAtualizacao = DateTime.Now;
                            produto.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                            try
                            {
                                _produtoRepositorio.Add(produto);
                            }
                            catch (Exception e)
                            {
                                errosBuilder.AppendLine($"Erro ao persistir Produtos na base de dados. Erro {e}.");
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncProduto)} : {e}. Item: {produto.ToStringLog()}");
                    }
                }

                try
                {
                    Elasticsearch elastic = new Elasticsearch(_escopo, _configGrafo);
                    elastic.EnviarElasticsearch(produtos);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao enviar para o Elasticsearch:" + e);
                }

                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SB1);
            } while (jArray.Count > 0);

            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Produto: {errosBuilder}.");
        }

        public async Task SyncProdutoComplemento(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();

            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SB5);

            // TODO - [Refatorar] Ajustar performance do código
            do
            {
                var produtosComplemento = new List<ProdutoComplemento>();

                foreach (var jObj in jArray)
                {
                    var produtoComplemento = new ProdutoComplemento();
                    try
                    {
                        var produtoId = jObj["Produto_Id"].ToString();

                        Produto produto = null;

                        if (!produtoId.IsNullOrEmpty())
                        {
                            produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == produtoId);
                        }

                        if (produto != null)
                        {
                            produtoComplemento.IdProduto = produto.IdProduto;
                            produtoComplemento.CampoCodigo = jObj["Id"].ToString();
                            if (jObj["Comprimento"].ToString() != "")
                            {
                                produtoComplemento.Comprimento = Convert.ToDecimal(jObj["Comprimento"].ToString());
                            }
                            else
                            {
                                produtoComplemento.Comprimento = 0;
                            }
                            if (jObj["Espessura"].ToString() != "")
                            {
                                produtoComplemento.Espessura = Convert.ToDecimal(jObj["Espessura"].ToString());
                            }
                            else
                            {
                                produtoComplemento.Espessura = 0;
                            }
                            if (jObj["Largura"].ToString() != "")
                            {
                                produtoComplemento.Largura = Convert.ToDecimal(jObj["Largura"].ToString());
                            }
                            else
                            {
                                produtoComplemento.Largura = 0;
                            }
                            if (jObj["Volume_m3"].ToString() != "")
                            {
                                produtoComplemento.VolumeM3 = Convert.ToDecimal(jObj["Volume_m3"].ToString());
                            }
                            else
                            {
                                produtoComplemento.VolumeM3 = 0;
                            }
                            produtoComplemento.CampoHTML = jObj["CampoHtml"].ToString();
                            if (jObj["Perfil"].ToString() != "")
                            {
                                produtoComplemento.Perfil = Convert.ToDecimal(jObj["Perfil"].ToString());
                            }
                            else
                            {
                                produtoComplemento.Perfil = 0;
                            }
                            if (jObj["Aro"].ToString() != "")
                            {
                                produtoComplemento.Aro = Convert.ToDecimal(jObj["Aro"].ToString());
                            }
                            else
                            {
                                produtoComplemento.Aro = 0;
                            }
                            produtoComplemento.Carga = jObj["Carga"].ToString();
                            produtoComplemento.Indice = jObj["Indice"].ToString();
                            produtoComplemento.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                            produtosComplemento.Add(produtoComplemento);

                            if (produtoComplemento.Largura > 0 && produtoComplemento.Aro > 0 && produtoComplemento.Perfil > 0)
                            {
                                List<long> veiculos = (from v in _veiculoMedidasPneusRepositorio.GetAll()
                                                        where v.Largura == produtoComplemento.Largura
                                                        && v.Aro == produtoComplemento.Aro
                                                        && v.Perfil == produtoComplemento.Perfil
                                                        select v.idVeiculo).ToList();
                                foreach (long id in veiculos)
                                {
                                    Veiculo veiculo = _veiculoRepositorio.GetSingle(a => a.Id == id);
                                    Elasticsearch elastic = new Elasticsearch(_escopo, _configGrafo);
                                    elastic.EnviarElasticsearch(produtoComplemento, veiculo);
                                }
                            }
                        }

                        else
                        {
                            errosBuilder.AppendLine($"Produto Id {produtoId} não encontrado.");
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncProdutoComplemento)} : {e}. Item: {produtoComplemento.ToStringLog()}");
                    }
                }

                foreach (var produtoComplemento in produtosComplemento)
                {
                    var v = isFirstLoad ? null : _produtoComplementoRepositorio.GetSingle(x => x.CampoCodigo == produtoComplemento.CampoCodigo);

                    if (v != null)
                    {
                        v.Descricao = produtoComplemento.Descricao;
                        v.Comprimento = produtoComplemento.Comprimento;
                        v.Espessura = produtoComplemento.Espessura;
                        v.Largura = produtoComplemento.Largura;
                        v.VolumeM3 = produtoComplemento.VolumeM3;
                        v.RegistroInativo = produtoComplemento.RegistroInativo;
                        v.DataAtualizacao = DateTime.Now;
                        v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        v.Perfil = produtoComplemento.Perfil;
                        v.Aro = produtoComplemento.Aro;
                        v.Carga = produtoComplemento.Carga;
                        v.Indice = produtoComplemento.Indice;
                    }
                    else
                    {
                        produtoComplemento.DataAtualizacao = DateTime.Now;
                        produtoComplemento.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _produtoComplementoRepositorio.Add(produtoComplemento);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Produto Complemento na base de dados. Erro {e}.");
                        }
                    }
                }

                _escopo.Finalizar();

                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SB5);
            } while (jArray.Count > 0);
            
            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Produto Complemento: {errosBuilder}.");
        }

        private static List<T> ConvertJsonToObject<T>(JArray jArray) where T : Entidade
        {
            var list = new List<T>();
            foreach (var jObj in jArray)
            {
                var entity = Activator.CreateInstance<T>();
                entity.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                entity.CampoCodigo = jObj["CampoCodigo"].ToString();
                entity.Descricao = jObj["CampoDescricao"].ToString();
                list.Add(entity);
            }

            return list;
        }
    }
}
