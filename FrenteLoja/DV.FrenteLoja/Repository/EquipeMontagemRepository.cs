// DV.FrenteLoja.Repository.EquipeMontagemRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class EquipeMontagemRepository : AbstractRepository<OrcamentoItemEquipeMontagem, int>
{
    public int TamanhoProfissionalMontagemPorTermo(string termoBusca, string idFilial)
    {
        return GetProfissionalMontagem(termoBusca, idFilial).Count();
    }

    public List<ProfissionalMontagemDto> ObterProfissionalMontagemPorNome(string termoBusca, int tamanhoPagina, int numeroPagina, string idFilial)
    {
        return GetProfissionalMontagem(termoBusca, idFilial).Skip(tamanhoPagina * (numeroPagina - 1)).Take(tamanhoPagina).ToList();
    }

    private List<ProfissionalMontagemDto> GetProfissionalMontagem(string termoBusca, string idFilial)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT c.* " +
                      "  FROM DM_CONSULTOR c " +
                      " INNER JOIN FT_EQUIPE_VAREJO e ON e.ID_CONSULTOR = c.ID_CONSULTOR " +
                      " WHERE ((@Busca     IS NULL OR (c.NOME_CONSULTOR LIKE '%' + @Busca + '%')) " +
                      "        OR (@Busca  IS NULL OR (c.ID_CONSULTOR   LIKE '%' + @Busca + '%'))) " +
                      "   AND (@idFilial   IS NULL OR (c.ID_LOJA        LIKE '%' + @idFilial + '%')) " +
                      "   AND c.ATIVO = 'S' " +
                      " ORDER BY c.NOME_CONSULTOR";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", ((object)termoBusca.ToLower()) ?? ((object)DBNull.Value));
            cmd.Parameters.AddWithValue("@idFilial", ((object)idFilial) ?? ((object)DBNull.Value));
            List<ProfissionalMontagemDto> list = new List<ProfissionalMontagemDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(new ProfissionalMontagemDto
                        {
                            Id = reader["ID_CONSULTOR"].ToString().Trim(),
                            ProfissionalNome = reader["NOME_CONSULTOR"].ToString().Trim()
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

    public OrcamentoItemEquipeMontagem GetById(long id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT o.*, c.NOME_CONSULTOR " +
                      "  FROM ORCAMENTO_ITEM_EQUIPE_MONTAGEM o " +
                      " INNER JOIN PowerData.dbo.DM_CONSULTOR c ON c.ID_CONSULTOR = o.IdVendedor " +
                      " WHERE o.Id = @Id " +
                      "   AND o.RegistroInativo <> 1 ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            OrcamentoItemEquipeMontagem o = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.HasRows && reader.Read())
                        o = Map(reader);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return o;
        }
    }

    public List<OrcamentoItemEquipeMontagem> GetByIdOrcamentoItem(long idOrcamentoItem)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT o.*, c.NOME_CONSULTOR " +
                      "  FROM ORCAMENTO_ITEM_EQUIPE_MONTAGEM o " +
                      " INNER JOIN PowerData.dbo.DM_CONSULTOR c ON c.ID_CONSULTOR = o.IdVendedor " +
                      " WHERE o.IdOrcamentoItem = @IdOrcamentoItem " +
                      "   AND o.RegistroInativo <> 1 " +
                      " ORDER BY o.Funcao ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", idOrcamentoItem);
            List<OrcamentoItemEquipeMontagem> list = new List<OrcamentoItemEquipeMontagem>();
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

    public List<OrcamentoItemEquipeMontagem> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT * " +
                      "  FROM ORCAMENTO_ITEM_EQUIPE_MONTAGEM " +
                      " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<OrcamentoItemEquipeMontagem> list = new List<OrcamentoItemEquipeMontagem>();
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

    public void Add(OrcamentoItemEquipeMontagem orcamentoFormaPagamento)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "INSERT INTO ORCAMENTO_ITEM_EQUIPE_MONTAGEM " +
                      "(  IdOrcamentoItem,  IdVendedor,  Funcao,  RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao )" +
                      "VALUES " +
                      "( @IdOrcamentoItem, @IdVendedor, @Funcao, @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao )";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamentoItem", orcamentoFormaPagamento.IdOrcamentoItem);
            cmd.Parameters.AddWithValue("@IdVendedor", orcamentoFormaPagamento.IdVendedor);
            cmd.Parameters.AddWithValue("@Funcao", orcamentoFormaPagamento.Funcao > 0 ? orcamentoFormaPagamento.Funcao : (object)DBNull.Value);
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

    public override void Save(OrcamentoItemEquipeMontagem entity)
    {}

    public override void Update(OrcamentoItemEquipeMontagem entity)
    {}

    public override void Delete(OrcamentoItemEquipeMontagem entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE ORCAMENTO_ITEM_EQUIPE_MONTAGEM  WHERE Id = @Id";
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
            string sql = "DELETE ORCAMENTO_ITEM_EQUIPE_MONTAGEM  WHERE Id = @Id";
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

    public void DeleteByIdOrcamentoItem(long id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE ORCAMENTO_ITEM_EQUIPE_MONTAGEM " +
                         " WHERE IdOrcamentoItem = @Id";
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

    private OrcamentoItemEquipeMontagem Map(SqlDataReader reader)
    {
        return new OrcamentoItemEquipeMontagem
        {
            Id = (long)reader["Id"],
            IdOrcamentoItem = (long)reader["IdOrcamentoItem"],
            IdVendedor = reader["IdVendedor"].ToString(),
            Descricao = reader["NOME_CONSULTOR"].ToString().Trim(),
            Funcao = (ProfissionalMontagemFuncao)(int)reader["Funcao"]
        };
    }
}
