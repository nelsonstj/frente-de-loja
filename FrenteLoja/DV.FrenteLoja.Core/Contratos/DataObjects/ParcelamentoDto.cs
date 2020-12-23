using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class ParcelamentoDto
	{
		public string NomeCondicaoPagamento { get; set; }
		public decimal ValorAcrescimo { get; set; }
		public List<ParcelaDto> Parcelas { get; set; }
	}
}