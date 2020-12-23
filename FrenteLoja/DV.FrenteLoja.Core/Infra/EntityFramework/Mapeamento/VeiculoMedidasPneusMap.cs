using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    class VeiculoMedidasPneusMap : EntityTypeConfiguration<VeiculoMedidasPneus>
    {
        public VeiculoMedidasPneusMap()
        {
            ToTable("VEICULO_MEDIDAS_PNEUS");
        }
    }
}