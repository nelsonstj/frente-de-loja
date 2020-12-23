using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using Newtonsoft.Json.Linq;
using DV.FrenteLoja.Core.Util;

namespace DV.FrenteLoja.Core.Servicos
{
	public class CargaCadastrosBasicosService : ICargaCadastrosBasicosService
	{
		private readonly IRepositorioEscopo _escopo;
		private readonly ICargaCadastrosProtheusSyncApi _protheusSyncApi;
		private readonly IRepositorio<Vendedor> _vendedorRepositorio;
		private readonly IRepositorio<TipoVenda> _tipoVendaRepositorio;
		private readonly IRepositorio<Marca> _marcaRepositorio;
		private readonly IRepositorio<GrupoProduto> _grupoProdutoRepositorio;
		private readonly IRepositorio<Banco> _bancoRepositorio;
		private readonly IRepositorio<LojaDellaVia> _lojaDellaViaRepositorio;
		private readonly IRepositorio<Transportadora> _transportadoraRepositorio;
		private readonly IRepositorio<MarcaModelo> _modeloRepositorio;
		private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
		//private readonly IRepositorio<Convenio> _convenioRepositorio;
		private readonly IRepositorio<Operador> _operadorRepositorio;
		private readonly IRepositorio<Cliente> _clienteRepositorio;
		private readonly IRepositorio<ParametroGeral> _parametroGeralRepositorio;
		private readonly IRepositorio<AdministradoraFinanceira> _adminstradoraFinanceiraRepositorio;
		private IRepositorio<CatalogoProdutosCorrelacionados> _cpcorrelacionadoRepositorio;

		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";
		public CargaCadastrosBasicosService(IRepositorioEscopo escopo, ICargaCadastrosProtheusSyncApi protheusSyncApi)
		{
			_escopo = escopo;
			_protheusSyncApi = protheusSyncApi;
			_vendedorRepositorio = escopo.GetRepositorio<Vendedor>();
			_tipoVendaRepositorio = escopo.GetRepositorio<TipoVenda>();
			_marcaRepositorio = escopo.GetRepositorio<Marca>();
			_grupoProdutoRepositorio = escopo.GetRepositorio<GrupoProduto>();
			_bancoRepositorio = escopo.GetRepositorio<Banco>();
			_lojaDellaViaRepositorio = escopo.GetRepositorio<LojaDellaVia>();
			_transportadoraRepositorio = escopo.GetRepositorio<Transportadora>();
			_modeloRepositorio = escopo.GetRepositorio<MarcaModelo>();
			//_convenioRepositorio = escopo.GetRepositorio<Convenio>();
			_operadorRepositorio = escopo.GetRepositorio<Operador>();
			_clienteRepositorio = _escopo.GetRepositorio<Cliente>();
			_parametroGeralRepositorio = _escopo.GetRepositorio<ParametroGeral>();
			_adminstradoraFinanceiraRepositorio = _escopo.GetRepositorio<AdministradoraFinanceira>();
			_marcaModeloVersaoRepositorio = _escopo.GetRepositorio<MarcaModeloVersao>();
			_cpcorrelacionadoRepositorio = _escopo.GetRepositorio<CatalogoProdutosCorrelacionados>();

		}

