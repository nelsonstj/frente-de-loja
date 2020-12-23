using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class CatalogoProdutosCorrelacionados : Entidade
    {
        public long IdCatalogo { get; set; }
        public virtual Catalogo Catalogo { get; set; }
        public string CodigoFabricante { get; set; }
    }
}
