using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Infra.Security;

namespace DV.FrenteLoja.Core.Servicos
{
    public class OrcamentoItemServico : IOrcamentoItemServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly ILojaDellaViaServico _dellaViaServico;
        private readonly ICondicaoPagamentoParcelasApi _condicaoPagamentoParcelasApi;
        private readonly IImpostosServico _impostosServico;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<OrcamentoItem> _orcamentoItemRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<TabelaPrecoItem> _tabelaPrecoItem;
        private readonly IRepositorio<ProdutoComplemento> _produtoComplementoRepositorio;
        private readonly IRepositorio<GrupoServicoAgregadoProduto> _grupoServicoAgregadoProdutoRepositorio;
        private readonly IRepositorio<OrcamentoItemEquipeMontagem> _equipeMontagemRepositorio;
        private readonly IRepositorio<DescontoModeloVenda> _descontoModeloVendaRepositorio;
        private readonly IRepositorio<SolicitacaoDescontoVendaAlcada> _solicitacaoDescontoVendaAlcadaRepositorio;
        private readonly IRepositorio<DescontoVendaAlcada> _descontoVendaAlcadaRepositorio;
        private readonly IRepositorio<DescontoVendaAlcadaGrupoProduto> _descontoVendaAlcadaGrupoProdutoRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;
        private readonly IRepositorio<Operador> _operadoRepositorio;
        private readonly IRepositorio<OrcamentoItemEquipeMontagem> _orcItemEquipeMontagem;
        private readonly IRepositorio<LojaDellaVia> _lojasDellaViaRepositorio;
        private readonly IRepositorio<CondicaoPagamento> _condicaoPagamentoRepositorio;
        private readonly IRepositorio<TipoVenda> _tipoVendaRepositorio;
        private readonly IRepositorio<Banco> _bancoRepositorio;
        private readonly IRepositorio<AdministradoraFinanceira> _admFinanceiraRepositorio;
        private readonly IRepositorio<OrcamentoFormaPagamento> _orcamentoFormaPagamentoRepositorio;

        public OrcamentoItemServico(IRepositorioEscopo escopo, ILojaDellaViaServico dellaViaServico, ICondicaoPagamentoParcelasApi condicaoPagamentoParcelasApi, IImpostosServico impostosServico)
        {
            _escopo = escopo;
            _dellaViaServico = dellaViaServico;
            _condicaoPagamentoParcelasApi = condicaoPagamentoParcelasApi;
            _impostosServico = impostosServico;
            _orcamentoRepositorio = _escopo.GetRepositorio<Orcamento>();
            _orcamentoItemRepositorio = _escopo.GetRepositorio<OrcamentoItem>();
            _produtoRepositorio = _escopo.GetRepositorio<Produto>();
            _tabelaPrecoItem = _escopo.GetRepositorio<TabelaPrecoItem>();
            _produtoComplementoRepositorio = _escopo.GetRepositorio<ProdutoComplemento>();
            _grupoServicoAgregadoProdutoRepositorio = escopo.GetRepositorio<GrupoServicoAgregadoProduto>();
            _equipeMontagemRepositorio = escopo.GetRepositorio<OrcamentoItemEquipeMontagem>();
            _descontoModeloVendaRepositorio = escopo.GetRepositorio<DescontoModeloVenda>();
            _solicitacaoDescontoVendaAlcadaRepositorio = escopo.GetRepositorio<SolicitacaoDescontoVendaAlcada>();
            _descontoVendaAlcadaRepositorio = escopo.GetRepositorio<DescontoVendaAlcada>();
            _operadoRepositorio = escopo.GetRepositorio<Operador>();
            _descontoVendaAlcadaGrupoProdutoRepositorio = escopo.GetRepositorio<DescontoVendaAlcadaGrupoProduto>();
            _vendedorRepositorio = escopo.GetRepositorio<Vendedor>();
            _orcItemEquipeMontagem = escopo.GetRepositorio<OrcamentoItemEquipeMontagem>();
            _lojasDellaViaRepositorio = escopo.GetRepositorio<LojaDellaVia>();
            _condicaoPagamentoRepositorio = escopo.GetRepositorio<CondicaoPagamento>();
            _bancoRepositorio = escopo.GetRepositorio<Banco>();
            _admFinanceiraRepositorio = escopo.GetRepositorio<AdministradoraFinanceira>();
            _orcamentoFormaPagamentoRepositorio = escopo.GetRepositorio<OrcamentoFormaPagamento>();
            _tipoVendaRepositorio = escopo.GetRepositorio<TipoVenda>();
        }

