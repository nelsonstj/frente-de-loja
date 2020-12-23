using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class VeiculoMap : EntityTypeConfiguration<Veiculo>
    {
        public VeiculoMap()
        {
            ToTable("VEICULO");

            Property(h => h.IdFraga).IsRequired();
            Property(h => h.AnoInicial).IsOptional();
            Property(h => h.AnoFinal).IsOptional();
            Property(h => h.IdMarcaModeloVersao).IsOptional();
            Property(h => h.IdVersaoMotor).IsOptional();

            HasOptional(o => o.MarcaModeloVersao).WithMany().HasForeignKey(f => f.IdMarcaModeloVersao);
            HasOptional(o => o.VersaoMotor).WithMany().HasForeignKey(f => f.IdVersaoMotor);

            Ignore(i => i.Descricao);            
            Ignore(i => i.RegistroInativo);
			Ignore(i => i.CampoCodigo);
			Ignore(i => i.DataAtualizacao);
            Ignore(i => i.UsuarioAtualizacao);
        }
    }
}