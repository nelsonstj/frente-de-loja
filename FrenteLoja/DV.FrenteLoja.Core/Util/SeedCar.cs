using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Util
{
    public class SeedCar
    {
        private const string URL_BASE = "http://fipeapi.appspot.com/api/1/carros/veiculos/";
        private const string URL_MARCAS = "http://fipeapi.appspot.com/api/1/carros/marcas.json";
        public static void SeedVeiculosData(IRepositorioEscopo escopo) 
        {
            IRepositorio<MarcaModelo> escopoModelo = escopo.GetRepositorio<MarcaModelo>();
            IRepositorio<Marca> escopoMarca = escopo.GetRepositorio<Marca>();

            var serializer = new JavaScriptSerializer();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(URL_MARCAS);
                RemoteCar[] jsonListCar = serializer.Deserialize<RemoteCar[]>(json);
                jsonListCar.ToList().ForEach(remoteCar =>
                {
                    var marca = new Marca()
                    {
                        Descricao = remoteCar.name
                    };
                    escopoMarca.Add(marca);
                    

                    json = wc.DownloadString(URL_BASE + remoteCar.id + ".json");
                    RemoteModelo[] jsonListModelo = serializer.Deserialize<RemoteModelo[]>(json);
                    jsonListModelo.ToList().ForEach(remoteModelo =>
                    {
                        escopoModelo.Add(new MarcaModelo()
                        {
                            Marca = marca,
                            Descricao = remoteModelo.name,
                            Id = remoteModelo.id
                        });
                    });
                });
            }
        }        
    }

    public class RemoteCar
    {
        public string name { get; set; }
        public string fipe_name { get; set; }
        public int order { get; set; }
        public string key { get; set; }
        public int id { get; set; }
    }

    public class RemoteModelo
    {
        public string fipe_marca { get; set; }
        public string name { get; set; }
        public string marca { get; set; }
        public string key { get; set; }
        public int id { get; set; }
        public string fipe_name { get; set; }
    }

}
