using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoItemProtheusDto
	{
		public OrcamentoItemProtheusDto()
		{
			Equipe = new List<OrcamentoItemEquipeMontagemProtheusDto>();
		}

		public string CodDescontoModeloVenda { get; set; }
		public int NrItem { get; set; }
		public int? NrItemProdutoPaiId { get; set; }
		public bool ReservaEstoque { get; set; }
		public string CodProduto { get; set; }
		public string ProdutoDescricao { get; set; }
		public decimal ValorTotalItem { get; set; }
		public decimal Quantidade { get; set; }
		public decimal PercDesconto { get; set; }
		public decimal PrecoUnitario { get; set; }
		public decimal ValorDesconto { get; set; }
		public string TipoOperacao { get; set; }
		public DescontoVendaAlcadaProtheusDto DescontoVendaAlcadaEnvio { get; set; }
        public List<DescontoVendaAlcadaProtheusDto> DescontoVendaAlcadaRetorno { get; set; }
        public List<OrcamentoItemEquipeMontagemProtheusDto> Equipe { get; set; }
		public bool RegistroInativo { get; set; }
	}
}