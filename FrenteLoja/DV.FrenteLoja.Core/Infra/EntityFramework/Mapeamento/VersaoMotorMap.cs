using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class VersaoMotorMap : EntityTypeConfiguration<VersaoMotor>
    {
        public VersaoMotorMap()
        {
            ToTable("VERSAO_MOTOR");

            Ignore(i => i.CampoCodigo);
            Ignore(i => i.RegistroInativo);
            Ignore(i => i.DataAtualizacao);
            Ignore(i => i.UsuarioAtualizacao);
        }
    }
}
