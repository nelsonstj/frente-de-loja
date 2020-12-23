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

public class FormaPagamentoRepository : AbstractRepository<PDCondicaoPagamento, int>
{
    public int TamanhoFormaPagamentoPorTermo(string termoBusca, string areaNegocio)
    {
        return GetFormasPagamento(termoBusca).Count();
    }

    public List<FormaPagamentoDto> ObterFormaPagamentoPorNome(string termoBusca, string areaNegocio, int tamanhoPagina, int numeroPagina)
    {
        var formaPagamentos = GetFormasPagamento(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
        var fpgtoDtos = new List<FormaPagamentoDto>();
        foreach (var formaPagamento in formaPagamentos)
        {
            fpgtoDtos.Add(new FormaPagamentoDto
            {
                Id = formaPagamento.IdFormaPagamento,
                Descricao = formaPagamento.DescricaoFormaPagamento,
                TipoFormaPagamento = ObterTipoFormaPagamento(formaPagamento.IdFormaPagamento)
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

    public List<PDFormaPagamento> GetFormasPagamento(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT * " +
                      "  FROM DM_FORMAPG " +
                      " WHERE (@Busca IS NULL OR (DS_FORMAPG  LIKE '%' + @Busca + '%')) " +
                      " ORDER BY DS_FORMAPG";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToUpper() ?? ((object)DBNull.Value));
            List<PDFormaPagamento> list = new List<PDFormaPagamento>();
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

    public List<PDFormaPagamento> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT * " +
                      "  FROM DM_FORMAPG " +
                      " ORDER BY DS_FORMAPG";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<PDFormaPagamento> list = new List<PDFormaPagamento>();
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

    public override void Save(PDCondicaoPagamento entity)
    {}

    public override void Update(PDCondicaoPagamento entity)
    {}

    public override void Delete(PDCondicaoPagamento entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE DM_FORMAPG Where ID_FORMAPG=@Id";
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
            string sql = "DELETE DM_FORMAPG Where ID_FORMAPG=@Id";
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

    private PDFormaPagamento Map(SqlDataReader reader)
    {
        return new PDFormaPagamento
        {
            IdFormaPagamento = reader["ID_FORMAPG"].ToString(),
            DescricaoFormaPagamento = reader["DS_FORMAPG"].ToString()
        };
    }
}
