using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class AdministradoraFinanceiraMap: EntityTypeConfiguration<AdministradoraFinanceira>
	{
		public AdministradoraFinanceiraMap()
		{
			ToTable("ADMINISTRACAO_FINANCEIRA");
			Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
				new IndexAnnotation(new IndexAttribute { IsUnique = true }));
		}
	}
}