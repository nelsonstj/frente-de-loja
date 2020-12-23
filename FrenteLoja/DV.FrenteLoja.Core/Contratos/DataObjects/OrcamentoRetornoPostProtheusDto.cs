using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoRetornoPostProtheusDto
	{
		public string CampoCodigoOrcamento { get; set; }
		public SituacaoAnaliseCredito SituacaoAnaliseCredito { get; set; }
		public string NumeroContratoSolicitacaoAnaliseCredito { get; set; }
		public string RespostaSolicitacao { get; set; }
		public bool ExisteAnaliseCredito { get; set; }
		public string CampoCodigoMarca { get; set; }
		public string CampoCodigoModelo { get; set; }

	}
}