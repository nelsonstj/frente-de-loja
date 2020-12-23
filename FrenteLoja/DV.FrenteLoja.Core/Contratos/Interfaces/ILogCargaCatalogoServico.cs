using System.Linq;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ILogCargaCatalogoServico
	{
		IQueryable<LogCargaCatalogoDto> ObterLogIntegracao(LogCargaCatalogoDto filtro);

		string ObterTextoErro(long id);
	}
}