// DV.FrenteLoja.Repository.CondicaoPagamentoRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class CondicaoPagamentoRepository : AbstractRepository<PDCondicaoPagamento, int>
{
    private AreaNegocioRepository _tipoVendaRepository = new AreaNegocioRepository();

    public int TamanhoCondicoesPagamentoPorTermo(string termoBusca, string tipoVenda)
    {
        string tipoVendaOrcamento = string.Empty;
        if (tipoVenda != null)
            tipoVendaOrcamento = _tipoVendaRepository.GetById(tipoVenda).Sigla;
        return GetCondicaoPagamento(termoBusca, tipoVendaOrcamento).Count();
    }

    public List<FormaPagamentoDto> ObterCondicoesPagamentoPorNome(string termoBusca, string tipoVenda, int tamanhoPagina, int numeroPagina)
    {
        var tipoVendaOrcamento = string.Empty;
        if (tipoVenda != null)
            tipoVendaOrcamento = _tipoVendaRepository.GetById(tipoVenda).Sigla;
        var formaPagamentos = GetCondicaoPagamento(termoBusca, tipoVendaOrcamento)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
        var fpgtoDtos = new List<FormaPagamentoDto>();
        foreach (var formaPagamento in formaPagamentos)
        {
            fpgtoDtos.Add(new FormaPagamentoDto
            {
                Id = formaPagamento.IdCondicaoPagamento,
                Descricao = formaPagamento.IdCondicaoPagamento + "-" + formaPagamento.DescricaoCondicaoPagamento,
                FormaPagamento = formaPagamento.IdFormaPagamento,
                TipoFormaPagamento = ObterTipoFormaPagamento(formaPagamento.IdFormaPagamento),
                PercentualAcrescimo = formaPagamento.ValorAcrescimo
            });
        }
        return fpgtoDtos;
    }

    private TipoFormaPagamento ObterTipoFormaPagamento(string descricao)
    {
        switch (descricao)
        {
            case "FI":
            case "CD":
            case "CC":
                return TipoFormaPagamento.Cartao;
            case "DP":
            case "CH":
            case "BOL":
                return TipoFormaPagamento.Banco;
            default:
                return TipoFormaPagamento.Dinheiro;
        }
    }

    public PDCondicaoPagamento GetCondicaoPagamentoById(string idCondPg, string tipoVenda)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT cp.* " +
                      "  FROM DM_CONDPG              cp " +
                      "  LEFT JOIN DM_FORMAPG        fp ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN DM_CONDPG_AREANEG ca ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " WHERE (@idCondPg  IS NULL OR (ca.ID_CONDPG   = @idCondPg)) " +
                      "   AND (@TipoVenda IS NULL OR (ca.ID_AREANEG  = @TipoVenda)) " +
                      " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idCondPg", ((object)idCondPg.ToUpper()) ?? ((object)DBNull.Value));
            cmd.Parameters.AddWithValue("@TipoVenda", ((object)tipoVenda?.ToUpper()) ?? ((object)DBNull.Value));
            PDCondicaoPagamento c = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        c = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return c;
        }
    }

    public List<PDCondicaoPagamento> GetCondicaoPagamento(string termoBusca, string tipoVenda)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT cp.* " +
                      "  FROM DM_CONDPG              cp " +
                      "  LEFT JOIN DM_FORMAPG        fp ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN DM_CONDPG_AREANEG ca ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " WHERE ((@Busca    IS NULL OR (cp.ID_CONDPG   LIKE '%' + @Busca + '%')) " +
                      "        OR (@Busca IS NULL OR (cp.NOME_CONDPG LIKE '%' + @Busca + '%')) " +
                      "        OR (@Busca IS NULL OR (cp.DS_CONDPG   LIKE '%' + @Busca + '%'))) " +
                      "   AND (@TipoVenda IS NULL OR (ca.ID_AREANEG  = @TipoVenda)) " +
                      " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", ((object)termoBusca.ToUpper()) ?? ((object)DBNull.Value));
            cmd.Parameters.AddWithValue("@TipoVenda", ((object)tipoVenda?.ToUpper()) ?? ((object)DBNull.Value));
            List<PDCondicaoPagamento> list = new List<PDCondicaoPagamento>();
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

    public List<PDCondicaoPagamento> GetCondicoesPagamento(string idCondPg, string tipoVenda)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT cp.* " +
                      "  FROM DM_CONDPG              cp " +
                      "  LEFT JOIN DM_FORMAPG        fp ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN DM_CONDPG_AREANEG ca ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " WHERE (@ID_CONDPG IS NULL OR (ca.ID_AREANEG  = @ID_CONDPG)) " +
                      "   AND (@TipoVenda IS NULL OR (ca.ID_AREANEG  = @TipoVenda)) " +
                      " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ID_CONDPG", ((object)idCondPg.ToUpper()) ?? ((object)DBNull.Value));
            cmd.Parameters.AddWithValue("@TipoVenda", ((object)tipoVenda?.ToUpper()) ?? ((object)DBNull.Value));
            List<PDCondicaoPagamento> list = new List<PDCondicaoPagamento>();
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

    public List<PDCondicaoPagamento> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_CONDPG " +
                      " ORDER BY NOME_CONDPG";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<PDCondicaoPagamento> list = new List<PDCondicaoPagamento>();
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

    public override void Save(PDCondicaoPagamento entity)
    {}

    public override void Update(PDCondicaoPagamento entity)
    {}

    public override void Delete(PDCondicaoPagamento entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE Orcamento Where Id=@Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.IdCondicaoPagamento);
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
            string sql = "DELETE Orcamento Where Id=@Id";
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

    private PDCondicaoPagamento Map(SqlDataReader reader)
    {
        return new PDCondicaoPagamento
        {
            IdCondicaoPagamento = reader["ID_CONDPG"].ToString().Trim(),
            NomeCondicaoPagamento = reader["NOME_CONDPG"].ToString().Trim(),
            DescricaoCondicaoPagamento = reader["DS_CONDPG"].ToString().Trim(),
            IdFormaPagamento = reader["ID_FORMAPG"].ToString().Trim(),
            PrazoMedio = reader["PRAZO_MEDIO"].ToString().Trim(),
            QtdParcelas = reader["NOME_CONDPG"].ToString().Trim().Contains(',') ? reader["NOME_CONDPG"].ToString().Trim().Split(',').Length : 0,
            ValorAcrescimo = Convert.ToDecimal(reader["PC_ACRESDV"].ToString().Trim())
        };
    }
}
