using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum StatusSolicitacao
	{
		[Description("Retornado")]
		Retornado,
		[Description("Pendente Retorno")]
		PendenteRetorno,
		[Description("Não enviado")]
		NaoEnviado
	}
}