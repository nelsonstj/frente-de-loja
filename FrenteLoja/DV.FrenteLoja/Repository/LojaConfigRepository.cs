using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public class LojaConfigRepository
{
    protected string StrConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;

    public List<LojaConfig> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "SELECT * " +
                         "  FROM LOJA_CONFIG " +
                         " ORDER BY idLoja";

            SqlCommand cmd = new SqlCommand(sql, conn);
            var list = new List<LojaConfig>();
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

    public LojaConfig GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM LOJA_CONFIG " +
                         " WHERE idLoja = @Id ";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            LojaConfig g = null;
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

    public void Insert(LojaConfig entity)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "INSERT INTO LOJA_CONFIG (idLoja, ConvenioPadrao, DescontoMax)" +
                                            " VALUES(@idLoja, @ConvenioPadrao, @DescontoMax)";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idLoja", entity.idLoja);
            cmd.Parameters.AddWithValue("@ConvenioPadrao", entity.ConvenioPadrao);
            cmd.Parameters.AddWithValue("@DescontoMax", entity.DescontoMax);
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

    public void Update(LojaConfig entity)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "UPDATE LOJA_CONFIG" +
                         " SET ConvenioPadrao = @ConvenioPadrao" +
                         " ,DescontoMax = @DescontoMax" +
                         " WHERE idLoja = @idLoja";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idLoja", entity.idLoja);
            cmd.Parameters.AddWithValue("@ConvenioPadrao", entity.ConvenioPadrao);
            cmd.Parameters.AddWithValue("@DescontoMax", entity.DescontoMax);

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

    public void Delete(LojaConfig entity)
    {
        DeleteById(entity.idLoja);
    }

    public void DeleteById(string id)
    {
        using (SqlConnection conn = new SqlConnection(StrConn))
        {
            string sql = "DELETE FROM LOJA_CONFIG WHERE idLoja = @Id";

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

    private LojaConfig Map(SqlDataReader reader)
    {
        return new LojaConfig
        {
            idLoja = reader["idLoja"].ToString(),
            ConvenioPadrao = reader["ConvenioPadrao"].ToString(),
            DescontoMax = float.Parse(reader["DescontoMax"].ToString())
        };
    }
}
