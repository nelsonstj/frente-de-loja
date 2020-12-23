using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Util;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaClienteServico : ICargaClienteServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly ICargaCadastrosProtheusSyncApi _protheusSyncApi;
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        private readonly IRepositorio<Marca> _marcaRepositorio;
        private readonly IRepositorio<MarcaModelo> _marcaModeloRepositorio;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
        private readonly IRepositorio<Veiculo> _veiculoRepositorio;
        private readonly IRepositorio<ClienteVeiculo> _clienteVeiculoRepositorio;
        private readonly IRepositorio<Banco> _bancoRepositorio;
        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";


        public CargaClienteServico(IRepositorioEscopo escopo, ICargaCadastrosProtheusSyncApi protheusSyncApi)
        {
            _escopo = escopo;
            _protheusSyncApi = protheusSyncApi;
            _clienteRepositorio = escopo.GetRepositorio<Cliente>();
            _marcaRepositorio = escopo.GetRepositorio<Marca>();
            _marcaModeloRepositorio = escopo.GetRepositorio<MarcaModelo>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _veiculoRepositorio = escopo.GetRepositorio<Veiculo>();
            _clienteVeiculoRepositorio = escopo.GetRepositorio<ClienteVeiculo>();
            _bancoRepositorio = escopo.GetRepositorio<Banco>();
        }
        public async Task SyncCliente(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cliente, TipoTabelaProtheus.SA1);
            do
            {
                var clientList = new List<Cliente>();
                foreach (var jObj in jArray)
                {
                    Cliente cliente = null;
                    try
                    {
                        var bancoId = jObj["CodigoBanco"].ToString();

                        Banco banco = null;

                        if (!bancoId.IsNullOrEmpty())
                        {
                            banco = _bancoRepositorio.GetSingle(x => x.CampoCodigo == bancoId);
                        }


                        cliente = new Cliente
                        {
                            CampoCodigo = jObj["Codigo"].ToString(),
                            Nome = jObj["Nome"].ToString(),
                            CNPJCPF = jObj["CNPJCPF"].ToString(),
                            Score = jObj["ScoreCliente"].ToString(),
                            BancoId = banco?.Id,
                            TipoCliente = jObj["TipoCliente"].ToString(),
                            Loja = jObj["LOJA"].ToString(),
                            RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString()),
                            Telefone = jObj["Telefone"].ToString(),
                            TelefoneComercial = jObj["TelefoneComercial"].ToString(),
                            TelefoneCelular = jObj["TelefoneCelular"].ToString(),
                            Email = jObj["Email"].ToString(),
                            ClassificacaoCliente = jObj["ClassificacaoCliente"].ToString()
                        };

                        if (jObj["StatusCliente"].ToString().IsNullOrEmpty() || jObj["StatusCliente"].ToString() != "1")
                        {
                            cliente.StatusCliente = StatusCliente.Liberado;
                        }
                        else
                        {
                            cliente.StatusCliente = StatusCliente.Bloqueado;
                        }

                        if (jObj["MotivoBloqueioCredito"].ToString().IsNullOrEmpty())
                            cliente.MotivoBloqueioCredito = StatusCreditoCliente.Liberado;
                        else
                            cliente.MotivoBloqueioCredito = (StatusCreditoCliente)Convert.ToInt32(jObj["MotivoBloqueioCredito"].ToString());

                        clientList.Add(cliente);

                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncCliente)} : {e}. Item: {cliente.ToStringLog()}");
                    }
                }

                var clienteRepositorio = new List<Cliente>();
                
                foreach (var cliente in clientList)
                {                   
                    Cliente c = null;
                    try
                    {
                        c = isFirstLoad ? null : _clienteRepositorio.GetSingle(x => x.CampoCodigo == cliente.CampoCodigo && x.Loja == cliente.Loja);

                        if (c != null)
                        {
                            c.Nome = cliente.Nome;
                            c.RegistroInativo = cliente.RegistroInativo;
                            c.DataAtualizacao = DateTime.Now;
                            c.CNPJCPF = cliente.CNPJCPF;
                            c.Score = cliente.Score;
                            c.StatusCliente = cliente.StatusCliente;
                            c.MotivoBloqueioCredito = cliente.MotivoBloqueioCredito;
                            c.TipoCliente = cliente.TipoCliente;
                            c.Loja = cliente.Loja;
                            c.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            c.BancoId = cliente.BancoId;
                            c.Telefone = cliente.Telefone;
                            c.TelefoneCelular = cliente.TelefoneCelular;
                            c.TelefoneComercial = cliente.TelefoneComercial;
                            c.Email = cliente.Email;
                            c.ClassificacaoCliente = cliente.ClassificacaoCliente;
                        }
                        else
                        {
                            cliente.DataAtualizacao = DateTime.Now;
                            cliente.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            clienteRepositorio.Add(cliente);
                        }                       
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncCliente)} : {e}. Item: {c.ToStringLog()}");
                    }
                }

                try
                {
                    _clienteRepositorio.AddRange(clienteRepositorio);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Clientes na base de dados. Erro {e}.");
                }

                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cliente, TipoTabelaProtheus.SA1);

            } while (jArray.Count > 0);
            _escopo.Finalizar();


            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Clientes: {errosBuilder}.");
        }

        public async Task SyncClienteVeiculo(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cliente, TipoTabelaProtheus.PA7);
       

            do
            {
                var clienteVeiculoList = new List<ClienteVeiculo>();
                foreach (var jObj in jArray)
                {
                    try
                    {
                        var clientId = jObj["Cliente_Id"].ToString();
                        var clientLoja = jObj["Cliente_Loja"].ToString();

                        // Se o ClienteId do veiculo não estiver presente, ignoramos o registro incompleto.
                        if (clientId.IsNullOrEmpty() || clientLoja.IsNullOrEmpty())
                        {
                            errosBuilder.AppendLine($"Cliente id {clientId} e/ou clientLoja {clientLoja} estão nulos.");
                            continue;
                        }


                        var cliente = _clienteRepositorio.GetSingle(x => x.CampoCodigo == clientId && x.Loja == clientLoja);

                        // Se o ClienteId do veiculo não estiver presente, ignoramos o registro incompleto.
                        if (cliente == null)
                        {
                            errosBuilder.AppendLine($"Cliente id não encontrado {clientId}.");
                            continue;
                        }

                        var marcaId = jObj["Marca_Id"].ToString();

                        if (marcaId.IsNullOrEmpty())
                        {
                            errosBuilder.AppendLine($"MarcaId não informado {marcaId}.");
                            continue;
                        }

                        var marca = _marcaRepositorio.GetSingle(x => x.CampoCodigo == marcaId);

                        // Se o veiculo não possuir marca, ignoramos a importaação desse registro.
                        if (marca == null)
                        {
                            errosBuilder.AppendLine($"Marca id não encontrado {marcaId}.");
                            continue;
                        }

                        var marcaModeloId = jObj["MarcaModelo_Id"].ToString();

                        if (marcaModeloId.IsNullOrEmpty())
                        {
                            errosBuilder.AppendLine($"marcaModeloId não informado {marcaModeloId}.");
                            continue;
                        }


                        var marcaModelo = _marcaModeloRepositorio.GetSingle(x => x.CampoCodigo == marcaModeloId);
                        // Se o veiculo não possuir marca, ignoramos a importaação desse registro.
                        if (marcaModelo == null)
                        {
                            errosBuilder.AppendLine($"Marca Modelo id não encontrado {marcaModeloId}.");
                            continue;
                        }

                        var marcaModeloVersao = _marcaModeloVersaoRepositorio.GetSingle(x =>
                        x.MarcaModelo.IdMarca == marca.Id && x.IdMarcaModelo == marcaModelo.Id && x.Descricao.Equals(Constants.VERSAO_DEFAULT, StringComparison.InvariantCultureIgnoreCase));

                        // Se não estiver registrado no banco um marca modelo versao com a descrição 'todos' no banco de dados, devemos então criar esse registro.
                        if (marcaModeloVersao == null)
                        {
                            marcaModeloVersao = new MarcaModeloVersao
                            {
                                IdMarcaModelo = marcaModelo.Id,
                                Descricao = Constants.VERSAO_DEFAULT,
                                DataAtualizacao = DateTime.Now,
                                UsuarioAtualizacao = UsuarioAtualizacaoServico,
                            };
                            marcaModeloVersao.Id = _marcaModeloVersaoRepositorio.Add(marcaModeloVersao).Id;
                        }

                        var veiculo = _veiculoRepositorio.GetSingle(a => a.IdMarcaModeloVersao == marcaModeloVersao.Id);

                        var clienteVeiculo = new ClienteVeiculo
                        {
                            CampoCodigo = jObj["Id"].ToString(),
                            ClienteId = cliente.IdCliente,
                            Placa = jObj["Placa"].ToString(),
                            Ano = Convert.ToInt32(jObj["VeiculoAno"].ToString() == string.Empty ? "0" : jObj["VeiculoAno"].ToString()),
                            Observacoes = jObj["Observacoes"].ToString(),
                            VeiculoIdFraga = veiculo.IdFraga,
                            RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString())
                        };
                        clienteVeiculoList.Add(clienteVeiculo);
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncClienteVeiculo)} : {e}.");
                    }
                }

                var clienteVeiculoRepositorio = new List<ClienteVeiculo>();

                foreach (var cliente in clienteVeiculoList)
                {
                    try
                    {
                        var c = isFirstLoad ? null : _clienteVeiculoRepositorio.GetSingle(x => x.VeiculoIdFraga == cliente.VeiculoIdFraga && x.ClienteId == cliente.ClienteId);

                        if (c != null)
                        {
                            c.Placa = cliente.Placa;
                            c.Ano = cliente.Ano;
                            c.Observacoes = cliente.Observacoes;
                            c.RegistroInativo = cliente.RegistroInativo;
                            c.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            cliente.DataAtualizacao = DateTime.Now;
                            cliente.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            clienteVeiculoRepositorio.Add(cliente);
                        } 
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncClienteVeiculo)} : {e}.");
                    }
                }

                try
                {
                    _clienteVeiculoRepositorio.AddRange(clienteVeiculoRepositorio);                 
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Cliente Veiculo na base de dados. Erro {e}.");
                }
                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Cliente, TipoTabelaProtheus.PA7);

            } while (jArray.Count > 0);
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Cliente Veiculo: {errosBuilder}.");
        }
    }
}