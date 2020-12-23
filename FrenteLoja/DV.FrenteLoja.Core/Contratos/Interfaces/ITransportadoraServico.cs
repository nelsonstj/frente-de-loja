using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ITransportadoraServico
	{
		int TamanhoTransportadoraPorTermo(string termoBusca);
		List<TransportadoraDto> ObterTransportadorasPorNome(string termoBusca, int tamanhoPagina, int numeroPagina);
	}
}