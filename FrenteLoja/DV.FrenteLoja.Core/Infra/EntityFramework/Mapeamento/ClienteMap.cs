using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class ClienteMap: EntityTypeConfiguration<Cliente>
	{
		public ClienteMap()
		{
			ToTable("CLIENTE");
			Ignore(x => x.Descricao);
			Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation("UK_CLienteLoja",
				new IndexAnnotation(new IndexAttribute("UK_CLienteLoja", 1) { IsUnique = true }));

			Property(a => a.Loja).HasMaxLength(3).HasColumnAnnotation("UK_CLienteLoja",
				new IndexAnnotation(new IndexAttribute("UK_CLienteLoja", 2){ IsUnique = true}));

            Property(a => a.TipoCliente).HasMaxLength(1);
		}
	}
}