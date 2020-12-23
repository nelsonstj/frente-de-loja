using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CheckupTruckMap : EntityTypeConfiguration<CheckupTruck>
    {
        public CheckupTruckMap()
        {
            ToTable("CHECKUP_TRUCK");
            HasRequired(a => a.Checkup).WithMany(x => x.CheckupTruckList).HasForeignKey(x => x.CheckupId);

            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);
        }
    }
}
