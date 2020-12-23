using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Banco:Entidade
    {
	    public virtual ICollection<OrcamentoFormaPagamento> FormaPagamentos { get; set; }
	    public virtual ICollection<Orcamento> Orcamentos { get; set; }
    }
}