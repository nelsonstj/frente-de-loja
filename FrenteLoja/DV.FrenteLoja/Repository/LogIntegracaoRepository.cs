using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

namespace DV.FrenteLoja.Repository
{
    public class LogIntegracaoRepository
    {
        protected string strConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<LogIntegracao> _repositorioLogIntegracao;

        public LogIntegracaoRepository(IRepositorioEscopo escopo)
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

        public List<LogIntegracaoDto> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string sql = "SELECT DISTINCT * " +
                             "  FROM LOG_INTEGRACAO " +
                             " ORDER BY DataAtualizacao";
                SqlCommand cmd = new SqlCommand(sql, conn);
                var list = new List<LogIntegracaoDto>();
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        while (reader.Read())
                            list.Add(Map(reader));
                }
                catch (Exception e)
                {
                    throw e;
                }
                return list;
            }
        }

        private LogIntegracaoDto Map(SqlDataReader reader)
        {
            return new LogIntegracaoDto
            {
                Id = (long)reader["Id"],
                TipoTabelaProtheus = (TipoTabelaProtheus)(int)reader["TipoTabelaProtheus"],
                StatusIntegracao = (StatusIntegracao)(int)reader["StatusIntegracao"],
                Log = reader["Log"].ToString().Trim(),
                DadosIntegracaoJson = reader["DadosIntegracaoJson"].ToString().Trim(),
                DataAtualizacao = (DateTime)reader["DataAtualizacao"]
            };
        }
    }
}
