// DV.FrenteLoja.Repository.OrcamentoFormaPagamentoRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class OrcamentoFormaPagamentoRepository : AbstractRepository<OrcamentoFormaPagamento, int>
{
    private readonly ICondicaoPagamentoParcelasApi _condicaoPagamentoParcelasApi;

    public OrcamentoFormaPagamentoRepository(ICondicaoPagamentoParcelasApi condicaoPagamentoParcelasApi)
    {
        _condicaoPagamentoParcelasApi = condicaoPagamentoParcelasApi;
    }

    public OrcamentoFormaPagamento GetById(long id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT ofp.*, cp.NOME_CONDPG, cp.DS_CONDPG AS Descricao, cp.ID_FORMAPG, cp.PC_ACRESDV, fp.DS_FORMAPG " +
                      "  FROM ORCAMENTO_FORMA_PAGAMENTO            ofp " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG         cp ON cp.ID_CONDPG  = ofp.IdCondicaoPagamento " +
                      "  LEFT JOIN PowerData.dbo.DM_FORMAPG        fp ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG_AREANEG ca ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " WHERE ofp.Id = @Id ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            OrcamentoFormaPagamento o = null;
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

    public OrcamentoFormaPagamentoDto GetByIdOrcamento(long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT ofp.*, cp.NOME_CONDPG, cp.DS_CONDPG AS Descricao, cp.ID_FORMAPG, cp.PC_ACRESDV, fp.DS_FORMAPG " +
                      "  FROM ORCAMENTO_FORMA_PAGAMENTO ofp " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG cp ON cp.ID_CONDPG = ofp.IdCondicaoPagamento " +
                      "  LEFT JOIN PowerData.dbo.DM_FORMAPG fp ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG_AREANEG ca ON ca.ID_CONDPG = cp.ID_CONDPG " +
                      " WHERE ofp.IdOrcamento = @IdOrcamento ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamento", idOrcamento);
            OrcamentoFormaPagamentoDto o = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        o = MapDto(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return o;
        }
    }

    public List<OrcamentoFormaPagamento> GetsByIdOrcamento(long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT ofp.*, cp.NOME_CONDPG, cp.DS_CONDPG AS Descricao, cp.ID_FORMAPG, cp.PC_ACRESDV, fp.DS_FORMAPG " +
                      "  FROM ORCAMENTO_FORMA_PAGAMENTO            ofp " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG         cp  ON cp.ID_CONDPG  = ofp.IdCondicaoPagamento " +
                      "  LEFT JOIN PowerData.dbo.DM_FORMAPG        fp  ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG_AREANEG ca  ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " WHERE ofp.IdOrcamento = @IdOrcamento " +
                      " ORDER BY 3";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamento", idOrcamento);
            var list = new List<OrcamentoFormaPagamento>();
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

    public List<OrcamentoFormaPagamentoDto> GetsByIdOrcamentoDto(long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT ofp.*, cp.NOME_CONDPG, cp.DS_CONDPG AS Descricao, cp.ID_FORMAPG, cp.PC_ACRESDV, fp.DS_FORMAPG " +
                      "  FROM ORCAMENTO_FORMA_PAGAMENTO            ofp " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG         cp  ON cp.ID_CONDPG  = ofp.IdCondicaoPagamento " +
                      "  LEFT JOIN PowerData.dbo.DM_FORMAPG        fp  ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG_AREANEG ca  ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " WHERE ofp.IdOrcamento = @IdOrcamento " +
                      " ORDER BY 3";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamento", idOrcamento);
            var list = new List<OrcamentoFormaPagamentoDto>();
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

    public List<OrcamentoFormaPagamentoDto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT ofp.*, cp.NOME_CONDPG, cp.DS_CONDPG AS Descricao, cp.ID_FORMAPG, fp.DS_FORMAPG " +
                      "  FROM ORCAMENTO_FORMA_PAGAMENTO            ofp " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG         cp  ON cp.ID_CONDPG  = ofp.IdCondicaoPagamento " +
                      "  LEFT JOIN PowerData.dbo.DM_FORMAPG        fp  ON fp.ID_FORMAPG = cp.ID_FORMAPG " +
                      " INNER JOIN PowerData.dbo.DM_CONDPG_AREANEG ca  ON ca.ID_CONDPG  = cp.ID_CONDPG " +
                      " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            var list = new List<OrcamentoFormaPagamentoDto>();
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

    public void InserirFormaPagamento(FormaPagamentoDto formaPagamentoDto, Orcamento orcamento)
    {
        var formasPagamento = GetsByIdOrcamentoDto(formaPagamentoDto.IdOrcamento);
        var valorParceladoSomado = formasPagamento.Any() ? formasPagamento.Sum(x => x.ValorTotal) : 0m;
        var valorRestante = orcamento.OrcamentoItens?.Select(x => x.TotalItem).Sum() + orcamento.ValorImpostos - valorParceladoSomado;
        if (formaPagamentoDto.PercentualAcrescimo > 0)
            valorRestante += decimal.Round(formaPagamentoDto.PercentualAcrescimo / 100m * (valorRestante ?? 0), 2);
        if (formaPagamentoDto.Valor > valorRestante)
            throw new NegocioException("O valor do pagamento excede o valor restante do orçamento.");
        switch (formaPagamentoDto.TipoFormaPagamento)
        {
            case TipoFormaPagamento.Cartao:
                if (string.IsNullOrEmpty(formaPagamentoDto.IdAdministradoraFinanceira))
                    throw new NegocioException("Bandeira não informada.");
                Add(new OrcamentoFormaPagamento
                {
                    IdOrcamento = formaPagamentoDto.IdOrcamento,
                    IdCondicaoPagamento = formaPagamentoDto.Id,
                    IdAdministradoraFinanceira = formaPagamentoDto.IdAdministradoraFinanceira,
                    TotalValorForma = formaPagamentoDto.Valor,
                    PercentualAcrescimo = formaPagamentoDto.PercentualAcrescimo
                });
                break;
            case TipoFormaPagamento.Banco:
                if (formaPagamentoDto.IdBanco == 0)
                    throw new NegocioException("Banco não informado.");
                Add(new OrcamentoFormaPagamento
                {
                    IdOrcamento = formaPagamentoDto.IdOrcamento,
                    IdCondicaoPagamento = formaPagamentoDto.Id,
                    IdBanco = formaPagamentoDto.IdBanco,
                    TotalValorForma = formaPagamentoDto.Valor,
                    PercentualAcrescimo = formaPagamentoDto.PercentualAcrescimo
                });
                break;
            case TipoFormaPagamento.Dinheiro:
                Add(new OrcamentoFormaPagamento
                {
                    IdOrcamento = formaPagamentoDto.IdOrcamento,
                    IdCondicaoPagamento = formaPagamentoDto.Id,
                    TotalValorForma = formaPagamentoDto.Valor
                });
                break;
            default:
                throw new NegocioException("Tipo da forma do pagamento não informado.");
        }
    }

    public void Add(OrcamentoFormaPagamento orcamentoFormaPagamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "INSERT INTO ORCAMENTO_FORMA_PAGAMENTO " +
                      "(  IdOrcamento,  IdCondicaoPagamento,  IdAdministradoraFinanceira,  IdBanco,  TotalValorForma,  PercentualAcrescimo,  RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao )" +
                      "VALUES " +
                      "( @IdOrcamento, @IdCondicaoPagamento, @IdAdministradoraFinanceira, @IdBanco, @TotalValorForma, @PercentualAcrescimo, @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao )";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamento", orcamentoFormaPagamento.IdOrcamento);
            cmd.Parameters.AddWithValue("@IdCondicaoPagamento", orcamentoFormaPagamento.IdCondicaoPagamento);
            cmd.Parameters.AddWithValue("@IdAdministradoraFinanceira", orcamentoFormaPagamento.IdAdministradoraFinanceira ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdBanco", orcamentoFormaPagamento.IdBanco ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalValorForma", orcamentoFormaPagamento.TotalValorForma);
            cmd.Parameters.AddWithValue("@PercentualAcrescimo", orcamentoFormaPagamento.PercentualAcrescimo);
            cmd.Parameters.AddWithValue("@RegistroInativo", 0);
            cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper() ?? (object)DBNull.Value);
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

    public override void Save(OrcamentoFormaPagamento entity)
    {}

    public override void Update(OrcamentoFormaPagamento entity)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            var sql = "UPDATE ORCAMENTO_FORMA_PAGAMENTO " +
                      "   SET IdOrcamento = @IdOrcamento, " +
                      "       IdCondicaoPagamento = @IdCondicaoPagamento, " +
                      "       IdAdministradoraFinanceira = @IdAdministradoraFinanceira, " +
                      "       IdBanco = @IdBanco, " +
                      "       TotalValorForma = @TotalValorForma " +
                      "       RegistroInativo = @RegistroInativo, " +
                      "       DataAtualizacao = @DataAtualizacao, " +
                      "       UsuarioAtualizacao = @UsuarioAtualizacao" +
                      " WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@IdOrcamento", entity.IdOrcamento);
            cmd.Parameters.AddWithValue("@IdCondicaoPagamento", entity.IdCondicaoPagamento);
            cmd.Parameters.AddWithValue("@IdAdministradoraFinanceira", entity.IdAdministradoraFinanceira);
            cmd.Parameters.AddWithValue("@IdBanco", entity.IdBanco);
            cmd.Parameters.AddWithValue("@TotalValorForma", entity.TotalValorForma);
            cmd.Parameters.AddWithValue("@RegistroInativo", entity.RegistroInativo);
            cmd.Parameters.AddWithValue("@DataAtualizacao", entity.DataAtualizacao);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", entity.UsuarioAtualizacao);
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

    public override void Delete(OrcamentoFormaPagamento entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE ORCAMENTO_FORMA_PAGAMENTO WHERE Id = @Id ";
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

    public void DeleteById(long orcamentoFormaPagamentoId)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE ORCAMENTO_FORMA_PAGAMENTO WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", orcamentoFormaPagamentoId);
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

    public long RemoverFormaPagamento(long idOrcamentoFormaPagamento)
    {
        OrcamentoFormaPagamento orcamentoFormaPagamento = GetById(idOrcamentoFormaPagamento);
        if (orcamentoFormaPagamento == null)
            throw new NegocioException("Forma de pagamento não encontrado.");
        Delete(orcamentoFormaPagamento);
        return orcamentoFormaPagamento.IdOrcamento;
    }

    private OrcamentoFormaPagamento Map(SqlDataReader reader)
    {
        return new OrcamentoFormaPagamento
        {
            Id = (long)reader["Id"],
            IdOrcamento = (long)reader["IdOrcamento"],
            IdCondicaoPagamento = reader["IdCondicaoPagamento"].ToString(),
            IdAdministradoraFinanceira = reader["IdAdministradoraFinanceira"].ToString(),
            IdBanco = (reader["IdBanco"] == DBNull.Value || reader["IdBanco"] == null) ? null : new long?((long)reader["IdBanco"]),
            TotalValorForma = (decimal)reader["TotalValorForma"],
            PercentualAcrescimo = Convert.ToDecimal(reader["PercentualAcrescimo"].ToString().Trim()),
            CondicaoPagamento = new PDCondicaoPagamento
            {
                IdFormaPagamento = reader["ID_FORMAPG"].ToString().Trim(),
                NomeCondicaoPagamento = reader["Descricao"].ToString().Trim(),
                QtdParcelas = reader["NOME_CONDPG"].ToString().Trim().Contains(',') ? reader["NOME_CONDPG"].ToString().Trim().Split(',').Length : 0,
                ValorAcrescimo = Convert.ToDecimal(reader["PC_ACRESDV"].ToString().Trim())
            }
        };
    }

    private OrcamentoFormaPagamentoDto MapDto(SqlDataReader reader)
    {
        var parcelas = reader["NOME_CONDPG"].ToString().Trim();
        int nrParcelas = 0;
        if (parcelas.Contains(','))
            nrParcelas = parcelas.Split(',').Length;
        return new OrcamentoFormaPagamentoDto
        {
            Id = (long)reader["Id"],
            CondicaoPagamento = reader["Descricao"].ToString().Trim(),
            QtdParcelas = nrParcelas,
            ValorTotal = (decimal)reader["TotalValorForma"],
            ValorParcela = ((nrParcelas > 1) ? ((decimal)reader["TotalValorForma"] / nrParcelas) : 0m),
            TemAcrescimo = Convert.ToDecimal(reader["PercentualAcrescimo"].ToString().Trim()) > 0 ? true : false
        };
    }

    public int TamanhoAdmFinanceiraPorTermo(string termoBusca, string idFormaPagto)
    {
        return GetAdmFinanceira(termoBusca, idFormaPagto).Count();
    }

    public List<AdministradoraFinanceiraDto> ObterAdmFinanceiraPorNome(int tamanhoPagina, int numeroPagina, string termoBusca, string idFormaPagto)
    {
        return GetAdmFinanceira(termoBusca, idFormaPagto)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<AdministradoraFinanceiraDto> GetAdmFinanceira(string termoBusca, string idFormaPagto)
    {
        termoBusca = ValidaNull(termoBusca);
        idFormaPagto = ValidaNull(idFormaPagto);
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_ADMINISTRADORAS " +
                      " WHERE ((@termoBusca    IS NULL OR (NOME_ADMIN    LIKE '%' + @termoBusca + '%')) " +
                      "        OR (@termoBusca IS NULL OR (ID_ADMIN      LIKE '%' + @termoBusca + '%')) " +
                      "        OR (@termoBusca IS NULL OR (DS_ADQUIRENTE LIKE '%' + @termoBusca + '%'))) " +
                      "   AND (@IdCondPagto    IS NULL OR (ID_FORMAPG    = @IdCondPagto )) " +
                      " ORDER BY ID_ADMIN";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@termoBusca", termoBusca?.ToLower() ?? ((object)DBNull.Value));
            cmd.Parameters.AddWithValue("@IdCondPagto", idFormaPagto?.ToLower() ?? ((object)DBNull.Value));
            List<AdministradoraFinanceiraDto> list = new List<AdministradoraFinanceiraDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(MapAF(reader));
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

    private AdministradoraFinanceiraDto MapAF(SqlDataReader reader)
    {
        return new AdministradoraFinanceiraDto
        {
            Id = reader["ID_ADMIN"].ToString().Trim(),
            Descricao = reader["ID_ADMIN"].ToString().Trim() + "-" + reader["NOME_ADMIN"].ToString().Trim()
        };
    }

    public ParcelamentoDto ObterParcelamento(long idOrcamentoFormaPagamento)
    {
        var formaPagamento = GetById(idOrcamentoFormaPagamento);
        if (formaPagamento == null)
            throw new NegocioException("Forma de pagamento não encontrada no banco de dados.");
        string erros;
        var parcelas = _condicaoPagamentoParcelasApi.ObterParcelas(formaPagamento.IdCondicaoPagamento, formaPagamento.TotalValorForma, out erros);
        if (!erros.IsNullOrEmpty())
            throw new NegocioException($"Erro(s) ao buscar condições de parcelamento: {erros}");
        var parcelamentoDto = new ParcelamentoDto();
        parcelamentoDto.NomeCondicaoPagamento = formaPagamento.CondicaoPagamento.NomeCondicaoPagamento;
        parcelamentoDto.Parcelas = parcelas;
        parcelamentoDto.ValorAcrescimo = formaPagamento.CondicaoPagamento.ValorAcrescimo;
        return parcelamentoDto;
    }

    private string ValidaNull(string campo)
    {
        if (string.IsNullOrEmpty(campo))
            campo = null;
        return campo;
    }
}