        /*      public OrcamentoProdutoBuscaDto InserirItensOrcamento(ModalDetalhesProdutoDto modalDetalhesProdutoDto)
        {
            var orcamento = _orcamentoRepositorio.GetSingle(x => !x.RegistroInativo && x.Id == modalDetalhesProdutoDto.IdOrcamento);

            var produtoPai = _produtoRepositorio.GetSingle(x => !x.RegistroInativo && x.Id == modalDetalhesProdutoDto.ProdutoPaiDto.Id);

            var tabelaPrecoItemPai = _tabelaPrecoItem.GetSingle(x => !x.RegistroInativo && x.ProdutoId == produtoPai.Id && x.TabelaPrecoId == orcamento.IdTabelaPreco);
            var nrItemPai = orcamento.OrcamentoItens.Count == 0 ? 1 : orcamento.OrcamentoItens.Max(x => x.NrItem) + 1;

            if (modalDetalhesProdutoDto.QuantidadePai == 0)
                throw new NegocioException("A quantidade do produto deve ser maior que zero.");

            var orcItemPai = new OrcamentoItem
            {
                OrcamentoId = modalDetalhesProdutoDto.IdOrcamento,
                ProdutoId = modalDetalhesProdutoDto.ProdutoPaiDto.Id,
                NrItem = nrItemPai,
                Quantidade = modalDetalhesProdutoDto.QuantidadePai,
                PrecoUnitario = tabelaPrecoItemPai.PrecoVenda,
                TotalItem = modalDetalhesProdutoDto.QuantidadePai * tabelaPrecoItemPai.PrecoVenda,
                NrItemProdutoPaiId = null,
                PercDescon = 0,
                ValorDesconto = 0
            };


            _orcamentoItemRepositorio.Add(orcItemPai);

            // Add os serviços agregados com quantidade > 0
            foreach (var prodServicoFilho in modalDetalhesProdutoDto.ProdutosAgregadosModalList)
            {
                if (prodServicoFilho.Quantidade > 0)
                {
                    var produto = _produtoRepositorio.GetSingle(x => !x.RegistroInativo && x.Id == prodServicoFilho.IdProduto);

                    var tabelaPrecoItem =
                        _tabelaPrecoItem.GetSingle(x => !x.RegistroInativo && x.ProdutoId == produto.Id && x.TabelaPrecoId == orcamento.IdTabelaPreco);
                    if (tabelaPrecoItem == null)
                        continue;

                    var orcItem = new OrcamentoItem
                    {
                        OrcamentoId = modalDetalhesProdutoDto.IdOrcamento,
                        ProdutoId = produto.Id,
                        NrItem = orcamento.OrcamentoItens.Max(x => x.NrItem) + 1,
                        Quantidade = prodServicoFilho.Quantidade,
                        PrecoUnitario = tabelaPrecoItem.PrecoVenda,
                        TotalItem = prodServicoFilho.Quantidade * tabelaPrecoItem.PrecoVenda,
                        NrItemProdutoPaiId = orcItemPai.NrItem,
                        PercDescon = 0,
                        ValorDesconto = 0,
                    };
                    _orcamentoItemRepositorio.Add(orcItem);
                }
            }
            RemoverPagamentos(orcamento.Id);
            _escopo.Finalizar();
            // }

            return ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orcamento.Id);
        }
*/
        /*        public OrcamentoProdutoBuscaDto RemoverItensOrcamento(long orcamentoId, long orcamentoItemId)
                {
                    var orcItem = _orcamentoItemRepositorio.FindByKey(orcamentoItemId);

                    // Se o NrItemProdutoPaiId é nulo ele é um pai e pode ter serviços correlacionados
                    if (orcItem.NrItemProdutoPaiId == null)
                    {
                        foreach (var item in _orcamentoItemRepositorio.Get(a => a.OrcamentoId == orcamentoId && a.NrItemProdutoPaiId == orcItem.NrItem))
                        {
                            // Removendo a equipe de montagem do serviço correlacionado.
                            foreach (var orcamentoItemEquipeMontagem in item.EquipeMontagemList.ToList())
                            {
                                _equipeMontagemRepositorio.Remove(orcamentoItemEquipeMontagem);
                            }
                            _solicitacaoDescontoVendaAlcadaRepositorio.Remove(x => x.IdOrcamentoItem == item.Id);
                            _orcamentoItemRepositorio.Remove(item);
                        }
                    }
                    if (orcItem.EquipeMontagemList.Count > 0)
                    {
                        // Removendo a equipe de montagem do serviço correlacionado.
                        foreach (var orcamentoItemEquipeMontagem in orcItem.EquipeMontagemList.ToList())
                        {
                            _equipeMontagemRepositorio.Remove(orcamentoItemEquipeMontagem);
                        }
                    }
                    _solicitacaoDescontoVendaAlcadaRepositorio.Remove(x => x.IdOrcamentoItem == orcItem.Id);

                    _orcamentoItemRepositorio.Remove(orcItem);

                    RemoverPagamentos(orcamentoId);

                    _impostosServico.CalcularImpostos(orcamentoId);

                    _escopo.Finalizar();

                    return ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orcamentoId);
                }
        */

        public OrcamentoProdutoBuscaDto ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(long idOrcamento)
        {
            var orcamentoItens = _orcamentoItemRepositorio.Get(x => x.OrcamentoId == idOrcamento).ToList();

            var orcamentoItensSomentePais = orcamentoItens.Where(x => x.NrItemProdutoPaiId == null);

            var produtoBuscaDto = new OrcamentoProdutoBuscaDto
            {

                IdOrcamento = idOrcamento
            };

            decimal total = 0;
            foreach (var orcamenteItenPai in orcamentoItensSomentePais)
            {
                if (orcamenteItenPai.Produto == null)
                    orcamenteItenPai.Produto = _produtoRepositorio.GetSingle(a => a.CampoCodigo == orcamenteItenPai.ProdutoId);

                var produtoSacola = new SacolaProdutoDto
                {
                    IdOrcamentoItem = orcamenteItenPai.Id,
                    Descricao = orcamenteItenPai.Produto.Descricao,
                    Quantidade = (int)orcamenteItenPai.Quantidade,
                    Valor = orcamenteItenPai.PrecoUnitario,
                };

                total += (produtoSacola.Quantidade * produtoSacola.Valor);

                // Varredura nos orcamento itens filhos (Servicos Agregados)
                foreach (var orcamentoIten in orcamentoItens.Where(x => x.NrItemProdutoPaiId == orcamenteItenPai.NrItem))
                {
                    var produtoSacolaFilho = new ServicoCorrelacionadoDto
                    {
                        IdOrcamentoItem = orcamentoIten.Id,
                        Descricao = orcamentoIten.Produto.Descricao,
                        Quantidade = (int)orcamentoIten.Quantidade,
                        Valor = orcamentoIten.PrecoUnitario,
                    };

                    total += (produtoSacolaFilho.Quantidade * produtoSacolaFilho.Valor);
                    produtoSacola.Servicos.Add(produtoSacolaFilho);
                }
                produtoBuscaDto.Produtos.Add(produtoSacola);
            }
            produtoBuscaDto.Total = total;
            return produtoBuscaDto;
        }

