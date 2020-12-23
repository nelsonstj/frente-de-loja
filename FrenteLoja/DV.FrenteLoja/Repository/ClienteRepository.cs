// DV.FrenteLoja.Repository.ClienteRepository
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Validator;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Repository;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class ClienteRepository : AbstractRepository<Cliente, int>
{
    private readonly ClienteValidator _clienteValidator;

    public ClienteRepository()
    {
        _clienteValidator = new ClienteValidator();
    }

    public int TamanhoClientePorTermo(string termoBusca)
    {
        return GetClientes(termoBusca).Count();
    }

    public List<ClienteDto> ObterClientePorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        var clientes = GetClientes(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
        return Mapper.Map<List<ClienteDto>>(clientes);
    }

    public List<Cliente> GetClientes(string termoBusca)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT TOP 1000 * " +
                      "  FROM DM_CLIENTES " +
                      " WHERE (@termoBusca IS NULL OR (NOME_CLIENTE LIKE '%' + @termoBusca + '%')) " +
                      " ORDER BY NOME_CLIENTE";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@termoBusca", termoBusca.ToUpper() ?? ((object)DBNull.Value));
            var list = new List<Cliente>();
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

    public ClienteDto ObterClientePorTipo(string info, string tipoConsulta)
    {
        var cliente = GetByTipo(info, tipoConsulta);
        if (cliente == null)
            return null;
        var clienteDto = Mapper.Map<ClienteDto>(cliente);
        var validationResult = _clienteValidator.Validate(clienteDto);
        if (validationResult.IsValid)
            return clienteDto;
        throw new NegocioException(string.Empty, validationResult.Errors);
    }

    public Cliente GetById(string id)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT *  " +
                      "  FROM DM_CLIENTES " +
                      " WHERE ID_CLIENTE = @ID_CLIENTE ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ID_CLIENTE", id);
            Cliente c = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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

    public Cliente GetByTipo(string info, string tipoConsulta)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var sql = "SELECT DISTINCT *  " +
                      "  FROM DM_CLIENTES ";
            switch (tipoConsulta)
            {
                case "id":
                    sql += " WHERE ID_CLIENTE = @Info ";
                    break;
                case "codigo":
                    sql += " WHERE ID_CLIENTE = @Info " +
                           "   AND (@Loja IS NULL OR (ID_LOJA = @Loja)) ";
                    break;
                case "cpf":
                    sql += " WHERE CPF_CNPJ = @Info    AND ID_LOJA  = '99' ";
                    break;
                case "cnpj":
                    sql += " WHERE CPF_CNPJ = @Info ";
                    break;
                case "nome":
                    sql += " WHERE NOME_CLIENTE LIKE '%' + @Info + '%'";
                    break;
            }
            var cmd = new SqlCommand(sql, conn);
            if (tipoConsulta == "codigo")
            {
                string cliente = info.Split('-')[0].ToUpper();
                string loja = info.Split('-')[1];
                cmd.Parameters.AddWithValue("@Info", cliente);
                if (!string.IsNullOrEmpty(loja))
                    cmd.Parameters.AddWithValue("@Loja", loja);
            }
            else
                cmd.Parameters.AddWithValue("@Info", info);
            Cliente c = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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

    public ClienteDto GetByCpfCnpj(string cpfCnpj)
    {
        using (SqlConnection conn = new SqlConnection(base.strConnPowerData))
        {
            var cmd = new SqlCommand("PRC_GET_DM_CLIENTES", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CPF_CNPJ", cpfCnpj);
            Cliente c = null;
            try
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                        c = Map(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Mapper.Map<ClienteDto>(c);
        }
    }

    public List<Cliente> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "SELECT DISTINCT *   FROM DM_CLIENTES  WHERE Id = @Id  ORDER BY NOME_CLIENTE";
            var cmd = new SqlCommand(sql, conn);
            var list = new List<Cliente>();
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

    public override void Save(Cliente entity)
    {}

    public override void Update(Cliente entity)
    {}

    public override void Delete(Cliente entity)
    {
        using (SqlConnection conn = new SqlConnection(base.strConn))
        {
            var sql = "DELETE DM_CLIENTES " +
                      " WHERE Id = @Id";
            var cmd = new SqlCommand(sql, conn);
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
            var sql = "DELETE DM_CLIENTES " +
                      " WHERE Id = @Id";
            var cmd = new SqlCommand(sql, conn);
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

    private Cliente Map(SqlDataReader reader)
    {
        string ddd = (reader["DS_DDD"].ToString().IndexOf('0') == 0) ? reader["DS_DDD"].ToString().Substring(1) : reader["DS_DDD"].ToString();
        string tel = (reader["DS_TELEFONE"].ToString().IndexOf('0') == 0) ? reader["DS_TELEFONE"].ToString().Substring(1) : reader["DS_TELEFONE"].ToString();
        return new Cliente
        {
            IdCliente = reader["ID_CLIENTE"].ToString().Trim(),
            Loja = reader["ID_LOJA"].ToString().Trim(),
            Nome = reader["NOME_CLIENTE"].ToString().Trim(),
            CNPJCPF = reader["CPF_CNPJ"].ToString().Trim(),
            Email = reader["DS_EMAIL"].ToString().Trim(),
            Telefone = (reader["DS_TELEFONE"].ToString().IndexOf('9') != 0) ? (string.IsNullOrEmpty(ddd) ? "00" + tel : ddd + tel) : null,
            TelefoneComercial = (reader["DS_TELEFONE"].ToString().IndexOf('9') != 0) ? (string.IsNullOrEmpty(ddd) ? "00" + tel : ddd + tel) : null,
            TelefoneCelular = (reader["DS_TELEFONE"].ToString().IndexOf('9') == 0) ? (string.IsNullOrEmpty(ddd) ? "00" + tel : ddd + tel) : null,
            TipoCliente = reader["TP_CLIENTE"].ToString().Trim(),
            CampoCodigo = reader["ID_CLIENTE"].ToString().Trim(),
            BancoId = (reader["BANCO"] == DBNull.Value || reader["BANCO"] == null || reader["BANCO"].ToString().Trim() == "") ? null : new long?(Convert.ToInt64(reader["BANCO"].ToString().Trim())),
            MotivoBloqueioCredito = (reader["BLOQUEIO"] == DBNull.Value || reader["BLOQUEIO"] == null || reader["BLOQUEIO"].ToString().Trim() == "") ? 0 : ObterStatusCreditoCliente(reader["BLOQUEIO"].ToString().Substring(0, 1))
        };
    }

    private StatusCreditoCliente ObterStatusCreditoCliente(string codBloqueio)
    {
        switch (codBloqueio)
        {
            case "A":
                return StatusCreditoCliente.CadastroInconsistente;
            case "B":
                return StatusCreditoCliente.SaldoDevedor;
            case "C":
                return StatusCreditoCliente.VendaEcommerce;
            case "D":
                return StatusCreditoCliente.TotalOleo;
            case "E":
                return StatusCreditoCliente.BaixadoPerdas;
            default:
                return (StatusCreditoCliente)Convert.ToInt32(codBloqueio);
        }
    }

private Cliente MapOld(SqlDataReader reader)
    {
        return new Cliente
        {
            Id = (long)reader["Id"],
            CNPJCPF = reader["CNPJCPF"].ToString(),
            Telefone = reader["Telefone"].ToString(),
            TelefoneComercial = reader["TelefoneComercial"].ToString(),
            TelefoneCelular = reader["TelefoneCelular"].ToString(),
            Email = reader["Email"].ToString(),
            Loja = reader["Loja"].ToString(),
            Nome = reader["Nome"].ToString(),
            StatusCliente = (StatusCliente)(int)reader["StatusCliente"],
            MotivoBloqueioCredito = (StatusCreditoCliente)(int)reader["MotivoBloqueioCredito"],
            TipoCliente = reader["TipoCliente"].ToString(),
            ClassificacaoCliente = reader["ClassificacaoCliente"].ToString(),
            Score = reader["Score"].ToString(),
            CampoCodigo = reader["CampoCodigo"].ToString()
        };
    }
}
