using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class CondicaoPagamento : Entidade
    {
	    public int QtdParcelas { get; set; }
	    public decimal ValorAcrescimo { get; set; }
	    public string FormaPagamento { get; set; }
	    public string FormaCondicaoPagamento { get; set; }
        public virtual ICollection<ConvenioCondicaoPagamento> ConvenioCondicaoPagamentoList { get; set; }
        /// <summary>
        /// Pode vir varios tipos de venda nesse campo, cada caracter da string corresponde a um tipo de venda
        /// </summary>
        public string ListaTipoVenda { get; set; }
	    public virtual ICollection<OrcamentoFormaPagamento> FormaPagamentos { get; set; }
	    public string TipoCondicaoPagamento { get; set; }
        public bool CondicaoDeVenda { get; set; }
    }
}
