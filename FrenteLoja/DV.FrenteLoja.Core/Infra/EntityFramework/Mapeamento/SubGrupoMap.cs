using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class SubGrupoMap : EntityTypeConfiguration<SubGrupo>
    {
        public SubGrupoMap()
        {
            ToTable("SUB_GRUPO");

			Ignore(i => i.CampoCodigo);
			Ignore(i => i.RegistroInativo);
		}
    }
}