using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity;
using System.Linq;

namespace DV.FrenteLoja.Core.Servicos
{
    public class LogIntegracaoServico : ILogIntegracaoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<LogIntegracao> _repositorioLogIntegracao;

        public LogIntegracaoServico(IRepositorioEscopo escopo)
        {
            _escopo = escopo;
            _repositorioLogIntegracao = escopo.GetRepositorio<LogIntegracao>();
        }

        public IQueryable<LogIntegracaoDto> ObterLogIntegracao(LogIntegracaoDto filtro)
        {
            IQueryable<LogIntegracao> result;
            if (filtro.StatusIntegracao != null && filtro.TipoTabelaProtheus != null)
            {
                result = from log in _repositorioLogIntegracao.GetAll()
                         where (DbFunctions.TruncateTime(log.DataAtualizacao) >= DbFunctions.TruncateTime(filtro.DataInicio) &&
                         DbFunctions.TruncateTime(log.DataAtualizacao) <= DbFunctions.TruncateTime(filtro.DataFim)) &&
                         log.TipoTabelaProtheus == filtro.TipoTabelaProtheus &&
                         log.StatusIntegracao == filtro.StatusIntegracao
                         orderby log.DataAtualizacao descending
                         select log;
            }
            else if (filtro.StatusIntegracao != null && filtro.TipoTabelaProtheus == null)
            {
                result = from log in _repositorioLogIntegracao.GetAll()
                         where (DbFunctions.TruncateTime(log.DataAtualizacao) >= DbFunctions.TruncateTime(filtro.DataInicio) &&
                         DbFunctions.TruncateTime(log.DataAtualizacao) <= DbFunctions.TruncateTime(filtro.DataFim)) &&
                         log.StatusIntegracao == filtro.StatusIntegracao
                         orderby log.DataAtualizacao descending
                         select log;
            }
            else if (filtro.TipoTabelaProtheus != null && filtro.StatusIntegracao == null)
            {
                result = from log in _repositorioLogIntegracao.GetAll()
                         where (DbFunctions.TruncateTime(log.DataAtualizacao) >= DbFunctions.TruncateTime(filtro.DataInicio) &&
                         DbFunctions.TruncateTime(log.DataAtualizacao) <= DbFunctions.TruncateTime(filtro.DataFim)) &&
                         log.TipoTabelaProtheus == filtro.TipoTabelaProtheus
                         orderby log.DataAtualizacao descending
                         select log;
            }
            else
            {
                result = from log in _repositorioLogIntegracao.GetAll()
                         where DbFunctions.TruncateTime(log.DataAtualizacao) >= DbFunctions.TruncateTime(filtro.DataInicio)
                         orderby log.DataAtualizacao descending
                         select log;
            }
            return result.ProjectTo<LogIntegracaoDto>();
        }

        public string ObterTextoErro(long id)
        {
            return _repositorioLogIntegracao.FindByKey(id)?.Log;
        }
    }
}
