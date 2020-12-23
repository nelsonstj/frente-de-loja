using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.EntityFramework.Mapeamento
{
    public class LogIntegracaoMap: EntityTypeConfiguration<LogIntegracao>
    {
        public LogIntegracaoMap()
        {
            ToTable("LOG_INTEGRACAO");
            Ignore(x => x.Descricao).Ignore(x => x.CampoCodigo).Ignore(x => x.RegistroInativo);
        }
    }
}
