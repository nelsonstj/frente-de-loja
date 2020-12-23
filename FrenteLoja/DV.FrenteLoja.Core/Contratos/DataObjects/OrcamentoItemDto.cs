using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoItemDto
	{
		public long Id { get; set; }
		public string Descricao { get; set; }
		public bool RegistroInativo { get; set; }
		public string CampoCodigo { get; set; }
		public int OrcamentoId { get; set; }
		public virtual OrcamentoDto Orcamento { get; set; }
		public string Item { get; set; }
		public decimal Quantidade { get; set; }
		public decimal PrecoUnitario { get; set; }
        //public decimal PrecoVenda { get; set; }
        public decimal TotalItem { get; set; }
		public int ProdutoPaiId { get; set; }
		public virtual ProdutoDto ProdutoPai { get; set; }
		public decimal ValorDesconto { get; set; }
		public decimal PercDescon { get; set; }
		public string TipoOperacao { get; set; }
		public DescontoModeloVendaUtilizado? DescontoModeloVendaUtilizado { get; set; }
	}
}