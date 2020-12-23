using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICargaOrcamentoServico
	{
		Task SyncOrcamento(bool isFirstLoad = false);
		Task SyncSolicitacaoAnaliseCredito(bool isFirstLoad = false);
	}
}