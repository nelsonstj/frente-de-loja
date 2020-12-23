using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ModalEstoqueDto
    {
        public string CampoCodigo { get; set; }
        public string DescricaoProduto { get; set; }
        public virtual List<ProdutoProtheusDto> ProdutoProtheus { get; set; }
    }
}
