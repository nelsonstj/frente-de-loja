using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using Timer = System.Timers.Timer;

namespace DV.FrenteLoja.Service
{
	partial class CargaProdutoServico : ServiceBase
	{
		private Timer _timer;
		private ICargaProdutoServico _cargaProdutoServico;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
		private LogIntegracao _logIntegracao;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

		public CargaProdutoServico()
		{
			InitializeComponent();
			_logIntegracaoRepositorio = BaseConfig.RepositorioEscopo.GetRepositorio<LogIntegracao>();
		}

		protected override void OnStart(string[] args)
		{

			int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoServicoProdutoServico"]);
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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
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
                }
                finally
                {
                    _logIntegracaoRepositorio.Add(_logIntegracao);
	                BaseConfig.RepositorioEscopo.Finalizar();

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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();

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
				}
				finally
				{
					_logIntegracaoRepositorio.Add(_logIntegracao);
					BaseConfig.RepositorioEscopo.Finalizar();
				}

				#endregion
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
				LogErro = $"Serviço {nameof(CargaProdutoServico)} foi interrompido."
			};

			_logIntegracaoRepositorio.Add(_logIntegracao);
		}
	}
}
