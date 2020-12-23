using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Service
{
	partial class CargaClienteServico : ServiceBase
	{
		private Timer _timer;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private ICargaClienteServico _clienteServico;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

		public CargaClienteServico()
		{
			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoClienteServico"]);
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
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
				LogErro = $"Serviço {nameof(CargaPrecoServico)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
			BaseConfig.RepositorioEscopo.Finalizar();
		}
	}
}
