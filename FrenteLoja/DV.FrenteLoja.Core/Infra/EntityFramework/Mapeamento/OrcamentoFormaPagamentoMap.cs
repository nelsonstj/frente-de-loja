using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class OrcamentoFormaPagamentoMap:EntityTypeConfiguration<OrcamentoFormaPagamento>
	{
		public OrcamentoFormaPagamentoMap()
		{
			ToTable("ORCAMENTO_FORMA_PAGAMENTO");
			//HasRequired(a => a.Orcamento).WithMany(x => x.FormaPagamentos).HasForeignKey(x => x.IdOrcamento);
			//HasOptional(a => a.AdministradoraFinanceira).WithMany(x => x.FormaPagamentos).HasForeignKey(x => x.IdAdministradoraFinanceira);
			//HasOptional(a => a.Banco).WithMany(x => x.FormaPagamentos).HasForeignKey(x => x.IdBanco);
            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo);
		}
	}
}