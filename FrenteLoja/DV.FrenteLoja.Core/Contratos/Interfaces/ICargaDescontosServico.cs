using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICargaDescontosServico
	{
		Task SyncDescontoVendaAlcada(bool isFirstLoad = false);
		Task SyncDescontoVendaAlcadaGrupoProduto(bool isFirstLoad = false);
		Task SyncDescontoModeloDeVenda(bool isFirstLoad = false);
	}
}