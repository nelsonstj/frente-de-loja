namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class GrupoServicoAgregadoProdutoDto
	{
		public string Id { get; set; }
		public string IdProduto { get; set; }
		public string Item { get; set; }
		public string Descricao { get; set; }
		public bool PermiteAlterarQuantidade { get; set; }
		public int Quantidade { get; set; }
		public decimal PrecoUnitario { get; set; }
		public decimal TotalItem { get; set; }
		public long IdOrcamentoItemFilho { get; set; }
	}
}