using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CatalogoMap : EntityTypeConfiguration<Catalogo>
    {
        public CatalogoMap()
        {
            ToTable("CATALOGO");
            Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);
            Property(a => a.VersaoMotor).HasMaxLength(30);
        }
    }
}
