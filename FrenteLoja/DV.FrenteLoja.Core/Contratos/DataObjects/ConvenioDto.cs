using System;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ConvenioDto
    {
	    public string Descricao { get; set; }
        public string CampoCodigo { get; set; }
        public string Id { get; set; }
        public string IdConvenio { get; set; }
        public string Observacoes { get; set; }
	    public string IdTabelaPreco { get; set; }
	    public virtual TabelaPrecoDto TabelaPreco { get; set; }
	    public DateTime DataInicioVigencia { get; set; }
	    public DateTime? DataFimVigencia { get; set; }
	    public bool TrocaCliente { get; set; }
	    public TrocaPrecoConvenio TrocaPreco { get; set; }
	    public bool TrocaProduto { get; set; }
	    public string IdCliente { get; set; }
	    public virtual ClienteDto Cliente { get; set; }
	    public bool TrocaTabelaPreco { get; set; }
	}
}
