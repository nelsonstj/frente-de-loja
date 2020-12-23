using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Servicos
{
    public class ProdutoServico : IProdutoServico
    {
        private readonly IEstoqueProtheusApi _estoqueProtheusApi;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<LojaDellaVia> _lojasRepositorio;
        private readonly IRepositorio<ProdutoComplemento> _produtoComplementoRepositorio;
        private readonly IRepositorio<GrupoServicoAgregadoProduto> _grupoServicoAgregadoProdutoRepositorio;
        private readonly IRepositorio<TabelaPrecoItem> _tabelaPrecoItemRepositorio;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<OrcamentoItem> _orcamentoItemRepositorio;

        public ProdutoServico(IRepositorioEscopo escopo, IEstoqueProtheusApi estoqueProtheusApi)
        {
            _estoqueProtheusApi = estoqueProtheusApi;
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _produtoComplementoRepositorio = escopo.GetRepositorio<ProdutoComplemento>();
            _lojasRepositorio = escopo.GetRepositorio<LojaDellaVia>();
            _tabelaPrecoItemRepositorio = escopo.GetRepositorio<TabelaPrecoItem>();
            _orcamentoRepositorio = escopo.GetRepositorio<Orcamento>();
            _orcamentoItemRepositorio = escopo.GetRepositorio<OrcamentoItem>();
            _grupoServicoAgregadoProdutoRepositorio = escopo.GetRepositorio<GrupoServicoAgregadoProduto>();
        }
        public async Task<List<ProdutoProtheusDto>> ObterSaldosProdutoLojaDellaVia(string[] idProdutos, string idLoja)
        {
            var jsonResult = await _estoqueProtheusApi.BuscaEstoqueProdutoFiliais(idProdutos, idLoja);

            var produtoProtheusDtoList = new List<ProdutoProtheusDto>();
            foreach (var jObj in jsonResult)
            {
                var produtoProtheus = new ProdutoProtheusDto();
                produtoProtheus.Filial = jObj["Filial"].ToString();
                produtoProtheus.NomeFilial = jObj["NomeFilial"].ToString();
                produtoProtheus.CodigoDellaVia = jObj["CodigoDellaVia"].ToString();
                produtoProtheus.SaldoDisponivel = Convert.ToDecimal(jObj["SaldoDisponivel"].ToString());
                produtoProtheus.SaldoAtual = Convert.ToDecimal(jObj["SaldoAtual"].ToString());

                produtoProtheusDtoList.Add(produtoProtheus);
            }
            return produtoProtheusDtoList;
        }

        /*        public async Task<ModalEstoqueDto> ObterSaldoProdutoLojasDellaVia(string idProduto, long[] idsLojaDellavia)
        {
            var produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == idProduto);
            if (produto == null)
                throw new NegocioException("Produto não encontrado.");

            var codLojasDellaVia = new List<string>();
            if (idsLojaDellavia == null)
                _lojasRepositorio.GetAll().ToList().ForEach(a => codLojasDellaVia.Add(a.CampoCodigo));
            else
            {

                foreach (var id in idsLojaDellavia)
                {
                    var loja = _lojasRepositorio.GetSingle(x => x.Id == id)?.CampoCodigo;
                    if (loja != null)
                        codLojasDellaVia.Add(loja);
                }
            }
            if (codLojasDellaVia.Count <= 0)
                throw new NegocioException("Nenhuma loja foi encontrada.");

            var jsonResult = await _estoqueProtheusApi.BuscaEstoqueProdutoFiliais(produto.CampoCodigo, codLojasDellaVia.ToArray());

            var modalEstoqueDto = new ModalEstoqueDto();

            modalEstoqueDto.CampoCodigo = produto.CampoCodigo;
            var produtoProtheusDtoList = new List<ProdutoProtheusDto>();
            foreach (var jObj in jsonResult)
            {
                var produtoProtheus = new ProdutoProtheusDto();
                produtoProtheus.Filial = jObj["Filial"].ToString();
                produtoProtheus.NomeFilial = jObj["NomeFilial"].ToString();
                produtoProtheus.CodigoDellaVia = jObj["CodigoDellaVia"].ToString();
                produtoProtheus.SaldoDisponivel = Convert.ToDecimal(jObj["SaldoDisponivel"].ToString());
                produtoProtheus.SaldoAtual = Convert.ToDecimal(jObj["SaldoAtual"].ToString());

                produtoProtheusDtoList.Add(produtoProtheus);
            }
            modalEstoqueDto.ProdutoProtheus = produtoProtheusDtoList.OrderByDescending(a => a.SaldoDisponivel).ToList();
            modalEstoqueDto.DescricaoProduto = produto.Descricao;

            return modalEstoqueDto;
        }
*/
        /*        public Dictionary<string, decimal> ObterPrecoProdutoPorOrcamento(long idOrcamento, string[] campoCodigoList)
        {
            var codPrecoDictionary = new Dictionary<string, decimal>();


            var orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento);


            foreach (var campoCodigo in campoCodigoList.Distinct().ToList())
            {
                try
                {
                    var p = _produtoRepositorio.GetSingle(x => x.CampoCodigo == campoCodigo);

                    var preco = _tabelaPrecoItemRepositorio.GetSingle(x => x.ProdutoId == p.Id && x.TabelaPrecoId == orcamento.IdTabelaPreco)?.PrecoVenda;

                    codPrecoDictionary.Add(campoCodigo, preco ?? 0);
                }
                catch (Exception)
                {

                    codPrecoDictionary.Add(campoCodigo, 0);
                }

            }

            return codPrecoDictionary;
        }
*/
        /*        public ModalDetalhesProdutoDto ObterDadosModalDetalhesProduto(long idOrcamento, string campoCodigoProduto)
        {

            var orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento);

            var modalDetalhes = new ModalDetalhesProdutoDto();
            // Produto
            var produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == campoCodigoProduto);
            if (produto == null)
                throw new NegocioException("Produto não localizado.");
            var produtoDto = Mapper.Map<ProdutoDto>(produto);

            // Tabela Preco
            var tabelaPrecoItemPai =
                _tabelaPrecoItemRepositorio.GetSingle(x => x.ProdutoId == produto.Id && x.TabelaPrecoId == orcamento.IdTabelaPreco);

            modalDetalhes.ProdutoPaiDto = produtoDto;
            modalDetalhes.IdOrcamento = idOrcamento;
            modalDetalhes.IdOrcamentoItemPai = 0;
            modalDetalhes.QuantidadePai = 0;
            modalDetalhes.PrecoUnitarioPai = tabelaPrecoItemPai?.PrecoVenda ?? 0;
            modalDetalhes.TotalItemPai = 0;

            var listaProdutoAgregados = new List<GrupoServicoAgregadoProdutoDto>();


            var itensServico = Mapper.Map<ICollection<GrupoServicoAgregadoProdutoDto>>(
                _grupoServicoAgregadoProdutoRepositorio.Get(x =>
                    x.IdGrupoServicoAgregado == produto.IdGrupoServicoAgregado && !x.RegistroInativo));
            // Servico agregado list
            foreach (var produtoAgregadoNaoAdicionado in itensServico)
            {
                var itemServicoModalDto = produtoAgregadoNaoAdicionado;

                var tabelaPrecoItem =
                    _tabelaPrecoItemRepositorio.GetSingle(x => x.ProdutoId == produtoAgregadoNaoAdicionado.IdProduto && x.TabelaPrecoId == orcamento.IdTabelaPreco);

                var produtoServ = _produtoRepositorio.FindByKey(produtoAgregadoNaoAdicionado.IdProduto);

                itemServicoModalDto.Quantidade = 0;
                itemServicoModalDto.PrecoUnitario = tabelaPrecoItem == null ? 0 : tabelaPrecoItem.PrecoVenda;
                itemServicoModalDto.TotalItem = itemServicoModalDto.Quantidade * itemServicoModalDto.PrecoUnitario;
                itemServicoModalDto.Descricao = produtoServ.CampoCodigo + " - " + produtoServ.Descricao;


                listaProdutoAgregados.Add(itemServicoModalDto);
            }

            modalDetalhes.ProdutosAgregadosModalList = listaProdutoAgregados;
            var produtoComplemento = _produtoComplementoRepositorio.GetSingle(x => x.IdProduto == produto.Id);
            if (produtoComplemento != null)
            {
                modalDetalhes.ProdutoComplementoPaiDto = Mapper.Map<ProdutoComplementoDto>(produtoComplemento);
                modalDetalhes.ProdutoComplementoPaiDto.hasCampoHTML = !string.IsNullOrEmpty(produtoComplemento.CampoHTML);
                produtoComplemento.CampoHTML = string.Empty; // Caso contrario o json ficaria mt grande.
            }

            return modalDetalhes;
        }
*/
        /*        public string ObterHtmlMaisDetalhesProduto(long idProduto)
                {
                    var produtoComplemento = _produtoComplementoRepositorio.GetSingle(x => x.IdProduto == idProduto);
                    if (produtoComplemento == null)
                        return string.Empty;
                    return produtoComplemento.CampoHTML;
                }
        */

        public int TamanhoTermoFabricantePeca(string termoBusca)
        {
            return ObterCatalogoQueryFabricantePeca(termoBusca)
                .Select(x => x.FabricantePeca)
                .Distinct()
                .Count();
        }
        public IQueryable<Produto> ObterCatalogoQueryFabricantePeca(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from catalogo in _produtoRepositorio.GetAll()
                   where DbFunctions.Like(catalogo.FabricantePeca, termoBusca)
                   orderby catalogo.FabricantePeca
                   select catalogo;
        }
        public IQueryable<string> ObterFabricantePecaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCatalogoQueryFabricantePeca(termoBusca)
                .Select(x => x.FabricantePeca)
                .Distinct();
        }
    }
}