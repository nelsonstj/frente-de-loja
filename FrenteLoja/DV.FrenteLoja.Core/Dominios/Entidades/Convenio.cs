using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;
using System;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Convenio : Entidade
    {
        public string IdConvenio { get; set; }
        public string Observacoes { get; set; }
        public string IdTabelaPreco { get; set; }
        public virtual TabelaPreco TabelaPreco { get; set; }
        public DateTime DataInicioVigencia { get; set; }
        public DateTime? DataFimVigencia { get; set; }
        public bool TrocaCliente { get; set; } 
        public TrocaPrecoConvenio TrocaPreco { get; set; }
		public bool TrocaProduto { get; set; }
        public string IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public bool TrocaTabelaPreco { get; set; }
        public virtual List<ConvenioCondicaoPagamento> ConvenioCondicaoPagamentoList { get; set; }
        public virtual List<ConvenioProduto> ConvenioProdutoList { get; set; }
        public virtual List<ConvenioCliente> ConvenioClienteList { get; set; }
        public virtual List<Orcamento> OrcamentoList { get; set; }
    }
}
