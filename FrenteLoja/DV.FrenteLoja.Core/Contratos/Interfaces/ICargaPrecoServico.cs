using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICargaPrecoServico
	{
		Task SyncTabelaPreco(bool isFirstLoad = false);
		Task SyncTabelaPrecoItem(bool isFirstLoad = false);
	}
}