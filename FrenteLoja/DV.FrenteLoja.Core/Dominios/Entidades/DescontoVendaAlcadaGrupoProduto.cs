using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class DescontoVendaAlcadaGrupoProduto:Entidade
	{
		public long GrupoProdutoId { get; set; }
		public virtual GrupoProduto GrupoProduto { get; set; }
		public decimal PercentualDescontoVendedor { get; set; }
        public decimal PercentualDescontoGerente { get; set; }
        public string AreaNegocioId { get; set; }
        public virtual PDAreaNegocio AreaNegocio { get; set; }
        public string LojaDellaviaId { get; set; }
        public virtual LojaDellaVia LojaDellaVia { get; set; }
    }
}