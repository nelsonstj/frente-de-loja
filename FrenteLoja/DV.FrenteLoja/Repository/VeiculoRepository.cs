// DV.FrenteLoja.Repository.VeiculoRepository
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;

public class VeiculoRepository
{
    private readonly HttpClient _httpClient;
    protected string strConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;
    protected string strConnFraga { get; } = WebConfigurationManager.ConnectionStrings["Fraga"].ConnectionString;

    public VeiculoRepository()
    {
        _httpClient = new HttpClient();
    }

    public ClienteVeiculoDto ObterVeiculoPorPlaca(string placa)
    {
        placa = (placa.Contains(" ") ? placa.Remove(placa.IndexOf(' '), 1) : placa);
        var clienteVeiculoDto = GetVeiculoPorPlacaDB(placa) ??
                                ObterVeiculoFragaApi(placa) ??
                                ObterVeiculoPorPlacaApi(placa);// ?? 
                                //await ObterVeiculoPorPlacaApi(placa);// ?? 
                                //await ObterVeiculoPorPlacaWS(placa);
        return clienteVeiculoDto;
    }

    public ClienteVeiculoDto GetVeiculoPorPlacaDB(string placa)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string sql = "SELECT * " +
                             "  FROM CLIENTE_VEICULO " +
                             " WHERE Placa LIKE '%' + @Placa + '%' ";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Placa", placa ?? "");
                ClienteVeiculoDto v = null;
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            v = new ClienteVeiculoDto
                            {
                                ClienteId = reader["ClienteId"].ToString(),
                                Placa = reader["Placa"].ToString(),
                                Observacoes = reader["Observacoes"].ToString(),
                                VeiculoIdFraga = reader["VeiculoIdFraga"].ToString(),
                                Ano = (int)reader["Ano"]
                            };
                        }
                    }
                    if (v == null) return v;
                    if (v.VeiculoIdFraga == null) return v;

                    var veiculo = GetVeiculo(v.VeiculoIdFraga);
                    if (veiculo == null) return v;

                    v.Marca = veiculo.Marca;
                    v.Modelo = veiculo.Modelo;
                    v.Versao = veiculo.Versao;
                    v.Motor = veiculo.Motor;
                    v.Origem = "Origem: BD Frente de Loja";
                }
                catch (Exception e3)
                {
                    throw e3;
                }
                return v;
            }
        }
        catch (NegocioException e2)
        {
            throw e2;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    //    public async Task<ClienteVeiculoDto> ObterVeiculoPorPlacaApi(string placa)
    public ClienteVeiculoDto ObterVeiculoFragaApi(string placa)
    {
        try
        {
            //placa = (placa.Contains(" ") ? placa.Remove(placa.IndexOf(' '), 1) : placa);
            ClienteVeiculoDto veiculo = null;
            var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaFragaApi"];
            var resourcePath = $"{wsAddress}veiculos?placa={placa}";
            HttpClient _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5.0);
            var response = _httpClient.GetAsync(resourcePath).Result;
            if (response.IsSuccessStatusCode)
            {
                var jObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var marca = jObject["marca"].ToString().Split('/')[0];
                var marcaComposta = 0;
                var modeloObj = jObject["modelo"].ToString();
                var ano = jObject["ano"].ToString();
                var anoModelo = jObject["anoModelo"].ToString();
                string modelo = null;
                string versao = null;
                if (marca == "I" || marca == "IMP")
                {
                    marca = modeloObj.Split('/')[1].Split(' ')[0];
                    if (marca == "ALFA") marcaComposta = 1;
                    modelo = modeloObj.Split('/')[1].Split(' ')[1 + marcaComposta];
                    versao = modeloObj.Split('/')[1].Split(' ')[2 + marcaComposta];
                }
                else
                {
                    modelo = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[0] : modeloObj.Split(' ')[0];
                    int index = modeloObj.Contains("/") ? modeloObj.Split('/')[1].IndexOf(' ') : modeloObj.IndexOf(' ');
                    if (index > -1)
                        versao = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Substring(index + 1) : modeloObj.Substring(index + 1);
                }

                var codigoFraga = jObject["codigosFraga"].ToString();
                var veiculoFragaId = codigoFraga == "[]" ? "" : codigoFraga.Replace("[","").Replace("\"]","").Replace("\r\n", "").Replace("\"", "").Replace(" ", "").Split(',')[0];
                if (veiculoFragaId == "") 
                    return veiculo;
                veiculo = new ClienteVeiculoDto();
                veiculo.SinespMarca = marca;
                veiculo.SinespModelo = modelo;
                veiculo.SinespVersao = versao;
                veiculo.SinespAno = ano;
                veiculo.SinespAnoModelo = anoModelo;
                var veiculoFraga = GetVeiculo(veiculoFragaId);
                if (veiculoFraga != null)
                {
                    veiculo.VeiculoIdFraga = veiculoFraga.VeiculoIdFraga;
                    veiculo.Marca = veiculoFraga.Marca;
                    veiculo.Modelo = veiculoFraga.Modelo;
                    veiculo.Versao = veiculoFraga.Versao;
                    veiculo.Motor = veiculoFraga.Motor;
                    veiculo.Ano = Convert.ToInt32(ano);
                    veiculo.Origem = "Origem: Fraga";
                }
                veiculo.ClienteId = null;
            }
            return veiculo;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public ClienteVeiculoDto ObterVeiculoPorPlacaApi(string placa)
    {
        try
        {
            //var placa1 = placa.Contains(" ") ? placa.Split(' ')[0] : placa;
            //string placa2 = placa.Contains(" ") ? placa.Split(' ')[1] : placa;
            //placa = (placa.Contains(" ") ? placa.Remove(placa.IndexOf(' '), 1) : placa);
            ClienteVeiculoDto veiculo = null;
            var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaApi"];
            var resourcePath = $"{wsAddress}{placa}/json";
            HttpClient _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5.0);
            //var response = await _httpClient.GetAsync(resourcePath);
            var response = _httpClient.GetAsync(resourcePath).Result;
            if (response.IsSuccessStatusCode)
            {
                //var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                var jObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var marca = jObject["marca"].ToString().Split('/')[0];
                var marcaComposta = 0;
                var modeloObj = jObject["modelo"].ToString();
                var ano = jObject["ano"].ToString();
                var anoModelo = jObject["anoModelo"].ToString();
                string modelo = null;
                string versao = null;
                //long ok = 0L;
                if (marca == "I" || marca == "IMP")
                {
                    marca = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[0] : modeloObj.Split(' ')[0];
                    if (marca == "ALFA") marcaComposta = 1;
                    modelo = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[1 + marcaComposta] : modeloObj.Split(' ')[1 + marcaComposta];
                    versao = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[2 + marcaComposta] : modeloObj.Split(' ')[2 + marcaComposta];
                }
                else
                {
                    //if (long.TryParse(placa2, out ok)) // Verificação se a placa é no modelo do Mercosul
                    //{
                        modelo = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[0] : modeloObj.Split(' ')[0];
                        int index = modeloObj.Contains("/") ? modeloObj.Split('/')[1].IndexOf(' ') : modeloObj.IndexOf(' ');
                        if (index > -1)
                            versao = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Substring(index+1) : modeloObj.Substring(index+1);
                    //}
                    //else
                    //{
                    //    modelo = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[0] : modeloObj.Split(' ')[0];
                    //    versao = modeloObj.Contains("/") ? modeloObj.Split('/')[1].Split(' ')[1] : modeloObj.Split(' ')[1];
                    //}
                }
                veiculo = new ClienteVeiculoDto();
                veiculo.SinespMarca = marca;
                veiculo.SinespModelo = modelo;
                veiculo.SinespVersao = versao;
                veiculo.SinespAno = ano;
                veiculo.SinespAnoModelo = anoModelo;

                veiculo.Marca = GetMarca(marca)?.Descricao;
                veiculo.Modelo = GetModelo(veiculo.Marca, modelo, ano)?.Descricao;
                veiculo.Versao = GetVersao(veiculo.Marca, veiculo.Modelo, versao, ano)?.Descricao;
                veiculo.Ano = Convert.ToInt32(ano);
                veiculo.ClienteId = null;
                veiculo.Origem = "Origem: Api carros";
            }
            return veiculo;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /*        public async Task<VeiculoClienteModel> ObterVeiculoPorPlacaWS(string placa)
            {
                try
                {
                    placa = placa.Contains("-") ? placa.Remove(placa.IndexOf('-'), 1) : placa;
                    VeiculoClienteModel marcaModeloVersaoDto = null;
                    var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaWS"];
                    var resourcePath = $"{wsAddress}?placa={placa}";
                    _httpClient.Timeout = TimeSpan.FromSeconds(5);
                    var response = await _httpClient.GetAsync(resourcePath);
                    if (response.IsSuccessStatusCode)
                    {
                        marcaModeloVersaoDto = new VeiculoClienteModel();
                        var content = await response.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(content);
                        var marcaModeloVersao = jObject["marca"].ToString();
                        var ano = Convert.ToInt32(jObject["anoModelo"].ToString());
                        var marca = marcaModeloVersao.Split('/')[0];
                        var modelo = marcaModeloVersao.Split('/')[1].Split(' ')[0];
                        var index = marcaModeloVersao.Split('/')[1].IndexOf(' ');
                        var versao = marcaModeloVersao.Split('/')[1].Substring(index);

                        var marcaEntidade = _repositorioMarca.GetSingle(x => x.Descricao.Equals(marca,
                            StringComparison.InvariantCultureIgnoreCase));
                        if (marcaEntidade == null)
                        {
                            marcaEntidade = new Marca()
                            {
                                Descricao = marca,
                                DataAtualizacao = DateTime.Now,
                                UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper()
                            };
                            _repositorioMarca.Add(marcaEntidade);
                        }

                        var modeloEntidade = _repositorioMarcaModelo.GetSingle(x =>
                            x.Descricao.Equals(modelo, StringComparison.InvariantCultureIgnoreCase));

                        if (modeloEntidade == null)
                        {
                            modeloEntidade = new MarcaModelo()
                            {
                                Descricao = modelo,
                                UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                                DataAtualizacao = DateTime.Now,
                                IdMarca = marcaEntidade.Id
                            };
                            _repositorioMarcaModelo.Add(modeloEntidade);
                        }

                        var versaoEntidade = _respositorioMarcaModeloVersao.GetSingle(x =>
                            x.Descricao.Equals(versao, StringComparison.InvariantCultureIgnoreCase));
                        if (versaoEntidade == null)
                        {
                            versaoEntidade = new MarcaModeloVersao()
                            {
                                Descricao = versao,
                                UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                                DataAtualizacao = DateTime.Now,
                                IdMarcaModelo = modeloEntidade.Id
                            };
                            _respositorioMarcaModeloVersao.Add(versaoEntidade);
                        }
                        var clienteVeiculo = _repositorioClienteVeiculo.GetSingle(x => x.Placa.Equals(placa, StringComparison.InvariantCultureIgnoreCase));

                        marcaModeloVersaoDto.IdMarca = marcaEntidade.Id;
                        marcaModeloVersaoDto.Marca = marcaEntidade.Descricao;
                        marcaModeloVersaoDto.IdModelo = modeloEntidade.Id;
                        marcaModeloVersaoDto.Modelo = modeloEntidade.Descricao;
                        marcaModeloVersaoDto.IdVersao = versaoEntidade.Id;
                        marcaModeloVersaoDto.Versao = versaoEntidade.Descricao;
                        marcaModeloVersaoDto.Ano = ano;
                        marcaModeloVersaoDto.IdCliente = clienteVeiculo?.ClienteId ?? 0;

                    }
                    return marcaModeloVersaoDto;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return null;
                }
            }
    */
    /*      public async Task<VeiculoClienteModel> ObterVeiculoPorPlacaWSAuxiliar(string placa)
            {
                try
                {
                    placa = placa.Contains("-") ? placa.Remove(placa.IndexOf('-'), 1).ToUpper() : placa.ToUpper();
                    ClienteMarcaModeloVersaoDto marcaModeloVersaoDto = null;
                    var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaWSAuxiliar"];
                    var resourcePath = $"{wsAddress}?placa={placa}";
                    _httpClient.Timeout = TimeSpan.FromSeconds(5);
                    var response = await _httpClient.GetAsync(resourcePath);
                    if (response.IsSuccessStatusCode)
                    {
                        marcaModeloVersaoDto = new ClienteMarcaModeloVersaoDto();
                        var content = await response.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(content);
                        var anoArray = jObject["dados_veiculo"]["ano_fabricacao"].ToString().Split('/');
                        var ano = Convert.ToInt32(anoArray.Length == 2 ? anoArray[1] : anoArray[0]);
                        var marca = jObject["dados_veiculo"]["marca"].ToString();
                        var modeloRaw = jObject["dados_veiculo"]["modelo"].ToString();
                        var index = modeloRaw.IndexOf(' ');
                        var modelo = modeloRaw.Substring(0, index);
                        var versao = modeloRaw.Substring(index);


                        var marcaEntidade = _repositorioMarca.GetSingle(x => x.Descricao.Equals(marca,
                            StringComparison.InvariantCultureIgnoreCase));
                        if (marcaEntidade == null)
                        {

                            marcaEntidade = new Marca()
                            {
                                Descricao = marca,
                                DataAtualizacao = DateTime.Now,
                                UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper()
                            };
                            _repositorioMarca.Add(marcaEntidade);
                        }

                        var modeloEntidade = _repositorioMarcaModelo.GetSingle(x =>
                            x.Descricao.Equals(modelo, StringComparison.InvariantCultureIgnoreCase));

                        if (modeloEntidade == null)
                        {
                            modeloEntidade = new MarcaModelo()
                            {
                                Descricao = modelo,
                                UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                                DataAtualizacao = DateTime.Now,
                                IdMarca = marcaEntidade.Id
                            };
                            _repositorioMarcaModelo.Add(modeloEntidade);
                        }

                        var versaoEntidade = _respositorioMarcaModeloVersao.GetSingle(x =>
                            x.Descricao.Equals(versao, StringComparison.InvariantCultureIgnoreCase));
                        if (versaoEntidade == null)
                        {
                            versaoEntidade = new MarcaModeloVersao()
                            {
                                Descricao = versao,
                                UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                                DataAtualizacao = DateTime.Now,
                                IdMarcaModelo = modeloEntidade.Id
                            };
                            _respositorioMarcaModeloVersao.Add(versaoEntidade);
                        }

                        var clienteVeiculo = (from clienteVei in _repositorioClienteVeiculo.GetAll()
                                              where clienteVei.Placa == placa
                                              select clienteVei).FirstOrDefault();


                        marcaModeloVersaoDto.IdMarca = marcaEntidade.Id;
                        marcaModeloVersaoDto.Marca = marcaEntidade.Descricao;
                        marcaModeloVersaoDto.IdModelo = modeloEntidade.Id;
                        marcaModeloVersaoDto.Modelo = modeloEntidade.Descricao;
                        marcaModeloVersaoDto.IdVersao = versaoEntidade.Id;
                        marcaModeloVersaoDto.Versao = versaoEntidade.Descricao;
                        marcaModeloVersaoDto.Ano = ano;
                        marcaModeloVersaoDto.IdCliente = clienteVeiculo?.ClienteId ?? 0;

                    }
                    return marcaModeloVersaoDto;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return null;
                }
            }
    */

    public ClienteVeiculoDto ObterVeiculoCliente(string clienteId, string veiculoIdFraga)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string sql = "SELECT * " +
                             "  FROM VEICULO_CLIENTE " +
                             " WHERE ClienteId      LIKE '%' + @ClienteId + '%' " +
                             "   AND VeiculoIdFraga LIKE '%' + @VeiculoIdFraga + '%' ";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                cmd.Parameters.AddWithValue("@VeiculoIdFraga", veiculoIdFraga ?? "");
                ClienteVeiculoDto v = null;
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        if (reader.HasRows && reader.Read())
                            v = new ClienteVeiculoDto
                            {
                                Placa = reader["Placa"].ToString(),
                                ClienteId = reader["ClienteId"].ToString(),
                                VeiculoIdFraga = reader["VeiculoIdFraga"].ToString(),
                                Ano = (int)reader["Ano"]
                            };
                    if (v == null) return v;
                    if (v.VeiculoIdFraga == null) return v;

                    var veiculo = GetVeiculo(v.VeiculoIdFraga);
                    if (veiculo == null) return v;

                    v.Marca = veiculo.Marca;
                    v.Modelo = veiculo.Modelo;
                    v.Versao = veiculo.Versao;
                    v.Motor = veiculo.Motor;
                }
                catch (Exception e3)
                {
                    throw e3;
                }
                return v;
            }
        }
        catch (NegocioException e2)
        {
            throw e2;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public VeiculoModel ObterVeiculoRetira()
    {
        var veiculoRetira = new VeiculoModel();
        veiculoRetira.Marca = "RETIRA";
        veiculoRetira.Modelo = "RETIRA";
        veiculoRetira.Versao = "RETIRA";
        veiculoRetira.Motor = "RETIRA";
        return veiculoRetira;
    }

    public void AddVeiculoCliente(string placa, string clienteId, string veiculoIdFraga, int? ano, string usuario)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "INSERT INTO CLIENTE_VEICULO " +
                         "(  ClienteId,  Placa,  RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao,  VeiculoIdFraga,  Ano )" +
                         "VALUES " +
                         "( @ClienteId, @Placa, @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao, @VeiculoIdFraga, @Ano )";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ClienteId", clienteId);
            cmd.Parameters.AddWithValue("@Placa", placa ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VeiculoIdFraga", veiculoIdFraga ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", ano > 0 ? ano : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RegistroInativo", 0);
            cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", usuario ?? (object)DBNull.Value);
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

    public VeiculoModel GetVeiculo(string veiculoIdFraga)
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT Veiculo_Id, Veiculo_Marca, Veiculo_Modelo, Veiculo_Versao, Veiculo_VersaoMotor, Veiculo_Inicio_Producao, Veiculo_Final_Producao " +
                         "  FROM FT_CATALOGO " +
                         " WHERE Veiculo_Id LIKE '%' + @Veiculo + '%' ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Veiculo", veiculoIdFraga ?? "");
            VeiculoModel v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.HasRows && reader.Read())
                    {
                        v = new VeiculoModel
                        {
                            VeiculoIdFraga = reader["Veiculo_Id"].ToString(),
                            Marca = reader["Veiculo_Marca"].ToString(),
                            Modelo = reader["Veiculo_Modelo"].ToString(),
                            Versao = reader["Veiculo_Versao"].ToString(),
                            Motor = reader["Veiculo_VersaoMotor"].ToString(),
                            AnoInicial = (Convert.ToDateTime(reader["Veiculo_Inicio_Producao"]).ToString("dd/MM/yyyy")).Split('/')[2],
                            AnoFinal = (Convert.ToDateTime(reader["Veiculo_Final_Producao"]).ToString("dd/MM/yyyy")).Split('/')[2]
                        };
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public VeiculoModel GetVeiculo(string marca, string modelo, string versao, string motor, string ano)
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Id, Veiculo_Marca, Veiculo_Modelo, Veiculo_Versao, Veiculo_VersaoMotor, Veiculo_Inicio_Producao, Veiculo_Final_Producao " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@VeiculoMarca  IS NULL OR (Veiculo_Marca         = @VeiculoMarca)) " +
                         "   AND (@VeiculoModelo IS NULL OR (Veiculo_Modelo        = @VeiculoModelo))" +
                         "   AND (@VeiculoVersao IS NULL OR (Veiculo_Versao        = @VeiculoVersao))" +
                         "   AND (@VersaoMotor   IS NULL OR (Veiculo_VersaoMotor   = @VersaoMotor))" +
                         "   AND (@VeiculoAno    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @VeiculoAno))" +
                         "   AND (@VeiculoAno    IS NULL OR (DATEPART(YEAR, Veiculo_Final_Producao)  >= @VeiculoAno)) ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@VeiculoMarca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VeiculoModelo", modelo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VeiculoVersao", versao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VersaoMotor", motor ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VeiculoAno", ano ?? (object)DBNull.Value);
            VeiculoModel v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.HasRows && reader.Read())
                    {
                        v = new VeiculoModel
                        {
                            VeiculoIdFraga = reader["Veiculo_Id"].ToString(),
                            Marca = reader["Veiculo_Marca"].ToString(),
                            Modelo = reader["Veiculo_Modelo"].ToString(),
                            Versao = reader["Veiculo_Versao"].ToString(),
                            Motor = reader["Veiculo_VersaoMotor"].ToString(),
                            AnoInicial = Convert.ToDateTime(reader["Veiculo_Inicio_Producao"]).ToString("dd/MM/yyyy"),
                            AnoFinal = Convert.ToDateTime(reader["Veiculo_Final_Producao"]).ToString("dd/MM/yyyy")
                        };
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public List<VeiculoModel> GetVeiculos(string veiculoIdFraga)
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT Veiculo_Id, Veiculo_Marca, Veiculo_Modelo, Veiculo_Versao, Veiculo_VersaoMotor, Veiculo_Inicio_Producao, Veiculo_Final_Producao " +
                         "  FROM FT_CATALOGO " +
                         " WHERE Veiculo_Id LIKE '%' + @Veiculo + '%' ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Veiculo", veiculoIdFraga ?? "");
            var list = new List<VeiculoModel>();
            VeiculoModel v2 = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        v2 = new VeiculoModel
                        {
                            VeiculoIdFraga = reader["Veiculo_Id"].ToString(),
                            Marca = reader["Veiculo_Marca"].ToString(),
                            Modelo = reader["Veiculo_Modelo"].ToString(),
                            Versao = reader["Veiculo_Versao"].ToString(),
                            Motor = reader["Veiculo_VersaoMotor"].ToString(),
                            AnoInicial = Convert.ToDateTime(reader["Veiculo_Inicio_Producao"]).ToString("dd/MM/yyyy"),
                            AnoFinal = Convert.ToDateTime(reader["Veiculo_Final_Producao"]).ToString("dd/MM/yyyy")
                        };
                        list.Add(v2);
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

    public int QuantidadeMarcasPorTermo(string termoBusca)
    {
        return GetMarcas(termoBusca).Count();
    }

    public List<VeiculoMarcaModel> ObterMarcaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        return GetMarcas(termoBusca)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public VeiculoMarcaModel GetMarca(string marca)
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            switch (marca)
            {
                case "CAOA":
                case "CAOACHERY":
                case "CAOA CHERY":
                    marca = "CHERY";
                    break;
                case "GM":
                    marca = "CHEVROLET";
                    break;
                case "LR":
                    marca = "LAND ROVER";
                    break;
                case "MB":
                case "M.BENZ":
                    marca = "MERCEDES BENZ";
                    break;
                case "MMC":
                    marca = "MITSUBISHI";
                    break;
                case "VW":
                    marca = "VOLKSWAGEN";
                    break;
            }
            marca = (marca.Contains(".") ? marca.Split('.')[1] : marca);
            string sql = "SELECT Veiculo_Marca " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Marca IS NULL OR (Veiculo_Marca LIKE '%' + @Marca + '%' )) ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca?.ToUpper() ?? (object)DBNull.Value);
            VeiculoMarcaModel v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.Read())
                        v = new VeiculoMarcaModel
                        {
                            Descricao = reader["Veiculo_Marca"].ToString()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public List<VeiculoMarcaModel> GetMarcas(string marca)
    {
        marca = ValidaNull(marca);
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Marca " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Marca IS NULL OR (Veiculo_Marca LIKE '%' + @Marca + '%' )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca?.ToUpper() ?? (object)DBNull.Value);
            var list = new List<VeiculoMarcaModel>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new VeiculoMarcaModel
                        {
                            Descricao = reader["Veiculo_Marca"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<VeiculoMarcaModel> GetAllMarcas()
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT Veiculo_Marca FROM FT_CATALOGO ORDER BY Veiculo_Marca", conn);
            var list = new List<VeiculoMarcaModel>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new VeiculoMarcaModel
                        {
                            Descricao = reader["Veiculo_Marca"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public int QuantidadeModelosPorTermo(string marca, string versao, string motor, string ano, string buscaModelo)
    {
        return GetModelos(marca, versao, motor, ano, buscaModelo).Count();
    }

    public List<VeiculoModeloModel> ObterModelosPelaMarca(int tamanhoPagina, int numeroPagina, string marca, string versao, string motor, string ano, string buscaModelo)
    {
        return GetModelos(marca, versao, motor, ano, buscaModelo)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public VeiculoModeloModel GetModelo(string marca, string modelo, string ano)
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Modelo " +
                         "  FROM FT_CATALOGO " +
                         " WHERE Veiculo_Marca  LIKE @Marca " +
                         "   AND (@Modelo IS NULL OR (Veiculo_Modelo LIKE '%' + @Modelo + '%' )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @Ano )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Final_Producao)  >= @Ano )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Modelo", modelo?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", ano ?? (object)DBNull.Value);
            VeiculoModeloModel v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.Read())
                        v = new VeiculoModeloModel
                        {
                            Descricao = reader["Veiculo_Modelo"].ToString()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public List<VeiculoModeloModel> GetModelos(string marca, string versao, string motor, string ano, string buscaModelo)
    {
        marca = ValidaNull(marca);
        buscaModelo = ValidaNull(buscaModelo);
        versao = ValidaNull(versao);
        motor = ValidaNull(motor);
        ano = ValidaNull(ano);
        if (string.IsNullOrEmpty(marca) && string.IsNullOrEmpty(buscaModelo) && string.IsNullOrEmpty(ano))
            ano = DateTime.Now.Year.ToString();
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Modelo " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Marca  IS NULL OR (Veiculo_Marca       = @Marca )) " +
                         "   AND (@Modelo IS NULL OR (Veiculo_Modelo      LIKE '%' + @Modelo + '%' )) " +
                         "   AND (@Versao IS NULL OR (Veiculo_Versao      = @Versao )) " +
                         "   AND (@Motor  IS NULL OR (Veiculo_VersaoMotor = @Motor )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @Ano )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Final_Producao)  >= @Ano )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Modelo", buscaModelo?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Versao", versao?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Motor", motor?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", ano ?? (object)DBNull.Value);
            var list = new List<VeiculoModeloModel>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new VeiculoModeloModel
                        {
                            Descricao = reader["Veiculo_Modelo"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public int QuantidadeVersoesPorTermo(string marca, string modelo, string motor, string ano, string buscaVersao)
    {
        return GetVersoes(marca, modelo, motor, ano, buscaVersao).Count();
    }

    public List<VeiculoVersaoModel> ObterVersoesPeloModelo(int tamanhoPagina, int numeroPagina, string marca, string modelo, string motor, string ano, string buscaVersao)
    {
        return GetVersoes(marca, modelo, motor, ano, buscaVersao)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public VeiculoVersaoModel GetVersao(string marca, string modelo, string versao, string ano)
    {
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Versao " +
                         "  FROM FT_CATALOGO " +
                         " WHERE Veiculo_Marca  LIKE @Marca " +
                         "   AND Veiculo_Modelo LIKE @Modelo " +
                         "   AND (@Versao IS NULL OR (Veiculo_Versao      LIKE '%' + @Versao + '%' )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @Ano )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Final_Producao)  >= @Ano )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Modelo", modelo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Versao", versao?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", ano ?? (object)DBNull.Value);
            VeiculoVersaoModel v = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.Read())
                        v = new VeiculoVersaoModel
                        {
                            Descricao = reader["Veiculo_Versao"].ToString()
                        };
            }
            catch (Exception e)
            {
                throw e;
            }
            return v;
        }
    }

    public List<VeiculoVersaoModel> GetVersoes(string marca, string modelo, string motor, string ano, string buscaVersao)
    {
        marca = ValidaNull(marca);
        modelo = ValidaNull(modelo);
        buscaVersao = ValidaNull(buscaVersao);
        motor = ValidaNull(motor);
        ano = ValidaNull(ano);
        if (string.IsNullOrEmpty(marca) && string.IsNullOrEmpty(modelo) && string.IsNullOrEmpty(buscaVersao) && string.IsNullOrEmpty(ano))
            ano = DateTime.Now.Year.ToString();
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Versao " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Marca  IS NULL OR (Veiculo_Marca       = @Marca )) " +
                         "   AND (@Modelo IS NULL OR (Veiculo_Modelo      = @Modelo )) " +
                         "   AND (@Versao IS NULL OR (Veiculo_Versao      LIKE '%' + @Versao + '%' )) " +
                         "   AND (@Motor  IS NULL OR (Veiculo_VersaoMotor = @Motor )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @Ano )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Final_Producao)  >= @Ano )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Modelo", modelo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Versao", buscaVersao?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Motor", motor?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", ano ?? (object)DBNull.Value);
            var list = new List<VeiculoVersaoModel>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new VeiculoVersaoModel
                        {
                            Descricao = reader["Veiculo_Versao"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public int QuantidadeMotoresPorTermo(string marca, string modelo, string versao, string ano, string buscaMotor)
    {
        return GetMotores(marca, modelo, versao, ano, buscaMotor).Count();
    }

    public List<VeiculoMotorModel> ObterVersoesMotorPeloIdVersaoVeiculo(int tamanhoPagina, int numeroPagina, string marca, string modelo, string versao, string ano, string buscaMotor)
    {
        return GetMotores(marca, modelo, versao, ano, buscaMotor)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<VeiculoMotorModel> GetMotores(string marca, string modelo, string versao, string ano, string buscaMotor)
    {
        marca = ValidaNull(marca);
        modelo = ValidaNull(modelo);
        versao = ValidaNull(versao);
        buscaMotor = ValidaNull(buscaMotor);
        ano = ValidaNull(ano);
        if (string.IsNullOrEmpty(marca) && string.IsNullOrEmpty(modelo) && string.IsNullOrEmpty(versao) && string.IsNullOrEmpty(buscaMotor) && string.IsNullOrEmpty(ano))
            ano = DateTime.Now.Year.ToString();
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_VersaoMotor " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Marca  IS NULL OR (Veiculo_Marca       = @Marca )) " +
                         "   AND (@Modelo IS NULL OR (Veiculo_Modelo      = @Modelo)) " +
                         "   AND (@Versao IS NULL OR (Veiculo_Versao      = @Versao)) " +
                         "   AND (@Motor  IS NULL OR (Veiculo_VersaoMotor LIKE '%' + @Motor + '%' )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @Ano )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Final_Producao)  >= @Ano )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Modelo", modelo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Versao", versao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Motor", buscaMotor?.ToUpper() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", ano ?? (object)DBNull.Value);
            var list = new List<VeiculoMotorModel>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new VeiculoMotorModel
                        {
                            Descricao = reader["Veiculo_VersaoMotor"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public int QuantidadeAnosPorTermo(string marca, string modelo, string versao, string motor, string buscaAno)
    {
        return GetAnos(marca, modelo, versao, motor, buscaAno).Count();
    }

    public List<AnosModel> ObterAnosVeiculo(int tamanhoPagina, int numeroPagina, string marca, string modelo, string versao, string motor, string buscaAno)
    {
        return GetAnos(marca, modelo, versao, motor, buscaAno)
            .Skip(tamanhoPagina * (numeroPagina - 1))
            .Take(tamanhoPagina)
            .ToList();
    }

    public List<AnosModel> GetAnos(string marca, string modelo, string versao, string motor, string buscaAno)
    {
        marca = ValidaNull(marca);
        modelo = ValidaNull(modelo);
        versao = ValidaNull(versao);
        motor = ValidaNull(motor);
        buscaAno = ValidaNull(buscaAno);
        if (string.IsNullOrEmpty(marca) && string.IsNullOrEmpty(modelo) && string.IsNullOrEmpty(versao) && string.IsNullOrEmpty(motor) && string.IsNullOrEmpty(buscaAno))
            buscaAno = DateTime.Now.Year.ToString();
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            string sql = "SELECT DISTINCT Veiculo_Inicio_Producao, Veiculo_Final_Producao " +
                         "  FROM FT_CATALOGO " +
                         " WHERE (@Marca  IS NULL OR (Veiculo_Marca       LIKE @Marca )) " +
                         "   AND (@Modelo IS NULL OR (Veiculo_Modelo      LIKE @Modelo)) " +
                         "   AND (@Versao IS NULL OR (Veiculo_Versao      LIKE @Versao)) " +
                         "   AND (@Motor  IS NULL OR (Veiculo_VersaoMotor LIKE @Motor )) " +
                         "   AND (@Ano    IS NULL OR (DATEPART(YEAR, Veiculo_Inicio_Producao) <= @Ano )) " +
                         " ORDER BY 1";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Marca", marca ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Modelo", modelo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Versao", versao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Motor", motor ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", buscaAno?.ToUpper() ?? (object)DBNull.Value);
            var list = new List<int>();
            var listAnos = new List<AnosModel>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        string dataInicial = reader["Veiculo_Inicio_Producao"].ToString().Split(' ')[0];
                        string dataFinal = reader["Veiculo_Final_Producao"].ToString().Split(' ')[0];
                        int anoInicial = Convert.ToInt32(dataInicial.Split('/')[2]);
                        list.Add(anoInicial);
                        int anoFinal = Convert.ToInt32(dataFinal.Split('/')[2]);
                        list.Add(anoFinal);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            if (list == null) return listAnos;
            list = list.Distinct().ToList();
            int anoMax = list.Max();
            if (anoMax >= DateTime.Now.Year + 2)
                anoMax = DateTime.Now.Year + 2;
            for (int c = anoMax; c >= list.Min(); c--)
                listAnos.Add(new AnosModel
                {
                    Id = c,
                    Descricao = c.ToString()
                });
            return listAnos;
        }
    }

    public List<CatalogoFraga> GetByDescription(string busca)
    {
        //dynamic item = JObject.Parse(busca);
        //Dictionary search = item.ToObject > ();
        //dynamic search = JsonConvert.DeserializeObject(busca);
        var serializer = new JavaScriptSerializer();
        var search = serializer.Deserialize<CatalogoFraga>(busca);
        using (SqlConnection conn = new SqlConnection(strConnFraga))
        {
            SqlCommand cmd = new SqlCommand("PRC_GetProdutoBySearch", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProdutoSearch", search.ProdutoDescricao ?? "");
            cmd.Parameters.AddWithValue("@ProdutoFabricantePeca", search.ProdutoFabricantePeca ?? "");
            cmd.Parameters.AddWithValue("@VeiculoMarca", search.VeiculoMarca ?? "");
            cmd.Parameters.AddWithValue("@VeiculoModelo", search.VeiculoModelo ?? "");
            cmd.Parameters.AddWithValue("@VeiculoVersao", search.VeiculoVersao ?? "");
            cmd.Parameters.AddWithValue("@VersaoMotor", search.VersaoMotor ?? "");
            cmd.Parameters.AddWithValue("@VeiculoAnoInicial", search.VeiculoAnoInicial ?? "");
            cmd.Parameters.AddWithValue("@VeiculoAnoFinal", search.VeiculoAnoFinal ?? "");
            var list = new List<CatalogoFraga>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(new CatalogoFraga
                        {
                            ProdutoDescricao = reader["Produto_Descricao"].ToString(),
                            ProdutoCodDellavia = reader["Produto_ERP_Id"].ToString(),
                            ProdutoCodFabricante = reader["Produto_Fabricante_Id"].ToString(),
                            ProdutoFabricantePeca = reader["Produto_Marca"].ToString(),
                            VeiculoMarca = reader["Veiculo_Marca"].ToString(),
                            VeiculoModelo = reader["Veiculo_Modelo"].ToString(),
                            VeiculoVersao = reader["Veiculo_Versao"].ToString(),
                            VersaoMotor = reader["Veiculo_VersaoMotor"].ToString(),
                            VeiculoAnoInicial = Convert.ToDateTime(reader["Veiculo_Inicio_Producao"]).ToString("dd/MM/yyyy"),
                            VeiculoAnoFinal = Convert.ToDateTime(reader["Veiculo_Final_Producao"]).ToString("dd/MM/yyyy")
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

    private string ValidaNull(string campo)
    {
        if (string.IsNullOrEmpty(campo))
            campo = null;
        return campo;
    }
}
