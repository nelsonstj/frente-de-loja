namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class ProdutoComplementoDto
	{
		public long Id { get; set; }
		public string Descricao { get; set; }
		public string CampoCodigo { get; set; }
		public string IdProduto { get; set; }
		public decimal? Comprimento { get; set; }
		public decimal? Espessura { get; set; }
		public decimal? Largura { get; set; }
		public decimal? VolumeM3 { get; set; }
		public decimal? Perfil { get; set; }
		public decimal? Aro { get; set; }
		public string Carga { get; set; }
		public string Indice { get; set; }

		public string CampoHTML { get; set; }
        public bool hasCampoHTML { get; set; }
    }
}