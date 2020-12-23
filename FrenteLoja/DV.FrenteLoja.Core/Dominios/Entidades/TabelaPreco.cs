using System;
using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class TabelaPreco : Entidade
    {
        public string IdTabelaPreco { get; set; }
        public DateTime DataDe { get; set; }
	    public DateTime? DataAte { get; set; }
	    public string CodCondicaoPagamento { get; set; }
		public virtual ICollection<TabelaPrecoItem> TabelaPrecoItems { get; set; }
        public virtual ICollection<Convenio> ConvenioList { get; set; }
        public virtual ICollection<Orcamento> OrcamentoList { get; set; }
    }
}
