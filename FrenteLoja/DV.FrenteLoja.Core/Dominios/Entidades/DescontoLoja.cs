namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class DescontoLoja
	{
		public string IdLoja { get; set; }
		public string IdAreaNegocio { get; set; }
		public decimal? PercentualDescontoLojaGerente { get; set; }
		public decimal? PercentualDescontoLojaVendedor { get; set; }
		public string IdGrupo { get; set; }
		public decimal? PercentualDescontoGrupoGerente { get; set; }
		public decimal? PercentualDescontoGrupoVendedor { get; set; }
    }
}