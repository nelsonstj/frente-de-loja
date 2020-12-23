using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class CatalogoCargaLogMap : EntityTypeConfiguration<CatalogoCargaLog>
    {
        public CatalogoCargaLogMap()
        {
            ToTable("CATALOGO_CARGA_LOG");
	        Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);

		}
	}
}
