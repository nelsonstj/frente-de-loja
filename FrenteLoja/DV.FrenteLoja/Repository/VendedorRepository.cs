// DV.FrenteLoja.Repository.VendedorRepository
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class VendedorRepository : AbstractRepository<VendedorDto, int>
{
    public VendedorDto ObterConsultorLogado()
    {
        string consultorCod = HttpContext.Current.User.Identity.GetIdOperador();
        VendedorDto consultor = GetByUser(consultorCod);
        if (consultor == null)
            return null;
        return Mapper.Map<VendedorDto>(consultor);
    }

    public int TamanhoVendedorPorTermo(string termoBusca)
    {
        return GetVendedores(termoBusca).Count();
    }

    public List<VendedorDto> ObterVendedorPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetVendedores(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public VendedorDto GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_CONSULTOR " +
                         " WHERE ID_CONSULTOR = @Id ";// +
                         //"   AND ATIVO = 'S' ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id.PadLeft(6, '0'));
            VendedorDto v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        v = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public VendedorDto GetByUser(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_CONSULTOR " +
                         " WHERE ID_USER = @Id ";// +
                         //"   AND ATIVO = 'S' ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id.PadLeft(6, '0'));
            VendedorDto v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        v = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public List<VendedorDto> GetVendedores(string termoBusca)
    {
        termoBusca = ValidaNull(termoBusca);
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_CONSULTOR " +
                         " WHERE (@Busca IS NULL OR (NOME_CONSULTOR LIKE '%' + @Busca + '%')) " +
                         "    OR (@Busca IS NULL OR (ID_CONSULTOR   LIKE '%' + @Busca + '%')) " +
                         //"   AND ATIVO <> 'N' " +
                         " ORDER BY NOME_CONSULTOR";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca?.ToUpper() ?? (object)DBNull.Value);
            List<VendedorDto> list = new List<VendedorDto>();
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
            return list; //.Where((VendedorDto s) => s.Ativo == 'S'.ToString()).ToList();
        }
    }

    public List<VendedorDto> GetVendedores(string termoBusca, long idFilial)
    {
        termoBusca = ValidaNull(termoBusca);
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_CONSULTOR " +
                         " WHERE ((@Busca     IS NULL OR (NOME_CONSULTOR LIKE '%' + @Busca + '%'))" +
                         "         OR (@Busca IS NULL OR (ID_CONSULTOR   LIKE '%' + @Busca + '%')))" +
                         "   AND (@idFilial   IS NULL OR (ID_LOJA        LIKE '%' + @idFilial + '%'))" +
                         //"   AND ATIVO = 'S' " +
                         " ORDER BY NOME_CONSULTOR";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Busca", termoBusca.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@idFilial", idFilial > 0 ? idFilial : (object)DBNull.Value);
            var list = new List<VendedorDto>();
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

    public List<VendedorDto> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "SELECT DISTINCT * " +
                         "  FROM DM_CONSULTOR " +
                         "   AND ATIVO = S " +
                         " ORDER BY NOME_CONSULTOR";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var list = new List<VendedorDto>();
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

    public override void Save(VendedorDto entity)
    {}

    public override void Update(VendedorDto entity)
    {}

    public override void Delete(VendedorDto entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            string sql = "DELETE VENDEDOR  WHERE Id = @Id";
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
            string sql = "DELETE VENDEDOR  WHERE Id = @Id";
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

    private VendedorDto Map(SqlDataReader reader)
    {
        return new VendedorDto
        {
            Nome = reader["NOME_CONSULTOR"].ToString().Trim(),
            CampoCodigo = reader["ID_CONSULTOR"].ToString().Trim(),
            IdConsultor = reader["ID_CONSULTOR"].ToString().Trim(),
            IdLoja = reader["ID_LOJA"].ToString().Trim(),
            IdRegional = reader["ID_REGIONAL"].ToString().Trim(),
            IdUser = reader["ID_USER"].ToString().Trim(),
            Ativo = reader["ATIVO"].ToString().Trim()
        };
    }

    private string ValidaNull(string campo)
    {
        if (string.IsNullOrEmpty(campo))
            campo = null;
        return campo;
    }
}
