using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.Servicos
{
	public class ConsultaPlacaWS:IConsultaPlacaWS
	{
		private readonly HttpClient _httpClient;

		public ConsultaPlacaWS(IRepositorioEscopo escopo)
		{
			_httpClient = new HttpClient();
		}

		public async Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlacaWS(string placa)
		{
			try
            {
                placa = placa.Contains("-") ? placa.Remove(placa.IndexOf('-'), 1) : placa;
                ClienteMarcaModeloVersaoDto marcaModeloVersaoDto = null;
                var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaWS"];
                var resourcePath = $"{wsAddress}?placa={placa}";
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
                var response = await _httpClient.GetAsync(resourcePath);

                if (response.IsSuccessStatusCode)
                {
                    marcaModeloVersaoDto = new ClienteMarcaModeloVersaoDto();
                    var content = await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(content);
                    var marcaModeloVersao = jObject["marca"].ToString();
                    var ano = Convert.ToInt32(jObject["ano"].ToString());
                    var marca = marcaModeloVersao.Split('/')[0];
                    var modelo = marcaModeloVersao.Split('/')[1].Split(' ')[0];
                    var index = marcaModeloVersao.Split('/')[1].IndexOf(' ');
                    var versao = marcaModeloVersao.Split('/')[1].Substring(index);
                }

                return marcaModeloVersaoDto;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
		}
	}
}