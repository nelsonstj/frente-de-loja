using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class AdministradoraFinanceira:Entidade
	{
        public string IdAdminFinanceira { get; set; }
        public string FormaPagamento { get; set; }
		public virtual ICollection<OrcamentoFormaPagamento> FormaPagamentos { get; set; }
	}
}