namespace DV.FrenteLoja.Models
{
    public class ProdutoModel
    {
        public long NumeroOrcamento { get; set; }
        public string ProdutoDescricao { get; set; }
        public string ProdutoCodDellavia { get; set; }
        public string ProdutoCodFabricante { get; set; }
        public string ProdutoFabricantePeca { get; set; }
        public string VeiculoMarca { get; set; }
        public string VeiculoModelo { get; set; }
        public string VeiculoVersao { get; set; }
        public string VersaoMotor { get; set; }
        public string VeiculoAnoInicial { get; set; }
        public string VeiculoAnoFinal { get; set; }
    }
}