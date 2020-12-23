using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace DV.FrenteLoja.Service
{
	partial class CargaVeiculosProdutoServicos : ServiceBase
	{
		private Timer _timer;
		private ICargaVeiculoProdutosServico _cargaVeiculosProdutoServico;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

		public CargaVeiculosProdutoServicos()
		{
			InitializeComponent();
			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
		}

		protected override void OnStart(string[] args)
		{

			int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoVeiculosProdutoServicos"]);
			_timer = new Timer();
			_timer.Interval = TimeSpan.FromMinutes(intervalo).TotalMilliseconds;
			_timer.AutoReset = true;
			_timer.Elapsed += Timer_Elapsed;
			_timer.Enabled = true;
			_timer.Start();
			Timer_Elapsed(null, null);
		}

		public void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
            try
            {
                _cargaVeiculosProdutoServico = BaseConfig.Container.GetInstance<ICargaVeiculoProdutosServico>();

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaVeiculosProdutoServico.SyncProdutos().Wait();
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.LogErro = exception.ToString();
                }
                finally
                {
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                    BaseConfig.RepositorioEscopo.Finalizar();

                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
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
				LogErro = $"Serviço {nameof(CargaVeiculosProdutoServicos)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
		}
	}
}
