// DV.FrenteLoja.Repository.ProdutoRepository
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

public class ProdutoRepository : AbstractRepository<Produto, int>
{
    private readonly IRepositorioEscopo _escopo;
    private readonly IProdutoServico _produtoServico;
    private readonly IEstoqueProtheusApi _estoqueProtheusApi;
    private TabelaPrecoRepository _tabelaPrecoRepository = new TabelaPrecoRepository();
    private LojaDellaViaRepository _lojaDellaViaRepository = new LojaDellaViaRepository();

    public ProdutoRepository(IRepositorioEscopo escopo, IProdutoServico produtoService, IEstoqueProtheusApi estoqueProtheusApi)
    {
        _escopo = escopo;
        _produtoServico = produtoService;
        _estoqueProtheusApi = estoqueProtheusApi;
    }

    public ModalDetalhesProdutoDto ObterDadosModalDetalhesProduto(Orcamento orcamento, string codigoDellavia, string loja)
    {
        var modalDetalhes = new ModalDetalhesProdutoDto();
        // Produto
        var produto = GetByCodigoDellavia(codigoDellavia, loja, orcamento.IdTabelaPreco) ?? GetByCodigoDellaviaPD(codigoDellavia, loja, orcamento.IdTabelaPreco);
        if (produto == null)
            throw new NegocioException("Produto não localizado.");
        //var produtoDto = Mapper.Map<ProdutoDto>(produto);

        modalDetalhes.ProdutoPaiDto = new ProdutoDto
        {
            Id = produto.ProdutoCodDellavia,
            Descricao = produto.ProdutoDescricao,
            CodigoFabricante = produto.ProdutoCodFabricante,
            Fabricante = produto.ProdutoFabricantePeca,
            CampoCodigo = produto.ProdutoCodDellavia,
            IdGrupoProduto = produto.CodigoGrupo,
            IdSubGrupoProduto = produto.CodigoSubGrupo
        };
        modalDetalhes.IdOrcamento = orcamento.Id;
        modalDetalhes.IdOrcamentoItemPai = 0L;
        // Estoque
        modalDetalhes.QuantidadePai = 0; // (int)(ObterSaldoProdutoLojasDellaVia(produto, idsLoja)?.ProdutoProtheus[0]?.SaldoDisponivel).Value;
        // Tabela Preco
        modalDetalhes.PrecoUnitarioPai = _tabelaPrecoRepository.GetTabelaPrecoItem(orcamento.IdTabelaPreco, produto.ProdutoCodDellavia)?.PrecoVenda ?? 0m;
        modalDetalhes.TotalItemPai = 0m;
        // Servico agregado list - Kit
        var itensKitServico = GetKitServico(produto.IdGrupoServicoAgregado, orcamento.IdTabelaPreco);
        var listaProdutoAgregados = new List<GrupoServicoAgregadoProdutoDto>();
        foreach (var servicos in itensKitServico)
        {
            var itemServicoModalDto = servicos;
            itemServicoModalDto.Quantidade = 0;
            itemServicoModalDto.PrecoUnitario = servicos.PrecoUnitario;
            itemServicoModalDto.TotalItem = itemServicoModalDto.Quantidade * itemServicoModalDto.PrecoUnitario;
            itemServicoModalDto.Descricao = ((servicos?.IdProduto + " - " + servicos?.Descricao) ?? "");
            listaProdutoAgregados.Add(itemServicoModalDto);
        }
        modalDetalhes.ProdutosAgregadosModalList = listaProdutoAgregados.Where(p => p.PrecoUnitario > 0m).ToList();
        var produtoComplemento = GetProdutoComplementoByCampoCodigo(produto.ProdutoCodDellavia);
        if (produtoComplemento != null)
        {
            modalDetalhes.ProdutoComplementoPaiDto = Mapper.Map<ProdutoComplementoDto>(produtoComplemento);
            modalDetalhes.ProdutoComplementoPaiDto.hasCampoHTML = !string.IsNullOrEmpty(produtoComplemento.CampoHTML);
            produtoComplemento.CampoHTML = string.Empty; // Caso contrario o json ficaria mt grande.
        }
        return modalDetalhes;
    }

    /*       public async Task<List<ProdutoProtheusDto>> ObterSaldosProdutoLojaDellaVia(string[] idProdutos, string idLoja)
           {
               var jsonResult = await _estoqueProtheusApi.BuscaEstoqueProdutoFiliais(idProdutos, idLoja);
               var produtoProtheusDtoList = new List<ProdutoProtheusDto>();
               foreach (var jObj in jsonResult)
               {
                   var produtoProtheus = new ProdutoProtheusDto();
                   produtoProtheus.Filial = jObj["Filial"].ToString();
                   produtoProtheus.NomeFilial = jObj["NomeFilial"].ToString();
                   produtoProtheus.CodigoDellaVia = jObj["CodigoDellaVia"].ToString();
                   produtoProtheus.SaldoDisponivel = Convert.ToDecimal(jObj["SaldoDisponivel"].ToString());
                   produtoProtheus.SaldoAtual = Convert.ToDecimal(jObj["SaldoAtual"].ToString());

                   produtoProtheusDtoList.Add(produtoProtheus);
               }
               return produtoProtheusDtoList;
           }
   */