        /*        public OrcamentoProdutoBuscaDto AtualizarItensOrcamento(ModalDetalhesProdutoDto modalDetalhesProdutoDto)
        {
            if (modalDetalhesProdutoDto.QuantidadePai <= 0)
                throw new NegocioException("A quantidade do produto deve ser maior que zero.");


            var orcItemPai = _orcamentoItemRepositorio.GetSingle(x => x.Id == modalDetalhesProdutoDto.IdOrcamentoItemPai);

            if (orcItemPai.IdDescontoModeloVenda != null)
            {
                orcItemPai.IdDescontoModeloVenda = null;
                orcItemPai.PercDescon = decimal.Zero;
                orcItemPai.ValorDesconto = decimal.Zero;
                orcItemPai.DescontoModeloVendaUtilizado = null;
            }

            orcItemPai.Quantidade = modalDetalhesProdutoDto.QuantidadePai;
            if (orcItemPai.PercDescon > 0)
            {
                var valorDesconto = Decimal.Round((orcItemPai.PercDescon / 100) * (orcItemPai.Quantidade * orcItemPai.PrecoUnitario), 2);
                orcItemPai.TotalItem = (orcItemPai.Quantidade * orcItemPai.PrecoUnitario) - valorDesconto;
                orcItemPai.ValorDesconto = valorDesconto;
            }
            else
                orcItemPai.TotalItem = modalDetalhesProdutoDto.QuantidadePai * orcItemPai.PrecoUnitario;

            long idTabelaPreco;

            if (orcItemPai.Orcamento == null)
            {
                idTabelaPreco = _orcamentoRepositorio.GetSingle(s => s.Id == orcItemPai.OrcamentoId).IdTabelaPreco;

            }
            else
            {
                idTabelaPreco = orcItemPai.Orcamento.IdTabelaPreco;
            }

            foreach (var prodServicoFilho in modalDetalhesProdutoDto.ProdutosAgregadosModalList)
            {
                // Se o item tiver qtd > 0 e id = 0, é um novo item.
                if (prodServicoFilho.Quantidade > 0 && prodServicoFilho.IdOrcamentoItemFilho == 0)
                {
                    var produto = _produtoRepositorio.GetSingle(x => x.Id == prodServicoFilho.IdProduto);
                    var tabelaPrecoItem =
                        _tabelaPrecoItem.GetSingle(x => x.ProdutoId == produto.Id && x.TabelaPrecoId == idTabelaPreco);

                    if (tabelaPrecoItem == null)
                        continue;

                    var orcItem = new OrcamentoItem
                    {
                        OrcamentoId = orcItemPai.OrcamentoId,
                        ProdutoId = produto.Id,
                        NrItem = orcItemPai.Orcamento.OrcamentoItens.Max(x => x.NrItem) + 1,
                        Quantidade = prodServicoFilho.Quantidade,
                        PrecoUnitario = tabelaPrecoItem.PrecoVenda,
                        TotalItem = prodServicoFilho.Quantidade * tabelaPrecoItem.PrecoVenda,
                        NrItemProdutoPaiId = orcItemPai.NrItem,
                        PercDescon = 0,
                        ValorDesconto = 0,
                    };

                    _orcamentoItemRepositorio.Add(orcItem);
                }

                else if (prodServicoFilho.Quantidade > 0 && prodServicoFilho.IdOrcamentoItemFilho > 0)
                {
                    //update
                    var orcamentoItem = _orcamentoItemRepositorio.GetSingle(x => x.Id == prodServicoFilho.IdOrcamentoItemFilho);

                    orcamentoItem.Quantidade = prodServicoFilho.Quantidade;
                    if (orcamentoItem.PercDescon > 0)
                    {
                        var valorDesconto = Decimal.Round((orcamentoItem.PercDescon / 100) * (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), 2);
                        orcamentoItem.TotalItem = (prodServicoFilho.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;
                        orcamentoItem.ValorDesconto = valorDesconto;
                    }
                    else
                        orcamentoItem.TotalItem = prodServicoFilho.Quantidade * orcamentoItem.PrecoUnitario;

                }
                else if (prodServicoFilho.Quantidade == 0 && prodServicoFilho.IdOrcamentoItemFilho > 0)
                {
                    //remover do orçamentoitem
                    var orcamentoItem = _orcamentoItemRepositorio.GetSingle(x => x.Id == prodServicoFilho.IdOrcamentoItemFilho);

                    _orcamentoItemRepositorio.Remove(orcamentoItem);
                }


            }
            RemoverPagamentos(orcItemPai.OrcamentoId);
            _escopo.Finalizar();
            return ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orcItemPai.OrcamentoId);
        }
*/
        /*        public ModalDetalhesProdutoDto ObterDadosModalDetalhesProduto(long idOrcamentoItemPai)
                {
                    var modalDetalhes = new ModalDetalhesProdutoDto();

                    var orcamentoItemPai = _orcamentoItemRepositorio.GetSingle(a => a.Id == idOrcamentoItemPai);

                    // Produto
                    var produtoDto = Mapper.Map<ProdutoDto>(orcamentoItemPai.Produto);

                    modalDetalhes.ProdutoPaiDto = produtoDto;
                    modalDetalhes.IdOrcamento = orcamentoItemPai.OrcamentoId;
                    modalDetalhes.IdOrcamentoItemPai = orcamentoItemPai.Id;
                    modalDetalhes.QuantidadePai = (int)orcamentoItemPai.Quantidade;
                    modalDetalhes.PrecoUnitarioPai = orcamentoItemPai.PrecoUnitario;
                    modalDetalhes.TotalItemPai = orcamentoItemPai.TotalItem;

                    // Busca todos os servicos agregados do produto.
                    var grupoServicoAgregadoProdutoDtoLista = Mapper.Map<ICollection<GrupoServicoAgregadoProdutoDto>>(
                        _grupoServicoAgregadoProdutoRepositorio.Get(x => x.IdGrupoServicoAgregado == orcamentoItemPai.Produto.IdGrupoServicoAgregado));

                    var listaProdutoAgregados = new List<GrupoServicoAgregadoProdutoDto>();


                    foreach (var itemOrcamento in _orcamentoItemRepositorio.Get(a => a.NrItemProdutoPaiId == orcamentoItemPai.NrItem && a.OrcamentoId == orcamentoItemPai.OrcamentoId))
                    {
                        var itemServicoModalDto = grupoServicoAgregadoProdutoDtoLista.FirstOrDefault(a => a.IdProduto == itemOrcamento.ProdutoId);

                        // Se já está persistido na orcamentoItem remove para não ser add no proximo for.
                        grupoServicoAgregadoProdutoDtoLista.Remove(itemServicoModalDto);

                        if (itemServicoModalDto != null)
                        {
                            itemServicoModalDto.Descricao = itemOrcamento.Produto.CampoCodigo + " - " + itemOrcamento.Produto.Descricao;
                            itemServicoModalDto.Quantidade = (int)itemOrcamento.Quantidade;
                            itemServicoModalDto.PrecoUnitario = itemOrcamento.PrecoUnitario;
                            itemServicoModalDto.TotalItem = itemOrcamento.TotalItem;
                            if (itemOrcamento.Id != 0)
                                itemServicoModalDto.IdOrcamentoItemFilho = itemOrcamento.Id;
                            listaProdutoAgregados.Add(itemServicoModalDto);
                        }
                    }

                    // Sugerindo os produtos que não foram add na primeira vez.
                    foreach (var produtoAgregadoNaoAdicionado in grupoServicoAgregadoProdutoDtoLista)
                    {
                        long idTabelaPreco;

                        if (orcamentoItemPai.Orcamento == null)
                        {
                            idTabelaPreco = _orcamentoRepositorio.GetSingle(s => s.Id == orcamentoItemPai.OrcamentoId).IdTabelaPreco;

                        }
                        else
                        {
                            idTabelaPreco = orcamentoItemPai.Orcamento.IdTabelaPreco;
                        }
                        var tabelaPrecoItem =
                            _tabelaPrecoItem.GetSingle(x => x.ProdutoId == produtoAgregadoNaoAdicionado.IdProduto && x.TabelaPrecoId == idTabelaPreco);

                        var produto =
                           _produtoRepositorio.GetSingle(x => x.Id == produtoAgregadoNaoAdicionado.IdProduto);


                        produtoAgregadoNaoAdicionado.Quantidade = 0;
                        produtoAgregadoNaoAdicionado.PrecoUnitario = tabelaPrecoItem == null ? decimal.Zero : tabelaPrecoItem.PrecoVenda;
                        produtoAgregadoNaoAdicionado.TotalItem = produtoAgregadoNaoAdicionado.Quantidade * produtoAgregadoNaoAdicionado.PrecoUnitario;
                        produtoAgregadoNaoAdicionado.Descricao = produto.CampoCodigo + " - " + produto.Descricao;


                        listaProdutoAgregados.Add(produtoAgregadoNaoAdicionado);
                    }


                    // Servico agregado list
                    modalDetalhes.ProdutosAgregadosModalList = listaProdutoAgregados;

                    var produtoComplemento = _produtoComplementoRepositorio.GetSingle(x => x.IdProduto == orcamentoItemPai.ProdutoId);
                    if (produtoComplemento != null)
                    {
                        modalDetalhes.ProdutoComplementoPaiDto = Mapper.Map<ProdutoComplementoDto>(produtoComplemento);
                        modalDetalhes.ProdutoComplementoPaiDto.hasCampoHTML = !string.IsNullOrEmpty(produtoComplemento.CampoHTML);
                        produtoComplemento.CampoHTML = string.Empty; // Caso contrario o json ficaria mt grande.
                    }


                    return modalDetalhes;
                }
        */
        /*        public AplicarDescontoDto ObterItemOrcamentoDesconto(long orcamentoId, long orcamentoItemId)
        {
            var orcamentoItem = _orcamentoItemRepositorio.GetSingle(x => x.Id == orcamentoItemId);
            var orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == orcamentoId);


            var codProduto = orcamentoItem.Produto.CampoCodigo;
            var tabelaPreco = orcamento.TabelaPreco.CampoCodigo;

            DescontoModeloVenda descontoModeloVenda;
            // Novo
            if (orcamentoItem.IdDescontoModeloVenda == null)
            {
                descontoModeloVenda = _descontoModeloVendaRepositorio
                    .Get(x => x.AreaNegocioId == orcamento.IdAreaNegocio
                    && x.CodigosDeProdutoLiberados.Contains(codProduto) 
                    && x.TabelasDePrecoAssociadas.Contains(tabelaPreco) 
                    && !x.RegistroInativo).FirstOrDefault();
            }
            // edição
            else
            {
                descontoModeloVenda = _descontoModeloVendaRepositorio.GetSingle(x => x.Id == orcamentoItem.IdDescontoModeloVenda);
            }

            var aplicarDescontoDto = new AplicarDescontoDto();

            //Geral
            aplicarDescontoDto.IdDescontoModeloVenda = descontoModeloVenda?.Id;
            aplicarDescontoDto.DescricaoProduto = orcamentoItem.Produto.Descricao;
            aplicarDescontoDto.QuantidadeProduto = orcamentoItem.Quantidade;
            aplicarDescontoDto.ValorOriginal = orcamentoItem.TotalItem + orcamentoItem.ValorDesconto;
            aplicarDescontoDto.PercentualDesconto = orcamentoItem.PercDescon;
            aplicarDescontoDto.ValorDesconto = orcamentoItem.ValorDesconto;
            aplicarDescontoDto.ValorTotalComDesconto = orcamentoItem.TotalItem;
            aplicarDescontoDto.DescontoModeloVendaUtilizado = orcamentoItem.DescontoModeloVendaUtilizado;
            aplicarDescontoDto.IdOrcamentoItem = orcamentoItem.Id;

            // Modelo Venda
            aplicarDescontoDto.DescontoModeloVendaQuantidade1 = descontoModeloVenda?.PercentualDesconto1;
            aplicarDescontoDto.DescontoModeloVendaQuantidade2 = descontoModeloVenda?.PercentualDesconto2;
            aplicarDescontoDto.DescontoModeloVendaQuantidade3 = descontoModeloVenda?.PercentualDesconto3;
            aplicarDescontoDto.DescontoModeloVendaQuantidade4 = descontoModeloVenda?.PercentualDesconto4;
            aplicarDescontoDto.PrecoUnitario = orcamentoItem.PrecoUnitario;

            // Desconto Alçada
            var solicitacaoDescontoVendaAlcada = _solicitacaoDescontoVendaAlcadaRepositorio.GetSingle(x =>
                x.IdOrcamentoItem == orcamentoItemId && x.StatusSolicitacaoAlcada == StatusSolicitacao.NaoEnviado);

            if (solicitacaoDescontoVendaAlcada != null)
            {
                aplicarDescontoDto.ObservacaoGeral = solicitacaoDescontoVendaAlcada.ObservacaoGeral;
                aplicarDescontoDto.ObservacaoItem = solicitacaoDescontoVendaAlcada.ObservacaoItem;
            }
            else
            {
                aplicarDescontoDto.ObservacaoGeral = null;
                aplicarDescontoDto.ObservacaoItem = null;
            }


            if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
            {
                var codOperador = HttpContext.Current.User.Identity.GetIdOperador();
                var operador = _operadoRepositorio.GetSingle(x => x.CampoCodigo == codOperador);
                aplicarDescontoDto.PercentualLimiteDesconto = operador.PercLimiteDesconto;
            }

            else
            {
                var loja = _dellaViaServico.ObterLojaUsuarioLogado();
                var descontoVendaAlcada = _descontoVendaAlcadaRepositorio.GetSingle(
                    x => x.LojaDellaviaId == loja.Id && x.AreaNegocioId == orcamento.IdAreaNegocio);

                if (descontoVendaAlcada != null)
                {
                    var idGrupoProduto = orcamentoItem.Produto.GrupoProduto.Id;

                    var descontoVendaAlcadaGrupo = _descontoVendaAlcadaGrupoProdutoRepositorio.GetSingle(
                        x => x.GrupoProduto.Id == idGrupoProduto
                        && x.AreaNegocioId == orcamento.IdAreaNegocio && x.LojaDellaviaId == loja.Id);

                    if (descontoVendaAlcadaGrupo == null)
                    {
                        aplicarDescontoDto.PercentualDescontoAlcadaGerente = descontoVendaAlcada.PercentualDescontoGerente;
                        aplicarDescontoDto.PercentualLimiteDesconto = descontoVendaAlcada.PercentualDescontoVendedor;
                    }
                    else
                    {
                        aplicarDescontoDto.PercentualDescontoAlcadaGerente = descontoVendaAlcadaGrupo.PercentualDescontoGerente;
                        aplicarDescontoDto.PercentualLimiteDesconto = descontoVendaAlcadaGrupo.PercentualDescontoVendedor;
                    }
                }
            }
            return aplicarDescontoDto;
        }
*/

