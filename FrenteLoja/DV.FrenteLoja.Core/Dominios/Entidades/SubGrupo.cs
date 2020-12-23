using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class SubGrupo : Entidade
    {
		public virtual ICollection<GrupoSubGrupo> GrupoSubGrupos { get; set; }
	}
}
