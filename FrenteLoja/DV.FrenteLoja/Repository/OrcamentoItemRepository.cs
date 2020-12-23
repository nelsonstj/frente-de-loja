// DV.FrenteLoja.Repository.OrcamentoItemRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class OrcamentoItemRepository : AbstractRepository<OrcamentoItem, int>
{
    private EquipeMontagemRepository _equipeMontagemRepository = new EquipeMontagemRepository();
    private VendedorRepository _vendedorRepository = new VendedorRepository();

    public OrcamentoItem GetById(long id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT i.*, l.CampoCodigo AS IdLoja, o.IdTabelaPreco, p.NOME_PRODUTO, p.CD_PRODUTO_FABRICANTE, p.NM_FABRICANTE, p.ID_KITSERV, " +
                         "       s.Grupo AS CodigoGrupo, s.Descricao AS DescricaoGrupo, g.CampoCodigo AS CodigoSubGrupo, g.Descricao AS DescricaoSubGrupo " +
                         "  FROM ORCAMENTO_ITEM i " +
                         " INNER JOIN ORCAMENTO o ON o.Id = i.OrcamentoId " +
                         " INNER JOIN LOJA_DELLAVIA l ON l.CampoCodigo = o.IdLojaDellaVia " +
                         "  LEFT JOIN PowerData.dbo.DM_PRODUTO p ON p.ID_PRODUTO = i.ProdutoId " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_PRODUTO   g ON g.CampoCodigo = SUBSTRING(p.DS_GRUPO,1,4) " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_SUB_GRUPO s ON s.Grupo = g.IdGrupoSubGrupo " +
                         " WHERE i.Id = @Id ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            OrcamentoItem o = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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

    public List<OrcamentoItem> GetOrcamentoItensByIdOrcamento(long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT i.*, l.CampoCodigo AS IdLoja, o.IdTabelaPreco, p.NOME_PRODUTO, p.CD_PRODUTO_FABRICANTE, p.NM_FABRICANTE, p.ID_KITSERV," +
                         "       s.Grupo AS CodigoGrupo, s.Descricao AS DescricaoGrupo, g.CampoCodigo AS CodigoSubGrupo, g.Descricao AS DescricaoSubGrupo " +
                         "  FROM ORCAMENTO_ITEM i " +
                         " INNER JOIN ORCAMENTO                          o ON o.Id          = i.OrcamentoId " +
                         " INNER JOIN LOJA_DELLAVIA                      l ON l.CampoCodigo = o.IdLojaDellaVia " +
                         "  LEFT JOIN PowerData.dbo.DM_PRODUTO           p ON p.ID_PRODUTO  = i.ProdutoId " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_PRODUTO   g ON g.CampoCodigo = SUBSTRING(p.DS_GRUPO,1,4) " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_SUB_GRUPO s ON s.Grupo       = g.IdGrupoSubGrupo " +
                         " WHERE i.OrcamentoId = @OrcamentoId " +
                         "   AND i.RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@OrcamentoId", idOrcamento);
            var list = new List<OrcamentoItem>();
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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

    public List<OrcamentoItem> GetOrcamentoItensByIdItemIdOrcamento(long idItem, long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT i.*, l.CampoCodigo AS IdLoja, o.IdTabelaPreco, p.NOME_PRODUTO, p.CD_PRODUTO_FABRICANTE, p.NM_FABRICANTE, p.ID_KITSERV, " +
                         "       s.Grupo AS CodigoGrupo, s.Descricao AS DescricaoGrupo, g.CampoCodigo AS CodigoSubGrupo, g.Descricao AS DescricaoSubGrupo " +
                         "  FROM ORCAMENTO_ITEM i " +
                         " INNER JOIN ORCAMENTO                          o ON o.Id          = i.OrcamentoId " +
                         " INNER JOIN LOJA_DELLAVIA                      l ON l.CampoCodigo = o.IdLojaDellaVia " +
                         "  LEFT JOIN PowerData.dbo.DM_PRODUTO           p ON p.ID_PRODUTO  = i.ProdutoId " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_PRODUTO   g ON g.CampoCodigo = SUBSTRING(p.DS_GRUPO,1,4) " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_SUB_GRUPO s ON s.Grupo       = g.IdGrupoSubGrupo " +
                         " WHERE i.NrItemProdutoPaiId =  @Id " +
                         "   AND i.OrcamentoId        =  @OrcamentoId " +
                         "   AND i.RegistroInativo    <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", idItem);
            cmd.Parameters.AddWithValue("@OrcamentoId", idOrcamento);
            var list = new List<OrcamentoItem>();
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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

    public void Add(OrcamentoItem orcamentoItem)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "INSERT INTO ORCAMENTO_ITEM " +
                        "( OrcamentoId,  IdDescontoModeloVenda,  NrItem,  Quantidade,  PrecoUnitario,  TotalItem,  ProdutoId, " +
                        "  NrItemProdutoPaiId,  ValorDesconto,  PercDescon,  TipoOperacao,  ReservaEstoque,  DescontoModeloVendaUtilizado, " +
                        "  RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao) " +
                        "VALUES " +
                        "(@OrcamentoId, @IdDescontoModeloVenda, @NrItem, @Quantidade, @PrecoUnitario, @TotalItem, @ProdutoId, " +
                        " @NrItemProdutoPaiId, @ValorDesconto, @PercDescon, @TipoOperacao, @ReservaEstoque, @DescontoModeloVendaUtilizado, " +
                        " @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao) ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@OrcamentoId ", orcamentoItem.OrcamentoId);
            cmd.Parameters.AddWithValue("@IdDescontoModeloVenda", orcamentoItem.IdDescontoModeloVenda ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@NrItem", orcamentoItem.NrItem);
            cmd.Parameters.AddWithValue("@Quantidade", orcamentoItem.Quantidade);
            cmd.Parameters.AddWithValue("@PrecoUnitario", orcamentoItem.PrecoUnitario);
            cmd.Parameters.AddWithValue("@TotalItem", orcamentoItem.TotalItem);
            cmd.Parameters.AddWithValue("@ProdutoId", orcamentoItem.ProdutoId);
            cmd.Parameters.AddWithValue("@NrItemProdutoPaiId", orcamentoItem.NrItemProdutoPaiId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ValorDesconto", orcamentoItem.ValorDesconto);
            cmd.Parameters.AddWithValue("@PercDescon", orcamentoItem.PercDescon);
            cmd.Parameters.AddWithValue("@TipoOperacao", orcamentoItem.TipoOperacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ReservaEstoque", orcamentoItem.ReservaEstoque);
            cmd.Parameters.AddWithValue("@DescontoModeloVendaUtilizado", orcamentoItem.DescontoModeloVendaUtilizado ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RegistroInativo", orcamentoItem.RegistroInativo);
            cmd.Parameters.AddWithValue("@DataAtualizacao", orcamentoItem.DataAtualizacao);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper());
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

    public override void Save(OrcamentoItem entity)
    {}

    public override void Update(OrcamentoItem orcamentoItem)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "UPDATE ORCAMENTO_ITEM " +
                         "   SET IdDescontoModeloVenda = @IdDescontoModeloVenda," +
                         "       Quantidade = @Quantidade," +
                         "       PrecoUnitario = @PrecoUnitario," +
                         "       TotalItem = @TotalItem," +
                         "       ProdutoId = @ProdutoId," +
                         "       NrItemProdutoPaiId = @NrItemProdutoPaiId," +
                         "       ValorDesconto = @ValorDesconto," +
                         "       PercDescon = @PercDescon," +
                         "       TipoOperacao = @TipoOperacao," +
                         "       ReservaEstoque = @ReservaEstoque," +
                         "       DescontoModeloVendaUtilizado = @DescontoModeloVendaUtilizado," +
                         "       RegistroInativo = @RegistroInativo," +
                         "       DataAtualizacao = @DataAtualizacao," +
                         "       UsuarioAtualizacao = @UsuarioAtualizacao" +
                         " WHERE OrcamentoId = @OrcamentoId" +
                         "   AND NrItem = @NrItem ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdDescontoModeloVenda", orcamentoItem.IdDescontoModeloVenda ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Quantidade", orcamentoItem.Quantidade);
            cmd.Parameters.AddWithValue("@PrecoUnitario", orcamentoItem.PrecoUnitario);
            cmd.Parameters.AddWithValue("@TotalItem", orcamentoItem.TotalItem);
            cmd.Parameters.AddWithValue("@ProdutoId", orcamentoItem.ProdutoId);
            cmd.Parameters.AddWithValue("@NrItemProdutoPaiId", orcamentoItem.NrItemProdutoPaiId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ValorDesconto", orcamentoItem.ValorDesconto);
            cmd.Parameters.AddWithValue("@PercDescon", orcamentoItem.PercDescon);
            cmd.Parameters.AddWithValue("@TipoOperacao", orcamentoItem.TipoOperacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ReservaEstoque", orcamentoItem.ReservaEstoque);
            cmd.Parameters.AddWithValue("@DescontoModeloVendaUtilizado", orcamentoItem.DescontoModeloVendaUtilizado ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RegistroInativo", orcamentoItem.RegistroInativo);
            cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper());
            cmd.Parameters.AddWithValue("@OrcamentoId ", orcamentoItem.OrcamentoId);
            cmd.Parameters.AddWithValue("@NrItem", orcamentoItem.NrItem);
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

    public override void Delete(OrcamentoItem entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE ORCAMENTO_ITEM " +
                         " WHERE Id = @Id";
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

    public void DeleteById(int idOrcamentoItem)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE ORCAMENTO_ITEM " +
                         " WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", idOrcamentoItem);
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

    /*#region [ DESCONTO MODELO VENDA ]
    public DescontoModeloVenda GetDescontoModeloVendaById(long? idDescontoModeloVenda)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DESCONTO_MODELO_VENDA " +
                      " WHERE (@Id IS NULL OR (Id LIKE '%' + @Id + '%')) " +
                      "   AND RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", idDescontoModeloVenda);
            DescontoModeloVenda d = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        d = MapDMV(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return d;
        }
    }

    public List<DescontoModeloVenda> GetDescontoModeloVenda(string areaNegocio, string codProduto, string tabelaPreco)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DESCONTO_MODELO_VENDA " +
                      " WHERE (@AreaNegocioId             IS NULL OR (AreaNegocioId             LIKE '%' + @AreaNegocioId + '%')) " +
                      "   AND (@CodigosDeProdutoLiberados IS NULL OR (CodigosDeProdutoLiberados LIKE '%' + @CodigosDeProdutoLiberados + '%')) " +
                      "   AND (@TabelasDePrecoAssociadas  IS NULL OR (TabelasDePrecoAssociadas  LIKE '%' + @TabelasDePrecoAssociadas + '%')) " +
                      "   AND RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AreaNegocioId", areaNegocio);
            cmd.Parameters.AddWithValue("@CodigosDeProdutoLiberados", codProduto);
            cmd.Parameters.AddWithValue("@TabelasDePrecoAssociadas", tabelaPreco);
            var list = new List<DescontoModeloVenda>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapDMV(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }
    #endregion
    */

    #region [ DESCONTO VENDA ALCADA ]
    public SolicitacaoDescontoVendaAlcada GetSolicitacaoDescontoVendaAlcada(long idOrcamentoItem, StatusSolicitacao statusSolicitacaoAlcada)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM SOLICITACAO_DESCONTO_VENDA_ALCADA " +
                         " WHERE IdOrcamentoItem = @IdOrcamentoItem " +
                         "   AND (@StatusSolicitacaoAlcada IS NULL OR (StatusSolicitacaoAlcada = @StatusSolicitacaoAlcada)) " +
                         "   AND Situacao  <> 4 " +
                         "   AND RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", idOrcamentoItem);
            cmd.Parameters.AddWithValue("@StatusSolicitacaoAlcada", statusSolicitacaoAlcada);
            SolicitacaoDescontoVendaAlcada s = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        s = MapSDVA(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return s;
        }
    }

    public List<SolicitacaoDescontoVendaAlcada> GetSolicitacoesDescontoVendaAlcada(long idOrcamentoItem, StatusSolicitacao? statusSolicitacaoAlcada)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM SOLICITACAO_DESCONTO_VENDA_ALCADA " +
                         " WHERE IdOrcamentoItem = @IdOrcamentoItem " +
                         "   AND (@StatusSolicitacaoAlcada IS NULL OR (StatusSolicitacaoAlcada LIKE '%' + @StatusSolicitacaoAlcada + '%')) " +
                         "   AND RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", idOrcamentoItem);
            cmd.Parameters.AddWithValue("@StatusSolicitacaoAlcada", statusSolicitacaoAlcada ?? (object)DBNull.Value);
            var list = new List<SolicitacaoDescontoVendaAlcada>();
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapSDVA(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<SolicitacaoDescontoVendaAlcada> GetSolicitacaoDescontoVendaAlcada(long idOrcamentoItem)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM SOLICITACAO_DESCONTO_VENDA_ALCADA " +
                         " WHERE IdOrcamentoItem = @IdOrcamentoItem ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", idOrcamentoItem);
            var list = new List<SolicitacaoDescontoVendaAlcada>();
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapSDVA(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public DescontoVendaAlcada GetDescontoVendaAlcada(string idLoja, string idAreaNegocio)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DESCONTO_VENDA_ALCADA " +
                         " WHERE LojaDellaviaId = @LojaDellaviaId " +
                         "   AND (@AreaNegocioId IS NULL OR (AreaNegocioId = @AreaNegocioId)) " +
                         "   AND RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@LojaDellaviaId", idLoja ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AreaNegocioId", idAreaNegocio ?? (object)DBNull.Value);
            DescontoVendaAlcada s = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        s = MapDVA(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return s;
        }
    }

    public List<DescontoLoja> GetDescontosLoja(string idLoja)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_DESC_LOJA_GRUPO", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@LOJA", idLoja ?? (object)DBNull.Value);
            var list = new List<DescontoLoja>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapDL(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public DescontoVendaAlcadaGrupoProduto GetDescontoVendaAlcadaGrupoProduto(long idGrupoProduto, string idAreaNegocio, string idLoja)
    {
        using (var conn = new SqlConnection(strConn))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO " +
                         " WHERE GrupoProdutoId = @GrupoProdutoId " +
                         "   AND (@AreaNegocioId  IS NULL OR (AreaNegocioId  = @AreaNegocioId)) " +
                         "   AND (@LojaDellaviaId IS NULL OR (LojaDellaviaId = @LojaDellaviaId)) " +
                         "   AND RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GrupoProdutoId", idGrupoProduto);
            cmd.Parameters.AddWithValue("@AreaNegocioId", idAreaNegocio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LojaDellaviaId", idLoja ?? (object)DBNull.Value);
            DescontoVendaAlcadaGrupoProduto d = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        d = MapDVAGP(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return d;
        }
    }

    public void AddSolicitacaoDescontoVendaAlcada(SolicitacaoDescontoVendaAlcada solicitacao)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "INSERT INTO SOLICITACAO_DESCONTO_VENDA_ALCADA " +
                      "(  IdOrcamentoItem,  DataSolicitacao,  ObservacaoItem,  ObservacaoGeral,  RespostaSolicitacao,  DataResposta,  StatusSolicitacaoAlcada,  ValorDesconto,  PercentualDesconto,  Situacao,  RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao )" +
                      "VALUES " +
                      "( @IdOrcamentoItem, @DataSolicitacao, @ObservacaoItem, @ObservacaoGeral, @RespostaSolicitacao, @DataResposta, @StatusSolicitacaoAlcada, @ValorDesconto, @PercentualDesconto, @Situacao, @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao )";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", solicitacao.IdOrcamentoItem);
            cmd.Parameters.AddWithValue("@DataSolicitacao", solicitacao.DataSolicitacao);
            cmd.Parameters.AddWithValue("@ObservacaoItem", solicitacao.ObservacaoItem ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ObservacaoGeral", solicitacao.ObservacaoGeral ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RespostaSolicitacao", solicitacao.RespostaSolicitacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DataResposta", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusSolicitacaoAlcada", solicitacao.StatusSolicitacaoAlcada);
            cmd.Parameters.AddWithValue("@ValorDesconto", solicitacao.ValorDesconto);
            cmd.Parameters.AddWithValue("@PercentualDesconto", solicitacao.PercentualDesconto);
            cmd.Parameters.AddWithValue("@Situacao", solicitacao.Situacao);
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

    public void UpdateSolicitacaoDescontoVendaAlcada(SolicitacaoDescontoVendaAlcada solicitacao)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "UPDATE SOLICITACAO_DESCONTO_VENDA_ALCADA " +
                      "   SET DataSolicitacao = @DataSolicitacao," +
                      "       ObservacaoItem = @ObservacaoItem," +
                      "	      ObservacaoGeral = @ObservacaoGeral," +
                      "	      RespostaSolicitacao = @RespostaSolicitacao," +
                      "       DataResposta = @DataResposta," +
                      "       StatusSolicitacaoAlcada = @StatusSolicitacaoAlcada," +
                      "       ValorDesconto = @ValorDesconto," +
                      "       PercentualDesconto = @PercentualDesconto," +
                      "       Situacao = @Situacao," +
                      "       RegistroInativo = @RegistroInativo," +
                      "       DataAtualizacao = @DataAtualizacao," +
                      "       UsuarioAtualizacao = @UsuarioAtualizacao" +
                      " WHERE IdOrcamentoItem = @IdOrcamentoItem";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DataSolicitacao", solicitacao.DataSolicitacao);
            cmd.Parameters.AddWithValue("@ObservacaoItem", solicitacao.ObservacaoItem ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ObservacaoGeral", solicitacao.ObservacaoGeral ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RespostaSolicitacao", solicitacao.RespostaSolicitacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DataResposta", solicitacao.DataResposta ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusSolicitacaoAlcada", solicitacao.StatusSolicitacaoAlcada);
            cmd.Parameters.AddWithValue("@ValorDesconto", solicitacao.ValorDesconto);
            cmd.Parameters.AddWithValue("@PercentualDesconto", solicitacao.PercentualDesconto);
            cmd.Parameters.AddWithValue("@Situacao", solicitacao.Situacao);
            cmd.Parameters.AddWithValue("@RegistroInativo", 0);
            cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", solicitacao.IdOrcamentoItem);
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

    public void DeleteSolicitacaoDescontoVendaAlcada(long idOrcamentoItem)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "DELETE SOLICITACAO_DESCONTO_VENDA_ALCADA " +
                      " WHERE IdOrcamentoItem = @IdOrcamentoItem ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", idOrcamentoItem);
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
    #endregion

    #region [ EQUIPE MONTAGEM ]
    public EquipeMontagemDto ObterEquipeMontagemDto(long idOrcamentoItem)
    {
        var orcItem = GetById(idOrcamentoItem);
        var equipeMontagemDto = new EquipeMontagemDto();
        equipeMontagemDto.DescricaoProduto = orcItem.Produto.Descricao;
        equipeMontagemDto.IdLojaOrcamento = orcItem.Orcamento.LojaDellaVia.CampoCodigo;
        var equipe = _equipeMontagemRepository.GetByIdOrcamentoItem(idOrcamentoItem);
        if (equipe.Count == 0)
            equipe.Add(new OrcamentoItemEquipeMontagem());
        foreach (var prof in equipe)
            equipeMontagemDto.Equipe.Add(new ProfissionalMontagemDto
            {
                Id = prof.IdVendedor,
                ProfissionalNome = prof.Descricao,
                Funcao = prof.Funcao
            });
        //equipeMontagemDto.Equipe = Mapper.Map<List<ProfissionalMontagemDto>>(equipe);
        return equipeMontagemDto;
    }

    public void InserirEquipeMontagem(EquipeMontagemDto equipeMontagemDto, long idOrcamentoItem)
    {
        equipeMontagemDto.Equipe = equipeMontagemDto.Equipe.Where(e => !string.IsNullOrEmpty(e.Id) && !e.Funcao.Equals(0)).ToList();
        if (equipeMontagemDto.Equipe.Count > 0)
            foreach (var equip in equipeMontagemDto.Equipe)
                if (equip.Funcao == 0 || equip.Id == null)
                    throw new NegocioException("É necessário incluir ao menos um profissional e sua função.");
        var orcamentoItemEquipeMontagemList = _equipeMontagemRepository.GetByIdOrcamentoItem(idOrcamentoItem);
        // Remover todos e adicionar da dto.
        foreach (var orcamentoItemEquipeMontagem in orcamentoItemEquipeMontagemList)
            _equipeMontagemRepository.Delete(orcamentoItemEquipeMontagem);
        foreach (var profissionalMontagemDto in equipeMontagemDto.Equipe)
        {
            var vendedor = _vendedorRepository.GetById(profissionalMontagemDto.Id);
            if (vendedor != null)
            {
                // Insere
                _equipeMontagemRepository.Add(new OrcamentoItemEquipeMontagem
                {
                    IdOrcamentoItem = idOrcamentoItem,
                    IdVendedor = vendedor.IdConsultor,
                    Funcao = profissionalMontagemDto.Funcao
                });
            }
        }
    }

    #endregion

    private OrcamentoItem Map(SqlDataReader reader)
    {
        return new OrcamentoItem
        {
            Id = (long)reader["Id"],
            OrcamentoId = (long)reader["OrcamentoId"],
            IdDescontoModeloVenda = (reader["IdDescontoModeloVenda"] == DBNull.Value || reader["IdDescontoModeloVenda"] == null) ? null : new long?((long)reader["IdDescontoModeloVenda"]),
            NrItem = (int)reader["NrItem"],
            Quantidade = (decimal)reader["Quantidade"],
            PrecoUnitario = (decimal)reader["PrecoUnitario"],
            TotalItem = (decimal)reader["TotalItem"],
            ProdutoId = reader["ProdutoId"].ToString(),
            NrItemProdutoPaiId = (reader["NrItemProdutoPaiId"] == DBNull.Value || reader["NrItemProdutoPaiId"] == null) ? null : new int?((int)reader["NrItemProdutoPaiId"]),
            ValorDesconto = (decimal)reader["ValorDesconto"],
            PercDescon = (decimal)reader["PercDescon"],
            TipoOperacao = reader["TipoOperacao"].ToString(),
            ReservaEstoque = (bool)reader["ReservaEstoque"],
            DescontoModeloVendaUtilizado = (DescontoModeloVendaUtilizado)((reader["DescontoModeloVendaUtilizado"] != DBNull.Value && reader["DescontoModeloVendaUtilizado"] != null) ? ((int)reader["DescontoModeloVendaUtilizado"]) : 0),
            RegistroInativo = (bool)reader["RegistroInativo"],
            DataAtualizacao = (DateTime)reader["DataAtualizacao"],
            UsuarioAtualizacao = reader["UsuarioAtualizacao"].ToString(),
            Produto = new Produto
            {
                IdGrupoServicoAgregado = reader["ID_KITSERV"].ToString().Trim(),
                Descricao = reader["NOME_PRODUTO"].ToString().Trim(),
                CodigoFabricante = reader["CD_PRODUTO_FABRICANTE"].ToString().Trim(),
                FabricantePeca = reader["NM_FABRICANTE"].ToString().Trim(),
                CampoCodigo = reader["ProdutoId"].ToString().Trim(),
                IdGrupoProduto = reader["CodigoGrupo"].ToString().Trim(),
                GrupoProduto = new GrupoProduto
                {
                    IdGrupoProduto = reader["CodigoGrupo"].ToString().Trim(),
                    Descricao = reader["DescricaoGrupo"].ToString().Trim(),
                    IdGrupoSubGrupo = reader["CodigoSubGrupo"].ToString().Trim()
                }
            },
            Orcamento = new Orcamento
            {
                LojaDellaVia = new LojaDellaVia
                {
                    CampoCodigo = reader["IdLoja"].ToString().Trim()
                },
                IdTabelaPreco = reader["IdTabelaPreco"].ToString().Trim(),
                TabelaPreco = new TabelaPreco
                {
                    IdTabelaPreco = reader["IdTabelaPreco"].ToString().Trim(),
                    CampoCodigo = reader["IdTabelaPreco"].ToString().Trim()
                }
            },
            SolicitacaoDescontoVendaAlcadaList = new List<SolicitacaoDescontoVendaAlcada>(),
            EquipeMontagemList = new List<OrcamentoItemEquipeMontagem>()
        };
    }

    private SolicitacaoDescontoVendaAlcada MapSDVA(SqlDataReader reader)
    {
        return new SolicitacaoDescontoVendaAlcada
        {
            Id = (long)reader["Id"],
            IdOrcamentoItem = (long)reader["IdOrcamentoItem"],
            DataSolicitacao = (DateTime)reader["DataSolicitacao"],
            ObservacaoItem = reader["ObservacaoItem"].ToString(),
            ObservacaoGeral = reader["ObservacaoGeral"].ToString(),
            RespostaSolicitacao = reader["RespostaSolicitacao"].ToString(),
            DataResposta = (reader["DataResposta"] == DBNull.Value || reader["DataResposta"] == null) ? null : new DateTime?((DateTime)reader["DataResposta"]),
            StatusSolicitacaoAlcada = (StatusSolicitacao)reader["StatusSolicitacaoAlcada"],
            ValorDesconto = (decimal)reader["ValorDesconto"],
            PercentualDesconto = (decimal)reader["PercentualDesconto"],
            Situacao = (SituacaoDescontoAlcada)reader["Situacao"]
        };
    }

    private DescontoVendaAlcada MapDVA(SqlDataReader reader)
    {
        return new DescontoVendaAlcada
        {
            Id = (long)reader["Id"],
            LojaDellaviaId = reader["LojaDellaviaId"].ToString(),
            PercentualDescontoVendedor = (decimal)reader["PercentualDescontoVendedor"],
            PercentualDescontoGerente = (decimal)reader["PercentualDescontoGerente"],
            CampoCodigo = reader["CampoCodigo"].ToString(),
            AreaNegocioId = reader["AreaNegocioId"].ToString()
        };
    }

    private DescontoVendaAlcadaGrupoProduto MapDVAGP(SqlDataReader reader)
    {
        return new DescontoVendaAlcadaGrupoProduto
        {
            Id = (long)reader["Id"],
            GrupoProdutoId = (long)reader[""],
            PercentualDescontoVendedor = (decimal)reader["PercentualDescontoVendedor"],
            PercentualDescontoGerente = (decimal)reader["PercentualDescontoGerente"],
            LojaDellaviaId = reader["LojaDellaviaId"].ToString(),
            CampoCodigo = reader["CampoCodigo"].ToString(),
            AreaNegocioId = reader["AreaNegocioId"].ToString()
        };
    }

    /*private DescontoModeloVenda MapDMV(SqlDataReader reader)
    {
        return new DescontoModeloVenda
        {
            Id = (long)reader["Id"],
            TabelasDePrecoAssociadas = reader["TabelasDePrecoAssociadas"].ToString(),
            CodigosDeProdutoLiberados = reader["CodigosDeProdutoLiberados"].ToString(),
            PercentualDesconto1 = (decimal)reader["PercentualDesconto1"],
            PercentualDesconto2 = (decimal)reader["PercentualDesconto2"],
            PercentualDesconto3 = (decimal)reader["PercentualDesconto3"],
            PercentualDesconto4 = (decimal)reader["PercentualDesconto4"],
            Bloqueado = (bool)reader["Bloqueado"],
            CampoCodigo = reader["CampoCodigo"].ToString(),
            AreaNegocioId = reader["AreaNegocioId"].ToString()
        };
    }*/

    private DescontoLoja MapDL(SqlDataReader reader)
    {
        return new DescontoLoja
        {
            IdLoja = reader["ID_LOJA"].ToString().Trim(),
            IdAreaNegocio = reader["ID_AREA"].ToString(),
            PercentualDescontoLojaGerente = (reader["LJ_DESC"] == DBNull.Value || reader["LJ_DESC"] == null || reader["LJ_DESC"].ToString() == "") ? null : new decimal?(Convert.ToDecimal(reader["LJ_DESC"].ToString().Trim())),
            PercentualDescontoLojaVendedor = (reader["LJ_DVDESC"] == DBNull.Value || reader["LJ_DVDESC"] == null || reader["LJ_DVDESC"].ToString() == "") ? null : new decimal?(Convert.ToDecimal(reader["LJ_DVDESC"].ToString().Trim())),
            IdGrupo = reader["ID_GRUPO"].ToString().Trim(),
            PercentualDescontoGrupoGerente = (reader["GRP_DESC"] == DBNull.Value || reader["GRP_DESC"] == null || reader["GRP_DESC"].ToString() == "") ? null : new decimal?(Convert.ToDecimal(reader["GRP_DESC"].ToString().Trim())),
            PercentualDescontoGrupoVendedor = (reader["GPR_DVDESC"] == DBNull.Value || reader["GPR_DVDESC"] == null || reader["GPR_DVDESC"].ToString() == "") ? null : new decimal?(Convert.ToDecimal(reader["GPR_DVDESC"].ToString().Trim()))
        };
    }
}
