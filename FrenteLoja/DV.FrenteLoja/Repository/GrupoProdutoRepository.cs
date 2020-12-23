using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public class GrupoProdutoRepository
{
    protected string StrConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;

    public List<GrupoProduto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM GRUPO_PRODUTO " +
                         " ORDER BY Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var list = new List<GrupoProduto>();
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

    public GrupoProduto GetById(long? id)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM GRUPO_PRODUTO " +
                         " WHERE Id = @Id ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            GrupoProduto g = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        g = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return g;
        }
    }

    public void Insert(GrupoProduto entity)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "INSERT INTO grupo_produto (Descricao, CampoCodigo, RegistroInativo, DataAtualizacao)" +
                                            " VALUES(@Descricao, @Codigo, 0, GETDATE())";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Descricao", entity.Descricao);
            cmd.Parameters.AddWithValue("@Codigo", entity.CampoCodigo);
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

    public void Update(GrupoProduto entity)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "UPDATE GRUPO_PRODUTO" +
                         " SET Descricao = @Descricao" +
                         " ,CampoCodigo = @Codigo" +
                         " ,DataAtualizacao = GETDATE()" +
                         "WHERE ID = @Id";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@Descricao", entity.Descricao);
            cmd.Parameters.AddWithValue("@Codigo", entity.CampoCodigo);

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

    public void Delete(GrupoProduto entity)
    {
        DeleteById(entity.Id);
    }

    public void DeleteById(long? id)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "DELETE FROM grupo_produto WHERE ID = @Id";

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

    private GrupoProduto Map(SqlDataReader reader)
    {
        return new GrupoProduto
        {
            Id = (long)reader["Id"],
            Descricao = reader["Descricao"].ToString(),
            CampoCodigo = reader["CampoCodigo"].ToString()
        };
    }
}
