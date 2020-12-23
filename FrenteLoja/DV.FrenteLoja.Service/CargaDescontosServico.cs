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
	partial class CargaDescontosServico : ServiceBase
	{
		private Timer _timer;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private ICargaDescontosServico _cargaDescontosServico;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";
		public CargaDescontosServico()
		{
			InitializeComponent();

			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
		}

		protected override void OnStart(string[] args)
		{
			int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoDescontosServico"]);
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();

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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();

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
				LogErro = $"Serviço {nameof(CargaDescontosServico)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
		}
	}
}
