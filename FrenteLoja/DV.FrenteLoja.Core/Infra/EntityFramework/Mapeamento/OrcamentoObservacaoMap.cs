using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class OrcamentoObservacaoMap : EntityTypeConfiguration<OrcamentoObservacao>
	{
		public OrcamentoObservacaoMap()
		{
			ToTable("ORCAMENTO_OBSERVACAO");
			Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo);
            
		}
	}
}