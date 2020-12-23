using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CondicaoPagamentoMap : EntityTypeConfiguration<CondicaoPagamento>
    {
        public CondicaoPagamentoMap()
        {
            ToTable("CONDICAO_PAGAMENTO");
	        Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
		        new IndexAnnotation(new IndexAttribute { IsUnique = true }));
		}
    }
}
