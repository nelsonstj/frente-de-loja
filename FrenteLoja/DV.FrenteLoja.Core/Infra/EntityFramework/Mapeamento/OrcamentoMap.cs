using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
	public class OrcamentoMap: EntityTypeConfiguration<Orcamento>
	{
		public OrcamentoMap()
		{
			ToTable("ORCAMENTO");
			Ignore(x => x.Descricao);
            //HasRequired(a => a.Convenio).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdConvenio).WillCascadeOnDelete(false);
            //HasRequired(a => a.Cliente).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdCliente);
            //HasRequired(a => a.TabelaPreco).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdTabelaPreco);
            //HasRequired(a => a.Vendedor).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdVendedor);
            //HasRequired(a => a.MarcaModeloVersao).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdMarcaModeloVersao);
            //HasRequired(a => a.LojaDellaVia).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdLojaDellaVia);
            //HasRequired(a => a.Operador).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdOperador);
            //HasOptional(a => a.Transportadora).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdTransportadora);
            //HasRequired(a => a.TipoVenda).WithMany(x => x.OrcamentoList).HasForeignKey(x => x.IdTipoVenda);
			HasOptional(a => a.Banco).WithMany(x => x.Orcamentos).HasForeignKey(x => x.IdBanco);
			Property(a => a.CampoCodigo).HasMaxLength(12).HasColumnAnnotation(IndexAnnotation.AnnotationName,
				new IndexAnnotation(new IndexAttribute { IsUnique = false }));
		}
	}
}