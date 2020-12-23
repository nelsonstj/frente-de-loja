using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class DescontoModeloVendaMap: EntityTypeConfiguration<DescontoModeloVenda>
	{
		public DescontoModeloVendaMap()
		{
			ToTable("DESCONTO_MODELO_VENDA");
            Ignore(x => x.Descricao);
        }
	}
}