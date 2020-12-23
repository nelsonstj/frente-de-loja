using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    class TipoVendaMap : EntityTypeConfiguration<TipoVenda>
    {
        public TipoVendaMap()
        {
            ToTable("TIPO_VENDA");

            Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}
