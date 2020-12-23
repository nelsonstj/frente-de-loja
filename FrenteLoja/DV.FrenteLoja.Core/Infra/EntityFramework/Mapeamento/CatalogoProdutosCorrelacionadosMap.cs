using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CatalogoProdutosCorrelacionadosMap : EntityTypeConfiguration<CatalogoProdutosCorrelacionados>
    {
        public CatalogoProdutosCorrelacionadosMap()
        {
            ToTable("CATALOGO_PRODUTO_CORRELACIONADO");
            HasRequired(a => a.Catalogo).WithMany(x => x.CatalogoProdutoRelacionadoList).HasForeignKey(x => x.IdCatalogo);
            Ignore(a => a.Descricao).Ignore(a => a.CampoCodigo).Ignore(a => a.RegistroInativo);
        }
    }
}
