using System;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class SolicitacaoDescontoVendaAlcada : Entidade
	{
		public long IdOrcamentoItem { get; set; }
		public virtual OrcamentoItem OrcamentoItem { get; set; }
		public DateTime DataSolicitacao { get; set; }
		public string ObservacaoItem { get; set; }
		public string ObservacaoGeral { get; set; }
		public string RespostaSolicitacao { get; set; }
		public DateTime? DataResposta { get; set; }
		public StatusSolicitacao StatusSolicitacaoAlcada { get; set; }
		public decimal ValorDesconto { get; set; }
		public decimal PercentualDesconto { get; set; }
        public SituacaoDescontoAlcada Situacao { get; set; }
    }
}