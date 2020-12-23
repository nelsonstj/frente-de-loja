using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class BuscaValoresProdutoDto
    {
        public int IdOrcamento { get; set; }
        public List<string> Produtos { get; set; }
    }
}
