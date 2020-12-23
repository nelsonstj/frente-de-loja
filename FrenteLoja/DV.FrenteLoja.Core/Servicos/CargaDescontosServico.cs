using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;

namespace DV.FrenteLoja.Core.Servicos
{
	public class CargaDescontosServico : ICargaDescontosServico
	{
		private readonly IRepositorioEscopo _escopo;
		private readonly ICargaCadastrosProtheusSyncApi _protheusSyncApi;
		private readonly IRepositorio<LojaDellaVia> _lojaDellaViaRepositorio;
		private readonly IRepositorio<DescontoVendaAlcada> _descontoVendaAlcadaRepositorio;
		private readonly IRepositorio<GrupoProduto> _grupoRepositorio;
		private readonly IRepositorio<DescontoVendaAlcadaGrupoProduto> _descontoVendaAlcadaGrupoProdutoRepositorio;
		private readonly IRepositorio<TipoVenda> _tipoVendaRepositorio;
		private readonly IRepositorio<DescontoModeloVenda> _descontoModeloVenda;
		private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

		public CargaDescontosServico(IRepositorioEscopo escopo, ICargaCadastrosProtheusSyncApi protheusSyncApi)
		{
			_escopo = escopo;
			_protheusSyncApi = protheusSyncApi;
			_descontoVendaAlcadaRepositorio = escopo.GetRepositorio<DescontoVendaAlcada>();
			_descontoVendaAlcadaGrupoProdutoRepositorio = escopo.GetRepositorio<DescontoVendaAlcadaGrupoProduto>();
			_lojaDellaViaRepositorio = escopo.GetRepositorio<LojaDellaVia>();
			_grupoRepositorio = escopo.GetRepositorio<GrupoProduto>();
			_descontoVendaAlcadaGrupoProdutoRepositorio = escopo.GetRepositorio<DescontoVendaAlcadaGrupoProduto>();
			_tipoVendaRepositorio = escopo.GetRepositorio<TipoVenda>();
			_descontoModeloVenda = escopo.GetRepositorio<DescontoModeloVenda>();
		}
		public async Task SyncDescontoVendaAlcada(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Desconto, TipoTabelaProtheus.PAN);
			var descontoVendaAlcadaList = new List<DescontoVendaAlcada>();
			var descontoVendaAlcadaRepositorio = new List<DescontoVendaAlcada>();

