using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum TipoFormaPagamento
	{
		[Description("Cartão")]
		Cartao,
		[Description("Banco")]
		Banco, 
		[Description("Dinheiro")]
		Dinheiro
	}
}