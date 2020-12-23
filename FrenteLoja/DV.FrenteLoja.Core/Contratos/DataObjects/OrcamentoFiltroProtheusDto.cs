namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoFiltroProtheusDto
	{
		public int? Status { get; set; }
		public string CodOrcamento { get; set; }
		public string CodPlaca { get; set; }
		public string CodCpf { get; set; }
		public string CodCnpj { get; set; }
		public string CodCliente { get; set; }
		public string CodVendedor { get; set; }
		public string CodFilial { get; set; }
	}
}