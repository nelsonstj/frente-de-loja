using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Service
{
	partial class CargaOrcamentoService : ServiceBase
	{
		private Timer _timer;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";
		private ICargaOrcamentoServico _cargaOrcamentoServico;

		public CargaOrcamentoService()
		{
			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			var intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoOrcamentoServico"]);
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();

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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
				}

				#endregion
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
				LogErro = $"Serviço {nameof(CargaProdutoServico)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
		}
	}
}
