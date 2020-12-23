using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class BuscaEstoqueProtheusDto
    {
        public string IdFilial { get; set; }
        public List<string> Produtos { get; set; }
    }
}
