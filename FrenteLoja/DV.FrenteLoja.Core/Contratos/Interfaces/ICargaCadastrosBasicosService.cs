using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICargaCadastrosBasicosService
    {
	    Task SyncTipoVenda(bool isFirstLoad = false);
        Task SyncMarcas(bool isFirstLoad = false);
        Task SyncVendedores(bool isFirstLoad = false);
        Task SyncTransportadoras(bool isFirstLoad = false);
        Task SyncBancos(bool isFirstLoad = false);
        Task SyncModelos(bool isFirstLoad = false);
        Task SyncLojasDellaVia(bool isFirstLoad = false);
	    Task SyncOperador(bool isFirstLoad = false);
	    Task SyncParametroGeral(bool isFirstLoad = false);
	    Task SyncAdministracaoFinanceira(bool isFirstLoad = false);

    }
}