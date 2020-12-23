using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum TipoOrcamento
	{
		[Description("Loja")]
		Loja = 0,
		[Description("Telemarketing")]
		Telemarketing = 1,
		[Description("Retira")]
		Retira = 2
	}
}