using DV.FrenteLoja.Core.Contratos.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Exceptions;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
	public class EstoqueApi : IEstoqueProtheusApi
	{
		private readonly HttpClient _client;

		public EstoqueApi()
		{
			_client = new HttpClient
			{
				MaxResponseContentBufferSize = 1000000 * 10 //10mb
			};

			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}


		public async Task<JArray> BuscaEstoqueProdutoFiliais(string codProduto, string[] codLojasDellaVia)
		{
			try
			{
				var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];

				var parametroLojasAggregate = codLojasDellaVia.Aggregate((x, y) => x + "-" + y);

				var resourcePath = "SALDORANGE";

				var json = $@"{{""PRODUTO"": ""{codProduto}"",""FILIAIS"": ""{parametroLojasAggregate}""}}";

				var uri = new Uri(string.Concat(baseAddress, resourcePath));

				using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
				{

					var response = await _client.PostAsync(uri, content);

					if (!response.IsSuccessStatusCode) throw new Exception($"Api respondeu com: {response}.\n");

					var contentResponsiveed = await response.Content.ReadAsStringAsync();

					var result = JArray.Parse(contentResponsiveed.Replace(@"\", ""));

					return result;
				}
			}
			catch (Exception e)
			{

				Debug.WriteLine(e.ToString());
				throw new NegocioException($"Não foi possivel se comunicar com o Protheus. Metodo {nameof(BuscaEstoqueProdutoFiliais)}. Descrição: " + e.Message);
			}
		}

		public async Task<JArray> BuscaEstoqueProdutoFiliais(string[] codProdutos,string codLojaDellaVia)
		{
			try
			{
				var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];

				var parametroProdutosAggregate = codProdutos.Aggregate((x, y) => x + "-" + y);

				var resourcePath = "SALDO";

				var json = $@"{{""FILIAL"": ""{codLojaDellaVia}"",""PRODUTOS"": ""{parametroProdutosAggregate}""}}";

				var uri = new Uri(string.Concat(baseAddress, resourcePath));

				using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
				{

					var response = _client.PostAsync(uri, content).Result;

					if (!response.IsSuccessStatusCode) throw new Exception($"Api respondeu com: {response}.\n");

					var contentResponsiveed = await response.Content.ReadAsStringAsync();

					var result = JArray.Parse(contentResponsiveed.Replace(@"\", ""));

					return result;
				}
			}
			catch (Exception e)
			{

				Debug.WriteLine(e.ToString());
				throw new NegocioException($"Não foi possivel se comunicar com o Protheus. Metodo {nameof(BuscaEstoqueProdutoFiliais)}. Descrição: " + e.Message);
			}
		}

	}
}
