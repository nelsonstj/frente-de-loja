using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IGrupoProdutoServico
    {
        TipoProdutoElasticSearch BuscaTipoProdutoElasticSearch(string campoCodigoGrupoProduto);
    }
}
