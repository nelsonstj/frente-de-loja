using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class MarcaModeloVersaoMap:EntityTypeConfiguration<MarcaModeloVersao>
	{
		public MarcaModeloVersaoMap()
		{
			ToTable("MARCA_MODELO_VERSAO");
			Ignore(x => x.CampoCodigo);
            HasRequired(a => a.MarcaModelo).WithMany(x => x.MarcaModeloVersoes).HasForeignKey(x => x.IdMarcaModelo);
        }
	}
}