using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum StatusIntegracao
    {
        [Description("Sucesso")]
        Sucesso = 1,
        [Description("Erro")]
        Erro = 2,
    }
}
