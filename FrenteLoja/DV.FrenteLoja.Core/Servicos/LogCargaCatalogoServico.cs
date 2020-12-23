using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity;
using System.Linq;

namespace DV.FrenteLoja.Core.Servicos
{
    public class LogCargaCatalogoServico:ILogCargaCatalogoServico
	{
		private readonly IRepositorioEscopo _escopo;
		private readonly IRepositorio<CatalogoCargaLog> _repositorioLogCargaCatalog;

		public LogCargaCatalogoServico(IRepositorioEscopo escopo)
		{
			_escopo = escopo;
			_repositorioLogCargaCatalog = escopo.GetRepositorio<CatalogoCargaLog>();
		}

		public IQueryable<LogCargaCatalogoDto> ObterLogIntegracao(LogCargaCatalogoDto filtro)
		{
			IQueryable<CatalogoCargaLog> result;

            
            if (filtro.StatusIntegracao != null)
            {
                result = from log in _repositorioLogCargaCatalog.GetAll()
                         where (DbFunctions.TruncateTime(log.DataAtualizacao) >= DbFunctions.TruncateTime(filtro.DataInicio) &&
                         DbFunctions.TruncateTime(log.DataAtualizacao) <= DbFunctions.TruncateTime(filtro.DataFim)) &&
                         log.StatusIntegracao == filtro.StatusIntegracao
                         orderby log.DataAtualizacao
                         select log;
            }
            
            else
            {
                result = from log in _repositorioLogCargaCatalog.GetAll()
                         where DbFunctions.TruncateTime(log.DataAtualizacao) == DbFunctions.TruncateTime(filtro.DataInicio)
                         orderby log.DataAtualizacao
                         select log;
            }


			return result.ProjectTo<LogCargaCatalogoDto>();
		}

		public string ObterTextoErro(long id)
		{
			return _repositorioLogCargaCatalog.FindByKey(id)?.LogImportacao;
		}
	}
}