    public ModalEstoqueDto ObterSaldoProdutoLojasDellaVia(CatalogoFraga produto, string[] idsLojaDellavia)
    {
        var lojasDellaVia = new List<LojaDellaVia>();
        if (idsLojaDellavia == null)
            lojasDellaVia = _lojaDellaViaRepository.GetAll();
        else
            foreach (string id in idsLojaDellavia)
            {
                var loja = _lojaDellaViaRepository.GetByCampoCodigo(id);
                if (loja != null)
                    lojasDellaVia.Add(loja);
            }
        if (lojasDellaVia.Count <= 0)
            throw new NegocioException("Nenhuma loja foi encontrada.");

        var modalEstoqueDto = new ModalEstoqueDto();
        var produtoProtheusDtoList = new List<ProdutoProtheusDto>();
        foreach (var loja in lojasDellaVia)
        {
            var qtdResult = GetEstoqueProdutoFiliais(produto.ProdutoCodDellavia, loja.CampoCodigo);
            //var jsonResult = await _estoqueProtheusApi.BuscaEstoqueProdutoFiliais(produto.ProdutoCodDellavia, codLojasDellaVia.ToArray());
            var produtoProtheus = new ProdutoProtheusDto
            {
                Filial = loja.Id.ToString(),
                NomeFilial = loja.Descricao,
                CodigoDellaVia = loja.CampoCodigo,
                SaldoDisponivel = Convert.ToDecimal(qtdResult),
                SaldoAtual = Convert.ToDecimal(qtdResult)
            };
            produtoProtheusDtoList.Add(produtoProtheus);
        }
        modalEstoqueDto.CampoCodigo = produto.ProdutoCodDellavia;
        modalEstoqueDto.ProdutoProtheus = produtoProtheusDtoList.OrderByDescending(a => a.SaldoDisponivel).ToList();
        modalEstoqueDto.DescricaoProduto = produto.ProdutoDescricao;
        return modalEstoqueDto;
    }

    public ProdutoComplementoDto ObterHtmlMaisDetalhesProduto(string ProdutoPaiId)
    {
        var produtoComplemento = GetProdutoComplementoByCampoCodigo(ProdutoPaiId);
        if (produtoComplemento == null)
            return null;
        return produtoComplemento;
    }

    public int TamanhoTermoFabricantePeca(string termoBusca)
    {
        return GetFabricantesPeca(termoBusca)
            .Distinct()
            .Count();
    }

