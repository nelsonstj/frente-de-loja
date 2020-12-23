using DV.FrenteLoja.Grafo.Enumeradores;
using Neo4jClient;
using System;

namespace DV.FrenteLoja.Grafo
{
    public class ConfigGrafo
    {

        private string Server { get; set; }
        private string UserID { get; set; }
        private string Password { get; set; }

        public GraphClient contextoGrafo;

        private GraphClient ClienteGrafo()
        {
            return new GraphClient(new Uri(Server), UserID, Password);
        }

        public ConfigGrafo(string connectioString)
        {
            String[] CN = connectioString.Split(';');

            this.Server = CN[0];
            this.UserID = CN[1];
            this.Password = CN[2];
        }

        public void Conectar(TipoConexao tipo)
        {
            try
            {
                contextoGrafo = ClienteGrafo();


                switch (tipo)
                {
                    case TipoConexao.Sincrona:
                        contextoGrafo.Connect();
                        break;
                    case TipoConexao.Assincrona:
                        contextoGrafo.ConnectAsync();
                        break;
                    default:
                        contextoGrafo.Connect();
                        break;
                }

            }
            catch (Exception) { }

            ////Não deletar Bruno.SIlva
            //SQLToGrafoCatalogo roda = new SQLToGrafoCatalogo();


            //Catalogo cat = new Catalogo();

            ////migrations
            //cat = new Catalogo { Id = 220, TipoVeiculo = TipoVeiculo.Carro };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.TipoVeiculo);
            //cat = new Catalogo { Id = 220, TipoVeiculo = TipoVeiculo.Moto };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.TipoVeiculo);

            ////Teste cadastro Marcas
            //cat = new Catalogo { Id = 220, MarcaVeiculo = "Ford" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Marca);
            //cat = new Catalogo { Id = 220, MarcaVeiculo = "Fiat" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Marca);
            //cat = new Catalogo { Id = 220, MarcaVeiculo = "GM" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Marca);
            //cat = new Catalogo { Id = 220, MarcaVeiculo = "Volkswagen" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Marca);
         

            ////Teste cadastro Versao
            //cat = new Catalogo { Id = 220, VersaoVeiculo = "1.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Versao);
            //cat = new Catalogo { Id = 220, VersaoVeiculo = "1.6" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Versao);
            //cat = new Catalogo { Id = 220, VersaoVeiculo = "2.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Versao);
            //cat = new Catalogo { Id = 220, VersaoVeiculo = "2.5" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Versao);

            ////Teste cadastro Modelos
            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Focus", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Ford", VersaoVeiculo = "1.6" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Focus", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Ford", VersaoVeiculo = "2.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Fiesta", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Ford", VersaoVeiculo = "1.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);


            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Fiesta", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Ford", VersaoVeiculo = "1.6" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Ranger", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Ford", VersaoVeiculo = "2.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Passat", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Volkswagen", VersaoVeiculo = "2.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Jetta", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Volkswagen", VersaoVeiculo = "2.5" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Palio", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Fiat", VersaoVeiculo = "1.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);
            
            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Cruze", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "GM", VersaoVeiculo = "2.0" };
            //roda.CriarCypherLabel(contextoGrafo, cat, CypherCreate.UniqueRoot, TypeNode.Modelo);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Marca);
            //roda.CriarCypherRelacionamentos(contextoGrafo, cat, TypeNode.Modelo, TypeNode.Versao);

            ////teste relacionamentos

            //cat = new Catalogo { Id = 220, ModeloVeiculo = "Focus", TipoVeiculo = TipoVeiculo.Carro, MarcaVeiculo = "Ford", VersaoVeiculo = "1.6" };
            
        }

    }
}
