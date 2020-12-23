using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoRetornoProtheusDto
	{
		public IEnumerable<OrcamentoProtheusDto> OrcamentosEnvio { get; set; }
	}
}