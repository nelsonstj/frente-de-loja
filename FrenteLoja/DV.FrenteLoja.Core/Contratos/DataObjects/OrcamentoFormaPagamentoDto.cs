namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoFormaPagamentoDto
	{
		public long Id { get; set; }
		public string CondicaoPagamento { get; set; }
		public int QtdParcelas { get; set; }
		public decimal ValorTotal { get; set; }
		public decimal ValorParcela { get; set; }
		public bool TemAcrescimo { get; set; }

	}
}