using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IVendedorServico
	{
		int TamanhoVendedorPorTermo(string termoBusca);
		List<VendedorDto> ObterVendedorPorNome(string termoBusca, int tamanhoPagina, int numeroPagina);
	}
}