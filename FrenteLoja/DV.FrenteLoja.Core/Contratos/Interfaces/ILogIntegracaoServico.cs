using DV.FrenteLoja.Core.Contratos.DataObjects;
using System.Linq;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ILogIntegracaoServico
    {
        IQueryable<LogIntegracaoDto> ObterLogIntegracao(LogIntegracaoDto filtro);

        string ObterTextoErro(long id);

    }
}
