using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum StatusCliente
	{
		[Description("Bloqueado")]
		Bloqueado = 1,
		[Description("Liberado")]
		Liberado = 2,
	}
}