using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum SituacaoAnaliseCredito
    {
        [Description("Em Análise")]
        EmAnalise = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Rejeitado")]
        Rejeitado = 3,
        [Description("Crediário")]
        Crediario = 4,
        [Description("OK")]
        OK = 5,
        [Description("Cancelado")]
        Cancelado = 5,
		[Description("Sem Análise")]
		SemAnalise = 9
    }
}
