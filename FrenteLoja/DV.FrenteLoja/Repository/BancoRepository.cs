// DV.FrenteLoja.Repository.FormaPagamentoRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class BancoRepository : AbstractRepository<BancoDto, int>
{
    public int TamanhoBancoPorTermo(string termoBusca)
    {
        return GetBancos(termoBusca).Count();
    }

    public List<BancoDto> ObterBancoPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetBancos(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<BancoDto> GetBancos(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT * " +
                      "  FROM DM_BANCOS " +
                      " WHERE (@Busca IS NULL OR (DS_BANCO  LIKE '%' + @Busca + '%')) " +
                      " ORDER BY DS_BANCO";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToUpper() ?? ((object)DBNull.Value));
            List<BancoDto> list = new List<BancoDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(Map(reader));
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

    public List<BancoDto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT * " +
                      "  FROM DM_BANCOS " +
                      " ORDER BY DS_BANCO";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<BancoDto> list = new List<BancoDto>();
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

    public override void Save(BancoDto entity)
    {}

    public override void Update(BancoDto entity)
    {}

    public override void Delete(BancoDto entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE DM_BANCOS WHERE ID_BANCO = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
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
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE DM_BANCOS WHERE ID_BANCO = @Id";
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

    private BancoDto Map(SqlDataReader reader)
    {
        return new BancoDto
        {
            Id = (long)reader["CD_BANCO"],
            Descricao = reader["NOME_BANCO"].ToString()
        };
    }
}
