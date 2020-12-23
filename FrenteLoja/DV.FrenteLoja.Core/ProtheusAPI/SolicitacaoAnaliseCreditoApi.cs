using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using Newtonsoft.Json;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
	public class SolicitacaoAnaliseCreditoApi: ISolicitacaoAnaliseCreditoApi
	{
		private readonly HttpClient _client;

		public SolicitacaoAnaliseCreditoApi()
		{
			_client = new HttpClient
			{
				MaxResponseContentBufferSize = 1000000 * 10 //10mb
			};
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<List<SolicitacaoAnaliseCreditoRetornoProtheus>> PostConsultaAnaliseCredito(List<SolicitacaoAnaliseCredito> analiseCreditoList)
		{
			try
			{
				var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
				var resourcePath = "CREDITANA";
				var parametro = new
				{
					Clientes = analiseCreditoList.Select(x => new
					{
						Cnpj = x.Orcamento.Cliente.CNPJCPF,
						Contrato = x.NumeroContrato
					})
				};
				var json = JsonConvert.SerializeObject(parametro);
				var uri = new Uri(string.Concat(baseAddress, resourcePath));
				using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
				{
					var response = await _client.PostAsync(uri, content);
					if (!response.IsSuccessStatusCode) 
						throw new Exception($"Api respondeu com: {response}.\n");
					var contentResponsiveed = await response.Content.ReadAsStringAsync();
                    if (contentResponsiveed.Substring(0,1)==",")
                        contentResponsiveed = "[" + contentResponsiveed.Substring(1, contentResponsiveed.Length - 1);

					var result = JsonConvert.DeserializeObject<List<SolicitacaoAnaliseCreditoRetornoProtheus>>(contentResponsiveed);
					return result;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				throw new NegocioException($"Não foi possivel se comunicar com o Protheus. Metodo {nameof(PostConsultaAnaliseCredito)}. Descrição: " + e.Message);
			}
		}
	}
}