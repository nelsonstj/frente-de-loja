using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IConsultaPlacaWS
	{
		Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlacaWS(string placa);
	}
}