// DV.FrenteLoja.Repository.ConvenioRepository
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

public class ConvenioRepository : AbstractRepository<Convenio, int>
{
    public ConvenioDto ObterConvenioUsuarioLogado()
    {
        string convenioCodigo = HttpContext.Current.User.Identity.GetConvenioPadrao();
        Convenio convenio = GetById(convenioCodigo);
        if (convenio == null)
            return null;
        return Mapper.Map<ConvenioDto>(convenio);
    }

    public int TamanhoConveniosPorTermo(string termoBusca, string cliente, string loja)
    {
        return GetConvenios(termoBusca, cliente, loja).Count();
    }

    public List<ConvenioDto> ObterConveniosPorTermo(int tamanhoPagina, int numeroPagina, string termoBusca, string cliente, string loja)
    {
        return GetConvenios(termoBusca, cliente, loja)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public ConvenioDto GetConvenio(string idConvenio)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_CONVENIO " +
                      " WHERE ID_CONVENIO = @Busca ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", idConvenio.ToLower() ?? "");
            ConvenioDto o = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.HasRows && reader.Read())
                        o = MapDtoPD(reader);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return o;
        }
    }

    public List<ConvenioDto> GetConvenios(string termoBusca, string cliente, string loja)
    {
        var list = GetConveniosByCliente(cliente, loja);
        if (list.Count > 0)
            list = list.Where(c => (c.DataFimVigencia >= DateTime.Now || c.DataFimVigencia == null) && (c.IdConvenio.Contains(termoBusca.ToUpper()) || c.Descricao.Contains(termoBusca.ToUpper()))).ToList();
        return list;
    }

    public List<ConvenioDto> GetConveniosByCliente(string cliente, string loja)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            SqlCommand cmd = new SqlCommand("PRC_GET_LIST_CONVENIO", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CODCLI", cliente);
            cmd.Parameters.AddWithValue("@LOJACLI", loja);
            List<ConvenioDto> list = new List<ConvenioDto>();
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
            return list.OrderBy(x => x.Descricao).ToList();
        }
    }

    public ConvenioDto ObterConvenioDto(string convenioCodigo)
    {
        Convenio convenio = GetById(convenioCodigo);
        if (convenio == null)
            return null;
        return Mapper.Map<ConvenioDto>(convenio);
    }

    public Convenio GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            SqlCommand cmd = new SqlCommand("PRC_GET_DM_CONVENIO", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_CONVENIO", id);
            Convenio convenio = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        convenio = Map(reader);
                    }
                    reader.NextResult();
                    convenio.ConvenioClienteList = new List<ConvenioCliente>();
                    while (reader.Read())
                    {
                        convenio.ConvenioClienteList.Add(new ConvenioCliente
                        {
                            IdConvenio = reader["ID_CONVENIO"].ToString().Trim(),
                            IdCliente = reader["ID_CLIENTE"].ToString().Trim(),
                            IdLoja = reader["ID_LOJA"].ToString().Trim()
                        });
                    }
                    reader.NextResult();
                    convenio.ConvenioProdutoList = new List<ConvenioProduto>();
                    while (reader.Read())
                    {
                        convenio.ConvenioProdutoList.Add(new ConvenioProduto
                        {
                            IdConvenio = reader["ID_CONVENIO"].ToString().Trim(),
                            IdGrupoProduto = reader["ID_GRUPO"].ToString().Trim(),
                            IdProduto = reader["ID_PRODUTO"].ToString().Trim(),
                            CampoCodigo = reader["ID_PRODUTO"].ToString().Trim()
                        });
                    }
                    reader.NextResult();
                    convenio.ConvenioCondicaoPagamentoList = new List<ConvenioCondicaoPagamento>();
                    while (reader.Read())
                    {
                        convenio.ConvenioCondicaoPagamentoList.Add(new ConvenioCondicaoPagamento
                        {
                            IdConvenio = reader["ID_CONVENIO"].ToString().Trim(),
                            IdCondicaoPagamento = reader["ID_CONDPG"].ToString().Trim()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return convenio;
        }
    }

    public List<ConvenioDto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT * " +
                      "  FROM DM_CONVENIO " +
                      " ORDER BY NOME_CONVENIO";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<ConvenioDto> list = new List<ConvenioDto>();
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

    public override void Save(Convenio entity)
    {}

    public override void Update(Convenio entity)
    {}

    public override void Delete(Convenio entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "DELETE DM_CONVENIO " +
                      " WHERE ID_CONVENIO = @ID_CONVENIO";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ID_CONVENIO", entity.IdConvenio);
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
            var sql = "DELETE DM_CONVENIO " +
                      " WHERE ID_CONVENIO = @ID_CONVENIO";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ID_CONVENIO", id);
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

    private Convenio Map(SqlDataReader reader)
    {
        Convenio convenio = new Convenio();
        convenio.IdConvenio = reader["ID_CONVENIO"].ToString().Trim();
        convenio.Descricao = reader["NOME_CONVENIO"].ToString().Trim();
        convenio.CampoCodigo = reader["ID_CONVENIO"].ToString().Trim();
        convenio.IdCliente = reader["ID_CLIENTE"].ToString().Trim();
        convenio.IdTabelaPreco = reader["ID_TABELA"].ToString().Trim();
        convenio.TrocaCliente = ((reader["TROCA_CLIENTE"].ToString().Trim() == 'S'.ToString()) ? true : false);
        convenio.TrocaProduto = ((reader["TROCA_PRODUTO"].ToString().Trim() == 'S'.ToString()) ? true : false);
        convenio.TrocaTabelaPreco = ((reader["TROCA_TABELA"].ToString().Trim() == 'S'.ToString()) ? true : false);
        convenio.TrocaPreco = (TrocaPrecoConvenio)Convert.ToInt32(reader["TROCA_PRECO"].ToString().Split('-')[0]);
        convenio.Observacoes = reader["OBSERVACAO"].ToString().Replace("\\13\\10","\n").Replace("\\s+"," ").Replace("\\n+","\n").Trim();
        convenio.DataInicioVigencia = DateTime.ParseExact(reader["DATA_DE"].ToString().Substring(6, 2) + "/" + reader["DATA_DE"].ToString().Substring(4, 2) + "/" + reader["DATA_DE"].ToString().Substring(0, 4) + " 00:00:00,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture);
        convenio.DataFimVigencia = (reader["DATA_ATE"] == DBNull.Value || reader["DATA_ATE"] == null) ? null : new DateTime?(DateTime.ParseExact(reader["DATA_ATE"].ToString().Substring(6, 2) + "/" + reader["DATA_ATE"].ToString().Substring(4, 2) + "/" + reader["DATA_ATE"].ToString().Substring(0, 4) + " 00:00:00,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture));
        return convenio;
    }

    private ConvenioDto MapDto(SqlDataReader reader)
    {
        return new ConvenioDto
        {
            Id = reader["ID_CONVENIO"].ToString().Trim(),
            IdConvenio = reader["ID_CONVENIO"].ToString().Trim(),
            Descricao = reader["NOME_CONVENIO"].ToString().Trim(),
            CampoCodigo = reader["ID_CONVENIO"].ToString().Trim(),
            DataInicioVigencia = DateTime.ParseExact(reader["DATA_DE"].ToString().Substring(6, 2) + "/" + reader["DATA_DE"].ToString().Substring(4, 2) + "/" + reader["DATA_DE"].ToString().Substring(0, 4) + " 00:00:00,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture),
            DataFimVigencia = (reader["DATA_ATE"] == DBNull.Value || reader["DATA_ATE"] == null) ? null : new DateTime?(DateTime.ParseExact(reader["DATA_ATE"].ToString().Substring(6, 2) + "/" + reader["DATA_ATE"].ToString().Substring(4, 2) + "/" + reader["DATA_ATE"].ToString().Substring(0, 4) + " 00:00:00,000", "dd/MM/yyyy HH:mm:ss,fff", CultureInfo.InvariantCulture))
        };
    }

    private ConvenioDto MapDtoPD(SqlDataReader reader)
    {
        return new ConvenioDto
        {
            Id = reader["ID_CONVENIO"].ToString().Trim(),
            IdConvenio = reader["ID_CONVENIO"].ToString().Trim(),
            Descricao = reader["NOME_CONVENIO"].ToString().Trim(),
            CampoCodigo = reader["ID_CONVENIO"].ToString().Trim()
        };
    }
}
