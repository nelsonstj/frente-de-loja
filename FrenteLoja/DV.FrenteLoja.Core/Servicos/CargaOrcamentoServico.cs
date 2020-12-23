using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaOrcamentoServico : ICargaOrcamentoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IOrcamentoApi _orcamentoApi;
        private readonly ISolicitacaoAnaliseCreditoApi _analiseCreditoApi;
        private readonly IRepositorio<LojaDellaVia> _lojaDellaViaRepositorio;
        //private readonly IRepositorio<Convenio> _convenioRepositorio;
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        private readonly IRepositorio<TabelaPreco> _tabelaPrecoRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<Banco> _bancoRepositorio;
        private readonly IRepositorio<Transportadora> _transportadoraRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<SolicitacaoAnaliseCredito> _solicitacaoAnaliseCreditoRepositorio;
        private readonly IRepositorio<DescontoModeloVenda> _descontoModeloVendaRepositorio;
        private readonly IRepositorio<OrcamentoItem> _orcamentoItemRepositorio;
        private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

        public CargaOrcamentoServico(IRepositorioEscopo escopo, IOrcamentoApi orcamentoApi, ISolicitacaoAnaliseCreditoApi analiseCreditoApi)
        {
            _escopo = escopo;
            _orcamentoApi = orcamentoApi;
            _analiseCreditoApi = analiseCreditoApi;
            _lojaDellaViaRepositorio = escopo.GetRepositorio<LojaDellaVia>();
            //_convenioRepositorio = escopo.GetRepositorio<Convenio>();
            _clienteRepositorio = escopo.GetRepositorio<Cliente>();
            _tabelaPrecoRepositorio = escopo.GetRepositorio<TabelaPreco>();
            _vendedorRepositorio = escopo.GetRepositorio<Vendedor>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _orcamentoRepositorio = escopo.GetRepositorio<Orcamento>();
            _bancoRepositorio = escopo.GetRepositorio<Banco>();
            _transportadoraRepositorio = escopo.GetRepositorio<Transportadora>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _solicitacaoAnaliseCreditoRepositorio = escopo.GetRepositorio<SolicitacaoAnaliseCredito>();
            _descontoModeloVendaRepositorio = escopo.GetRepositorio<DescontoModeloVenda>();
            _orcamentoItemRepositorio = escopo.GetRepositorio<OrcamentoItem>();
            _logIntegracaoRepositorio = escopo.GetRepositorio<LogIntegracao>();
        }

        public async Task SyncOrcamento(bool isFirstLoad = false)
        {

            var orcamentosProtheus = await _orcamentoApi.ObterOrcamentos();

            foreach (OrcamentoProtheusDto orcDto in orcamentosProtheus)
            {
                try
                {
                    string retorno = string.Empty;
                    if (orcDto.Numero.IsNullOrEmpty() || orcDto.CodDellaVia.IsNullOrEmpty())
                        retorno = $"Nr Protheus {orcDto.Numero} e/ou Filial {orcDto.CodDellaVia} estão nulos";
                    else
                        retorno = _orcamentoApi.AtualizarOrcamento(orcDto);
                    if (!retorno.IsNullOrEmpty())
                    {
                        _logIntegracaoRepositorio.Add(new LogIntegracao
                        {
                            DataAtualizacao = DateTime.Now,
                            UsuarioAtualizacao = UsuarioAtualizacaoServico,
                            StatusIntegracao = StatusIntegracao.Erro,
                            Log = $"Erro na atualização dos orçamentos com origem portal: Nr Protheus {orcDto.Numero} - {orcDto.CodDellaVia} Erro: {retorno}",
                            TipoTabelaProtheus = TipoTabelaProtheus.SL1
                        });
                    }

                }
                catch (Exception e)
                {

                    throw e;
                }
            }

        }

        public async Task SyncSolicitacaoAnaliseCredito(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var solicitacaoAnaliseCreditoList = _solicitacaoAnaliseCreditoRepositorio.Get(x => x.StatusSolicitacaoAnaliseCredito == StatusSolicitacao.PendenteRetorno);

            List<SolicitacaoAnaliseCreditoRetornoProtheus> retornoProtheus;

            try
            {

                retornoProtheus = await _analiseCreditoApi.PostConsultaAnaliseCredito(solicitacaoAnaliseCreditoList.ToList());
            }
            catch (Exception e)
            {
                errosBuilder.AppendLine(e.ToString());
                return;
            }

            foreach (var item in retornoProtheus)
            {
                if (item.Contrato.IsNullOrEmpty())
                    continue;

                var solicitacaoAnaliseCredito = _solicitacaoAnaliseCreditoRepositorio.GetSingle(x => x.NumeroContrato == item.Contrato);

                if (solicitacaoAnaliseCredito != null && item.StatusSolicitacaoAnaliseCredito != 2)
                {
                    solicitacaoAnaliseCredito.StatusSolicitacaoAnaliseCredito = StatusSolicitacao.Retornado;
                    solicitacaoAnaliseCredito.DataResposta = DateTime.Now;

                    switch (item.StatusSolicitacaoAnaliseCredito)
                    {
                        case 1:
                            solicitacaoAnaliseCredito.SituacaoAnaliseCredito = SituacaoAnaliseCredito.OK;
                            break;
                        case 3:
                            solicitacaoAnaliseCredito.SituacaoAnaliseCredito = SituacaoAnaliseCredito.Aprovado;
                            break;
                        case 4:
                            solicitacaoAnaliseCredito.SituacaoAnaliseCredito = SituacaoAnaliseCredito.Rejeitado;
                            break;
                        case 5:
                            solicitacaoAnaliseCredito.SituacaoAnaliseCredito = SituacaoAnaliseCredito.Crediario;
                            break;
                        case 6:
                            solicitacaoAnaliseCredito.SituacaoAnaliseCredito = SituacaoAnaliseCredito.Cancelado;
                            break;
                        default:
                            break;
                    }
                }
            }

            _escopo.Finalizar();
        }
    }
}