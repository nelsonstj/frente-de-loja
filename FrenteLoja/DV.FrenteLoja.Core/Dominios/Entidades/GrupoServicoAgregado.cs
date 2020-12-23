using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class GrupoServicoAgregado : Entidade
    {
        public string Filial { get; set; }
        public virtual ICollection<Produto> ProdutoList { get; set; }
        public virtual ICollection<GrupoServicoAgregadoProduto> KitServicoList { get; set; }
    }
}
