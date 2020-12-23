using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Produto : Entidade
    {
        public string IdProduto { get; set; }
        public string IdGrupoProduto { get; set; }
        public virtual GrupoProduto GrupoProduto { get; set; }
        public string CodigoFabricante { get; set; }
        public string IdGrupoServicoAgregado { get; set; }
        public string FabricantePeca { get; set; }
        public virtual ICollection<ProdutoComplemento> ProdutoComplemento { get; set; }

    }
}
