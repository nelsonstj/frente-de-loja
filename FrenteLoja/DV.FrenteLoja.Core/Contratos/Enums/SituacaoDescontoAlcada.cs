using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum SituacaoDescontoAlcada
    {
        [Description("Pendente")]
        Pendente = 1,
        [Description("Liberado")]
        Liberado = 2,
        [Description("Recusado")]
        Recusado = 3,
        [Description("Cancelado")]
        Cancelado = 4,
        [Description("Aguardando")]
        Aguardando = 5,
    }
}
