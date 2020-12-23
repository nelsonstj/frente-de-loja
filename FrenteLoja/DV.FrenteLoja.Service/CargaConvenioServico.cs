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
	partial class CargaConvenioServico : ServiceBase
	{
		private Timer _timer;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private ICargaConvenioServico _cargaConvenioServico;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

		public CargaConvenioServico()
		{
			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoConvenioServico"]);
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();

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
				Debug.WriteLine(ex);
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
				LogErro = $"Serviço {nameof(CargaConvenioServico)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
			BaseConfig.RepositorioEscopo.Finalizar();
		}
	}
}
