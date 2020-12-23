using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class SolicitacaoDescontoVendaAlcadaMap:EntityTypeConfiguration<SolicitacaoDescontoVendaAlcada>
	{
		public SolicitacaoDescontoVendaAlcadaMap()
		{
			ToTable("SOLICITACAO_DESCONTO_VENDA_ALCADA");
            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo);
            HasRequired(a => a.OrcamentoItem).WithMany(x => x.SolicitacaoDescontoVendaAlcadaList).HasForeignKey(x => x.IdOrcamentoItem);
        }
	}
}