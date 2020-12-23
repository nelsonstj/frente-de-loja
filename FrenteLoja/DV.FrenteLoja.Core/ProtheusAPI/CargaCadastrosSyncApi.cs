using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Exceptions;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
    public class CargaCadastrosSyncApi : ICargaCadastrosProtheusSyncApi
    {
        private readonly HttpClient _client;

        public CargaCadastrosSyncApi()
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 1000000 * 10, //10mb
				Timeout = TimeSpan.FromHours(2)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<JArray> GetDataFromKeyTable(EndpointProtheus endpointProtheus, TipoTabelaProtheus tabelaProtheus)
        {
            try
            {
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];

                var resourcePath = $"{endpointProtheus}?codtabela={tabelaProtheus}";

                var uri = new Uri(string.Concat(baseAddress, resourcePath));

                var response = await _client.GetAsync(uri);

                if (!response.IsSuccessStatusCode) throw new Exception($"Api respondeu com: {response}.\n");

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // Não existem novos dados na base para integrar. 
                    return new JArray();
                }
                var content = await response.Content.ReadAsStringAsync();

                var result = JArray.Parse(content.Replace(@"\", ""));

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new ServicoIntegracaoException($"Erro no método {nameof(GetDataFromKeyTable)}. Descrição: "+e.Message);
            }
        }
    }
}