using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ElasticsearchItemDto
    {
        public ElasticsearchItemDto()
        {

            ProdutoCodFabricante = string.Empty;
            ProdutoCodDellavia = string.Empty;
            VeiculoMarca = string.Empty;
            VeiculoModelo = string.Empty;
            VeiculoVersao = string.Empty;
            ProdutoDescricao = string.Empty;
            ProdutoFabricantePeca = string.Empty;
            ProdutoInformacaoComplementar = string.Empty;
            PrioridadeOrdenacao = "ZZZZ"; //Ultimo indice na ordenação dentro do Protheus
            VersaoMotor = string.Empty;
        }

        public string ProdutoCodFabricante { get; set; }
        public string ProdutoCodDellavia { get; set; }
        public TipoProdutoElasticSearch ProdutoCodGrupo { get; set; }
        public string VeiculoMarca { get; set; }
        public string VeiculoModelo { get; set; }
        public string VeiculoVersao { get; set; }
        public int VeiculoAnoInicial { get; set; }
        public int VeiculoAnoFinal { get; set; }
        public string ProdutoDescricao { get; set; }
        public string ProdutoFabricantePeca { get; set; }
        public string ProdutoInformacaoComplementar { get; set; }
        public string PrioridadeOrdenacao { get; set; }
        public string VersaoMotor { get; set; }
    }
}