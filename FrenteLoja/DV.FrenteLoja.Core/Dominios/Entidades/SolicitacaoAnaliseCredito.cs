using System;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class SolicitacaoAnaliseCredito : Entidade
	{
		public long IdOrcamento { get; set; }
		public virtual Orcamento Orcamento { get; set; }
		public DateTime DataSolicitacao { get; set; }
        public StatusSolicitacao StatusSolicitacaoAnaliseCredito { get; set; }
        public SituacaoAnaliseCredito SituacaoAnaliseCredito { get; set; }
		public DateTime? DataResposta { get; set; }
        public string RespostaSolicitacao { get; set; }
		public string NumeroContrato { get; set; }
    }
}