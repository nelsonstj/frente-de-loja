using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class ClienteVeiculoMap:EntityTypeConfiguration<ClienteVeiculo>
	{
		public ClienteVeiculoMap()
		{
			ToTable("CLIENTE_VEICULO");
            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo);
            Property(a => a.Placa).HasMaxLength(10);
            Property(a => a.UsuarioAtualizacao).HasMaxLength(100);
            Property(a => a.Observacoes).HasMaxLength(200);
            //HasRequired(a => a.Veiculo).WithMany(x => x.ClienteVeiculoList).HasForeignKey(x => x.VeiculoId);
            //HasRequired(a => a.Orcamento).WithMany(x => x.CheckupList).HasForeignKey(x => x.OrcamentoId);
        }
	}
}