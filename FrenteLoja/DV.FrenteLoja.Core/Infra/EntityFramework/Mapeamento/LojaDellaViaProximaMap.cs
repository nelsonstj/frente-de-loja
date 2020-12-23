using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    class LojaDellaViaProximaMap: EntityTypeConfiguration<LojaDellaViaProxima>
    {
        public LojaDellaViaProximaMap()
        {
            ToTable("LOJA_DELLAVIA_PROXIMA");
            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);
            HasRequired(a => a.LojaDellaViaReferencia).WithMany(x => x.LojasDellaViaProximas).HasForeignKey(x => x.IdLojaDellaViaReferencia);
        }
    }
}
