using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Util;
using Newtonsoft.Json;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class FormaPagamentoDto
	{
		public string Id { get; set; }
		public long IdOrcamento { get; set; }
		public string Descricao { get; set; }
		[JsonConverter(typeof(FormatEnumAsStringDescription))]
		public TipoFormaPagamento TipoFormaPagamento { get; set; }
		public string FormaPagamento { get; set; }
		public long IdBanco { get; set; }
		public string IdAdministradoraFinanceira { get; set; }
		public decimal Valor { get; set; }
		public decimal PercentualAcrescimo { get; set; }
	}
}