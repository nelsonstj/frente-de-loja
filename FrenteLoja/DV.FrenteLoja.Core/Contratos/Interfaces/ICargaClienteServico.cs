using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICargaClienteServico
	{
		Task SyncCliente(bool isFirstLoad = false);
		Task SyncClienteVeiculo(bool isFirstLoad = false);
	}
}