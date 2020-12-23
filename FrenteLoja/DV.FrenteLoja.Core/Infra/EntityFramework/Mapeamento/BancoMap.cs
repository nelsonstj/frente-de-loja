using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class BancoMap : EntityTypeConfiguration<Banco>
    {
        public BancoMap()
        {
            ToTable("BANCO");
	        Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
		        new IndexAnnotation(new IndexAttribute { IsUnique = true }));
		}
    }
}