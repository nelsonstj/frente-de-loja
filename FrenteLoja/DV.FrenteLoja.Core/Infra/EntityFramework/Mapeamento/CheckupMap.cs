using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;



namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CheckupMap : EntityTypeConfiguration<Checkup>
    {
        public CheckupMap()
        {
            ToTable("CHECKUP");
            HasRequired(a => a.Orcamento).WithMany(x => x.CheckupList).HasForeignKey(x => x.OrcamentoId);
            HasOptional(a => a.Vendedor).WithMany(x => x.CheckupList).HasForeignKey(x => x.VendedorId);
            HasOptional(a => a.TecnicoResponsavel).WithMany(x => x.CheckupTecnicoList)
                .HasForeignKey(x => x.TecnicoResponsavelId);           

            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);
        }
    }
}
