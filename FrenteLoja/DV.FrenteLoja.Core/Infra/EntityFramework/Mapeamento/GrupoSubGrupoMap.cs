using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class GrupoSubGrupoMap : EntityTypeConfiguration<GrupoSubGrupo>
    {
        public GrupoSubGrupoMap()
        {
            ToTable("GRUPO_SUB_GRUPO");

			//Property(h => h.IdSubGrupo).IsRequired();
			//Property(h => h.Grupo).IsRequired();						
			//HasRequired(a => a.SubGrupo).WithMany(x => x.GrupoSubGrupos).HasForeignKey(x => x.IdSubGrupo);

			Ignore(i => i.CampoCodigo);
			Ignore(i => i.RegistroInativo);
		}
    }
}