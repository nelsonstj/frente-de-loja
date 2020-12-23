using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using Timer = System.Timers.Timer;

namespace DV.FrenteLoja.Service
{
	partial class CargaVeiculoServico : ServiceBase
	{
		private Timer _timer;
		private ICargaVeiculoServico _cargaVeiculoServico;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

		public CargaVeiculoServico()
		{
			InitializeComponent();
			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
		}

		protected override void OnStart(string[] args)
		{

			int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoVeiculoServico"]);
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
                _cargaVeiculoServico = BaseConfig.Container.GetInstance<ICargaVeiculoServico>();

                try
                {
                    _logIntegracao = new LogIntegracao()
                    {
                        DataAtualizacao = DateTime.Now,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico
                    };

                    _cargaVeiculoServico.SyncVeiculo().Wait();
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
				LogErro = $"Serviço {nameof(CargaVeiculoServico)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
		}
	}
}