			foreach (var jObj in jArray)
			{
				var descontoVendaAlcada = new DescontoVendaAlcada();

				try
				{
					var filialId = jObj["Filial"].ToString();
					var filial = _lojaDellaViaRepositorio.GetSingle(x => x.CampoCodigo == filialId);
					if (filial == null)
					{
						errosBuilder.AppendLine($"Filial id {filialId} não encontrada.");
						continue;
					}

					var tipoVendaId = jObj["TipoVenda"].ToString();
					var tipoVenda = _tipoVendaRepositorio.GetSingle(x => x.CampoCodigo == tipoVendaId);
					if (tipoVenda == null)
					{
						errosBuilder.AppendLine($"Tipo de Venda id {tipoVendaId} não encontrada.");
						continue;
					}

					descontoVendaAlcada.CampoCodigo = jObj["Id"].ToString();
					descontoVendaAlcada.LojaDellaviaId = filial.CampoCodigo;
					descontoVendaAlcada.PercentualDescontoVendedor = Convert.ToDecimal(jObj["Desconto_PercIni"].ToString());
					descontoVendaAlcada.PercentualDescontoGerente = Convert.ToDecimal(jObj["Desconto_PercFinal"].ToString());
					descontoVendaAlcada.AreaNegocioId = tipoVenda.Id.ToString();
					descontoVendaAlcada.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
					descontoVendaAlcadaList.Add(descontoVendaAlcada);
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncDescontoVendaAlcada)} : {e}. Item: {descontoVendaAlcada.ToStringLog()}");
				}
			}

			foreach (var descontoVendaAlcada in descontoVendaAlcadaList)
			{
				var v = isFirstLoad ? null : _descontoVendaAlcadaRepositorio.GetSingle(x => x.CampoCodigo == descontoVendaAlcada.CampoCodigo);


				try
				{
					if (v != null)
					{

						v.LojaDellaviaId = descontoVendaAlcada.LojaDellaviaId;
						v.PercentualDescontoVendedor = descontoVendaAlcada.PercentualDescontoVendedor;
						v.PercentualDescontoGerente = descontoVendaAlcada.PercentualDescontoGerente;
						v.AreaNegocioId = descontoVendaAlcada.AreaNegocioId;
						v.RegistroInativo = descontoVendaAlcada.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						descontoVendaAlcada.DataAtualizacao = DateTime.Now;
						descontoVendaAlcada.UsuarioAtualizacao = UsuarioAtualizacaoServico;
						descontoVendaAlcadaRepositorio.Add(descontoVendaAlcada);
					}
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncDescontoVendaAlcada)} : {e}. Item: {v.ToStringLog()}");
				}
			}

			try
			{
				_descontoVendaAlcadaRepositorio.AddRange(descontoVendaAlcadaRepositorio);
				_escopo.Finalizar();

			}
			catch (Exception e)
			{
				errosBuilder.AppendLine($"Erro ao persistir Desconto Venda Alcada na base de dados. Erro {e}.");
			}


            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Descontos Venda Alçada: {errosBuilder}.");

			
		}

		public async Task SyncDescontoVendaAlcadaGrupoProduto(bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Desconto, TipoTabelaProtheus.PAO);
			var descontoVendaAlcadaGrupoProdutoList = new List<DescontoVendaAlcadaGrupoProduto>();
			var descontoVendaAlcadaGrupoProdutoRepositorio = new List<DescontoVendaAlcadaGrupoProduto>();

			
			foreach (var jObj in jArray)
			{
				var descontoVendaAlcadaGrupoProduto = new DescontoVendaAlcadaGrupoProduto();
				try
				{
					var filialId = jObj["Filial"].ToString();
					var filial = _lojaDellaViaRepositorio.GetSingle(x => x.CampoCodigo == filialId);
					if (filial == null)
					{
						errosBuilder.AppendLine($"Filial id {filialId} não encontrada.");
						continue;
					}

					var tipoVendaId = jObj["TipoVenda"].ToString();
					var tipoVenda = _tipoVendaRepositorio.GetSingle(x => x.CampoCodigo == tipoVendaId);
					if (tipoVenda == null)
					{
						errosBuilder.AppendLine($"Tipo de Venda id {tipoVendaId} não encontrada.");
						continue;
					}

					var grupoProdutoId = jObj["Grupo"].ToString();
					var grupoProduto = _grupoRepositorio.GetSingle(x => x.CampoCodigo == grupoProdutoId);

					if (grupoProduto == null)
					{
						errosBuilder.AppendLine($"Grupo Produto Id {grupoProdutoId} não encontrada.");
						continue;
					}

					descontoVendaAlcadaGrupoProduto.CampoCodigo = jObj["Id"].ToString();
					descontoVendaAlcadaGrupoProduto.GrupoProdutoId = grupoProduto.Id;
					descontoVendaAlcadaGrupoProduto.LojaDellaviaId = filial.CampoCodigo;
					descontoVendaAlcadaGrupoProduto.AreaNegocioId = tipoVenda.Id.ToString();
					descontoVendaAlcadaGrupoProduto.PercentualDescontoVendedor = Convert.ToDecimal(jObj["Desconto_PercIni"].ToString());
					descontoVendaAlcadaGrupoProduto.PercentualDescontoGerente = Convert.ToDecimal(jObj["Desconto_PercFinal"].ToString());
					descontoVendaAlcadaGrupoProduto.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
					descontoVendaAlcadaGrupoProdutoList.Add(descontoVendaAlcadaGrupoProduto);
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncDescontoVendaAlcadaGrupoProduto)} : {e}. Item: {descontoVendaAlcadaGrupoProduto.ToStringLog()}");
				}
			}

			foreach (var descontoVendaAlcada in descontoVendaAlcadaGrupoProdutoList)
			{
				DescontoVendaAlcadaGrupoProduto v = null;
				try
				{
					v = isFirstLoad ? null : _descontoVendaAlcadaGrupoProdutoRepositorio.GetSingle(x => x.CampoCodigo == descontoVendaAlcada.CampoCodigo);

					if (v != null)
					{
						v.GrupoProdutoId = descontoVendaAlcada.GrupoProdutoId;
						v.LojaDellaviaId = descontoVendaAlcada.LojaDellaviaId;
						v.PercentualDescontoVendedor = descontoVendaAlcada.PercentualDescontoVendedor;
						v.AreaNegocioId = descontoVendaAlcada.AreaNegocioId;
						v.PercentualDescontoGerente = descontoVendaAlcada.PercentualDescontoGerente;
						v.RegistroInativo = descontoVendaAlcada.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						descontoVendaAlcada.DataAtualizacao = DateTime.Now;
						descontoVendaAlcada.UsuarioAtualizacao = UsuarioAtualizacaoServico;
						descontoVendaAlcadaGrupoProdutoRepositorio.Add(descontoVendaAlcada);
					}
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncDescontoVendaAlcadaGrupoProduto)} : {e}. Item: {v.ToStringLog()}");
				}
			}
            
            

			try
			{
				_descontoVendaAlcadaGrupoProdutoRepositorio.AddRange(descontoVendaAlcadaGrupoProdutoRepositorio);
				_escopo.Finalizar();
			}
			catch (Exception e)
			{
				errosBuilder.AppendLine($"Erro ao persistir Descontos Vendas Alçadas na base de dados. Erro {e}.");
			}

            if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Descontos Venda Alçada Grupo Produto: {errosBuilder}.");
            
		}

		public async Task SyncDescontoModeloDeVenda (bool isFirstLoad = false)
		{
			var errosBuilder = new StringBuilder();
			var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Desconto, TipoTabelaProtheus.ZAL);
			var descontoModeloVendaList = new List<DescontoModeloVenda>();
			var  descontoModeloVendaRepositorio = new List<DescontoModeloVenda>();
			foreach (var jObj in jArray)
			{
				var descontoModeloVenda = new DescontoModeloVenda();

				try
				{
					var tipoVendaId = jObj["Area"].ToString();
					var tipoVenda = _tipoVendaRepositorio.GetSingle(x => x.CampoCodigo == tipoVendaId);
					if (tipoVenda == null)
					{
						errosBuilder.AppendLine($"Tipo Venda id {tipoVendaId} não encontrado.");
						continue;
					}
					descontoModeloVenda.CampoCodigo = jObj["Id"].ToString();
					descontoModeloVenda.TabelasDePrecoAssociadas = jObj["Tabela"].ToString();
					descontoModeloVenda.CodigosDeProdutoLiberados = jObj["Produtos_01"].ToString() + jObj["Produtos_02"];
					descontoModeloVenda.PercentualDesconto1 = Convert.ToDecimal(jObj["Percentual_01"].ToString());
					descontoModeloVenda.PercentualDesconto2 = Convert.ToDecimal(jObj["Percentual_02"].ToString());
					descontoModeloVenda.PercentualDesconto3 = Convert.ToDecimal(jObj["Percentual_03"].ToString());
					descontoModeloVenda.PercentualDesconto4 = Convert.ToDecimal(jObj["Percentual_04"].ToString());
					descontoModeloVenda.AreaNegocioId = tipoVenda.Id.ToString();
					descontoModeloVenda.Bloqueado = jObj["Bloqueado"].ToString() == "1";
					descontoModeloVenda.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
					descontoModeloVendaList.Add(descontoModeloVenda);
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncDescontoModeloDeVenda)} : {e}. Item: {descontoModeloVenda?.ToStringLog()}");
				}
			}

			
			foreach (var descontoVendaAlcada in descontoModeloVendaList)
			{
				DescontoModeloVenda v = null;
				try
				{
					v = isFirstLoad ? null : _descontoModeloVenda.GetSingle(x => x.CampoCodigo == descontoVendaAlcada.CampoCodigo);


					if (v != null)
					{

						v.TabelasDePrecoAssociadas = descontoVendaAlcada.TabelasDePrecoAssociadas;
						v.CodigosDeProdutoLiberados = descontoVendaAlcada.CodigosDeProdutoLiberados;
						v.PercentualDesconto1 = descontoVendaAlcada.PercentualDesconto1;
						v.PercentualDesconto2 = descontoVendaAlcada.PercentualDesconto2;
						v.PercentualDesconto3 = descontoVendaAlcada.PercentualDesconto3;
						v.PercentualDesconto4 = descontoVendaAlcada.PercentualDesconto4;
						v.AreaNegocioId = descontoVendaAlcada.AreaNegocioId;
						v.Bloqueado = descontoVendaAlcada.Bloqueado;
						v.RegistroInativo = descontoVendaAlcada.RegistroInativo;
						v.DataAtualizacao = DateTime.Now;
						v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
					}
					else
					{
						descontoVendaAlcada.DataAtualizacao = DateTime.Now;
						descontoVendaAlcada.UsuarioAtualizacao = UsuarioAtualizacaoServico;
						descontoModeloVendaRepositorio.Add(descontoVendaAlcada);
					}
				}
				catch (Exception e)
				{
					errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncDescontoModeloDeVenda)} : {e}. Item: {v.ToStringLog()}");
				}
			}

			try
			{
				_descontoModeloVenda.AddRange(descontoModeloVendaRepositorio);
				_escopo.Finalizar();

			}
			catch (Exception e)
			{
				errosBuilder.AppendLine($"Erro ao persistir Descontos Modelo de Venda na base de dados. Erro {e}.");
			}
			
			if (errosBuilder.Length > 0)
				throw new Exception($"Erros de importação de Descontos Modelo de Venda: {errosBuilder}.");
		}
	}
}