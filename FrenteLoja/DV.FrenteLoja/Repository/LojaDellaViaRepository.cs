// DV.FrenteLoja.Repository.LojaDellaViaRepository
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class LojaDellaViaRepository : AbstractRepository<LojaDellaVia, int>
{
    public LojaDellaViaDto ObterLojaUsuarioLogado()
    {
        List<string> lojaCodigo = HttpContext.Current.User.Identity.GetLojaPadrao();
        LojaDellaVia loja = GetByCampoCodigo(lojaCodigo[0]);
        if (loja == null)
            throw new Exception("Loja não encontrada no perfil logado do usuário.");
        return Mapper.Map<LojaDellaViaDto>(loja);
    }

    public int TamanhoLojasPorTermo(string termoBusca)
    {
        return GetLojas_PD(termoBusca).Count();
    }

    public List<LojaDellaViaDto> ObterLojasPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetLojas_PD(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<LojaDellaVia> ObterLojasProximas(string idLojaDellavia)
    {
        var lojasProximas = new List<LojaDellaViaDto>();
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_LOJAS_PROXIMAS " +
                      " WHERE ID_LOJA = @Id " +
                      " ORDER BY 2";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", idLojaDellavia);
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        lojasProximas.Add(new LojaDellaViaDto
                        {
                            CampoCodigo = reader["ID_LJPROX"].ToString().Trim()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            //return list;
        }
        var lojas = new List<LojaDellaVia>();
        foreach (var lojaDella in lojasProximas)
            lojas.Add(GetByCampoCodigo(lojaDella.CampoCodigo));
        return lojas;
    }

    public LojaDellaVia GetById(long? id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM LOJA_DELLAVIA " +
                      " WHERE Id = @Id " +
                      " ORDER BY Descricao";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            LojaDellaVia l = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        l = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return l;
        }
    }

    public LojaDellaVia GetById_PD(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_LOJA " +
                      " WHERE CD_LOJA = @Id " +
                      " ORDER BY NOME_LOJA";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            LojaDellaVia l = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        l = new LojaDellaVia
                        {
                            CampoCodigo = reader["CD_LOJA"].ToString().Trim(),
                            Descricao = reader["NOME_LOJA"].ToString().Trim()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return l;
        }
    }
    public LojaDellaVia GetByCampoCodigo(string campoCodigo)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM LOJA_DELLAVIA " +
                      " WHERE CampoCodigo = @CampoCodigo " +
                      " ORDER BY Descricao";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CampoCodigo", campoCodigo);
            LojaDellaVia l = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        l = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return l;
        }
    }

    public List<LojaDellaViaDto> GetLojasById_PD(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_LOJA " +
                      " WHERE CD_LOJA = @Id " +
                      " ORDER BY CD_LOJA";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            var lojas = new List<LojaDellaViaDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        lojas.Add(new LojaDellaViaDto
                        {
                            CampoCodigo = reader["CD_LOJA"].ToString().Trim()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return lojas;
        }
    }

    public List<LojaDellaViaDto> GetLojasById(long id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT Id, Descricao " +
                      "  FROM LOJA_DELLAVIA " +
                      " WHERE Id = @Id " +
                      " ORDER BY 2";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            List<LojaDellaViaDto> list = new List<LojaDellaViaDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new LojaDellaViaDto
                        {
                            Id = (long)reader["Id"],
                            Descricao = reader["Descricao"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<LojaDellaViaDto> GetLojas(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT Id, Descricao, CampoCodigo " +
                      "  FROM LOJA_DELLAVIA " +
                      " WHERE Descricao   LIKE '%' + @Busca + '%' " +
                      "    OR CampoCodigo LIKE '%' + @Busca + '%' " +
                      "   AND RegistroInativo <> 1 " +
                      " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToLower() ?? (object)DBNull.Value);
            var list = new List<LojaDellaViaDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new LojaDellaViaDto
                        {
                            Id = (long)reader["Id"],
                            Descricao = reader["CampoCodigo"].ToString() + "-" + reader["Descricao"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<LojaDellaViaDto> GetLojas_PD(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT * " +
                      "  FROM DM_LOJA " +
                      " WHERE CD_LOJA   LIKE '%' + @Busca + '%' " +
                      "    OR NOME_LOJA LIKE '%' + @Busca + '%' " +
                      " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToLower() ?? (object)DBNull.Value);
            var list = new List<LojaDellaViaDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new LojaDellaViaDto
                        {
                            CampoCodigo = reader["CD_LOJA"].ToString().Trim(),
                            Descricao = reader["NOME_LOJA"].ToString().Trim()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<LojaDellaVia> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT * " +
                      "  FROM DM_LOJA " +
                      " ORDER BY CD_LOJA";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<LojaDellaVia> list = new List<LojaDellaVia>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new LojaDellaVia
                        {
                            CampoCodigo = reader["CD_LOJA"].ToString().Trim(),
                            Descricao = reader["NOME_LOJA"].ToString().Trim()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public override void Save(LojaDellaVia entity)
    {}

    public override void Update(LojaDellaVia entity)
    {}

    public override void Delete(LojaDellaVia entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            string sql = "DELETE LOJA_DELLAVIA WHERE Id = @Id";
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
            string sql = "DELETE LOJA_DELLAVIA WHERE Id = @Id";
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

    private LojaDellaVia Map(SqlDataReader reader)
    {
        var loja = new LojaDellaVia();
        loja.Id = (long)reader["Id"];
        loja.Descricao = reader["Descricao"].ToString();
        loja.CampoCodigo = reader["CampoCodigo"].ToString();
        loja.Logradouro = reader["Logradouro"].ToString();
        loja.Bairro =  reader["Bairro"].ToString();
        loja.Cidade = reader["Cidade"].ToString();
        loja.Estado = reader["Estado"].ToString();
        loja.Cep = reader["Cep"].ToString();
        loja.Cnpj = reader["Cnpj"].ToString();
        loja.InscricaoEstadual = reader["InscricaoEstadual"].ToString();
        loja.Telefone = reader["Telefone"].ToString();
        loja.BancoId = (long)reader["BancoId"];
        return loja;
    }
}
