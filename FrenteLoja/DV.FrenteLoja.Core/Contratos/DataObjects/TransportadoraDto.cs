using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class TransportadoraDto
	{
		public long Id { get; set; }
		public string Descricao { get; set; }
		public string CampoCodigo { get; set; }
		public ICollection<OrcamentoDto> OrcamentoList { get; set; }

	}
}