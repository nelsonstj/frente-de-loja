using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ModalDetalhesProdutoDto
    {
        public ModalDetalhesProdutoDto() {
            ProdutoPaiDto = new ProdutoDto();
            ProdutoComplementoPaiDto = new ProdutoComplementoDto();
            ProdutosAgregadosModalList = new List<GrupoServicoAgregadoProdutoDto>();
        }
        public long IdOrcamento { get; set; }
		public long IdOrcamentoItemPai { get; set; }
		public int QuantidadePai { get; set; }
		public decimal PrecoUnitarioPai { get; set; }
		public decimal TotalItemPai { get; set; }
		public virtual ProdutoDto ProdutoPaiDto { get; set; }
        public string InformacaoComplementarPai { get; set; }
        public virtual ProdutoComplementoDto ProdutoComplementoPaiDto { get; set; }
		public virtual List<GrupoServicoAgregadoProdutoDto> ProdutosAgregadosModalList { get; set; }
		
	}
}