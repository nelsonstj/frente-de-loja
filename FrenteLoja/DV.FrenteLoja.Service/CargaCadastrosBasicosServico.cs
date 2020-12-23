using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using Timer = System.Timers.Timer;
using System.Configuration;

namespace DV.FrenteLoja.Service
{
    public partial class CargaCadastrosBasicosServico : ServiceBase
    {
        private Timer _timer;
        private ICargaCadastrosBasicosService _cargaCadastrosBasicosService;
        private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
        private ICargaConvenioServico _cargaConvenioServico;
        private ICargaPrecoServico _cargaPrecoServico;
        private ICargaProdutoServico _cargaProdutoServico;
        private ICargaClienteServico _clienteServico;
        private ICargaDescontosServico _cargaDescontosServico;
        private ICargaOrcamentoServico _cargaOrcamentoServico;
        private LogIntegracao _logIntegracao;
        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

        public CargaCadastrosBasicosServico()
        {
            InitializeComponent();

            _logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
        }

        protected override void OnStart(string[] args)
        {
            int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoCadastrosBasicos"]);
            _timer = new Timer();
            _timer.Interval = TimeSpan.FromMinutes(intervalo).TotalMilliseconds;
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
            Timer_Elapsed(null, null);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {


                _cargaCadastrosBasicosService = BaseConfig.Container.GetInstance<ICargaCadastrosBasicosService>();

                #region Marcas

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PA0,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncMarcas().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region Modelo
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PA1,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncModelos().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region Banco
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SA6,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncBancos().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();

                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region Tipo de Vendas
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PAG,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncTipoVenda().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();

                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region  Vendedores
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SA3,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncVendedores().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {

                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region Operador
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SU7,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncOperador().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region Transportadoras
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SA4,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncTransportadoras().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region LojaDellaVia
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SLJ,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    //_cargaCadastrosBasicosService.SyncLojasDellaVia().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region ParametroGeral
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SX6,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncParametroGeral().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region AdministradoraFinanceira
                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SAE,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaCadastrosBasicosService.SyncAdministracaoFinanceira().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                //Novos temporario

                _cargaProdutoServico = BaseConfig.Container.GetInstance<ICargaProdutoServico>();

                #region Grupo Produto

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SBM,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaProdutoServico.SyncGrupoProduto().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region Produto

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SB1,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                   _cargaProdutoServico.SyncProduto().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region Grupo Serviço Agregados Produtos

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PA3,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaProdutoServico.SyncGrupoServicoAgregadosProdutos();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region Produto Complemento

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SB5,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaProdutoServico.SyncProdutoComplemento().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                _clienteServico = BaseConfig.Container.GetInstance<ICargaClienteServico>();

                #region Cliente

                try
                {
                    _logIntegracao = new LogIntegracao
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SA1,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _clienteServico.SyncCliente().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region Cliente Veiculo

                try
                {
                    _logIntegracao = new LogIntegracao
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PA7,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _clienteServico.SyncClienteVeiculo().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                { }

                #endregion

                _cargaPrecoServico = BaseConfig.Container.GetInstance<ICargaPrecoServico>();

                #region Tabela Preco

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.DA0,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaPrecoServico.SyncTabelaPreco().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region Tabela Preco Item

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.DA1,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaPrecoServico.SyncTabelaPrecoItem().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion
                _cargaConvenioServico = BaseConfig.Container.GetInstance<ICargaConvenioServico>();

                #region Convenio

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PA6,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaConvenioServico.SyncConvenio().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region CondicaoPagamento

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SE4,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaConvenioServico.SyncCondicaoPagamento().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region ConvenioCondicaoPagamento

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PBQ,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaConvenioServico.SyncConvenioCondicaoPagamento().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                #region ConvenioCliente

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PBO,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaConvenioServico.SyncConvenioCliente().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region ConvenioProduto

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PBP,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaConvenioServico.SyncConvenioProduto().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                _cargaDescontosServico = BaseConfig.Container.GetInstance<ICargaDescontosServico>();

                #region DescontoVendaAlcada

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PAN,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaDescontosServico.SyncDescontoVendaAlcada().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region DescontoVendaAlcadaGrupoProduto

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.PAO,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaDescontosServico.SyncDescontoVendaAlcadaGrupoProduto().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region DescontoModeloDeVenda

                try
                {
                    _logIntegracao = new LogIntegracao
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.ZAL,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaDescontosServico.SyncDescontoModeloDeVenda().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion

                _cargaOrcamentoServico = BaseConfig.Container.GetInstance<ICargaOrcamentoServico>();

                #region Orcamentos

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.SL1,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaOrcamentoServico.SyncOrcamento().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {

                }

                #endregion

                #region Solicitacao Analise de Credito

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        TipoTabelaProtheus = TipoTabelaProtheus.MAH,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaOrcamentoServico.SyncSolicitacaoAnaliseCredito().Wait();

                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();
                }
                finally
                {
                }

                #endregion
                _logIntegracaoRepositorio.Add(new LogIntegracao
                {
                    DataAtualizacao = DateTime.Now,
                    TipoTabelaProtheus = TipoTabelaProtheus.XXX,
                    UsuarioAtualizacao = UsuarioAtualizacaoServico,
                    StatusIntegracao = StatusIntegracao.Sucesso,
                    LogErro = "Finalizado com Sucesso"
                });
                BaseConfig.RepositorioEscopo.Finalizar();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            finally
            {
                BaseConfig.RepositorioEscopo.Finalizar();
                Debug.WriteLine("# Importação finalizada.");
            }

        }

        protected override void OnStop()
        {
            _logIntegracao = new LogIntegracao
            {
                DataAtualizacao = DateTime.Now,
                UsuarioAtualizacao = UsuarioAtualizacaoServico,
                StatusIntegracao = StatusIntegracao.Erro,
                LogErro = $"Serviço {nameof(CargaCadastrosBasicosServico)} foi interrompido."
            };

            _logIntegracaoRepositorio.Add(_logIntegracao);
            BaseConfig.RepositorioEscopo.Finalizar();
        }
    }
}
