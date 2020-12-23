using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class DescontoModeloVenda:Entidade
	{
		public string TabelasDePrecoAssociadas { get; set; }
		public string CodigosDeProdutoLiberados { get; set; }
		public decimal PercentualDesconto1 { get; set; }
		public decimal PercentualDesconto2 { get; set; }
		public decimal PercentualDesconto3 { get; set; }
		public decimal PercentualDesconto4 { get; set; }
		public string AreaNegocioId { get; set; }
		public bool Bloqueado { get; set; }
		public virtual PDAreaNegocio AreaNegocio { get; set; }
        public virtual ICollection<OrcamentoItem> OrcamentoItemList { get; set; }
    }
}