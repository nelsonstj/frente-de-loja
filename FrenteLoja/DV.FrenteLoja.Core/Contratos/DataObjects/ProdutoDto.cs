using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class ProdutoDto
	{
		public string Id { get; set; }
		public string Descricao { get; set; }
        [Display(Name = "CÓD. DELLAVIA")]
        public string CampoCodigo { get; set; }
		public string IdGrupoProduto { get; set; }
		public string IdSubGrupoProduto { get; set; }
		[Display(Name = "CÓD. FABRICANTE")]
        public string CodigoFabricante { get; set; }
		[Display(Name = "FABRICANTE")]
		public string Fabricante { get; set; }
	}
}