// DV.FrenteLoja.Repository.TransportadoraRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

public class TransportadoraRepository
{
    protected string strConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;

    public int TamanhoTransportadoraPorTermo(string termoBusca)
    {
        return GetTransportadoras(termoBusca).Count();
    }

    public List<TransportadoraDto> ObterTransportadorasPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetTransportadoras(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<TransportadoraDto> GetTransportadoras(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT DISTINCT Id, Descricao, CampoCodigo " +
                         "  FROM TRANSPORTADORA " +
                         " WHERE Descricao   LIKE '%' + @Busca + '%' " +
                         "   AND CampoCodigo LIKE '%' + @Busca + '%' " +
                         "   AND RegistroInativo <> 1 " +
                         " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToLower() ?? "");
            var list = new List<TransportadoraDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(new TransportadoraDto
                        {
                            Id = (long)reader["Id"],
                            Descricao = reader["Descricao"].ToString(),
                            CampoCodigo = reader["CampoCodigo"].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public TransportadoraDto GetTransportadorasById(long? id)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT DISTINCT Id, Descricao, CampoCodigo " +
                         "  FROM TRANSPORTADORA " +
                         " WHERE Id = @Id " +
                         " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            TransportadoraDto t = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        t = new TransportadoraDto
                        {
                            Id = (long)reader["Id"],
                            Descricao = reader["Descricao"].ToString(),
                            CampoCodigo = reader["CampoCodigo"].ToString()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return t;
        }
    }
}
