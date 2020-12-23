using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class GrupoProdutoMap : EntityTypeConfiguration<GrupoProduto>
    {
        public GrupoProdutoMap()
        {
            ToTable("GRUPO_PRODUTO");
	        Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
		        new IndexAnnotation(new IndexAttribute { IsUnique = true }));

			//HasOptional(a => a.GrupoSubGrupo).WithMany(x => x.GrupoProdutos).HasForeignKey(x => x.IdGrupoSubGrupo);
			
		}
    }
}
