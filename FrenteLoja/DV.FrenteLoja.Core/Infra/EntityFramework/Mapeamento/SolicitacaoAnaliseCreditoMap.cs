using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class SolicitacaoAnaliseCreditoMap : EntityTypeConfiguration<SolicitacaoAnaliseCredito>
    {
        public SolicitacaoAnaliseCreditoMap()
        {
            ToTable("SOLICITACAO_ANALISE_CREDITO");
            Ignore(x => x.Descricao);
            HasRequired(a => a.Orcamento).WithMany(x => x.SolicitacaoAnaliseCreditoList).HasForeignKey(x => x.IdOrcamento);
        }
    }
}
