using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Grafo;
using DV.FrenteLoja.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Servicos.Comuns
{
    class Elasticsearch
    {
        private readonly ConfigGrafo _configGrafo;
        private readonly IRepositorio<Catalogo> _catalogoRepositorio;

        private readonly IRepositorio<VeiculoMedidasPneus> _veiculoMedidasPneusRepositorio;
        private readonly IRepositorio<ProdutoComplemento> _produtoComplementoRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<Marca> _marcaRepositorio;
        private readonly IRepositorio<MarcaModelo> _marcaModeloRepositorio;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;

        private readonly IGrupoProdutoServico _grupoProdutoServico;

        public Elasticsearch(IRepositorioEscopo escopo, 
                             ConfigGrafo configGrafo = null)
        {
            _configGrafo = configGrafo;
            _catalogoRepositorio = escopo.GetRepositorio<Catalogo>();
            _veiculoMedidasPneusRepositorio = escopo.GetRepositorio<VeiculoMedidasPneus>();
            _produtoComplementoRepositorio = escopo.GetRepositorio<ProdutoComplemento>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _marcaRepositorio = escopo.GetRepositorio<Marca>();
            _marcaModeloRepositorio = escopo.GetRepositorio<MarcaModelo>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _grupoProdutoServico = new Core.Servicos.GrupoProdutoServico(escopo);
        }

        public void EnviarElasticsearch(List<Produto> produtos)
        {
            var elasticsearchItemDtos = new List<ElasticsearchItemDto>();

            foreach (var p in produtos)
            {
                elasticsearchItemDtos.AddRange(EnviarElasticsearch(p));
            }

            new Grafo.Conversores.SQLToGrafoCatalogo().Importar(elasticsearchItemDtos, _configGrafo.contextoGrafo, OrigemCarga.Produto);
        }

        public List<ElasticsearchItemDto> EnviarElasticsearch(Produto p)
        {
            var elasticsearchItemDtos = new List<ElasticsearchItemDto>();
            var listalogos = _catalogoRepositorio.Get(x => x.CodigoFabricante == p.CodigoFabricante).ToList();
            var elasticsearchItemDto = new ElasticsearchItemDto();

            if (!listalogos.Any())
            {
                elasticsearchItemDto.ProdutoCodFabricante = p.CodigoFabricante;
                elasticsearchItemDto.ProdutoCodDellavia = p.CampoCodigo;
                elasticsearchItemDto.ProdutoCodGrupo = _grupoProdutoServico.BuscaTipoProdutoElasticSearch(p.GrupoProduto.CampoCodigo);
                elasticsearchItemDto.PrioridadeOrdenacao = p.GrupoProduto.CampoCodigo;
                elasticsearchItemDto.ProdutoFabricantePeca = p.FabricantePeca ?? string.Empty;
                elasticsearchItemDto.ProdutoDescricao = p.Descricao;
                elasticsearchItemDtos.Add(elasticsearchItemDto);
            }
            else
            {
                foreach (var catalogo in listalogos)
                {
                    elasticsearchItemDto = new ElasticsearchItemDto()
                    {
                        ProdutoCodFabricante = catalogo.CodigoFabricante,
                        VeiculoModelo = catalogo.ModeloVeiculo,
                        VeiculoMarca = catalogo.MarcaVeiculo,
                        VeiculoVersao = catalogo.VersaoVeiculo,
                        ProdutoFabricantePeca = catalogo.FabricantePeca,
                        VeiculoAnoInicial = catalogo.AnoInicial,
                        VeiculoAnoFinal = catalogo.AnoFinal,
                        ProdutoInformacaoComplementar = catalogo.InformacoesComplementares,
                        ProdutoCodDellavia = p.CampoCodigo,
                        ProdutoCodGrupo = _grupoProdutoServico.BuscaTipoProdutoElasticSearch(p.GrupoProduto.CampoCodigo),
                        PrioridadeOrdenacao = p.GrupoProduto.CampoCodigo,
                        ProdutoDescricao = p.Descricao
                    };
                    elasticsearchItemDtos.Add(elasticsearchItemDto);
                }
            }
            return elasticsearchItemDtos;
        }

        public void EnviarElasticsearch(Veiculo veiculo)
        {
            var elasticsearchItemDtos = new List<ElasticsearchItemDto>();

            List<VeiculoMedidasPneus> vmpList = _veiculoMedidasPneusRepositorio.Get(a => a.idVeiculo == veiculo.Id).ToList();
            foreach (VeiculoMedidasPneus vmp in vmpList)
            {
                List<ProdutoComplemento> produtosCompativeis = _produtoComplementoRepositorio.Get(x => x.Aro == vmp.Aro
                                                                                                && x.Largura == vmp.Largura
                                                                                                && x.Perfil == vmp.Perfil)
                                                                                                .ToList();
                foreach (ProdutoComplemento produtoComplemento in produtosCompativeis)
                {
                    List<Produto> produtos = _produtoRepositorio.Get(a => a.IdProduto == produtoComplemento.IdProduto).ToList();
                    foreach (Produto produto in produtos)
                    {
                        EnviarElasticsearch(produto, veiculo);
                    }
                }
            }
            if (elasticsearchItemDtos.Count() > 0)
            {
                new Grafo.Conversores.SQLToGrafoCatalogo().Importar(elasticsearchItemDtos, _configGrafo.contextoGrafo, OrigemCarga.Produto);
            }
        }

        public void EnviarElasticsearch(ProdutoComplemento produtoComplemento, Veiculo veiculo)
        {
            var elasticsearchItemDtos = new List<ElasticsearchItemDto>();

            List<Produto> produtos = _produtoRepositorio.Get(a => a.IdProduto == produtoComplemento.IdProduto).ToList();
            foreach (Produto produto in produtos)
            {
                elasticsearchItemDtos.AddRange(EnviarElasticsearch(produto, veiculo));
            }
            new Grafo.Conversores.SQLToGrafoCatalogo().Importar(elasticsearchItemDtos, _configGrafo.contextoGrafo, OrigemCarga.Produto);
        }

        public List<ElasticsearchItemDto> EnviarElasticsearch(Produto produto, Veiculo veiculo)
        {
            var elasticsearchItemDtos = new List<ElasticsearchItemDto>();
            var listalogos = _catalogoRepositorio.Get(x => x.CodigoFabricante == produto.CodigoFabricante).ToList();

            var elasticsearchItemDto = new ElasticsearchItemDto();

            if (!listalogos.Any())
            {
                MarcaModeloVersao versao = _marcaModeloVersaoRepositorio.GetSingle(a => a.Id == veiculo.IdMarcaModeloVersao);
                MarcaModelo modelo = _marcaModeloRepositorio.GetSingle(a => a.Id == versao.IdMarcaModelo);
                Marca marca = _marcaRepositorio.GetSingle(a => a.Id == modelo.IdMarca);
                elasticsearchItemDto = new ElasticsearchItemDto()
                {
                    VeiculoModelo = modelo.Descricao,
                    VeiculoMarca = marca.Descricao,
                    VeiculoVersao = versao.Descricao,
                    VeiculoAnoInicial = veiculo.AnoInicial.Year,
                    VeiculoAnoFinal = veiculo.AnoFinal.Year,
                    ProdutoCodFabricante = produto.CodigoFabricante,
                    ProdutoCodDellavia = produto.CampoCodigo,
                    ProdutoCodGrupo = _grupoProdutoServico.BuscaTipoProdutoElasticSearch(produto.GrupoProduto.CampoCodigo),
                    PrioridadeOrdenacao = produto.GrupoProduto.CampoCodigo,
                    ProdutoFabricantePeca = produto.FabricantePeca ?? string.Empty,
                    ProdutoDescricao = produto.Descricao,
                    VersaoMotor = veiculo.VersaoMotor.Descricao
                };
                elasticsearchItemDtos.Add(elasticsearchItemDto);
            }
            else
            {
                foreach (var catalogo in listalogos)
                {
                    elasticsearchItemDto = new ElasticsearchItemDto()
                    {
                        ProdutoCodFabricante = catalogo.CodigoFabricante,
                        VeiculoModelo = catalogo.ModeloVeiculo,
                        VeiculoMarca = catalogo.MarcaVeiculo,
                        VeiculoVersao = catalogo.VersaoVeiculo,
                        ProdutoFabricantePeca = catalogo.FabricantePeca,
                        VeiculoAnoInicial = catalogo.AnoInicial,
                        VeiculoAnoFinal = catalogo.AnoFinal,
                        ProdutoInformacaoComplementar = catalogo.InformacoesComplementares,
                        ProdutoCodDellavia = produto.CampoCodigo,
                        ProdutoCodGrupo = _grupoProdutoServico.BuscaTipoProdutoElasticSearch(produto.GrupoProduto.CampoCodigo),
                        PrioridadeOrdenacao = produto.GrupoProduto.CampoCodigo,
                        ProdutoDescricao = produto.Descricao,
                        VersaoMotor = veiculo.VersaoMotor.Descricao
                    };
                    elasticsearchItemDtos.Add(elasticsearchItemDto);
                }
            }
            return elasticsearchItemDtos;
        }
    }
}
