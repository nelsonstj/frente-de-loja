using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class OrcamentoItemMap: EntityTypeConfiguration<OrcamentoItem>
	{
		public OrcamentoItemMap()
		{
			ToTable("ORCAMENTO_ITEM");
			Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo);
            HasOptional(a => a.DescontoModeloVenda).WithMany(x => x.OrcamentoItemList).HasForeignKey(x => x.IdDescontoModeloVenda);
        }
	}
}