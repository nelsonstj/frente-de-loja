using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class CatalogoDto
    {
        public long Id { get; set; }
        public string CodigoFabricante { get; set; }
        public string CodigoDellavia { get; set; }
        public string Descricao { get; set; }
        public string InformacoesComplementares { get; set; }
        public string MarcaVeiculo { get; set; }
        public string ModeloVeiculo { get; set; }
        public string VersaoVeiculo { get; set; }
        public int AnoInicial { get; set; }
        public int AnoFinal { get; set; }
        public List<string> CatalogoProdutoRelacionadoList { get; set; }
        public string VersaoMotor { get; set; }
    }
}
