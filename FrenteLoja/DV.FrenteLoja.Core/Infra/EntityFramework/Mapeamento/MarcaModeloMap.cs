using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class MarcaModeloMap : EntityTypeConfiguration<MarcaModelo>
    {
        public MarcaModeloMap()
        {
            ToTable("MARCA_MODELO");
			HasRequired(a => a.Marca).WithMany(x => x.MarcaModeloList).HasForeignKey(x => x.IdMarca);
	        Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
		        new IndexAnnotation(new IndexAttribute { IsUnique = false }));
		}
    }
}
