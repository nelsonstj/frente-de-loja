namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class OrcamentoFormaPagamentoProtheusDto
    {
        public string CodCondicaoPagamento { get; set; }
        public string CodAdministradora { get; set; }
        public string Forma { get; set; }
        public decimal ValorTotal { get; set; }
	    public string Banco { get; set; }
	    public string Descricao { get; set; }
    }
}