    public List<string> ObterFabricantePecaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        termoBusca = termoBusca.ToLower();
        return GetFabricantesPeca(termoBusca)
            .Distinct().ToList();
    }

    public List<string> GetFabricantesPeca(string termoBusca)
    {
        termoBusca = ValidaNull(termoBusca);
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            /*string sql = "SELECT DISTINCT Produto_Marca " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Busca IS NULL OR (Produto_Marca LIKE '%' + @Busca + '%')) " +
                         " ORDER BY Produto_Marca";*/
            string sql = "SELECT DISTINCT NM_FABRICANTE " +
                         "  FROM DM_PRODUTO " +
                         " WHERE (@Busca IS NULL OR (NM_FABRICANTE LIKE '%' + @Busca + '%')) " +
                         " ORDER BY NM_FABRICANTE";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca?.ToLower() ?? (object)DBNull.Value);
            var list = new List<string>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(reader["NM_FABRICANTE"].ToString().Trim());
            }
            catch (Exception e)
            {
                throw e;
            }
            return list.Where(f => f != "").ToList();
        }
    }

    public List<Produto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "SELECT * FROM Produto ORDER BY Descricao";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<Produto> list = new List<Produto>();
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

    public Produto GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT * " +
                         "  FROM DM_PRODUTO " +
                         " WHERE ID_PRODUTO = @Id ";// +
                         //"   AND BLOQUEADO = 'N'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            Produto p = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        p = MapPD(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return p;
        }
    }

    public CatalogoFraga GetByCodigoDellavia(string codigoDellavia, string loja, string tabelaPreco)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnFraga))
        {
            string sql = "SELECT DISTINCT c.*, p.NM_FABRICANTE, p.CD_PRODUTO_FABRICANTE, p.ID_KITSERV, e.QT_EST, t.VL_PRCVEN, " +
                         "       s.Grupo AS CodigoGrupo, s.Descricao AS DescricaoGrupo, g.CampoCodigo AS CodigoSubGrupo, g.Descricao AS DescricaoSubGrupo " +
                         "  FROM FT_CATALOGO c " +
                         "  LEFT JOIN PowerData.dbo.DM_PRODUTO           p ON p.ID_PRODUTO  = c.Produto_ERP_Id " +
                         "  LEFT JOIN PowerData.dbo.FT_ESTOQUE	         e ON e.ID_PRODUTO  = p.ID_PRODUTO AND e.ID_LOCAL = '01' AND (@Loja IS NULL OR (e.CD_LOJA = @Loja))" +
                         "  LEFT JOIN PowerData.dbo.DM_TABELAS_DE_PRECOS t ON t.ID_PRODUTO  = p.ID_PRODUTO AND (@Tabela IS NULL OR (t.ID_TABELA = @Tabela))" +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_PRODUTO   g ON g.CampoCodigo = SUBSTRING(p.DS_GRUPO,1,4) " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_SUB_GRUPO s ON s.Grupo       = g.IdGrupoSubGrupo " +
                         " WHERE c.Produto_ERP_Id = @Produto_ERP_Id " +
                         "   AND t.VL_PRCVEN	  > 0 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Produto_ERP_Id", codigoDellavia ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Loja", loja?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Tabela", tabelaPreco?.ToUpper() ?? (object)DBNull.Value);
            CatalogoFraga c = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        c = MapCatalogoFraga(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return c;
        }
    }

    public CatalogoFraga GetByCodigoDellaviaPD(string busca, string loja, string tabelaPreco)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_DM_PRODUTO", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@COD_PROD", busca ?? (object)DBNull.Value);
            CatalogoFraga c = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        c = MapCatalogoFragaPD(reader, "proc", loja, tabelaPreco);
            }
            catch (Exception e)
            {
                throw e;
            }
            return c;
        }
    }

    public List<CatalogoFraga> GetByDescription(CatalogoFraga search, string loja, string tabelaPreco)
    {
        var list = new List<CatalogoFraga>();
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT TOP 1000 " +
                         "       s.Grupo AS CodigoGrupo, s.Descricao AS DescricaoGrupo, g.CampoCodigo AS CodigoSubGrupo, g.Descricao AS DescricaoSubGrupo, " +
                         "       c.Produto_Descricao, c.Produto_ERP_Id, c.Produto_Fabricante_Id, c.Produto_Marca, " +
                         //"       c.Veiculo_Marca, c.Veiculo_Modelo, c.Veiculo_Versao, c.Veiculo_VersaoMotor, c.Veiculo_Inicio_Producao, c.Veiculo_Final_Producao, " +
                         "       p.NM_FABRICANTE, p.CD_PRODUTO_FABRICANTE, p.DS_HTML, " +
                         "       p.ID_KITSERV, e.QT_EST, t.VL_PRCVEN " +
                         "  FROM FT_CATALOGO c " +
                         "  LEFT JOIN PowerData.dbo.DM_PRODUTO           p ON p.ID_PRODUTO = c.Produto_ERP_Id " +
                         "  LEFT JOIN PowerData.dbo.FT_ESTOQUE	         e ON e.ID_PRODUTO = p.ID_PRODUTO AND e.ID_LOCAL = '01' AND (@Loja IS NULL OR (e.CD_LOJA = @Loja))" +
                         "  LEFT JOIN PowerData.dbo.DM_TABELAS_DE_PRECOS t ON t.ID_PRODUTO = p.ID_PRODUTO AND (@Tabela IS NULL OR (t.ID_TABELA = @Tabela))" +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_PRODUTO   g ON g.CampoCodigo = SUBSTRING(p.DS_GRUPO,1,4) " +
                         "  LEFT JOIN Frenteloja_Dev.dbo.GRUPO_SUB_GRUPO s ON s.Grupo = g.IdGrupoSubGrupo " +
                         " WHERE ((@ProdutoSearch     IS NULL OR (c.Produto_Descricao     LIKE '%' + @ProdutoSearch + '%'))" +
                         "        OR (@ProdutoSearch  IS NULL OR (p.CD_PRODUTO_FABRICANTE LIKE '%' + @ProdutoSearch + '%'))" +
                         "        OR (@ProdutoSearch  IS NULL OR (c.Produto_ERP_Id        LIKE '%' + @ProdutoSearch + '%')))" +
                         "   AND ((@FabricantePeca    IS NULL OR (c.Produto_Marca         LIKE '%' + @FabricantePeca + '%'))" +
                         "        OR (@FabricantePeca IS NULL OR (p.NM_FABRICANTE         LIKE '%' + @FabricantePeca + '%')))" +
                         "   AND (@CodigoGrupo        IS NULL OR (s.Grupo                 = @CodigoGrupo)) " +
                         "   AND (@CodigoSubGrupo     IS NULL OR (g.CampoCodigo           = @CodigoSubGrupo)) ";
                if (!string.IsNullOrEmpty(search.VeiculoIdFraga))
                  sql += "   AND (@VeiculoIdFraga     IS NULL OR (c.Veiculo_Id            = @VeiculoIdFraga)) ";
                else { 
                  sql += "   AND (@VeiculoMarca       IS NULL OR (c.Veiculo_Marca         = @VeiculoMarca)) " +
                         "   AND (@VeiculoModelo      IS NULL OR (c.Veiculo_Modelo        = @VeiculoModelo))" +
                         "   AND (@VeiculoVersao      IS NULL OR (c.Veiculo_Versao        = @VeiculoVersao))" +
                         "   AND (@VersaoMotor        IS NULL OR (c.Veiculo_VersaoMotor   = @VersaoMotor))" ;
                 }
                  sql += "   AND (@VeiculoAnoInicial  IS NULL OR (DATEPART(YEAR, c.Veiculo_Inicio_Producao) <= @VeiculoAnoInicial))" +
                         "   AND (@VeiculoAnoFinal    IS NULL OR (DATEPART(YEAR, c.Veiculo_Final_Producao)  >= @VeiculoAnoFinal)) " +
                         "   AND t.VL_PRCVEN		  > 0 " +
                         "   AND c.Produto_ERP_Id     IS NOT NULL " +
                         //"   AND p.BLOQUEADO             = 'N'" +
                         " GROUP BY s.Grupo, s.Descricao, g.CampoCodigo, g.Descricao, " +
                         "          c.Produto_Descricao, c.Produto_ERP_Id, c.Produto_Fabricante_Id, c.Produto_Marca, " +
                         "		    p.NM_FABRICANTE, p.CD_PRODUTO_FABRICANTE, p.DS_HTML, " +
                         "		    p.ID_KITSERV, e.QT_EST, t.VL_PRCVEN";
                         //" ORDER BY e.QT_EST DESC ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ProdutoSearch", search.ProdutoDescricao?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FabricantePeca", search.ProdutoFabricantePeca?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoGrupo", (search.CodigoGrupo != "0" && search.CodigoGrupo != null) ? search.CodigoGrupo : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoSubGrupo", search.CodigoSubGrupo ?? (object)DBNull.Value);
            if (!string.IsNullOrEmpty(search.VeiculoIdFraga))
                cmd.Parameters.AddWithValue("@VeiculoIdFraga", search.VeiculoIdFraga ?? (object)DBNull.Value);
            else
            {
                cmd.Parameters.AddWithValue("@VeiculoMarca", search.VeiculoMarca ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VeiculoModelo", search.VeiculoModelo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VeiculoVersao", search.VeiculoVersao ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VersaoMotor", search.VersaoMotor ?? (object)DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@VeiculoAnoInicial", search.VeiculoAnoInicial ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VeiculoAnoFinal", search.VeiculoAnoFinal ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Loja", loja?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Tabela", tabelaPreco?.ToUpper() ?? (object)DBNull.Value);
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapCatalogoFraga(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<CatalogoFraga> GetByDescriptionServicoPD(CatalogoFraga search, string loja, string tabelaPreco)
    {
        using (SqlConnection conn = new SqlConnection(strConnPowerData))
        {
            string sql = "SELECT DISTINCT s.Grupo AS CodigoGrupo, s.Descricao AS DescricaoGrupo, g.CampoCodigo AS CodigoSubGrupo, g.Descricao AS DescricaoSubGrupo, " +
                         " p.NOME_PRODUTO, p.ID_PRODUTO , p.CD_PRODUTO_FABRICANTE, p.NM_FABRICANTE, " +
                         " p.ID_KITSERV, e.QT_EST, t.VL_PRCVEN" +
                         "  FROM DM_PRODUTO p " +
                         "  LEFT JOIN PowerData.dbo.FT_ESTOQUE	         e ON e.ID_PRODUTO = p.ID_PRODUTO AND e.ID_LOCAL = '01' AND (@Loja IS NULL OR (e.CD_LOJA = @Loja))" +
                         "  LEFT JOIN PowerData.dbo.DM_TABELAS_DE_PRECOS t ON t.ID_PRODUTO = p.ID_PRODUTO AND (@Tabela IS NULL OR (t.ID_TABELA = @Tabela))" +
                         " INNER JOIN Frenteloja_Dev.dbo.GRUPO_PRODUTO   g ON g.CampoCodigo = SUBSTRING(p.DS_GRUPO,1,4) " +
                         " INNER JOIN Frenteloja_Dev.dbo.GRUPO_SUB_GRUPO s ON s.Grupo = g.IdGrupoSubGrupo " +
                         " WHERE ((@ProdutoSearch        IS NULL OR (p.NOME_PRODUTO          LIKE '%' + @ProdutoSearch + '%'))" +
                         "        OR (@ProdutoSearch     IS NULL OR (p.CD_PRODUTO_FABRICANTE LIKE '%' + @ProdutoSearch + '%'))" +
                         "        OR (@ProdutoSearch     IS NULL OR (p.ID_PRODUTO            LIKE '%' + @ProdutoSearch + '%')))" +
                         "   AND (@ProdutoFabricantePeca IS NULL OR (p.NM_FABRICANTE         LIKE '%' + @ProdutoFabricantePeca + '%'))" +
                         "   AND (@CodigoGrupo           IS NULL OR (s.Grupo                 = @CodigoGrupo)) " +
                         "   AND (@CodigoSubGrupo        IS NULL OR (g.CampoCodigo           = @CodigoSubGrupo)) " +
                         "   AND t.VL_PRCVEN			 > 0 " +
                         //"   AND p.BLOQUEADO             = 'N'" +
                         " ORDER BY p.ID_PRODUTO ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ProdutoSearch", search.ProdutoDescricao?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ProdutoFabricantePeca", search.ProdutoFabricantePeca?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoGrupo", search.CodigoGrupo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoSubGrupo", search.CodigoSubGrupo?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Loja", loja?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Tabela", tabelaPreco?.ToUpper() ?? (object)DBNull.Value);
            var list = new List<CatalogoFraga>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapCatalogoFragaPD(reader, "sql", loja, tabelaPreco));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<CatalogoFraga> GetByDescriptionPD(string busca, string loja, string tabelaPreco)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        CatalogoFraga search = serializer.Deserialize<CatalogoFraga>(busca);
        using (SqlConnection conn = new SqlConnection(strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_DM_PRODUTO", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@COD_PROD", search.ProdutoDescricao ?? (object)DBNull.Value);
            var list = new List<CatalogoFraga>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapCatalogoFragaPD(reader, "proc", loja, tabelaPreco));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list.Where(c => c.Valor != "0,00" && c.Valor != null).ToList();
        }
    }

    public Dictionary<string, decimal> ObterPrecoProdutoPorOrcamento(Orcamento orcamento, string loja, string[] campoCodigoList)
    {
        var codPrecoDictionary = new Dictionary<string, decimal>();
        foreach (string campoCodigo in campoCodigoList.Distinct().ToList())
        {
            try
            {
                var p = GetByCodigoDellavia(campoCodigo, loja, orcamento.IdTabelaPreco) ?? GetByCodigoDellaviaPD(campoCodigo, loja, orcamento.IdTabelaPreco);
                codPrecoDictionary.Add(campoCodigo, _tabelaPrecoRepository.GetTabelaPrecoItem(orcamento.IdTabelaPreco, p.ProdutoCodDellavia)?.PrecoVenda ?? 0m);
            }
            catch (Exception)
            {
                codPrecoDictionary.Add(campoCodigo, 0m);
            }
        }
        return codPrecoDictionary;
    }

    public ProdutoComplementoDto GetProdutoComplementoByCampoCodigo(string ProdutoPaiId)
    {
        using (SqlConnection conn = new SqlConnection(strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_DM_PRODUTO", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@COD_PROD", ProdutoPaiId?.PadLeft(6, '0') ?? (object)DBNull.Value);
            var list = new List<ProdutoComplementoDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new ProdutoComplementoDto
                        {
                            //Id = (long)reader["Id"],
                            IdProduto = reader["ID_PRODUTO"].ToString(),
                            //Comprimento = (decimal)reader["Comprimento"],
                            //Espessura = (decimal)reader["Espessura"],
                            Largura = (reader["PN_LARG"] == DBNull.Value || reader["PN_LARG"] == null || reader["PN_LARG"].ToString().Trim() == "") ? null : new decimal?(Convert.ToDecimal(reader["PN_LARG"])),
                            //VolumeM3 = (decimal)reader["VolumeM3"],
                            CampoHTML = reader["DS_HTML"].ToString(),
                            hasCampoHTML = (reader["DS_HTML"].ToString().Trim() == "" || reader["DS_HTML"].ToString() == null) ? false : true,
                            Descricao = reader["NOME_PRODUTO"].ToString(),
                            Perfil = (reader["PN_PERFIL"] == DBNull.Value || reader["PN_PERFIL"] == null || reader["PN_PERFIL"].ToString().Trim() == "") ? null : new decimal?(Convert.ToDecimal(reader["PN_PERFIL"])),
                            Aro = (reader["PN_ARO"] == DBNull.Value || reader["PN_PERFIL"] == null || reader["PN_LARG"].ToString().Trim() == "") ? null : new decimal?(Convert.ToDecimal(reader["PN_ARO"])),
                            Carga = reader["PN_CARGA"].ToString(),
                            Indice = reader["IND_VELOCIDADE"].ToString(),
                            CampoCodigo = reader["ID_PRODUTO"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list.FirstOrDefault();

            /*string sql = "SELECT * " +
                         "  FROM PRODUTO_COMPLEMENTO " +
                         " WHERE CampoCodigo = @CampoCodigo";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CampoCodigo", CampoCodigo);
            ProdutoComplemento p = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        p = new ProdutoComplemento
                        {
                            Id = (long)reader["Id"],
                            IdProduto = (long)reader["IdProduto"],
                            Comprimento = (decimal)reader["Comprimento"],
                            Espessura = (decimal)reader["Espessura"],
                            Largura = (decimal)reader["Largura"],
                            VolumeM3 = (decimal)reader["VolumeM3"],
                            CampoHTML = reader["CampoHTML"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            Perfil = (decimal)reader["Perfil"],
                            Aro = (decimal)reader["Aro"],
                            Carga = reader["Carga"].ToString(),
                            Indice = reader["Indice"].ToString(),
                            CampoCodigo = reader["CampoCodigo"].ToString()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return p;*/
        }
    }

    public List<GrupoServicoAgregadoProdutoDto> GetKitServico(string idKit, string idTabelaPreco)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT k.*, p.NOME_PRODUTO, t.VL_PRCVEN " +
                         "  FROM DM_KIT_SERVICO k " +
                         "  LEFT JOIN DM_PRODUTO p ON p.ID_PRODUTO = k.ID_PRODUTO " +
                         "  LEFT JOIN DM_TABELAS_DE_PRECOS t ON t.ID_PRODUTO = p.ID_PRODUTO " +
                         " WHERE k.ID_KITSERV = @IdKit " +
                         "   AND (@IdTabela   IS NULL OR (t.ID_TABELA = @IdTabela)) " +
                         " ORDER BY k.NR_ITEM ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdKit", idKit);
            cmd.Parameters.AddWithValue("@IdTabela", idTabelaPreco ?? (object)DBNull.Value);
            var list = new List<GrupoServicoAgregadoProdutoDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapKit(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<GrupoServicoAgregadoProdutoDto> GetKitServicoByProduto(string idProduto)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_KIT_SERVICO " +
                         " WHERE ID_PRODUTO = @IdProduto " +
                         " ORDER BY NR_ITEM ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdProduto", idProduto);
            var list = new List<GrupoServicoAgregadoProdutoDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(MapKit(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }
    /*        public List<GrupoServicoAgregadoProdutoDto> GetGrupoServicoAgregadoProduto(string codigoDellaVia)
    {
        using (var conn = new SqlConnection(strConn))
        {
            string sql = "SELECT * " +
                         "  FROM GRUPO_SERVICO_AGREGADO_PRODUTO " +
                         " WHERE IdProduto = @IdProduto" +
                         "   AND RegistroInativo <> 1 ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdProduto", codigoDellaVia);
            var list = new List<GrupoServicoAgregadoProdutoDto>();
            GrupoServicoAgregadoProdutoDto p = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        p = new GrupoServicoAgregadoProdutoDto
                        {
                            Id = (long)reader["Id"],
                            IdProduto = reader["IdProduto"].ToString(),
                            Item = reader["Item"].ToString(),
                            PermiteAlterarQuantidade = (bool)reader["PermiteAlterarQuantidade"],
                            Quantidade = (int)reader["Quantidade"],
                            //IdGrupoServicoAgregado = reader["IdGrupoServicoAgregado"].ToString(),
                            Descricao = reader["Descricao"].ToString()
                            //CampoCodigo = reader["CampoCodigo"].ToString()
                        };
                        list.Add(p);
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
*/

    #region [ ESTOQUE ]
    public int GetEstoqueProdutoFiliais(string codProduto, string codLojasDellaVia)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_ESTOQUE", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@COD_PROD", codProduto ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LOJA", codLojasDellaVia ?? (object)DBNull.Value);
            int qtd = 0;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        qtd = Convert.ToInt32(reader["QT_EST"].ToString().Trim());
            }
            catch (Exception e)
            {
                throw e;
            }
            return qtd;
        }
    }
    #endregion

    public override void Save(Produto entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "INSERT INTO Produto " +
                        "(Descricao, " +
                        "IdGrupoProduto, " +
                        "CampoCodigo, " +
                        "IdGrupoServicoAgregado, " +
                        "RegistroInativo, " +
                        "DataAtualizacao, " +
                        "UsuarioAtualizacao, " +
                        "CodigoFabricante, " +
                        "FabricantePeca)" +
                        "VALUES " +
                        "(@Descricao, " +
                        "@IdGrupoProduto, " +
                        "@CampoCodigo, " +
                        "@IdGrupoServicoAgregado, " +
                        "@RegistroInativo, " +
                        "@DataAtualizacao, " +
                        "@UsuarioAtualizacao, " +
                        "@CodigoFabricante, " +
                        "@FabricantePeca)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Descricao", entity.Descricao);
            cmd.Parameters.AddWithValue("@IdGrupoProduto", entity.IdGrupoProduto);
            cmd.Parameters.AddWithValue("@CampoCodigo", entity.CampoCodigo);
            cmd.Parameters.AddWithValue("@IdGrupoServicoAgregado", entity.IdGrupoServicoAgregado);
            cmd.Parameters.AddWithValue("@RegistroInativo", entity.RegistroInativo);
            cmd.Parameters.AddWithValue("@DataAtualizacao", entity.DataAtualizacao);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", entity.UsuarioAtualizacao);
            cmd.Parameters.AddWithValue("@CodigoFabricante", entity.CodigoFabricante);
            cmd.Parameters.AddWithValue("@FabricantePeca", entity.FabricantePeca);
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

    public override void Update(Produto entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "UPDATE Produto SET " +
                        "Descricao=@Descricao, " +
                        "IdGrupoProduto=@IdGrupoProduto, " +
                        "CampoCodigo=@CampoCodigo, " +
                        "IdGrupoServicoAgregado=@IdGrupoServicoAgregado, " +
                        "RegistroInativo=@RegistroInativo, " +
                        "DataAtualizacao=@DataAtualizacao, " +
                        "UsuarioAtualizacao=@UsuarioAtualizacao, " +
                        "CodigoFabricante=@CodigoFabricante, " +
                        "FabricantePeca=@FabricantePeca " +
                        "WHERE Id=@Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Descricao", entity.Descricao);
            cmd.Parameters.AddWithValue("@IdGrupoProduto", entity.IdGrupoProduto);
            cmd.Parameters.AddWithValue("@CampoCodigo", entity.CampoCodigo);
            cmd.Parameters.AddWithValue("@IdGrupoServicoAgregado", entity.IdGrupoServicoAgregado);
            cmd.Parameters.AddWithValue("@RegistroInativo", entity.RegistroInativo);
            cmd.Parameters.AddWithValue("@DataAtualizacao", entity.DataAtualizacao);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", entity.UsuarioAtualizacao);
            cmd.Parameters.AddWithValue("@CodigoFabricante", entity.CodigoFabricante);
            cmd.Parameters.AddWithValue("@FabricantePeca", entity.FabricantePeca);
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

    public override void Delete(Produto entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE Produto Where Id=@Id";
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
            string sql = "DELETE Produto Where Id=@Id";
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

    private Produto Map(SqlDataReader reader)
    {
        return new Produto
        {
            Id = (long)reader["Id"],
            Descricao = reader["Descricao"].ToString(),
            IdGrupoProduto = reader["IdGrupoProduto"].ToString(),
            CampoCodigo = reader["CampoCodigo"].ToString(),
            IdGrupoServicoAgregado = reader["IdGrupoServicoAgregado"].ToString(),
            RegistroInativo = (bool)reader["RegistroInativo"],
            DataAtualizacao = (DateTime)reader["DataAtualizacao"],
            UsuarioAtualizacao = reader["UsuarioAtualizacao"].ToString(),
            CodigoFabricante = reader["CodigoFabricante"].ToString(),
            FabricantePeca = reader["FabricantePeca"].ToString()
        };
    }

    private Produto MapPD(SqlDataReader reader)
    {
        return new Produto
        {
            IdProduto = reader["ID_PRODUTO"].ToString().Trim(),
            Descricao = reader["NOME_PRODUTO"].ToString().Trim(),
            IdGrupoProduto = reader["CD_AGRUP_DV"].ToString().Trim(),
            CampoCodigo = reader["ID_PRODUTO"].ToString().Trim(),
            IdGrupoServicoAgregado = reader["ID_KITSERV"].ToString().Trim(),
            CodigoFabricante = reader["CD_PRODUTO_FABRICANTE"].ToString().Trim(),
            FabricantePeca = reader["NM_FABRICANTE"].ToString().Trim()
        };
    }

    private CatalogoFraga MapCatalogoFraga(SqlDataReader reader)
    {
        var cat = new CatalogoFraga();
        cat.ProdutoDescricao = reader["Produto_Descricao"].ToString();
        cat.ProdutoCodDellavia = reader["Produto_ERP_Id"].ToString().Trim();
        cat.ProdutoCodFabricante = reader["CD_PRODUTO_FABRICANTE"].ToString().Trim() ?? reader["Produto_Fabricante_Id"].ToString();
        cat.ProdutoFabricantePeca = reader["NM_FABRICANTE"].ToString().Trim() ?? reader["Produto_Marca"].ToString();
        cat.CodigoGrupo = reader["CodigoGrupo"].ToString();
        cat.DescricaoGrupo = reader["DescricaoGrupo"].ToString();
        cat.CodigoSubGrupo = reader["CodigoSubGrupo"].ToString();
        cat.DescricaoSubGrupo = reader["DescricaoSubGrupo"].ToString();
        //VeiculoMarca = reader["Veiculo_Marca"].ToString(),
        //VeiculoModelo = reader["Veiculo_Modelo"].ToString(),
        //VeiculoVersao = reader["Veiculo_Versao"].ToString(),
        //VersaoMotor = reader["Veiculo_VersaoMotor"].ToString(),
        //VeiculoAnoInicial = Convert.ToDateTime(reader["Veiculo_Inicio_Producao"]).ToString("dd/MM/yyyy"),
        //VeiculoAnoFinal = Convert.ToDateTime(reader["Veiculo_Final_Producao"]).ToString("dd/MM/yyyy"),
        cat.IdGrupoServicoAgregado = reader["ID_KITSERV"].ToString();
        cat.Estoque = (reader["QT_EST"] == DBNull.Value || reader["QT_EST"] == null) ? 0 : Convert.ToInt32(reader["QT_EST"].ToString().Trim());
        cat.Valor = (reader["VL_PRCVEN"] == DBNull.Value || reader["VL_PRCVEN"] == null) ? "0,00" : $"{reader["VL_PRCVEN"]:C}";
        return cat;
    }

    private CatalogoFraga MapCatalogoFragaPD(SqlDataReader reader, string origem, string loja, string tabelaPreco)
    {
        var cat = new CatalogoFraga();
        cat.ProdutoDescricao = reader["NOME_PRODUTO"].ToString().Trim();
        cat.ProdutoCodDellavia = reader["ID_PRODUTO"].ToString().Trim();
        cat.ProdutoCodFabricante = reader["CD_PRODUTO_FABRICANTE"].ToString().Trim();
        cat.ProdutoFabricantePeca = reader["NM_FABRICANTE"].ToString().Trim();
        cat.IdGrupoServicoAgregado = reader["ID_KITSERV"].ToString().Trim();
        if (origem != "proc")
        {
            cat.CodigoGrupo = reader["CodigoGrupo"].ToString();
            cat.DescricaoGrupo = reader["DescricaoGrupo"].ToString();
            cat.CodigoSubGrupo = reader["CodigoSubGrupo"].ToString();
            cat.DescricaoSubGrupo = reader["DescricaoSubGrupo"].ToString();
            cat.Estoque = (reader["QT_EST"] == DBNull.Value || reader["QT_EST"] == null) ? 0 : Convert.ToInt32(reader["QT_EST"].ToString().Trim());
            cat.Valor = (reader["VL_PRCVEN"] == DBNull.Value || reader["VL_PRCVEN"] == null) ? "0,00" : $"{reader["VL_PRCVEN"]:C}";
        }
        else
        {
            var produto = GetByCodigoDellavia(cat.ProdutoCodDellavia, loja, tabelaPreco);
            if (produto != null)
            {
                cat.Estoque = (int)produto?.Estoque;
                cat.Valor = produto?.Valor ?? "0,00";
            } else
            {
                cat.Estoque = (int)GetEstoqueProdutoFiliais(cat.ProdutoCodDellavia, loja);
                cat.Valor = _tabelaPrecoRepository.GetTabelaPrecoItem(tabelaPreco, cat.ProdutoCodDellavia)?.PrecoVenda.ToString();
            }
        }
        return cat;
    }

    private PDProduto MapProdutoPD(SqlDataReader reader)
    {
        return new PDProduto
        {
            ID_PRODUTO = reader["ID_PRODUTO"].ToString(),
            NOME_PRODUTO = reader["NOME_PRODUTO"].ToString(),
            TP_PRODUTO = reader["TP_PRODUTO"].ToString(),
            DS_UN_MEDIDA = reader["DS_UN_MEDIDA"].ToString(),
            CD_AGRUP_PIRELLI = reader["CD_AGRUP_PIRELLI"].ToString(),
            DS_AGRUP_PIRELLI = reader["DS_AGRUP_PIRELLI"].ToString(),
            CD_AGRUP_DV = reader["CD_AGRUP_DV"].ToString(),
            DS_AGRUP_DV_N1 = reader["DS_AGRUP_DV_N1"].ToString(),
            DS_AGRUP_DV_N2 = reader["DS_AGRUP_DV_N2"].ToString(),
            DS_AGRUP_DV_N3 = reader["DS_AGRUP_DV_N3"].ToString(),
            DS_MODELO = reader["DS_MODELO"].ToString(),
            DS_MEDIDA = reader["DS_MEDIDA"].ToString(),
            DS_AGRUP_DIR = reader["DS_AGRUP_DIR"].ToString(),
            DS_SAP = reader["DS_SAP"].ToString(),
            NM_FABRICANTE = reader["NM_FABRICANTE"].ToString(),
            DS_COMPOSICAO = reader["DS_COMPOSICAO"].ToString(),
            CD_PRODUTO_FABRICANTE = reader["CD_PRODUTO_FABRICANTE"].ToString(),
            IND_VELOCIDADE = reader["IND_VELOCIDADE"].ToString(),
            DS_GRUPO = reader["DS_GRUPO"].ToString(),
            BLOQUEADO = (bool)reader["BLOQUEADO"],
            PN_LARG = reader["PN_LARG"].ToString(),
            PN_PERFIL = reader["PN_PERFIL"].ToString(),
            PN_ARO = reader["PN_ARO"].ToString(),
            PN_CARGA = reader["PN_CARGA"].ToString(),
            PN_VELOC = reader["PN_VELOC"].ToString(),
            ID_KITSERV = reader["ID_KITSERV"].ToString()
        };
    }

    private GrupoServicoAgregadoProdutoDto MapKit(SqlDataReader reader)
    {
        return new GrupoServicoAgregadoProdutoDto
        {
            Id = reader["ID_KITSERV"].ToString().Trim(),
            Descricao = reader["NOME_PRODUTO"].ToString().Trim(),
            Item = reader["NR_ITEM"].ToString().Trim(),
            IdProduto = reader["ID_PRODUTO"].ToString().Trim(),
            Quantidade = Convert.ToInt32(reader["QUANT"]),
            PermiteAlterarQuantidade = (reader["ALT_QUANT"].ToString().Trim() == 'S'.ToString()) ? true : false,
            PrecoUnitario = Convert.ToDecimal(reader["VL_PRCVEN"])
        };
    }

    private string ValidaNull(string campo)
    {
        if (string.IsNullOrEmpty(campo))
            campo = null;
        return campo;
    }
}
