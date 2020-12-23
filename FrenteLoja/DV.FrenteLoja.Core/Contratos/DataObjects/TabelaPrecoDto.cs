namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class TabelaPrecoDto
	{
        public string Id { get; set; }
        public string Descricao { get; set; }
		public string IdConvenio { get; set; }
		public virtual ConvenioDto Convenio { get; set; }
        public string CampoCodigo { get; set; }
    }
}