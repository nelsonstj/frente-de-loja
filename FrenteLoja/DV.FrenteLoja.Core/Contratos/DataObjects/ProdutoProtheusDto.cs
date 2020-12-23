namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ProdutoProtheusDto
    {
        public long Id { get; set; }
        public string Filial { get; set; }
	    public string NomeFilial { get; set; }
	    public string CodigoDellaVia { get; set; }
	    public decimal SaldoDisponivel { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
