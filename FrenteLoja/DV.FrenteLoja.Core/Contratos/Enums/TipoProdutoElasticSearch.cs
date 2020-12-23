using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum TipoProdutoElasticSearch
    {
        [Description("Pneus")]
        Pneus = 1,
        [Description("Freios")]
        Freio = 2,
        [Description("Lubrificantes")]
        Lubrificantes = 3,
        [Description("Servicos")]
        Servicos = 4,
        [Description("Acessorios")]
        Acessorios = 5,
        [Description("Suspensao")]
        Suspensao = 6
    }
}



