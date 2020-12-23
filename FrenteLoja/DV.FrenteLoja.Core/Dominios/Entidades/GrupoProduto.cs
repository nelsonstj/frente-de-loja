using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class GrupoProduto : Entidade
    {
        [Display(Name = "Id Grupo")]
        public string IdGrupoProduto { get; set; }
        [Display(Name = "Id Subgrupo")]
        public string IdGrupoSubGrupo { get; set; }

        [Display(Name = "Grupo Subgrupo")]
        public virtual GrupoSubGrupo GrupoSubGrupo { get; set; }
		public virtual ICollection<Produto> ProdutoList { get; set; }
        public virtual ICollection<DescontoVendaAlcadaGrupoProduto> DescontoVendaAlcadaGrupoProdutoList { get; set; }
        
    }
}