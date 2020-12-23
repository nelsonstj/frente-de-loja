using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ITabelaPrecoServico
	{
		int TamanhoTabelaPrecoPorTermo(string termoBusca);


		List<TabelaPrecoDto> ObterTabelaPrecoPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina);
	}
}