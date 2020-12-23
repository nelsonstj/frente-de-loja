using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICargaProdutoServico
    {
        Task SyncGrupoProduto(bool isFirstLoad = false);
        Task SyncGrupoServicoAgregadosProdutos(bool isFirstLoad = false);
        Task SyncProduto(bool isFirstLoad = false);
        Task SyncProdutoComplemento(bool isFirstLoad = false);
    }
}