using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class GrupoSubGrupo : Entidade
    {
        public string IdGrupoSubGrupo { get; set; }
        public string IdSubGrupo { get; set; }
        public TipoProdutoElasticSearch Grupo { get; set; }

		public virtual SubGrupo SubGrupo { get; set; }
		public virtual ICollection<GrupoProduto> GrupoProdutos { get; set; }
	}
}
