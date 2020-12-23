using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class OperadorMap: EntityTypeConfiguration<Operador>
	{
		public OperadorMap()
		{
			//ToTable("OPERADOR");
			//HasOptional(x => x.Vendedor).WithMany().HasForeignKey(x => x.VendedorId);
		}
	}
}