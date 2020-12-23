namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class VendedorDto
	{
		public long Id { get; set; }
		public string Nome { get; set; }
		public string FilialOrigem { get; set; }
        public string CampoCodigo { get; set; }
        public string IdConsultor { get; set; }
        public string IdLoja { get; set; }
        public string IdRegional { get; set; }
        public string IdUser { get; set; }
        public string Ativo { get; set; }
    }
}