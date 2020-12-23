using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CheckupCarMap : EntityTypeConfiguration<CheckupCar>
    {
        public CheckupCarMap()
        {
            ToTable("CHECKUP_CAR");
            HasRequired(a => a.Checkup).WithMany(x => x.CheckupCarList).HasForeignKey(x => x.CheckupId);


            Property(a => a.ConvergenciaTotalInicial)
                .HasPrecision(18, 1);
            Property(a => a.ConvergenciaEsquerdoInicial)
                .HasPrecision(18, 1);
            Property(a => a.ConvergenciaDireitoInicial)
                .HasPrecision(18, 1);

            Property(a => a.ConvergenciaTotalFinal)
               .HasPrecision(18, 1);
            Property(a => a.ConvergenciaEsquerdoFinal)
                .HasPrecision(18, 1);
            Property(a => a.ConvergenciaDireitoFinal)
                .HasPrecision(18, 1);

            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);
        }
    }
}
