using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Exceptions;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
	public class LoginApi: ILoginProtheusApi
	{
		private readonly HttpClient _client;

		public LoginApi()
		{
			_client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_client.Timeout = TimeSpan.FromMinutes(5);
		}

		public async Task<JObject> LoginUsuario(string nome, string password)
		{
			try
			{
				var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
				var resourcePath = "USUARIO";
				var json = $@"{{""coduser"": ""{nome}"",""userpass"": ""{password}""}}";
				var uri = new Uri(string.Concat(baseAddress, resourcePath));
				using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
				{
					var response = await _client.PostAsync(uri,content);
					var contentResponsiveed = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode) 
						throw new ServicoIntegracaoException(contentResponsiveed);
                    var result = JArray.Parse(contentResponsiveed.Replace(@"\", ""));
					return result.First.ToObject<JObject>();
				}
			}
            catch(ServicoIntegracaoException excepton)
            {
                JObject errorMessage = JObject.Parse(excepton.Message);
                throw new ServicoIntegracaoException(excepton.Message, new Exception(errorMessage["errorMessage"].ToString()));
            }
			catch (Exception e)
			{
                throw new ServicoIntegracaoException(e.Message, e);
			}
		}
	}
}