using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class CatalogoArquivoMap : EntityTypeConfiguration<CatalogoArquivo>
	{
		public CatalogoArquivoMap()
		{
			ToTable("CATALOGO_ARQUIVO");
			Ignore(a => a.Descricao).Ignore(a => a.CampoCodigo).Ignore(a => a.RegistroInativo);
		}
	}
}
