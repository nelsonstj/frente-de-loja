using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class AreaNegocioRepository : AbstractRepository<PDAreaNegocio, int>
{
    public AreaNegocioDto ObterAreaNegocioUsuarioLogado()
    {
        string codigoAreaNegocio = HttpContext.Current.User.Identity.GetAreaNegocio();
        if (string.IsNullOrWhiteSpace(codigoAreaNegocio))
            return null;
        AreaNegocioDto areaNegocio = GetById(codigoAreaNegocio);
        return Mapper.Map<AreaNegocioDto>(areaNegocio);
    }

    public int QuantidadeAreaNegociosPorTermo(string termoBusca)
    {
        return GetAreaNegocioByTermo(termoBusca).Count();
    }

    public List<AreaNegocioDto> ObterAreaNegocioPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        termoBusca = termoBusca.ToLower();
        return GetAreaNegocioByTermo(termoBusca).Skip(tamanhoPagina * (numeroPagina - 1)).Take(tamanhoPagina).ToList();
    }

    public List<AreaNegocioDto> GetAreaNegocioByTermo(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT *   FROM DM_AREANEG  WHERE NOME_AREA   LIKE '%' + @Busca + '%'  ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", ((object)termoBusca.ToLower()) ?? ((object)DBNull.Value));
            List<AreaNegocioDto> list = new List<AreaNegocioDto>();
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

    public AreaNegocioDto GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT * " +
                         "  FROM DM_AREANEG " +
                         " WHERE ID_AREA = @Id " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id ?? (object)DBNull.Value);
            AreaNegocioDto t = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        t = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return t;
        }
    }

    public List<AreaNegocioDto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT *   FROM DM_AREANEG  ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<AreaNegocioDto> list = new List<AreaNegocioDto>();
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

    public override void Save(PDAreaNegocio entity)
    {}

    public override void Update(PDAreaNegocio entity)
    {}

    public override void Delete(PDAreaNegocio entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "DELETE DM_AREANEG  WHERE ID_AREA = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.IdArea);
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
            string sql = "DELETE DM_AREANEG  WHERE ID_AREA = @Id";
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

    private AreaNegocioDto Map(SqlDataReader reader)
    {
        return new AreaNegocioDto
        {
            Id = reader["ID_AREA"].ToString().Trim(),
            Descricao = reader["NOME_AREA"].ToString().Trim(),
            Sigla = reader["ID_AREA"].ToString().Trim()
        };
    }
}