        public int TamanhoProfissionalMontagemPorTermo(string termoBusca, long idFilial)
        {
            return ObterProfissionalMontagemQuery(termoBusca, idFilial).Count();
        }

        public List<ProfissionalMontagemDto> ObterProfissionalMontagemPorNome(string termoBusca, int tamanhoPagina, int numeroPagina, long idFilial)
        {
            return ObterProfissionalMontagemQuery(termoBusca, idFilial)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<ProfissionalMontagemDto>()
                .ToList();
        }

        private IQueryable<Vendedor> ObterProfissionalMontagemQuery(string termoBusca, long idFilial)
        {
            var loja = _lojasDellaViaRepositorio.GetSingle(x => x.Id == idFilial);

            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            var debug =
                from vendedor in _vendedorRepositorio.GetAll()
                where (DbFunctions.Like(vendedor.Nome, termoBusca) ||
                       DbFunctions.Like(vendedor.CampoCodigo, termoBusca)) &&
                      vendedor.FilialOrigem.Equals(loja.CampoCodigo, StringComparison.CurrentCultureIgnoreCase)
                      && !vendedor.RegistroInativo
                orderby vendedor.CampoCodigo
                select vendedor;
            return debug;
        }

/*        public OrcamentoPagamentoDto InserirFormaPagamento(FormaPagamentoDto formaPagamentoDto)
        {           
            var formasPagamento = _orcamentoFormaPagamentoRepositorio.Get(x => x.IdOrcamento == formaPagamentoDto.IdOrcamento);
            var valorParceladoSomado = formasPagamento.Any() ? formasPagamento.Sum(x => x.TotalValorForma) : decimal.Zero;

            var valorTotalOrc = _orcamentoRepositorio.GetSingle(x => x.Id == formaPagamentoDto.IdOrcamento).OrcamentoItens.Select(x => x.TotalItem)
                .Sum();

            var valorImpostos = _orcamentoRepositorio.GetSingle(x => x.Id == formaPagamentoDto.IdOrcamento).ValorImpostos;

            var valorRestante = valorTotalOrc + valorImpostos - valorParceladoSomado;

            if (formaPagamentoDto.Valor > valorRestante)
                throw new NegocioException("O valor do pagamento excede o valor restante do orçamento.");

            switch (formaPagamentoDto.TipoFormaPagamento)
            {
                case TipoFormaPagamento.Cartao:
                    {
                        if (formaPagamentoDto.IdAdministradoraFinanceira == 0)
                            throw new NegocioException("Bandeira não informada.");
                        _orcamentoFormaPagamentoRepositorio.Add(
                            new OrcamentoFormaPagamento
                            {
                                IdOrcamento = formaPagamentoDto.IdOrcamento,
                                IdAdministradoraFinanceira = formaPagamentoDto.IdAdministradoraFinanceira,
                                TotalValorForma = formaPagamentoDto.Valor,
                                IdCondicaoPagamento = formaPagamentoDto.Id
                            });
                    }
                    break;
                case TipoFormaPagamento.Banco:
                    {
                        if (formaPagamentoDto.IdBanco == 0)
                            throw new NegocioException("Banco não informado.");

                        _orcamentoFormaPagamentoRepositorio.Add(
                            new OrcamentoFormaPagamento
                            {
                                IdOrcamento = formaPagamentoDto.IdOrcamento,
                                IdBanco = formaPagamentoDto.IdBanco,
                                TotalValorForma = formaPagamentoDto.Valor,
                                IdCondicaoPagamento = formaPagamentoDto.Id
                            });
                    }
                    break;
                case TipoFormaPagamento.Dinheiro:
                    _orcamentoFormaPagamentoRepositorio.Add(
                        new OrcamentoFormaPagamento
                        {
                            IdOrcamento = formaPagamentoDto.IdOrcamento,
                            TotalValorForma = formaPagamentoDto.Valor,
                            IdCondicaoPagamento = formaPagamentoDto.Id
                        });
                    break;
                default:
                    throw new NegocioException("Tipo da forma do pagamento não informado.");
            }

            _escopo.Finalizar();
            return ObterOrcamentoPagamentoDto(formaPagamentoDto.IdOrcamento);
        }
*/
/*        public OrcamentoPagamentoDto RemoverFormaPagamento(long idOrcamentoFormaPagamento)
        {
            var orcamentoFormaPagamento = _orcamentoFormaPagamentoRepositorio.GetSingle(x => x.Id == idOrcamentoFormaPagamento);

            if (orcamentoFormaPagamento == null)
                throw new NegocioException("Forma de pagamento não encontrado.");

            _orcamentoFormaPagamentoRepositorio.Remove(orcamentoFormaPagamento);
            _escopo.Finalizar();

            return ObterOrcamentoPagamentoDto(orcamentoFormaPagamento.IdOrcamento);
        }
*/
/*        public OrcamentoPagamentoDto ObterOrcamentoPagamentoDto(long idOrcamento)
        {
            var orcamentoPagamentoDto = new OrcamentoPagamentoDto();


            var orcamentoFormaPagamentos =
                from ofp in _orcamentoFormaPagamentoRepositorio.Get(x => x.IdOrcamento == idOrcamento).ToList()
                join fp in _condicaoPagamentoRepositorio.GetAll() on ofp.IdCondicaoPagamento equals fp.Id
                select new OrcamentoFormaPagamentoDto { Id = ofp.Id, CondicaoPagamento = fp.Descricao, QtdParcelas = fp.QtdParcelas, ValorTotal = ofp.TotalValorForma, ValorParcela = fp.QtdParcelas > 1 ? ofp.TotalValorForma / fp.QtdParcelas : 0, TemAcrescimo = fp.ValorAcrescimo > 0 };

            orcamentoPagamentoDto.FormasPagamentos = Mapper.Map<List<OrcamentoFormaPagamentoDto>>(orcamentoFormaPagamentos);


            // Condição Pagamento default
            var orcamentoFormaPagamento = _orcamentoFormaPagamentoRepositorio.GetAll().FirstOrDefault();
            if (orcamentoFormaPagamento != null)
                orcamentoPagamentoDto.CondicaoPagamento = orcamentoFormaPagamento.Id + ";" + orcamentoFormaPagamento.Descricao;


            // Somatoria do valor de todas as formas de pagamento.
            var valorParceladoSomado = orcamentoPagamentoDto.FormasPagamentos.Select(x => x.ValorTotal).Sum();
            var valorTotalOrc = decimal.Zero;
            if (_orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento).OrcamentoItens != null)
                valorTotalOrc = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento).OrcamentoItens.Select(x => x.TotalItem)
                    .Sum();

            var valorImpostos = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento).ValorImpostos;

            orcamentoPagamentoDto.ValorRestante = valorTotalOrc + valorImpostos - valorParceladoSomado;

            return orcamentoPagamentoDto;
        }
*/
/*        public int TamanhoFormaPagamentoPorTermo(string termoBusca, long? tipoVenda)
        {
            string tipoVendaOrcamento = string.Empty;
            if (tipoVenda != null)
                tipoVendaOrcamento = _tipoVendaRepositorio.GetSingle(x => x.Id == tipoVenda).CampoCodigo;
            return ObterCondicaoPagamentoQuery(termoBusca, tipoVendaOrcamento).Count();
        }
        public List<FormaPagamentoDto> ObterFormaPagamentoPorNome(string termoBusca, long? tipoVenda, int tamanhoPagina, int numeroPagina)
        {
            string tipoVendaOrcamento = string.Empty;
            if (tipoVenda != null)
                tipoVendaOrcamento = _tipoVendaRepositorio.GetSingle(x => x.Id == tipoVenda).CampoCodigo;
            var formaPagamentoDtos = ObterCondicaoPagamentoQuery(termoBusca, tipoVendaOrcamento)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<FormaPagamentoDto>()
                .ToList();

            foreach (var formaPagamentoDto in formaPagamentoDtos)
            {
                formaPagamentoDto.TipoFormaPagamento = ObterTipoFormaPagamento(formaPagamentoDto.FormaPagamento);
            }

            return formaPagamentoDtos;
        }
*/
/*        private TipoFormaPagamento ObterTipoFormaPagamento(string descricao)
        {

            switch (descricao)
            {
                case "FI":
                case "CD":
                case "CC":
                    return TipoFormaPagamento.Cartao;
                case "DP":
                case "CH":
                case "BOL":
                    return TipoFormaPagamento.Banco;
                default:
                    return TipoFormaPagamento.Dinheiro;
            }
        }
*/
/*        private IQueryable<CondicaoPagamento> ObterCondicaoPagamentoQuery(string termoBusca, string tipoVenda)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();
            return from condicaoPagamento in _condicaoPagamentoRepositorio.GetAll()
                   where (DbFunctions.Like(condicaoPagamento.Descricao, termoBusca) ||
                          DbFunctions.Like(condicaoPagamento.CampoCodigo, termoBusca))
                         && condicaoPagamento.ListaTipoVenda.Contains(tipoVenda)
                         && !condicaoPagamento.RegistroInativo && condicaoPagamento.CondicaoDeVenda
                   orderby condicaoPagamento.CampoCodigo
                   select condicaoPagamento;
        }
*/
        public int TamanhoBancoPorTermo(string termoBusca)
        {
            return ObterBancoQuery(termoBusca).Count();
        }

