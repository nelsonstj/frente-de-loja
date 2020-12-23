using DV.FrenteLoja.Core.Contratos.Enums;
using System;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class SolicitacaoAnaliseCreditoDto
    {
        public long IdOrcamento { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public StatusSolicitacao StatusSolicitacaoAnaliseCredito { get; set; }
        public SituacaoAnaliseCredito SituacaoAnaliseCredito { get; set; }
        public DateTime? DataResposta { get; set; }
        public string RespostaSolicitacao { get; set; }
    }
}
