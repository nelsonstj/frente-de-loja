using System.Data.Entity.ModelConfiguration;
using System.Reflection;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class OrcamentoItemEquipeMontagemMap: EntityTypeConfiguration<OrcamentoItemEquipeMontagem>
	{
		public OrcamentoItemEquipeMontagemMap()
		{
			ToTable("ORCAMENTO_ITEM_EQUIPE_MONTAGEM");
            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo);
            //HasRequired(a => a.Vendedor).WithMany(x => x.OrcamentoItemEquipeMontagemList).HasForeignKey(x => x.IdVendedor).WillCascadeOnDelete(false);
            HasRequired(a => a.OrcamentoItem).WithMany(x => x.EquipeMontagemList).HasForeignKey(x => x.IdOrcamentoItem).WillCascadeOnDelete(false);
        }
	}
}