using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class OrcamentoFormaPagamento : Entidade
	{
		public long IdOrcamento { get; set; }
		public virtual Orcamento Orcamento { get; set; }
		public string IdCondicaoPagamento { get; set; }
		public virtual PDCondicaoPagamento CondicaoPagamento { get; set; }
		public string IdAdministradoraFinanceira { get; set; }
		public virtual AdministradoraFinanceira AdministradoraFinanceira { get; set; }
		public long? IdBanco { get; set; }
		public virtual Banco Banco { get; set; }
		public decimal PercentualAcrescimo { get; set; }
		public decimal TotalValorForma { get; set; }
	}
}