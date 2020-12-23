using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class TabelaPrecoItem:Entidade
	{
		public string TabelaPrecoId { get; set; }
		public virtual TabelaPreco TabelaPreco { get; set; }
		public string ProdutoId { get; set; }
		public virtual Produto Produto { get; set; }
		public decimal PrecoVenda { get; set; }

	}
}