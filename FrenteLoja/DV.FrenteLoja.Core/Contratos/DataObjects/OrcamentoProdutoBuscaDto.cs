using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class OrcamentoProdutoBuscaDto
    {
        public OrcamentoProdutoBuscaDto()
        {
            Produtos = new List<SacolaProdutoDto>();
        }
        public List<SacolaProdutoDto> Produtos { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }
	    public long IdOrcamento { get; set; }
    }
}