        public List<BancoDto> ObterBancoPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            return ObterBancoQuery(termoBusca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<BancoDto>()
                .ToList();
        }

        private IQueryable<Banco> ObterBancoQuery(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from banco in _bancoRepositorio.GetAll()
                   where (DbFunctions.Like(banco.Descricao, termoBusca) ||
                          DbFunctions.Like(banco.CampoCodigo, termoBusca))
                         && !banco.RegistroInativo
                   orderby banco.CampoCodigo
                   select banco; ;
        }

/*        public int TamanhoAdmFinanceiraPorTermo(string termoBusca)
        {
            return ObterAdmFinanceiraQuery(termoBusca).Count();
        }
*/
/*        public List<AdministradoraFinanceiraDto> ObterAdmFinanceiraPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            return ObterAdmFinanceiraQuery(termoBusca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<AdministradoraFinanceiraDto>()
                .ToList();
        }
*/
/*        private IQueryable<AdministradoraFinanceira> ObterAdmFinanceiraQuery(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();
            return from administradoraFinanceira in _admFinanceiraRepositorio.GetAll()
                   where (DbFunctions.Like(administradoraFinanceira.Descricao, termoBusca))
                         && !administradoraFinanceira.RegistroInativo
                   orderby administradoraFinanceira.CampoCodigo
                   select administradoraFinanceira; ;
        }
*/
/*        public void InserirEquipeMontagem(EquipeMontagemDto equipeMontagemDto, long idOrcamentoItem)
        {
            // Remover todos e adicionar da dto.
            var orcamentoItemEquipeMontagemList = _orcItemEquipeMontagem.Get(x => x.IdOrcamentoItem == idOrcamentoItem);
            foreach (var orcamentoItemEquipeMontagem in orcamentoItemEquipeMontagemList)
            {
                _orcItemEquipeMontagem.Remove(orcamentoItemEquipeMontagem);
            }

            foreach (var profissionalMontagemDto in equipeMontagemDto.Equipe)
            {
                var vendedor = _vendedorRepositorio.GetSingle(x => x.Id == profissionalMontagemDto.Id);
                if (vendedor == null)
                    continue;
                _orcItemEquipeMontagem.Add(new OrcamentoItemEquipeMontagem
                {
                    IdOrcamentoItem = idOrcamentoItem,
                    IdVendedor = vendedor.Id,
                    Funcao = profissionalMontagemDto.Funcao
                });
            }
            _escopo.Finalizar();
        }
*/
/*        public EquipeMontagemDto ObterEquipeMontagemDto(long idOrcamentoItem)
        {
            var orcItem = _orcamentoItemRepositorio.GetSingle(x => x.Id == idOrcamentoItem);

            EquipeMontagemDto equipeMontagemDto = new EquipeMontagemDto();
            equipeMontagemDto.DescricaoProduto = orcItem.Produto.Descricao;
            equipeMontagemDto.IdLojaOrcamento = orcItem.Orcamento.LojaDellaVia.Id;

            List<OrcamentoItemEquipeMontagem> equipe = _equipeMontagemRepositorio.Get(x => x.IdOrcamentoItem == idOrcamentoItem).ToList();
            if (equipe.Count == 0)
                equipe.Add(new OrcamentoItemEquipeMontagem());
            equipeMontagemDto.Equipe = Mapper.Map<List<ProfissionalMontagemDto>>(equipe);

            return equipeMontagemDto;
        }
*/
        public ParcelamentoDto ObterParcelamento(long idOrcamentoFormaPagamento)
        {
            var parcelamentoDto = new ParcelamentoDto();
            var formaPagamento = _orcamentoFormaPagamentoRepositorio.GetSingle(x => x.Id == idOrcamentoFormaPagamento);

            if (formaPagamento == null)
                throw new NegocioException("Forma de pagamento não encontrada no banco de dados.");

            string erros;
            var parcelas = _condicaoPagamentoParcelasApi.ObterParcelas(formaPagamento.CondicaoPagamento.IdFormaPagamento.ToString(),
                formaPagamento.TotalValorForma, out erros);

            if (!erros.IsNullOrEmpty())
                throw new NegocioException($"Erro(s) ao buscar condições de parcelamento: {erros}");

            parcelamentoDto.NomeCondicaoPagamento = formaPagamento.Descricao;
            parcelamentoDto.Parcelas = parcelas;
            //TODO
            parcelamentoDto.ValorAcrescimo = 0; //formaPagamento.CondicaoPagamento.ValorAcrescimo;

            return parcelamentoDto;
        }

/*        public void AplicarDescontoItensOrcamento(AplicarDescontoDto aplicarDescontoDto)
        {
            var idOrcamentoItem = aplicarDescontoDto.IdOrcamentoItem;
            var orcamentoItem = _orcamentoItemRepositorio.GetSingle(x => x.Id == idOrcamentoItem);

            var orcamento = orcamentoItem.Orcamento;

            // Validação do modelo de venda.
            // Se tiver sido selecionado um modelo de venda com quantidade maior que a quantidade original, então devemos atualizar a quantidade no orcamento item.
            if (aplicarDescontoDto.DescontoModeloVendaUtilizado != null)
            {
                var descontoModeloVenda = _descontoModeloVendaRepositorio.GetSingle(x => x.Id == aplicarDescontoDto.IdDescontoModeloVenda);

                var descontoModeloVendaArray = new[]
                {
                    descontoModeloVenda.PercentualDesconto1,
                    descontoModeloVenda.PercentualDesconto2,
                    descontoModeloVenda.PercentualDesconto3,
                    descontoModeloVenda.PercentualDesconto4,
                };

                if (aplicarDescontoDto.DescontoModeloVendaUtilizado.GetHashCode() > orcamentoItem.Quantidade)
                    orcamentoItem.Quantidade = aplicarDescontoDto.DescontoModeloVendaUtilizado.GetHashCode();


                orcamentoItem.PercDescon = descontoModeloVendaArray[aplicarDescontoDto.DescontoModeloVendaUtilizado.GetHashCode() - 1];

                var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), orcamentoItem.PercDescon)[1];
                orcamentoItem.ValorDesconto = valorDesconto;
                orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                orcamentoItem.DescontoModeloVendaUtilizado = aplicarDescontoDto.DescontoModeloVendaUtilizado;
                orcamentoItem.IdDescontoModeloVenda = aplicarDescontoDto.IdDescontoModeloVenda;
                // Double check.
                //if (orcamentoItem.TotalItem != aplicarDescontoDto.ValorTotalComDesconto)
                //    throw new NegocioException("Os valores informados não correspondem com o desconto possível para esse item.");

            }
            //Modelo Venda é exceção, não valida limites de desconto
            else
            {
                orcamentoItem.DescontoModeloVendaUtilizado = null;
                orcamentoItem.DescontoModeloVenda = null;

                // Validação do desconto do Operador (TMK).
                if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK // Perfil telemarketing.
                    && aplicarDescontoDto.DescontoModeloVendaUtilizado == null) // Sem Modelo de desconto selecionado.
                {
                    var codOperador = HttpContext.Current.User.Identity.GetIdOperador();
                    var operador = _operadoRepositorio.GetSingle(x => x.CampoCodigo == codOperador);

                    if (operador.PercLimiteDesconto < aplicarDescontoDto.PercentualDesconto) // Validar o percentual de desconto é maior que PercLimiteDesconto.
                        throw new NegocioException($"Percentual de desconto acima do permitido {operador.PercLimiteDesconto}% !");

                    var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
                    orcamentoItem.ValorDesconto = valorDesconto;
                    orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                    // Double check no valor calculado com o valor informado.
                    //if (orcamentoItem.TotalItem != aplicarDescontoDto.ValorTotalComDesconto)
                    //    throw new NegocioException("Os valores informados não correspondem com o desconto possível para esse item.");

                    orcamentoItem.ValorDesconto = valorDesconto;
                    orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
                    orcamentoItem.DescontoModeloVendaUtilizado = null;


                }
                // Valido desconto venda alçada
                else
                {

                    var loja = _dellaViaServico.ObterLojaUsuarioLogado();
                    var descontoVendaAlcada = _descontoVendaAlcadaRepositorio.GetSingle(
                        x => x.LojaDellaviaId == loja.Id && x.AreaNegocioId == orcamento.IdAreaNegocio);

                    if (descontoVendaAlcada != null)
                    {
                        var idGrupoProduto = orcamentoItem.Produto.GrupoProduto.Id;

                        var descontoVendaAlcadaGrupo = _descontoVendaAlcadaGrupoProdutoRepositorio.GetSingle(
                            x => x.GrupoProduto.Id == idGrupoProduto
                                 && x.AreaNegocioId == orcamento.IdAreaNegocio && x.LojaDellaviaId == loja.Id);

                        if (descontoVendaAlcadaGrupo == null)
                        {

                            // Validar o percentual de desconto é menor que PercLimiteDesconto do vendedor.
                            if (aplicarDescontoDto.PercentualDesconto <= descontoVendaAlcada.PercentualDescontoVendedor)
                            {

                                var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
                                orcamentoItem.ValorDesconto = valorDesconto;
                                orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
                                orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                                // Double check no valor calculado com o valor informado.
                                //if (orcamentoItem.TotalItem != aplicarDescontoDto.ValorTotalComDesconto)
                                //    throw new NegocioException("Os valores informados não correspondem com o desconto possível para esse item.");
                            }
                            // Caso contrario validar se o desconto informado é menor que o limite de desconto do gerente.
                            else if (aplicarDescontoDto.PercentualDesconto < descontoVendaAlcada.PercentualDescontoGerente)
                            {
                                var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
                                orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
                                orcamentoItem.ValorDesconto = valorDesconto;
                                orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                                // Double check no valor calculado com o valor informado.
                                //if (orcamentoItem.TotalItem != aplicarDescontoDto.ValorTotalComDesconto)
                                //    throw new NegocioException("Os valores informados não correspondem com o desconto possível para esse item.");

                                if (aplicarDescontoDto.ObservacaoItem.IsNullOrEmpty())
                                    throw new NegocioException("O campo \'Observação Item\' é obrigatório.");

                                if (aplicarDescontoDto.ObservacaoGeral.IsNullOrEmpty())
                                    throw new NegocioException("O campo \'Observacao Geral\' é obrigatório.");

                                var codOperador = HttpContext.Current.User.Identity.GetIdOperador();
                                var operador = _operadoRepositorio.GetSingle(x => x.CampoCodigo == codOperador);

                                // Criar solicitação de venda alçada.
                                var solicitacaoDescontoVendaAlcada = new SolicitacaoDescontoVendaAlcada
                                {
                                    DataAtualizacao = DateTime.Now,
                                    DataSolicitacao = DateTime.Now,
                                    PercentualDesconto = orcamentoItem.PercDescon,
                                    IdOrcamentoItem = orcamentoItem.Id,
                                    ObservacaoGeral = aplicarDescontoDto.ObservacaoGeral,
                                    ObservacaoItem = aplicarDescontoDto.ObservacaoItem,
                                    StatusSolicitacaoAlcada = StatusSolicitacao.NaoEnviado,
                                    UsuarioAtualizacao = operador.NomeOperador,
                                    ValorDesconto = valorDesconto
                                };

                                _solicitacaoDescontoVendaAlcadaRepositorio.Add(solicitacaoDescontoVendaAlcada);
                            }
                            // Desconto excede limite do desconto do gerente.
                            else
                            {
                                throw new NegocioException("O desconto de venda alçada excede o limite máximo de desconto do gerente.");
                            }

                        }
                        // A mesma coisa do de cima para Desconto Venda Alçada *Grupo*
                        else
                        {
                            aplicarDescontoDto.PercentualDescontoAlcadaGerente = descontoVendaAlcadaGrupo.PercentualDescontoGerente;
                            aplicarDescontoDto.PercentualLimiteDesconto = descontoVendaAlcadaGrupo.PercentualDescontoVendedor;

                            // Validar o percentual de desconto é menor que PercLimiteDesconto do vendedor.
                            if (aplicarDescontoDto.PercentualDesconto <= descontoVendaAlcadaGrupo.PercentualDescontoVendedor)
                            {
                                var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
                                orcamentoItem.ValorDesconto = valorDesconto;
                                orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
                                orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                                // Double check no valor calculado com o valor informado.
                                //if (orcamentoItem.TotalItem != aplicarDescontoDto.ValorTotalComDesconto)
                                //    throw new NegocioException("Os valores informados não correspondem com o desconto possível para esse item.");
                            }
                            // Caso contrario validar se o desconto informado é menor que o limite de desconto do gerente.
                            else if (aplicarDescontoDto.PercentualDesconto < descontoVendaAlcadaGrupo.PercentualDescontoGerente)
                            {

                                var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
                                orcamentoItem.ValorDesconto = valorDesconto;
                                orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
                                orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                                // Double check no valor calculado com o valor informado.
                                //if (orcamentoItem.TotalItem != aplicarDescontoDto.ValorTotalComDesconto)
                                //    throw new NegocioException("Os valores informados não correspondem com o desconto possível para esse item.");

                                if (aplicarDescontoDto.ObservacaoItem.IsNullOrEmpty())
                                    throw new NegocioException("O campo \'Observação Item\' é obrigatório.");

                                if (aplicarDescontoDto.ObservacaoGeral.IsNullOrEmpty())
                                    throw new NegocioException("O campo \'Observacao Geral\' é obrigatório.");

                                var codOperador = HttpContext.Current.User.Identity.GetIdOperador();
                                var operador = _operadoRepositorio.GetSingle(x => x.CampoCodigo == codOperador);

                                // Criar solicitação de venda alçada.
                                var solicitacaoDescontoVendaAlcada = new SolicitacaoDescontoVendaAlcada
                                {
                                    DataAtualizacao = DateTime.Now,
                                    DataSolicitacao = DateTime.Now,
                                    PercentualDesconto = orcamentoItem.PercDescon,
                                    IdOrcamentoItem = orcamentoItem.Id,
                                    ObservacaoGeral = aplicarDescontoDto.ObservacaoGeral,
                                    ObservacaoItem = aplicarDescontoDto.ObservacaoItem,
                                    StatusSolicitacaoAlcada = StatusSolicitacao.NaoEnviado,
                                    UsuarioAtualizacao = operador.NomeOperador,
                                    ValorDesconto = valorDesconto
                                };

                                _solicitacaoDescontoVendaAlcadaRepositorio.Add(solicitacaoDescontoVendaAlcada);
                            }
                            // Desconto excede limite do desconto do gerente.
                            else
                            {
                                throw new NegocioException("O desconto de venda alçada excede o limite máximo de desconto do gerente.");
                            }
                        }
                    }
                    else
                    {
                        orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;

                        var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
                        orcamentoItem.ValorDesconto = valorDesconto;
                        orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
                        orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;

                        orcamentoItem.DescontoModeloVendaUtilizado = null;
                    }
                }

            }
            RemoverPagamentos(orcamento.Id);
            _impostosServico.CalcularImpostos(orcamento);

            _escopo.Finalizar();
        }
*/
        /// <summary>
        /// Calcula o desconto com base nos parametros informados.
        /// </summary>
        /// <param name="total">Valor do item</param>
        /// <param name="descontoPercentual">Desconto percentual</param>
        /// <param name="descontoValor">Valor do desconto.</param>
        /// <returns>Array de decimals, primeiro elemento é o valor do desconto o segundo representa o total calculado ja com o desconto.</returns>
        public static decimal[] CalculaDesconto(decimal total, decimal? descontoPercentual = null, decimal? descontoValor = null)
        {
            if (!(descontoPercentual == null ^ descontoValor == null))
                throw new NegocioException("Deve ser informado um e apenas um tipo de desconto.");

            decimal valorDesconto;
            decimal totalComDesconto;

            if (descontoPercentual != null)
            {
                valorDesconto = (decimal)(descontoPercentual / 100 * total);
                totalComDesconto = total - valorDesconto;
            }
            else
            {
                valorDesconto = (decimal)(descontoValor / total * 100);
                totalComDesconto = (decimal)(total - descontoValor);
            }


            return new[] { Decimal.Round(totalComDesconto, 2), Decimal.Round(valorDesconto, 2) };
        }



        public void RemoverPagamentos(long orcamentoId)
        {
            var orc = _orcamentoRepositorio.FindByKey(orcamentoId);

            foreach (var pagamento in orc.FormaPagamentos.ToList())
            {
                _orcamentoFormaPagamentoRepositorio.Remove(pagamento);
            }
            //_escopo.Finalizar();
        }

    }
}