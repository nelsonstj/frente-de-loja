using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICalculoImpostosApi
	{
		decimal CalculoImpostos(Orcamento orcamento);
	}
}