using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class GrupoServicoAgregadoProduto : Entidade
    {
        public string IdProduto { get; set; }
        public string Item { get; set; }
        public bool PermiteAlterarQuantidade { get; set; }
        public decimal Quantidade { get; set; }
        public string IdGrupoServicoAgregado { get; set; }
    }
}
