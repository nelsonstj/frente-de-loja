using DV.FrenteLoja.Core.Contratos.Enums;
using System;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class SolicitacaoDescontoVendaAlcadaDto
    {
        public long IdOrcamentoItem { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string ObservacaoItem { get; set; }
        public string ObservacaoGeral { get; set; }
        public string RespostaSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public StatusSolicitacao StatusSolicitacaoAlcada { get; set; }
        public SituacaoDescontoAlcada Situacao { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal PercentualDesconto { get; set; }
    }
}
