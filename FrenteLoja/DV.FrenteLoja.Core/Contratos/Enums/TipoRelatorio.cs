using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum TipoRelatorio
	{
		[Description("DV.FrenteLoja.Reports.Templates.RelOrcamento")]
		Orcamento = 0,
		[Description("DV.FrenteLoja.Reports.Templates.Exemplo")]
		Exemplo = 1
	}
}