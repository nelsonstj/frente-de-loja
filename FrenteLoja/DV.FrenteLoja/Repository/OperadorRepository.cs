// DV.FrenteLoja.Repository.OperadorRepository
using AutoMapper;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class OperadorRepository : AbstractRepository<OperadorPD, int>
{
    public OperadorPD ObterOperadorUsuarioLogado()
    {
        string operadorCod = HttpContext.Current.User.Identity.GetIdOperador();
        OperadorPD operador = GetById(operadorCod);
        if (operador == null)
            return null;
        return Mapper.Map<OperadorPD>(operador);
    }

    public int TamanhoOperadorPorTermo(string termoBusca)
    {
        return GetOperadores(termoBusca).Count();
    }

    public List<OperadorPD> ObterOperadorPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetOperadores(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<OperadorPD> GetOperadores(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_OPERADOR " +
                      " WHERE (@Busca IS NULL OR (NOME_OPERADOR  LIKE '%' + @Busca + '%')) " +
                      " ORDER BY NOME_OPERADOR";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToLower() ?? (object)DBNull.Value);
            var list = new List<OperadorPD>();
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

    public List<OperadorPD> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT * FROM DM_OPERADOR ORDER BY NOME_OPERADOR";
            var cmd = new SqlCommand(sql, conn);
            var list = new List<OperadorPD>();
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

    public OperadorPD GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * FROM DM_OPERADOR WHERE ID_OPERADOR = @Id ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id.PadLeft(6, '0'));
            OperadorPD o = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        o = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return o;
        }
    }

    public override void Save(OperadorPD entity)
    {}

    public override void Update(OperadorPD entity)
    {}

    public override void Delete(OperadorPD entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "DELETE DM_OPERADOR WHERE ID_OPERADOR = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.IdOperador);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public void DeleteById(int id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "DELETE DM_OPERADOR WHERE ID_OPERADOR = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    private OperadorPD Map(SqlDataReader reader)
    {
        return new OperadorPD
        {
            IdOperador = reader["ID_OPERADOR"].ToString(),
            IdConsultor = reader["ID_CONSULTOR"].ToString(),
            NomeOperador = reader["NOME_OPERADOR"].ToString()
        };
    }
}
