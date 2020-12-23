using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace DV.FrenteLoja.Core.ProtheusAPI
{
    public class CatalogoApi : ICatalogoProtheusApi
    {
        private readonly HttpClient _client;

        public CatalogoApi()
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 1000000 * 10 //10mb
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromHours(1);
        }

        public List<Catalogo> PostCatalogo(List<Catalogo> catalogoList, out string erros)
        {

            var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];

            var catalogoRetornoProtheus = new List<Catalogo>();
            erros = string.Empty;

            foreach (var catalogList in catalogoList.SplitList(10000))
            {

                try
                {
                    var catalogListToProtheus = catalogList.Select(x => new
                    {
                        AnoInicial = x.AnoInicial.ToString(),
                        AnoFinal = x.AnoFinal.ToString(),
                        x.CodigoFabricante,
                        x.FabricantePeca,
                        x.MarcaVeiculo,
                        x.ModeloVeiculo,
                        x.VersaoVeiculo,
                        Descricao = x.Descricao.Replace("\"", "")
                    });

                    var path = HttpContext.Current.Server.MapPath("~/App_Data/LocalJsonFile.json");

                    using (TextWriter textWriter = File.CreateText(path))
                    {
                        var serializer = new JsonSerializer();

                        serializer.Serialize(textWriter, catalogListToProtheus);
                    }

                    var json = File.ReadAllText(path);
                    json = "{\"classname\": \"FULL_CATALOGO\",\"CATALOGO\": " + json + "}";
                    using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        var uri = new Uri($"{baseAddress}/CATALOGO");
                        var result = _client.PostAsync(uri, content).Result;


                        if (result.IsSuccessStatusCode)
                        {
                            var contentResponse = result.Content.ReadAsStringAsync().Result;

                            var catalogoFull = JObject.Parse(contentResponse);
                            var catalogos = JArray.Parse(catalogoFull["CATALOGO"].ToString().Replace(@"\", ""));


                            foreach (var c in catalogos)
                            {
                                Catalogo catalogo = new Catalogo();
                                catalogo.CodigoDellavia = c["CODDV"].ToString(); //Código Dellavia.
                                catalogo.CodigoFabricante = c["CODFAB"].ToString(); // Código Fabricante.
                                catalogo.FabricantePeca = c["FABRIC"].ToString(); // Descricao Fabricante.
                                catalogoRetornoProtheus.Add(catalogo);
                            }
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
                finally
                {
                    GC.Collect();
                }
            }

            return catalogoRetornoProtheus;
        }
    }
}