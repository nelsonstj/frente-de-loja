using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IImpostosServico
	{
		void CalcularImpostos(Orcamento orcamento);
		void CalcularImpostos(long idOrcamento);
	}
}