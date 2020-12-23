using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using DV.FrenteLoja;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
	public class CalculoImpostosApi : ICalculoImpostosApi
	{
		private readonly HttpClient _client;
		private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;

		public CalculoImpostosApi(IRepositorioEscopo escopo)
		{
			_client = new HttpClient();
			_logIntegracaoRepositorio = escopo.GetRepositorio<LogIntegracao>();
		}

		public decimal CalculoImpostos(Orcamento orcamento)
		{
			var parametros = new
			{
				CodLojaCliente = orcamento.Cliente.Loja,
				CodCliente = orcamento.IdCliente,
				TipoOperacao = "01", // Fixo
				Produtos = orcamento.OrcamentoItens.Select(x => new
				{
					CodProduto = x.ProdutoId,
					x.Quantidade,
					VlrUnit = x.PrecoUnitario
				})
			};
			var json = JsonConvert.SerializeObject(parametros);
			var _logIntegracao = new LogIntegracao
			{
				DataAtualizacao = DateTime.Now,
				UsuarioAtualizacao = HttpContext.Current.User.Identity.Name,
				RequestIntegracaoJson = json,
			};
			try
			{
				var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
				var resourcePath = "CALC_FIS";
				var uri = new Uri(string.Concat(baseAddress, resourcePath));
				using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
				{
					content.Headers.Add("tenantId", "01," + orcamento.LojaDellaVia.CampoCodigo);
					var response = _client.PostAsync(uri, content).Result;
					var contentResponse = response.Content.ReadAsStringAsync().Result;
					_logIntegracao.ResponseIntegracaoJson = JObject.Parse(contentResponse).ToString();
					if (!response.IsSuccessStatusCode)
						throw new NegocioException($"Imposto do orçamento { orcamento.Id } não foi calculado: { response }.\n");
					_logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
					_logIntegracao.Log = $"Imposto do orçamento { orcamento.Id } foi calculado com sucesso.";
					return ((dynamic)JObject.Parse(contentResponse)).VALORIMPOSTOS;
				}
			}
			catch (Exception e)
			{
				_logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
				_logIntegracao.Log = e.Message;
				_logIntegracao.ResponseIntegracaoJson = _logIntegracao.ResponseIntegracaoJson ?? e.Message;
				Debug.WriteLine(e.ToString());
				throw new ServicoIntegracaoException($"Erro no método {nameof(CalculoImpostos)}. Descrição: " + e.Message);
			}
			finally
			{
				_logIntegracaoRepositorio.Add(_logIntegracao);
			}
		}
	}
}