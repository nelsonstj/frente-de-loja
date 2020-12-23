using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class DescontoVendaAlcadaGrupoProdutoMap: EntityTypeConfiguration<DescontoVendaAlcadaGrupoProduto>
	{
		public DescontoVendaAlcadaGrupoProdutoMap()
		{
			ToTable("DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO");
            Ignore(x => x.Descricao);
            HasRequired(a => a.GrupoProduto).WithMany(x => x.DescontoVendaAlcadaGrupoProdutoList).HasForeignKey(x => x.GrupoProdutoId);
			Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
				new IndexAnnotation(new IndexAttribute { IsUnique = true }));
		}
	}
}