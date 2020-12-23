// DV.FrenteLoja.Repository.TabelaPrecoRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

public class TabelaPrecoRepository : AbstractRepository<TabelaPreco, int>
{
    public int TamanhoTabelaPrecoPorTermo(string termoBusca)
    {
        return GetTabelasPreco(termoBusca).Count();
    }

    public List<TabelaPrecoDto> ObterTabelaPrecoPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetTabelasPreco(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<TabelaPrecoDto> GetTabelasPreco(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT ID_TABELA ,DS_TABELA ,DT_DE ,DT_ATE " +
                         "  FROM DM_TABELAS_DE_PRECOS " +
                         " WHERE ((@termoBusca    IS NULL OR (ID_TABELA LIKE '%' + @termoBusca + '%'))" +
                         "        OR (@termoBusca IS NULL OR (DS_TABELA LIKE '%' + @termoBusca + '%')))" +
                         "   AND (DT_ATE >= @data  OR DT_ATE = '' OR DT_ATE IS NULL)" +
                         " ORDER BY ID_TABELA";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@termoBusca", termoBusca.ToLower() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyyMMdd"));
            var list = new List<TabelaPrecoDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapDto(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public TabelaPreco GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_TABELAS_DE_PRECOS " +
                         " WHERE ID_TABELA = @Id " +
                         " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id ?? (object)DBNull.Value);
            TabelaPreco t = null;
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

    public TabelaPrecoDto GetTabelaPrecoDto(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_TABELAS_DE_PRECOS " +
                         " WHERE ID_TABELA = @Id " +
                         " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id ?? (object)DBNull.Value);
            TabelaPrecoDto t = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        t = MapDto(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return t;
        }
    }

    public TabelaPrecoItem GetTabelaPrecoItem(string tabelaPrecoId, string codProduto)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_DM_TABELAS_DE_PRECOS", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@COD_TAB", tabelaPrecoId);
            cmd.Parameters.AddWithValue("@COD_PROD", codProduto ?? (object)DBNull.Value);
            TabelaPrecoItem t = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        t = new TabelaPrecoItem
                        {
                            TabelaPrecoId = reader["ID_TABELA"].ToString(),
                            Descricao = reader["DS_TABELA"].ToString(),
                            ProdutoId = reader["ID_PRODUTO"].ToString(),
                            PrecoVenda = (decimal)Convert.ToDouble(reader["VL_PRCVEN"]),
                            CampoCodigo = reader["ID_TABELA"].ToString()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return t;
        }
    }

    public override void Save(TabelaPreco entity)
    {}

    public override void Update(TabelaPreco entity)
    {}

    public override void Delete(TabelaPreco entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "DELETE DM_TABELAS_DE_PRECOS " +
                         " WHERE ID_TABELA = @Id";
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
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "DELETE DM_TABELAS_DE_PRECOS " +
                         " WHERE ID_TABELA = @Id";
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

    private TabelaPreco Map(SqlDataReader reader)
    {
        TabelaPreco tabelaPreco = new TabelaPreco();
        tabelaPreco.IdTabelaPreco = reader["ID_TABELA"].ToString();
        tabelaPreco.Descricao = reader["DS_TABELA"].ToString();
        tabelaPreco.CampoCodigo = reader["ID_TABELA"].ToString();
        tabelaPreco.DataDe = DateTime.ParseExact(reader["DT_DE"].ToString().Substring(6, 2) + "/" + reader["DT_DE"].ToString().Substring(4, 2) + "/" + reader["DT_DE"].ToString().Substring(0, 4) + " 00:00:00,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture);
        //tabelaPreco.DataAte = (reader["DT_ATE"] == DBNull.Value || reader["DT_ATE"] == null) ? null : new DateTime?(DateTime.ParseExact(reader["DT_ATE"].ToString().Substring(6, 2) + "/" + reader["DT_ATE"].ToString().Substring(4, 2) + "/" + reader["DT_ATE"].ToString().Substring(0, 4) + " 23:59:59,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture));
        tabelaPreco.DataAte = string.IsNullOrEmpty(reader["DT_ATE"].ToString().Trim()) ? null : new DateTime?(DateTime.ParseExact(reader["DT_ATE"].ToString().Substring(6, 2) + "/" + reader["DT_ATE"].ToString().Substring(4, 2) + "/" + reader["DT_ATE"].ToString().Substring(0, 4) + " 23:59:59,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture));
        return tabelaPreco;
    }

    private TabelaPrecoDto MapDto(SqlDataReader reader)
    {
        return new TabelaPrecoDto
        {
            Id = reader["ID_TABELA"].ToString(),
            Descricao = reader["DS_TABELA"].ToString(),
            CampoCodigo = reader["ID_TABELA"].ToString()
        };
    }
}
