using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Util;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
	public class CondicaoPagamentoParcelasApi : ICondicaoPagamentoParcelasApi
	{
		private readonly HttpClient _client;

		public CondicaoPagamentoParcelasApi()
		{
			_client = new HttpClient
			{
				MaxResponseContentBufferSize = 1000000 * 10 //10mb
			};
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_client.Timeout = TimeSpan.FromMinutes(5);
		}

		public List<ParcelaDto> ObterParcelas(string codCondicaoPagamento, decimal valorTotal, out string erros)
		{
			var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
			var parcelas = new List<ParcelaDto>();
			erros = string.Empty;
            try
			{
				var json = $@"{{""ValorTotal"": {valorTotal.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-GB"))}, ""Condicao"": ""{codCondicaoPagamento}""}}";
				using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
				{
					var uri = new Uri($"{baseAddress}/PARCELAS");
					var result = _client.PostAsync(uri, content).Result;
					if (result.IsSuccessStatusCode)
					{
						var contentResponse = result.Content.ReadAsStringAsync().Result;
						var parcelasJArray = JArray.Parse(contentResponse.Replace(@"\", ""));
						parcelas.AddRange(parcelasJArray.Select(jToken =>
						{
							var dto = new ParcelaDto();
							dto.DataPagamento = ProtheusConversions.ProtheusDate2DotNetDate(jToken["Vencimento"].ToString());
							dto.Valor = Convert.ToDecimal(jToken["Valor"].ToString());
							return dto;
						}));
					}
					else
					{
						erros += $"Api retornou código {result.StatusCode} msgm: {result.RequestMessage}";
					}
				}
			}
			catch (Exception e)
			{
				erros += e.ToString();
				throw new Exception(erros);
			}
			return parcelas;
		}
	}
}