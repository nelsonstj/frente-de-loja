using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICondicaoPagamentoParcelasApi
	{
		List<ParcelaDto> ObterParcelas(string codCondicaoPagamento, decimal valorTotal, out string erros);
	}
}