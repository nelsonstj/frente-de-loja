using DV.FrenteLoja.Core.Contratos.Enums;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class AplicarDescontoDto
	{
		public long IdOrcamentoItem { get; set; }
		public long? IdDescontoModeloVenda { get; set; }
		public string DescricaoProduto { get; set; }
		public decimal QuantidadeProduto { get; set; }
        [Display(Name = "Valor original")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal ValorOriginal { get; set; }
        public string ValorOriginalDescricao { get; set; }
        public decimal PrecoUnitario { get; set; }
        [Display(Name = "% desc.")]
        public decimal PercentualDesconto { get; set; }
        public string PercentualDescontoDescricao { get; set; }
        [Display(Name = "Valor desconto")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal ValorDesconto { get; set; }
        public string ValorDescontoDescricao { get; set; }
        [Display(Name = "Valor com desconto")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal ValorTotalComDesconto { get; set; }
        public string ValorTotalComDescontoDescricao { get; set; }
        //public decimal? DescontoModeloVendaQuantidade1 { get; set; }
        //public decimal? DescontoModeloVendaQuantidade2 { get; set; }
        //public decimal? DescontoModeloVendaQuantidade3 { get; set; }
        //public decimal? DescontoModeloVendaQuantidade4 { get; set; }
        public decimal? PercentualDescontoLoja { get; set; }
        public decimal? PercentualDescontoLojaDv { get; set; }
        public decimal? PercentualDescontoLojaGrupo { get; set; }
        public decimal? PercentualDescontoLojaGrupoDv { get; set; }
        [Display(Name = "Observação Item")]
        public string ObservacaoItem { get; set; }
        [Display(Name = "Observação Geral")]
        public string ObservacaoGeral { get; set; }
		public decimal PercentualLimiteDesconto { get; set; }
		public decimal PercentualDescontoAlcadaGerente { get; set; }
		public DescontoModeloVendaUtilizado? DescontoModeloVendaUtilizado { get; set; }
	}
}