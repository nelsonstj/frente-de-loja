using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICargaConvenioServico
	{
		Task SyncCondicaoPagamento(bool isFirstLoad = false);
		Task SyncConvenio(bool isFirstLoad = false);
		Task SyncConvenioCondicaoPagamento(bool isFirstLoad = false);
		Task SyncConvenioCliente(bool isFirstLoad = false);
		Task SyncConvenioProduto(bool isFirstLoad = false);
	}
}