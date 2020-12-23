using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Transportadora:Entidade
    {
	  public virtual ICollection<Orcamento> OrcamentoList { get; set; }
    }
}