		public async Task SyncTipoVenda(bool isFirstLoad = false)
		{
			StringBuilder errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cadastro, TipoTabelaProtheus.PAG);
			var tipoVendaList = ConvertJsonToObject<TipoVenda>(jArray);
			TipoVenda v = null;
			foreach (var tv in tipoVendaList)
			{
				try
				{
					v = isFirstLoad ? null : _tipoVendaRepositorio.GetSingle(x => x.CampoCodigo == tv.CampoCodigo);

					if (v != null)
					{
						v.Descricao = tv.Descricao;
						v.CampoCodigo = tv.CampoCodigo;
						v.RegistroInativo = tv.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						tv.CampoCodigo = tv.CampoCodigo;
						tv.DataAtualizacao = DateTime.Now;
						tv.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _tipoVendaRepositorio.Add(tv);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir TipoVenda na base de dados. Erro {e}.");
                        }
                    }
                }
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncTipoVenda)} : {e}. Item: {v.ToStringLog()}");
				}
            }
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de TipoVendas: {errosBuilder}.");
		}

		public async Task SyncMarcas(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cadastro, TipoTabelaProtheus.PA0);
			var marcas = ConvertJsonToObject<Marca>(jArray);
			foreach (var marca in marcas)
			{

				try
				{
					var marcaEntidade = isFirstLoad ? null : _marcaRepositorio.GetSingle(x => x.CampoCodigo == marca.CampoCodigo);
					if (marcaEntidade != null)
					{
						marcaEntidade.Descricao = marca.Descricao;
						marcaEntidade.RegistroInativo = marca.RegistroInativo;	
						marcaEntidade.DataAtualizacao = DateTime.Now;
						marcaEntidade.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						marca.DataAtualizacao = DateTime.Now;
						marca.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _marcaRepositorio.Add(marca);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Marcas na base de dados. Erro {e}.");
                        }
                    }
                }
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncMarcas)} : {e}.");
				}
            }
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Marcas: {errosBuilder}.");
		}

		public async Task SyncVendedores(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SA3);
			do
			{
				var vendedores = new List<Vendedor>();

				foreach (var jObj in jArray)
				{
					try
					{
						var vendedor = new Vendedor
						{
							CampoCodigo = jObj["CampoCodigo"].ToString(),
							Nome = jObj["CampoDescricao"].ToString(),
							FilialOrigem = jObj["FilialOrigem"].ToString(),
							RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString())
						};
						vendedores.Add(vendedor);
					}
					catch (Exception e)
					{
						errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncVendedores)} : {e}.");
					}
				}

				foreach (var vendedor in vendedores)
				{
					var v = isFirstLoad ? null : _vendedorRepositorio.GetSingle(x => x.CampoCodigo == vendedor.CampoCodigo);

					if (v != null)
					{
						v.Nome = vendedor.Nome;
						v.RegistroInativo = vendedor.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.FilialOrigem = vendedor.FilialOrigem;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						vendedor.DataAtualizacao = DateTime.Now;
						vendedor.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _vendedorRepositorio.Add(vendedor);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Vendedores na base de dados. Erro {e}.");
                        }
                    }
				}
				jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SA3);

			} while (jArray.Count > 0);

            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Vendedor: {errosBuilder}.");
		}

		public async Task SyncTransportadoras(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cadastro, TipoTabelaProtheus.SA4);
			var transportadoras = ConvertJsonToObject<Transportadora>(jArray);
			Transportadora v = null;
			foreach (var transportadora in transportadoras)
			{
				try
				{

					v = _transportadoraRepositorio.GetSingle(x => x.CampoCodigo == transportadora.CampoCodigo);

					if (v != null)
					{
						v.Descricao = transportadora.Descricao;
						v.RegistroInativo = transportadora.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						transportadora.DataAtualizacao = DateTime.Now;
						transportadora.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _transportadoraRepositorio.Add(transportadora);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Transportadoras na base de dados. Erro {e}.");
                        }
                    }
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncTransportadoras)} : {e}. Item: {v.ToStringLog()}");
				}
			}
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Transportadoras: {errosBuilder}.");
		}

		public async Task SyncBancos(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cadastro, TipoTabelaProtheus.SA6);
			do
			{

				var bancos = ConvertJsonToObject<Banco>(jArray);
				Banco v = null;
				foreach (var banco in bancos)
				{
					try
					{
						v = isFirstLoad ? null : _bancoRepositorio.GetSingle(x => x.CampoCodigo == banco.CampoCodigo);

						if (v != null)
						{
							v.Descricao = banco.Descricao;
							v.RegistroInativo = banco.RegistroInativo;
							v.DataAtualizacao = DateTime.Now;
							v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
						}
						else
						{
							banco.DataAtualizacao = DateTime.Now;
							banco.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                            try
                            {
                                _bancoRepositorio.Add(banco);
                            }
                            catch (Exception e)
                            {
                                errosBuilder.AppendLine($"Erro ao persistir Bancos na base de dados. Erro {e}.");
                            }
                        }
                    }
					catch (Exception e)
					{
						errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncBancos)} : {e}. Item: {v.ToStringLog()}");
					}
                }

                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cadastro, TipoTabelaProtheus.SA6);

			} while (jArray.Count > 0);

            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Banco: {errosBuilder}.");
		}


		public async Task SyncLojasDellaVia(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SLJ);
            var lojaDellaViaList = new List<LojaDellaVia>();


			foreach (var jObj in jArray)
			{
				var loja = new LojaDellaVia();
				try
				{

					var bancoId = jObj["CodigoBanco"].ToString();
					var banco = _bancoRepositorio.GetSingle(x => x.CampoCodigo == bancoId);

					loja.CampoCodigo = jObj["CampoCodigo"].ToString();
					loja.BancoId = banco?.Id;
					loja.Descricao = jObj["CampoDescricao"].ToString();
					loja.Logradouro = jObj["Logradouro"].ToString();
					loja.Bairro = jObj["Bairro"].ToString();
					loja.Cidade = jObj["Cidade"].ToString();
					loja.Estado = jObj["Estado"].ToString();
					loja.Cep = jObj["Cep"].ToString();
					loja.Cnpj = jObj["Cnpj"].ToString();
					loja.InscricaoEstadual = jObj["InscricaoEstadual"].ToString();
					loja.Telefone = jObj["Telefone"].ToString();
					loja.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
					lojaDellaViaList.Add(loja);
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncLojasDellaVia)} : {e}. Item: {loja.ToStringLog()}");
				}
			}

			foreach (var lojaDellaVia in lojaDellaViaList)
			{
				var v = isFirstLoad ? null : _lojaDellaViaRepositorio.GetSingle(x => x.CampoCodigo == lojaDellaVia.CampoCodigo);

				try
				{
					if (v != null)
					{
						v.Descricao = lojaDellaVia.Descricao;
						v.RegistroInativo = lojaDellaVia.RegistroInativo;
						v.Logradouro = lojaDellaVia.Logradouro;
						v.Bairro = lojaDellaVia.Bairro;
						v.Cidade = lojaDellaVia.Cidade;
						v.Estado = lojaDellaVia.Estado;
						v.Cep = lojaDellaVia.Cep;
						v.Cnpj = lojaDellaVia.Cnpj;
						v.InscricaoEstadual = lojaDellaVia.InscricaoEstadual;
						v.Telefone = lojaDellaVia.Telefone;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						lojaDellaVia.DataAtualizacao = DateTime.Now;
						lojaDellaVia.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _lojaDellaViaRepositorio.Add(lojaDellaVia);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Lojas DellaVia na base de dados. Erro {e}.");
                        }
                    }
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncLojasDellaVia)} : {e}. Item: {v.ToStringLog()}");
				}
			}
            _escopo.Finalizar();


            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Lojas DellaVia: {errosBuilder}.");
		}

		public async Task SyncOperador(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SU7);
			do
			{
				var operadores = new List<Operador>();

				foreach (var jObj in jArray)
				{
					var operador = new Operador();
					try
					{
						var vendedorId = jObj["Vendedor_Id"].ToString();
						var vendedor = _vendedorRepositorio.GetSingle(x => x.CampoCodigo == vendedorId);

						operador.IdConsultor = vendedor?.Id.ToString();
						operador.Descricao = jObj["Nome"].ToString();
						operador.PercLimiteDesconto = Convert.ToDecimal(jObj["Desconto"].ToString());
						operador.CampoCodigo = jObj["Id"].ToString();
						operador.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
						operadores.Add(operador);

					}
					catch (Exception e)
					{
						errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncOperador)} : {e}. Item: {operador.ToStringLog()}");
					}
				}

				foreach (var operador in operadores)
				{
					var v = isFirstLoad ? null : _operadorRepositorio.GetSingle(x => x.CampoCodigo == operador.CampoCodigo);

					try
					{
						if (v != null)
						{

							v.Descricao = operador.Descricao;
							v.RegistroInativo = operador.RegistroInativo;
							v.DataAtualizacao = DateTime.Now;
							v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
						}
						else
						{
							operador.DataAtualizacao = DateTime.Now;
							operador.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                            try
                            {
                                _operadorRepositorio.Add(operador);
                            }
                            catch (Exception e)
                            {
                                errosBuilder.AppendLine($"Erro ao persistir Operadores na base de dados. Erro {e}.");
                            }
                        }
					}
					catch (Exception e)
					{
						errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncOperador)} : {e}. Item: {v.ToStringLog()}");
					}
				}
				jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SU7);

			} while (jArray.Count > 0);
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Operadores: {errosBuilder}.");
		}
		public async Task SyncParametroGeral(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var parametroGerais = new List<ParametroGeral>();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SX6);
			foreach (var jObj in jArray)
			{
				var parametroGeral = new ParametroGeral();

				try
				{
					parametroGeral.CampoCodigo = jObj["ChaveProtheus"].ToString();
					parametroGeral.Descricao = jObj["Valor"].ToString();
					parametroGeral.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
					parametroGerais.Add(parametroGeral);
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncParametroGeral)} : {e}. Item: {parametroGeral.ToStringLog()}");
				}
			}

			foreach (var parametroGeral in parametroGerais)
			{
				var v = isFirstLoad ? null : _parametroGeralRepositorio.GetSingle(x => x.CampoCodigo == parametroGeral.CampoCodigo);

				try
				{
					if (v != null)
					{
						v.CampoCodigo = parametroGeral.CampoCodigo;
						v.Descricao = parametroGeral.Descricao;
						v.RegistroInativo = parametroGeral.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						parametroGeral.DataAtualizacao = DateTime.Now;
						parametroGeral.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _parametroGeralRepositorio.Add(parametroGeral);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Parametros na base de dados. Erro {e}.");
                        }
                    }
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncParametroGeral)} : {e}. Item: {v.ToStringLog()}");
				}
            }
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Parametro Geral: {errosBuilder}.");

		}
		public async Task SyncAdministracaoFinanceira(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var administradoraFinanceiraList = new List<AdministradoraFinanceira>();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SAE);
			foreach (var jObj in jArray)
			{
				var administradoraFinanceira = new AdministradoraFinanceira();

				try
				{
					administradoraFinanceira.Descricao = jObj["Descricao"].ToString();
					administradoraFinanceira.CampoCodigo = jObj["Codigo"].ToString();
					administradoraFinanceira.FormaPagamento = jObj["Tipo"].ToString();
					administradoraFinanceira.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
					administradoraFinanceiraList.Add(administradoraFinanceira);
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncAdministracaoFinanceira)} : {e}. Item: {administradoraFinanceira.ToStringLog()}");
				}
			}


			foreach (var administradoraFinanceira in administradoraFinanceiraList)
			{
				AdministradoraFinanceira v = null;
				try
				{
					v = isFirstLoad ? null : _adminstradoraFinanceiraRepositorio.GetSingle(x =>
						 x.CampoCodigo == administradoraFinanceira.CampoCodigo);

					if (v != null)
					{
						v.Descricao = administradoraFinanceira.Descricao;
						v.RegistroInativo = administradoraFinanceira.RegistroInativo;
						v.FormaPagamento = administradoraFinanceira.FormaPagamento;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						administradoraFinanceira.DataAtualizacao = DateTime.Now;
						administradoraFinanceira.UsuarioAtualizacao = UsuarioAtualizacaoServico;

                        try
                        {
                            _adminstradoraFinanceiraRepositorio.Add(administradoraFinanceira);
                        }
                        catch (Exception e)
                        {
                            errosBuilder.AppendLine($"Erro ao persistir Administracao Financeiras base de dados. Erro {e}.");
                        }
                    }
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncAdministracaoFinanceira)} : {e}. Item: {v.ToStringLog()}");
				}
			}
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Administracao Financeira: {errosBuilder}.");
		}

		public async Task SyncModelos(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.PA1);

			var modelos = new List<MarcaModelo>();
			do
			{
				modelos = new List<MarcaModelo>();

				foreach (var jObj in jArray)
				{
					var marcaModelo = new MarcaModelo();
					try
					{
						var marcaId = jObj[nameof(Marca)].ToString();
						var marca = _marcaRepositorio.GetSingle(a => a.CampoCodigo == marcaId);

						if (marca != null)
						{
							marcaModelo.CampoCodigo = jObj["CampoCodigo"].ToString();
							marcaModelo.Descricao = jObj["CampoDescricao"].ToString();
							marcaModelo.IdMarca = marca.Id;
							marcaModelo.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
							modelos.Add(marcaModelo);
						}

					}
					catch (Exception e)
					{
						errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncModelos)} : {e}. Item: {marcaModelo.ToStringLog()}");
					}
				}

				MarcaModelo modeloEntiadde = null;
				foreach (var modelo in modelos)
				{
					try
					{

						modeloEntiadde = isFirstLoad ? null : _modeloRepositorio.GetSingle(x => x.CampoCodigo == modelo.CampoCodigo);
						if (modeloEntiadde != null)
						{
							modeloEntiadde.Descricao = modelo.Descricao;
							modeloEntiadde.RegistroInativo = modelo.RegistroInativo;
							modeloEntiadde.DataAtualizacao = DateTime.Now;
							modeloEntiadde.UsuarioAtualizacao = UsuarioAtualizacaoServico;
							modelo.Id = modeloEntiadde.Id;
						}
						else
						{
							modelo.DataAtualizacao = DateTime.Now;
							modelo.UsuarioAtualizacao = UsuarioAtualizacaoServico;
							modelo.Id = _modeloRepositorio.Add(modelo).Id;

						}

						if (modelo.Marca != null)
						{
							var marcaModeloVersao = _marcaModeloVersaoRepositorio.GetSingle(a => a.MarcaModelo.Id == modelo.Id && a.MarcaModelo.Marca.Id == modelo.Marca.Id && a.Descricao.Equals(Constants.VERSAO_DEFAULT, StringComparison.InvariantCultureIgnoreCase));
							if (marcaModeloVersao == null)
							{
								//criar a marca
								marcaModeloVersao = new MarcaModeloVersao
								{
									IdMarcaModelo = modelo.Id,
									Descricao = Constants.VERSAO_DEFAULT,
									DataAtualizacao = DateTime.Now,
									RegistroInativo = false,
									UsuarioAtualizacao = UsuarioAtualizacaoServico
								};


                                try
                                {
                                    _marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
                                }
                                catch (Exception e)
                                {
                                    errosBuilder.AppendLine($"Erro ao persistir Modelos base de dados. Erro {e}.");
                                }
                            }
						}
					}
					catch (Exception e)
					{
						errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncModelos)} : {e}. Item: {modeloEntiadde.ToStringLog()}");
					}
				}
				jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.PA1);
			} while (jArray.Count > 0);

            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Modelo: {errosBuilder}.");
		}



		private static List<T> ConvertJsonToObject<T>(JArray jArray) where T : Entidade
		{
			var list = new List<T>();
			foreach (var jObj in jArray)
			{
				var entity = Activator.CreateInstance<T>();
				entity.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
				entity.CampoCodigo = jObj["CampoCodigo"].ToString();
				entity.Descricao = jObj["CampoDescricao"].ToString();
				list.Add(entity);
			}

			return list;
		}
	}
}