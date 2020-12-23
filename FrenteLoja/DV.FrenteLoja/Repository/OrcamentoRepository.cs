// DV.FrenteLoja.Repository.OrcamentoRepository
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Contratos.Validator;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Models;
using DV.FrenteLoja.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

public class OrcamentoRepository
{
    private EquipeMontagemRepository _equipeMontagemRepository = new EquipeMontagemRepository();
    private VeiculoRepository _veiculoRepository = new VeiculoRepository();
    private ClienteRepository _clienteRepository = new ClienteRepository();
    private ConvenioRepository _convenioRepository = new ConvenioRepository();
    private TransportadoraRepository _transportadoraRepository = new TransportadoraRepository();
    private LojaDellaViaRepository _lojaDellaViaRepository = new LojaDellaViaRepository();
    private OperadorRepository _operadorRepository = new OperadorRepository();
    private AreaNegocioRepository _areaNegocioRepository = new AreaNegocioRepository();
    private VendedorRepository _vendedorRepository = new VendedorRepository();
    private CondicaoPagamentoRepository _condicaoPagamentoRepository = new CondicaoPagamentoRepository();
    private readonly ProdutoRepository _produtoRepository;
    private readonly OrcamentoItemRepository _orcamentoItemRepository;
    private readonly TabelaPrecoRepository _tabelaPrecoRepository;
    private readonly OrcamentoFormaPagamentoRepository _orcamentoFormaPagamentoRepository;
    private readonly IRepositorioEscopo _escopo;
    private readonly ICalculoImpostosApi _calculoImpostosApi;
    private readonly IOrcamentoApi _orcamentoApi;
    private readonly ClienteValidator _clienteValidator;

    protected string strConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;

    public OrcamentoRepository(IRepositorioEscopo escopo, ICalculoImpostosApi calculoImpostosApi, IOrcamentoApi orcamentoApi, ProdutoRepository produtoRepository, OrcamentoItemRepository orcamentoItemRepository, TabelaPrecoRepository tabelaPrecoRepository, OrcamentoFormaPagamentoRepository orcamentoFormaPagamentoRepository)
    {
        _escopo = escopo;
        _calculoImpostosApi = calculoImpostosApi;
        _orcamentoApi = orcamentoApi;
        _clienteValidator = new ClienteValidator();
        _produtoRepository = produtoRepository;
        _orcamentoItemRepository = orcamentoItemRepository;
        _tabelaPrecoRepository = tabelaPrecoRepository;
        _orcamentoFormaPagamentoRepository = orcamentoFormaPagamentoRepository;
    }

    #region [ Orçamento ]
    public ICollection<OrcamentoConsultaDto> ObterListaOrcamentoVencidosHoje()
    {
        try
        {
            var orcamento = GetAllToday();
            return Mapper.Map<ICollection<OrcamentoConsultaDto>>(orcamento.ToList());
        }
        catch (NegocioException n)
        {
            throw n;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public ICollection<OrcamentoConsultaDto> ObterListaOrcamento(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento)
    {
        var query = new Orcamento();
        if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() != PerfilAcessoUsuario.TMK)
        {
            string loja = query.IdLojaDellaVia = HttpContext.Current.User.Identity.GetLojaPadrao().First();
        }
        else
        {
            string loginUsuario = query.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
        }

        if (statusOrcamento.HasValue)
            query.StatusOrcamento = statusOrcamento.Value;
        try
        {
            termoBusca = ValidaBuscaOrcamentos(tipoFiltro, termoBusca, statusOrcamento);
            switch (tipoFiltro)
            {
                case TipoFiltroOrcamento.IdOrcamento:
                    {
                        long idOrcamento = 0;
                        long.TryParse(termoBusca, out idOrcamento);
                        query.Id = idOrcamento;
                        break;
                    }
                case TipoFiltroOrcamento.NumeroProtheus:
                    {
                        string orcamentoCampoCodigo = query.CampoCodigo = termoBusca.Split('-')[0];
                        break;
                    }
                case TipoFiltroOrcamento.Placa:
                    query.Placa = termoBusca;
                    break;
                case TipoFiltroOrcamento.CPFCLiente:
                case TipoFiltroOrcamento.CNPJCliente:
                    query.Cliente = new Cliente{ CNPJCPF = termoBusca };
                    break;
                case TipoFiltroOrcamento.CodigoCliente:
                    query.Cliente = new Cliente { IdCliente = termoBusca };
                    if (termoBusca.Length == 9)
                    {
                        string clienteId = termoBusca.Split('-')[0];
                        string clienteLoja = termoBusca.Split('-')[1];
                        query.Cliente.IdCliente = query.IdCliente = clienteId;
                    }
                    break;
                case TipoFiltroOrcamento.Vendedor:
                    {
                        string vendedorCampoCodigo = query.IdVendedor = termoBusca;
                        break;
                    }
                case TipoFiltroOrcamento.LojaDestino:
                    {
                        query.LojaDellaVia = new LojaDellaVia { CampoCodigo = termoBusca };
                        break;
                    }
            }
            var orcamentos = GetOrcamentoByQuery(query);
            return Mapper.Map<ICollection<OrcamentoConsultaDto>>(orcamentos.Take(100).ToList());
        }
        catch (NegocioException i)
        {
            throw i;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<ICollection<OrcamentoConsultaDto>> ObterListaOrcamentoProtheus(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento)
    {
        if (tipoFiltro == TipoFiltroOrcamento.IdOrcamento)
            return new List<OrcamentoConsultaDto>();
        termoBusca = ValidaBuscaOrcamentos(tipoFiltro, termoBusca, statusOrcamento);
        var orcamentoFiltro = new OrcamentoFiltroProtheusDto
        {
            Status = statusOrcamento == StatusOrcamento.Todos ? null : new int?(statusOrcamento.GetHashCode()),
            CodVendedor = tipoFiltro == TipoFiltroOrcamento.Vendedor ? termoBusca : null,
            CodCliente = tipoFiltro == TipoFiltroOrcamento.CodigoCliente ? termoBusca : null,
            CodCnpj = tipoFiltro == TipoFiltroOrcamento.CNPJCliente ? termoBusca : null,
            CodCpf = tipoFiltro == TipoFiltroOrcamento.CPFCLiente ? termoBusca : null,
            CodOrcamento = tipoFiltro == TipoFiltroOrcamento.NumeroProtheus ? termoBusca : null,
            CodPlaca = tipoFiltro == TipoFiltroOrcamento.Placa ? termoBusca : null
        };
        if (!orcamentoFiltro.CodOrcamento.IsNullOrEmpty())
            orcamentoFiltro.CodFilial = null;
        else
        {
            orcamentoFiltro.CodFilial = tipoFiltro == TipoFiltroOrcamento.LojaDestino ? termoBusca : HttpContext.Current.User.Identity.GetLojaPadraoCampoCodigo();
            if (tipoFiltro == TipoFiltroOrcamento.CodigoCliente)
            {
                orcamentoFiltro.CodCliente = termoBusca.Replace("-", "");
                if (termoBusca.Length < 9)
                    orcamentoFiltro.CodCliente += orcamentoFiltro.CodFilial;
            }
        }
        return await _orcamentoApi.ObterOrcamentos(orcamentoFiltro);
    }

    public string ValidaBuscaOrcamentos(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento)
    {
        var loginUsuario = HttpContext.Current.User.Identity.Name.ToUpper();
        try
        {
            switch (tipoFiltro)
            {
                case TipoFiltroOrcamento.IdOrcamento:
                    long idOrcamento = 0;
                    long.TryParse(termoBusca, out idOrcamento);
                    if (idOrcamento == 0)
                        throw new NegocioException("Informe o ID do Orçamento.");
                    break;
                case TipoFiltroOrcamento.NumeroProtheus:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe o número Protheus - Loja. Formato esperado: 000000-00.");
                    if (!termoBusca.Contains("-"))
                        termoBusca = termoBusca + "-" + HttpContext.Current.User.Identity.GetLojaPadraoCampoCodigo().Split(',').FirstOrDefault();
                    if (termoBusca.Length != 9)
                        throw new NegocioException("O filtro número Protheus - Loja está em um formato incorreto. Formato esperado: 000000-00 ou 000000.");
                    break;
                case TipoFiltroOrcamento.Placa:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe a placa.");
                    termoBusca = termoBusca.Replace("-", "").ToUpper();
                    break;
                case TipoFiltroOrcamento.CPFCLiente:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe o CPF do Cliente.");
                    termoBusca = termoBusca.Replace(".", "").Replace("-", "").Replace("/", "");
                    break;
                case TipoFiltroOrcamento.CNPJCliente:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe o CNPJ do Cliente.");
                    termoBusca = termoBusca.Replace(".", "").Replace("-", "").Replace("/", "");
                    break;
                case TipoFiltroOrcamento.CodigoCliente:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe o código do cliente - Loja. Formato esperado: 000000-00 ou 000000.");
                    if (termoBusca.Length != 9 && termoBusca.Length != 6)
                        throw new NegocioException("O filtro código do cliente - Loja está em um formato incorreto. Formato esperado: 000000-00 ou 000000.");
                    break;
                case TipoFiltroOrcamento.Vendedor:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe o código do vendedor.");
                    break;
                case TipoFiltroOrcamento.LojaDestino:
                    if (string.IsNullOrEmpty(termoBusca))
                        throw new NegocioException("Informe o código da Loja.");
                    break;
            }
            return termoBusca;
        }
        catch (NegocioException n)
        {
            throw n;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public long IniciarOrcamento(OrcamentoDto novoOrcamentoDto)
    {
        if (novoOrcamentoDto.CodigoCliente.Contains("-"))
            novoOrcamentoDto.CodigoCliente = novoOrcamentoDto.CodigoCliente.Split('-')[0];
        var cliente = _clienteRepository.GetById(novoOrcamentoDto.CodigoCliente);
        if (cliente == null)
            throw new NegocioException("Cliente não encontrado");
        if (novoOrcamentoDto.Convenio == null)
            throw new NegocioException("Convênio não encontrado");
        var convenio = _convenioRepository.GetConvenio(novoOrcamentoDto.Convenio);
        if (convenio == null)
            throw new NegocioException("Convênio não encontrado");
        var lojaOrcamento = (novoOrcamentoDto.TipoOrcamento == TipoOrcamento.Telemarketing) ? novoOrcamentoDto.LojaDestino : HttpContext.Current.User.Identity.GetLojaPadrao().First();
        var lojaDellaVia = _lojaDellaViaRepository.GetByCampoCodigo(lojaOrcamento);
        if (lojaDellaVia == null)
            throw new NegocioException("Loja não encontrada");
        var veiculo = new VeiculoModel();
        if (novoOrcamentoDto.TipoOrcamento != TipoOrcamento.Retira)
        {
            veiculo = _veiculoRepository.GetVeiculo(novoOrcamentoDto.MarcaVeiculoDescricao, 
                                                    novoOrcamentoDto.ModeloVeiculoDescricao, 
                                                    novoOrcamentoDto.VersaoVeiculoDescricao, 
                                                    novoOrcamentoDto.VersaoMotorDescricao,
                                                    novoOrcamentoDto.AnoDescricao);
            if (veiculo == null)
                throw new NegocioException("Veículo não encontrado");
        }
        var transportadora = new TransportadoraDto();
        if (novoOrcamentoDto.Transportadora.HasValue)
            transportadora = _transportadoraRepository.GetTransportadorasById(novoOrcamentoDto.Transportadora);
        var operador = new VendedorDto();
        if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
        {
            operador = _vendedorRepository.GetByUser(HttpContext.Current.User.Identity.GetIdOperador());
            if (operador == null)
                throw new NegocioException("Operador não encontrado");
        }
        var tabelaPreco = _tabelaPrecoRepository.GetTabelaPrecoDto(novoOrcamentoDto.TabelaPreco);
        if (tabelaPreco == null)
            throw new NegocioException("Tabela de Preço não encontrada");
        var tipoVenda = _areaNegocioRepository.GetById(novoOrcamentoDto.AreaNegocio);
        if (tipoVenda == null)
            throw new NegocioException("Tipo de Venda não encontrada");
        var vendedor = _vendedorRepository.GetById(novoOrcamentoDto.Vendedor);
        if (vendedor == null)
            throw new NegocioException("Vendedor não encontrado");
        Orcamento orcamento;
        if (novoOrcamentoDto.Id > 0)
        {
            orcamento = GetOrcamentoById(novoOrcamentoDto.Id);
            if (orcamento == null)
                throw new NegocioException("Orçamento não encontrado.");
            // Chamar rotina de calcular impostos quando houver mudança de cliente e/ou loja destino.
            if (cliente.IdCliente != orcamento.IdCliente || lojaDellaVia.CampoCodigo != orcamento.IdLojaDellaVia)
                CalcularImpostos(orcamento);
            orcamento.Ano = novoOrcamentoDto.AnoVeiculo;
            orcamento.Complemento = novoOrcamentoDto.Observacao;
            orcamento.DataValidade = novoOrcamentoDto.DataValidade;
            orcamento.IdCliente = cliente.IdCliente;
            orcamento.IdConvenio = convenio.IdConvenio;
            orcamento.IdLojaDellaVia = lojaDellaVia.CampoCodigo;
            orcamento.VeiculoIdFraga = veiculo?.VeiculoIdFraga;
            orcamento.IdOperador = operador.IdConsultor;
            orcamento.IdTabelaPreco = tabelaPreco.Id;
            orcamento.IdAreaNegocio = tipoVenda.Id;
            orcamento.IdVendedor = vendedor.IdConsultor;
            orcamento.IdTransportadora = transportadora?.Id;
            orcamento.KM = novoOrcamentoDto.QuilometragemVeiculo;
            orcamento.Placa = string.IsNullOrEmpty(novoOrcamentoDto.PlacaVeiculo) ? null : Regex.Replace(novoOrcamentoDto.PlacaVeiculo, " ", "");
            orcamento.Telefone = novoOrcamentoDto.TelefoneCliente;
            orcamento.TelefoneCelular = novoOrcamentoDto.CelularCliente;
            orcamento.TelefoneComercial = novoOrcamentoDto.TelefoneComercialCliente;
            orcamento.InformacoesCliente = novoOrcamentoDto.InformacoesCliente;
            orcamento.MensagemNF = novoOrcamentoDto.MensagemNF;
            orcamento.Xped = novoOrcamentoDto.Xped;
            orcamento.TipoOrcamento = novoOrcamentoDto.TipoOrcamento;
            try
            {
                Update(orcamento);
            }
            catch (Exception excecao)
            {
                throw new NegocioException("Erro ao salvar o orçamento", excecao);
            }
        }
        else
        {
            orcamento = new Orcamento
            {
                Ano = novoOrcamentoDto.AnoVeiculo,
                Complemento = novoOrcamentoDto.Observacao,
                DataValidade = novoOrcamentoDto.DataValidade,
                IdCliente = cliente.IdCliente,
                IdConvenio = convenio.IdConvenio,
                IdLojaDellaVia = lojaDellaVia.CampoCodigo,
                VeiculoIdFraga = veiculo.VeiculoIdFraga,
                IdOperador = operador.IdConsultor,
                IdTabelaPreco = tabelaPreco.Id,
                IdAreaNegocio = tipoVenda.Id,
                IdVendedor = vendedor.IdConsultor,
                IdTransportadora = transportadora?.Id,
                KM = novoOrcamentoDto.QuilometragemVeiculo,
                Placa = string.IsNullOrEmpty(novoOrcamentoDto.PlacaVeiculo) ? null : Regex.Replace(novoOrcamentoDto.PlacaVeiculo, " ", ""),
                Telefone = novoOrcamentoDto.TelefoneCliente,
                TelefoneCelular = novoOrcamentoDto.CelularCliente,
                TelefoneComercial = novoOrcamentoDto.TelefoneComercialCliente,
                InformacoesCliente = novoOrcamentoDto.InformacoesCliente,
                MensagemNF = novoOrcamentoDto.MensagemNF,
                Xped = novoOrcamentoDto.Xped,
                StatusOrcamento = StatusOrcamento.EmAberto,
                TipoOrcamento = novoOrcamentoDto.TipoOrcamento
            };
            // Insere novo orçamento
            try
            {
                orcamento.Id = Add(orcamento);
            }
            catch (Exception excecao)
            {
                throw new NegocioException("Erro ao incluir o orçamento", excecao);
            }
        }
        // Validação do orçamento, cliente.
        var clienteDto = Mapper.Map<ClienteDto>(cliente);
        var resultado = _clienteValidator.Validate(clienteDto);
        if (!resultado.IsValid)
            throw new NegocioException(string.Empty, resultado.Errors);
        
        if (novoOrcamentoDto.TipoOrcamento != TipoOrcamento.Retira)
        {
            if (string.IsNullOrEmpty(_veiculoRepository.ObterVeiculoCliente(orcamento.IdCliente, veiculo?.VeiculoIdFraga)?.VeiculoIdFraga))
                _veiculoRepository.AddVeiculoCliente(orcamento.Placa, orcamento.IdCliente, veiculo.VeiculoIdFraga, orcamento.Ano, orcamento.UsuarioAtualizacao);
            /*novoOrcamentoDto.MarcaVeiculoDescricao = veiculo.Marca;
            novoOrcamentoDto.ModeloVeiculoDescricao = veiculo.Modelo;
            novoOrcamentoDto.VersaoVeiculoDescricao = veiculo.Versao;
            novoOrcamentoDto.VersaoMotorDescricao = veiculo.Motor;
            novoOrcamentoDto.AnoVeiculo = novoOrcamentoDto.AnoVeiculo;*/
        }
        return orcamento.Id;
    }

    public OrcamentoDto ObterOrcamentoPorId(long idOrcamento)
    {
        #region [ Orçamento ]
        var orc = GetOrcamentoById(idOrcamento);
        if (orc == null)
            throw new NegocioException($"Orçamento {idOrcamento} não encontrado.");
        if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
        {
            var usuario = HttpContext.Current.User.Identity.Name.ToUpper();
            if (orc.UsuarioAtualizacao != usuario)
                throw new NegocioException("Usuário não tem permissão para acessar o orçamento.");
        }
        else
        {
            var lojasUsuario = HttpContext.Current.User.Identity.GetLojaPadrao();
            if (!lojasUsuario.Contains(orc.IdLojaDellaVia))
                throw new NegocioException("Filial não tem permissão para acessar o orçamento.");
        }
        #region [ Início ]
        var orcamentoDto = new OrcamentoDto();
        orcamentoDto.Id = idOrcamento;
        orcamentoDto.CampoCodigo = orc.CampoCodigo;
        orcamentoDto.TipoOrcamento = orc.TipoOrcamento;
        orcamentoDto.Observacao = orc.Complemento;
        orcamentoDto.DataValidade = orc.DataValidade;
        orcamentoDto.Transportadora = orc.IdTransportadora;
        orcamentoDto.Xped = orc.Xped;
        orcamentoDto.MensagemNF = orc.MensagemNF;
        orcamentoDto.DataCriacao = orc.DataCriacao;
        orcamentoDto.StatusSomenteLeitura = ((orc.StatusOrcamento != 0 && orc.StatusOrcamento != StatusOrcamento.Reserva) || !(orc.DataValidade >= DateTime.Today));
        orcamentoDto.CampoCodigo = orc.CampoCodigo;
        orcamentoDto.Complemento = orc.Complemento;
        #endregion

        #region [ Área de Negócio ]
        orcamentoDto.AreaNegocio = orc.IdAreaNegocio;
        if (orc.IdAreaNegocio == null)
            throw new NegocioException("Tipo de Venda não encontrada");
        orcamentoDto.AreaNegocioDescricao = orc.AreaNegocio.NomeArea;
        #endregion

        #region [ Convenio ]
        var convenio = _convenioRepository.GetById(orc.IdConvenio);
        if (convenio == null)
            throw new NegocioException("Convênio não encontrado");
        orcamentoDto.Convenio = orc.IdConvenio;
        orcamentoDto.ConvenioDescricao = $"{convenio.CampoCodigo} - {convenio.Descricao}";
        orcamentoDto.TrocaTabelaPreco = convenio.TrocaTabelaPreco;
        orcamentoDto.TrocaPrecoConvenio = convenio.TrocaPreco;
        orcamentoDto.InformacaoConvenio = convenio.Observacoes;
        #endregion

        #region [ Tabela de preço ]
        orcamentoDto.TabelaPreco = orc.IdTabelaPreco;
        if (orc.IdTabelaPreco == null)
            throw new NegocioException("Tabela de Preço não encontrada");
        orcamentoDto.TabelaPrecoDescricao = orc.TabelaPreco.CampoCodigo;
        #endregion

        #region [ Veículo ]
        var veiculo = new VeiculoModel();
        if (orc.TipoOrcamento != TipoOrcamento.Retira)
        {
            veiculo = _veiculoRepository.GetVeiculo(orc.VeiculoIdFraga);
            if (veiculo == null)
                throw new NegocioException("Veículo não encontrado");
        }
        else
            veiculo = _veiculoRepository.ObterVeiculoRetira();
        orcamentoDto.MarcaVeiculoDescricao = veiculo?.Marca;
        orcamentoDto.ModeloVeiculoDescricao = veiculo?.Modelo;
        orcamentoDto.VersaoVeiculoDescricao = veiculo?.Versao;
        orcamentoDto.VersaoMotorDescricao = veiculo?.Motor;
        orcamentoDto.PlacaVeiculo = orc.Placa;
        orcamentoDto.AnoVeiculo = orc.Ano;
        orcamentoDto.AnoDescricao = orc.Ano.ToString();
        orcamentoDto.QuilometragemVeiculo = orc.KM;
        orcamentoDto.VeiculoIdFraga = orc.VeiculoIdFraga;
        #endregion

        #region [ Cliente ]
        if (orc.IdCliente == null)
            throw new NegocioException("Cliente não encontrado");
        orcamentoDto.IdCliente = orc.IdCliente;
        orcamentoDto.CodigoCliente = orc.Cliente.CampoCodigo + "-" + orc.Cliente.Loja;
        orcamentoDto.CPFCNPJCliente = orc.Cliente.CNPJCPF;
        orcamentoDto.NomeCliente = orc.Cliente.Nome;
        orcamentoDto.ClassificacaoCliente = orc.Cliente.ClassificacaoCliente;
        orcamentoDto.ScoreCliente = orc.Cliente.Score;
        orcamentoDto.EmailCliente = orc.Cliente.Email;
        orcamentoDto.StatusCliente = orc.Cliente.StatusCliente;
        orcamentoDto.CelularCliente = orc.Cliente.TelefoneCelular;
        orcamentoDto.TelefoneComercialCliente = orc.Cliente.TelefoneComercial;
        orcamentoDto.TelefoneCliente = orc.Cliente.Telefone;
        orcamentoDto.InformacoesCliente = orc.InformacoesCliente;
        #endregion

        #endregion

        #region [ Equipe de vendas ]

        #region [ Loja ]
        if (orc.IdLojaDellaVia == null)
            throw new NegocioException("Loja não encontrada");
        orcamentoDto.LojaDestino = orc.IdLojaDellaVia;
        orcamentoDto.LojaDestinoDescricao = orc.LojaDellaVia.Descricao;
        orcamentoDto.LojaDestinoCampoCodigo = orc.LojaDellaVia.CampoCodigo;
        orcamentoDto.LogradouroLoja = orc.LojaDellaVia.Logradouro;
        orcamentoDto.BairroLoja = orc.LojaDellaVia.Bairro;
        orcamentoDto.CidadeLoja = orc.LojaDellaVia.Cidade;
        orcamentoDto.EstadoLoja = orc.LojaDellaVia.Estado;
        orcamentoDto.CepLoja = orc.LojaDellaVia.Cep;
        orcamentoDto.CnpjLoja = orc.LojaDellaVia.Cnpj;
        orcamentoDto.InscricaoEstadualLoja = orc.LojaDellaVia.InscricaoEstadual;
        orcamentoDto.TelefoneLoja = orc.LojaDellaVia.Telefone;
        #endregion

        #region [ Vendedor ]
        orcamentoDto.Vendedor = orc.IdVendedor;
        var vendedor = _vendedorRepository.GetById(orc.IdVendedor);
        if (vendedor == null)
            throw new NegocioException("Vendedor não encontrado");
        orcamentoDto.VendedorDescricao = vendedor.IdConsultor + " - " + vendedor.Nome;
        #endregion

        #region [ Transportadora ]
        orcamentoDto.Transportadora = orc.IdTransportadora;
        if (orcamentoDto.Transportadora != null)
            orcamentoDto.TransportadoraDescricao = orc.Transportadora.Descricao;
        #endregion

        #endregion

        #region [ Busca de Produtos ]
        orcamentoDto.OrcamentoProduto.IdOrcamento = orc.Id;
        orcamentoDto.OrcamentoProduto = ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orc.Id);
        orcamentoDto.OrcamentoProdutoCount = orcamentoDto.OrcamentoProduto.Produtos.Count;
        #endregion

        #region [ Negociação ]
        #region [ Formas de Pagamento ]
        orcamentoDto.FormaPagamento = ObterOrcamentoPagamentoDto(orc.Id);

        #region [ Acréscimo ]
        var valorParceladoSomado = orcamentoDto.FormaPagamento.FormasPagamentos.Select(x => x.ValorTotal).Sum();
        orcamentoDto.ValorRestante = orcamentoDto.FormaPagamento.ValorRestante = (decimal)(orcamentoDto.OrcamentoProduto.Total + orc.ValorImpostos - valorParceladoSomado);
        decimal percentAjuste = 0;
        if (valorParceladoSomado > 0)
            percentAjuste = CalculaAjuste(orcamentoDto.OrcamentoProduto.Total, null, orcamentoDto.ValorRestante)[1] * -1 / 100m;
        if (percentAjuste > 0 && orcamentoDto.FormaPagamento.FormasPagamentos.Count > 0 && orcamentoDto.FormaPagamento.FormasPagamentos.Where(x => x.TemAcrescimo == true).ToList().Count > 0)
        {
            foreach (var produto in orcamentoDto.OrcamentoProduto.Produtos)
            {
                produto.Valor = decimal.Round(produto.Valor + (produto.Valor * percentAjuste), 2);
                produto.ValorTotal = decimal.Round(produto.ValorTotal + (produto.ValorTotal * percentAjuste), 2);
                foreach (var servico in produto.Servicos)
                {
                    servico.Valor = decimal.Round(servico.Valor + (servico.Valor * percentAjuste), 2);
                    servico.ValorTotal = decimal.Round(servico.ValorTotal + (servico.ValorTotal * percentAjuste), 2);
                }
            }
            orcamentoDto.OrcamentoProduto.Total = decimal.Round(orcamentoDto.OrcamentoProduto.Total + (orcamentoDto.OrcamentoProduto.Total * percentAjuste), 2);
            orcamentoDto.ValorRestante = orcamentoDto.FormaPagamento.ValorRestante = (decimal)(orcamentoDto.OrcamentoProduto.Total + orc.ValorImpostos - valorParceladoSomado);
        }
        orcamentoDto.ValorImpostos = orc.ValorImpostos;
        orcamentoDto.ReservaEstoque = orc.PossuiReservaEstoque;
        #endregion

        #endregion
        #endregion

        #region [ Finalização ]
        orcamentoDto.SolicitacoesAnaliseCredito = Mapper.Map<List<SolicitacaoAnaliseCreditoDto>>(GetSolicitacaoAnaliseCreditoByOrcamento(orc.Id));
        #endregion

        #region [ Relatório ]
        orcamentoDto.ObservacoesRelatorio = new ObservacaoDto();
        foreach (var obs in GetOrcamentoObservacaoByTipo((long)TipoObservacao.Observacao))
            orcamentoDto.ObservacoesRelatorio.Informacoes.Add(obs.Conteudo);

        orcamentoDto.AtividadesDellaViaRelatorio = new List<ObservacaoDto>();
        var listaAtividadeDellaVia = GetOrcamentoObservacaoByTipo((long)TipoObservacao.AtividadeDellaVia).ToList();
        foreach (var titulo in listaAtividadeDellaVia.GroupBy(a => a.Titulo))
        {
            var titObs = new ObservacaoDto { Titulo = titulo.Key };
            foreach (OrcamentoObservacao obs in listaAtividadeDellaVia.Where(a => a.Titulo == titulo.Key))
                titObs.Informacoes.Add(obs.Conteudo);
            orcamentoDto.AtividadesDellaViaRelatorio.Add(titObs);
        }
        #endregion

        return orcamentoDto;
    }

    public Orcamento GetOrcamentoById(long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT DISTINCT o.*, " +
                         "       c.ID_LOJA AS ClienteLoja, c.CPF_CNPJ AS CPFCNPJ, c.NOME_CLIENTE AS ClienteNome, c.DS_EMAIL AS ClienteEmail, c.DS_TELEFONE AS ClienteTelefone, c.DS_DDD AS ClienteDdd, " +
                         "       l.CampoCodigo AS LojaCampoCodigo, l.Descricao AS LojaDescricao, l.Logradouro AS LojaLogradouro, l.Bairro AS LojaBairro, l.Cidade AS LojaCidade, l.Estado AS LojaEstado, l.Cep AS LojaCep, l.Cnpj AS LojaCnpj, l.InscricaoEstadual AS LojaIE, l.Telefone AS LojaTelefone, " +
                         "       a.ID_AREA, a.NOME_AREA, " +
                         "       tp.ID_TABELA, tp.DS_TABELA, " +
                         "       t.CampoCodigo AS TransportadoraCampoCodigo, t.Descricao AS TransportadoraDescricao " +
                         "  FROM ORCAMENTO o " +
                         "  LEFT JOIN PowerData.dbo.DM_CLIENTES c ON c.ID_CLIENTE = o.IdCliente " +
                         "  LEFT JOIN PowerData.dbo.DM_AREANEG a ON a.ID_AREA = o.IdAreaNegocio " +
                         "  LEFT JOIN PowerData.dbo.DM_TABELAS_DE_PRECOS tp ON tp.ID_TABELA = o.IdTabelaPreco " +
                         "  LEFT JOIN LOJA_DELLAVIA l ON l.CampoCodigo = o.IdLojaDellaVia " +
                         "  LEFT JOIN TRANSPORTADORA t ON t.Id = o.IdTransportadora " +
                         " WHERE o.Id = @Id " +
                         "   AND o.RegistroInativo <> 1 ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", idOrcamento);
            Orcamento o = null;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    if (reader.HasRows && reader.Read())
                    {
                        string ddd = (reader["ClienteDdd"].ToString().Trim().IndexOf('0') == 0) ? reader["ClienteDdd"].ToString().Trim().Substring(1) : reader["ClienteDdd"].ToString().Trim();
                        string tel = (reader["ClienteTelefone"].ToString().Trim().IndexOf('0') == 0) ? reader["ClienteTelefone"].ToString().Trim().Substring(1) : reader["ClienteTelefone"].ToString().Trim();
                        o = new Orcamento
                        {
                            Id = (long)reader["Id"],
                            IsOrigemProtheus = (bool)reader["IsOrigemProtheus"],
                            IdConvenio = reader["IdConvenio"].ToString(),
                            IdCliente = reader["IdCliente"].ToString(),
                            Cliente = new Cliente
                            {
                                Loja = reader["ClienteLoja"].ToString().Trim(),
                                Nome = reader["ClienteNome"].ToString().Trim(),
                                CNPJCPF = reader["CPFCNPJ"].ToString().Trim(),
                                Email = reader["ClienteEmail"].ToString().Trim(),
                                Telefone = ((reader["ClienteTelefone"].ToString().Trim().IndexOf('9') != 0) ? (string.IsNullOrEmpty(ddd) ? "00" + tel : ddd + tel) : null),
                                TelefoneComercial = ((reader["ClienteTelefone"].ToString().Trim().IndexOf('9') != 0) ? (string.IsNullOrEmpty(ddd) ? "00" + tel : ddd + tel) : null),
                                TelefoneCelular = ((reader["ClienteTelefone"].ToString().Trim().IndexOf('9') == 0) ? (string.IsNullOrEmpty(ddd) ? "00" + tel : ddd + tel) : null),
                                CampoCodigo = reader["IdCliente"].ToString().Trim()
                            },
                            DataValidade = (DateTime)reader["DataValidade"],
                            IdTabelaPreco = reader["IdTabelaPreco"].ToString(),
                            TabelaPreco = new TabelaPreco 
                            { 
                                IdTabelaPreco = reader["ID_TABELA"].ToString().Trim(),
                                Descricao = reader["DS_TABELA"].ToString().Trim(),
                                CampoCodigo = reader["ID_TABELA"].ToString().Trim()
                            },
                            Complemento = reader["Complemento"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            IdVendedor = reader["IdVendedor"].ToString(),
                            InformacoesCliente = reader["InformacoesCliente"].ToString(),
                            Placa = reader["Placa"].ToString(),
                            Ano = (reader["Ano"] == DBNull.Value || reader["Ano"] == null) ? null : new int?((int)reader["Ano"]),
                            TelefoneComercial = reader["TelefoneComercial"].ToString(),
                            TelefoneCelular = reader["TelefoneCelular"].ToString(),
                            IdLojaDellaVia = reader["IdLojaDellaVia"].ToString(),
                            LojaDellaVia = new LojaDellaVia
                            {
                                CampoCodigo = reader["LojaCampoCodigo"].ToString().Trim(),
                                Descricao = reader["LojaDescricao"].ToString().Trim(),
                                Logradouro = reader["LojaLogradouro"].ToString().Trim(),
                                Bairro = reader["LojaBairro"].ToString().Trim(),
                                Cidade = reader["LojaCidade"].ToString().Trim(),
                                Estado = reader["LojaEstado"].ToString().Trim(),
                                Cep = reader["LojaCep"].ToString().Trim(),
                                Cnpj = reader["LojaCnpj"].ToString().Trim(),
                                InscricaoEstadual = reader["LojaIE"].ToString().Trim(),
                                Telefone = reader["LojaTelefone"].ToString().Trim()
                            },
                            IdOperador = reader["IdOperador"].ToString(),
                            IdAreaNegocio = reader["IdAreaNegocio"].ToString(),
                            AreaNegocio = new PDAreaNegocio
                            {
                                IdArea = reader["ID_AREA"].ToString().Trim(),
                                NomeArea = reader["NOME_AREA"].ToString().Trim()
                            },
                            IdTransportadora = (reader["IdTransportadora"] == DBNull.Value || reader["IdTransportadora"] == null) ? null : new long?((long)reader["IdTransportadora"]),
                            Transportadora = new Transportadora
                            {
                                CampoCodigo = reader["TransportadoraCampoCodigo"].ToString(),
                                Descricao = reader["TransportadoraDescricao"].ToString()
                            },
                            PossuiReservaEstoque = (bool)reader["PossuiReservaEstoque"],
                            StatusOrcamento = (StatusOrcamento)(int)reader["StatusOrcamento"],
                            ExisteAlcadaPendente = (bool)reader["ExisteAlcadaPendente"],
                            KM = (reader["KM"] == DBNull.Value || reader["KM"] == null) ? null : new int?((int)reader["KM"]),
                            TipoOrcamento = (TipoOrcamento)(int)reader["TipoOrcamento"],
                            Xped = reader["Xped"].ToString(),
                            MensagemNF = reader["MensagemNF"].ToString(),
                            IdBanco = (reader["IdBanco"] == DBNull.Value || reader["IdBanco"] == null) ? null : new long?((long)reader["IdBanco"]),
                            DataCriacao = (DateTime)reader["DataCriacao"],
                            ValorImpostos = (decimal)reader["ValorImpostos"],
                            CampoCodigo = reader["CampoCodigo"].ToString(),
                            DataAtualizacao = (DateTime)reader["DataAtualizacao"],
                            UsuarioAtualizacao = reader["UsuarioAtualizacao"].ToString(),
                            VeiculoIdFraga = reader["VeiculoIdFraga"].ToString()
                        };
                    }
            }
            catch (Exception e)
            {
                throw e;
            }
            return o;
        }
    }

    public List<OrcamentoConsultaDto> GetAllToday()
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT O.*, C.NOME_CLIENTE AS ClienteNome, L.Descricao AS LojaNome, V.NOME_CONSULTOR " +
                         "  FROM ORCAMENTO O " +
                         "  LEFT JOIN LOJA_DELLAVIA              L ON L.CampoCodigo  = O.IdLojaDellaVia " +
                         "  LEFT JOIN PowerData.dbo.DM_CLIENTES  C ON C.ID_CLIENTE   = O.IdCliente " +
                         "  LEFT JOIN PowerData.dbo.DM_CONSULTOR V ON V.ID_CONSULTOR = O.IdVendedor " +
                         " WHERE O.DataValidade       = CAST(@DataValidade AS DATE) " +
                         "   AND (@UsuarioAtualizacao IS NULL OR (O.UsuarioAtualizacao = @UsuarioAtualizacao)) " +
                         "   AND O.RegistroInativo <> 1 " +
                         " ORDER BY O.DataCriacao DESC ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DataValidade", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper() ?? (object)DBNull.Value);
            var list = new List<OrcamentoConsultaDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new OrcamentoConsultaDto
                        {
                            Id = (long)reader["Id"],
                            DataValidade = string.Format("{0:dd/MM/yyyy}", reader["DataValidade"]),
                            CampoCodigo = reader["CampoCodigo"].ToString(),
                            NomeCliente = reader["ClienteNome"].ToString(),
                            LojaDestino = reader["LojaNome"].ToString(),
                            Vendedor = reader["NOME_CONSULTOR"].ToString(),
                            DataCriacao = string.Format("{0:dd/MM/yyyy}", reader["DataCriacao"]),
                            Status = EnumExtension.GetDescription((StatusOrcamento)(int)reader["StatusOrcamento"])
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public List<OrcamentoConsultaDto> GetOrcamentoByQuery(Orcamento orcamento)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT TOP 100 O.*, C.NOME_CLIENTE AS ClienteNome, L.Descricao AS LojaNome, V.NOME_CONSULTOR " +
                         "  FROM ORCAMENTO O " +
                         "  LEFT JOIN LOJA_DELLAVIA              L ON L.CampoCodigo  = O.IdLojaDellaVia " +
                         "  LEFT JOIN PowerData.dbo.DM_CLIENTES  C ON C.ID_CLIENTE   = O.IdCliente " +
                         "  LEFT JOIN PowerData.dbo.DM_CONSULTOR V ON V.ID_CONSULTOR = O.IdVendedor " +
                         " WHERE (@Id                 IS NULL OR (O.Id                 = @Id)) " +
                         "   AND (@IdCliente          IS NULL OR (O.IdCliente          = @IdCliente))" +
                         "   AND (@Placa              IS NULL OR (O.Placa              = @Placa)) " +
                         "   AND (@IdLojaDellaVia     IS NULL OR (O.IdLojaDellaVia     = @IdLojaDellaVia)) " +
                         "   AND (@UsuarioAtualizacao IS NULL OR (O.UsuarioAtualizacao = @UsuarioAtualizacao)) " +
                         "   AND (@CampoCodigo        IS NULL OR (O.CampoCodigo        = @CampoCodigo)) " +
                         "   AND (@IdVendedor         IS NULL OR (O.IdVendedor         = @IdVendedor)) " +
                         "   AND (@LojaDestino        IS NULL OR (L.CampoCodigo        = @LojaDestino))" +
                         "   AND (@ClienteCPFCNPJ     IS NULL OR (C.CPF_CNPJ           = @ClienteCPFCNPJ))" +
                         "   AND (@ClienteCampoCodigo IS NULL OR (C.ID_CLIENTE         = @ClienteCampoCodigo))";
            if (orcamento.StatusOrcamento == StatusOrcamento.EmAberto)
                sql +=
                         "   AND (@StatusOrcamento    IS NULL OR (O.StatusOrcamento    =  @StatusOrcamento " +
                         "                                        AND O.DataValidade   >= CAST(@DataValidade AS DATE))) ";
            if (orcamento.StatusOrcamento == StatusOrcamento.EmAbertoVencido)
                sql +=
                         "   AND (@StatusOrcamento    IS NULL OR ((O.StatusOrcamento    =  0 " +
                         "                                         OR O.StatusOrcamento =  @StatusOrcamento)" +
                         "                                        AND O.DataValidade    <  CAST(@DataValidade AS DATE))) ";
            sql += "   AND O.RegistroInativo <> 1 " +
                         " ORDER BY O.DataCriacao DESC ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", orcamento.Id > 0 ? orcamento.Id : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdCliente", orcamento.IdCliente ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdVendedor", orcamento.IdVendedor ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Placa", orcamento.Placa ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdLojaDellaVia", orcamento.IdLojaDellaVia ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LojaDestino", orcamento.LojaDellaVia?.CampoCodigo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CampoCodigo", orcamento.CampoCodigo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ClienteCPFCNPJ", orcamento.Cliente?.CNPJCPF ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ClienteCampoCodigo", orcamento.Cliente?.IdCliente ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusOrcamento", orcamento.StatusOrcamento);
            cmd.Parameters.AddWithValue("@DataValidade", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", orcamento.UsuarioAtualizacao ?? (object)DBNull.Value);
            var list = new List<OrcamentoConsultaDto>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new OrcamentoConsultaDto
                        {
                            Id = (long)reader["Id"],
                            DataValidade = string.Format("{0:dd/MM/yyyy}", reader["DataValidade"]),
                            CampoCodigo = reader["CampoCodigo"].ToString(),
                            NomeCliente = reader["ClienteNome"].ToString(),
                            LojaDestino = reader["LojaNome"].ToString(),
                            Vendedor = reader["NOME_CONSULTOR"].ToString(),
                            DataCriacao = string.Format("{0:dd/MM/yyyy}", reader["DataCriacao"]),
                            Status = EnumExtension.GetDescription((StatusOrcamento)(int)reader["StatusOrcamento"])
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public long Add(Orcamento orcamento)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "INSERT INTO ORCAMENTO " +
                        "(IsOrigemProtheus,  IdConvenio,  IdCliente,  DataValidade,  IdTabelaPreco,  Complemento,  Telefone,  IdVendedor, " +
                        " InformacoesCliente,   Placa,  Ano,  TelefoneComercial,  TelefoneCelular,  IdLojaDellaVia,  IdOperador,  IdTransportadora, " +
                        " PossuiReservaEstoque,   StatusOrcamento,  ExisteAlcadaPendente, KM,  IdAreaNegocio,  TipoOrcamento,  Xped,  MensagemNF, " +
                        " DataCriacao,  UsuarioCriacao,   ValorImpostos,   RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao,  VeiculoIdFraga) " +
                        "VALUES " +
                        "(@IsOrigemProtheus, @IdConvenio, @IdCliente, @DataValidade, @IdTabelaPreco, @Complemento, @Telefone, @IdVendedor, " +
                        " @InformacoesCliente, @Placa, @Ano, @TelefoneComercial, @TelefoneCelular, @IdLojaDellaVia, @IdOperador, @IdTransportadora," +
                        " @PossuiReservaEstoque, @StatusOrcamento, @ExisteAlcadaPendente, @KM, @IdAreaNegocio, @TipoOrcamento, @Xped, @MensagemNF, " +
                        " @DataCriacao, @UsuarioCriacao, @ValorImpostos, @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao, @VeiculoIdFraga); " +
                        "SELECT CAST(scope_identity() AS bigint) ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IsOrigemProtheus", 0);
            cmd.Parameters.AddWithValue("@IdConvenio ", orcamento.IdConvenio);
            cmd.Parameters.AddWithValue("@IdCliente", orcamento.IdCliente);
            cmd.Parameters.AddWithValue("@DataValidade", orcamento.DataValidade);
            cmd.Parameters.AddWithValue("@IdTabelaPreco", orcamento.IdTabelaPreco);
            cmd.Parameters.AddWithValue("@Complemento", orcamento.Complemento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefone", orcamento.Telefone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdVendedor", orcamento.IdVendedor ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@InformacoesCliente", orcamento.InformacoesCliente ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Placa", orcamento.Placa ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", orcamento.Ano > 0 ? orcamento.Ano : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TelefoneComercial", orcamento.TelefoneComercial ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TelefoneCelular", orcamento.TelefoneCelular ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdLojaDellaVia", orcamento.IdLojaDellaVia ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdOperador", orcamento.IdOperador ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTransportadora", orcamento.IdTransportadora > 0 ? orcamento.IdTransportadora : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PossuiReservaEstoque", 0);
            cmd.Parameters.AddWithValue("@StatusOrcamento", orcamento.StatusOrcamento);
            cmd.Parameters.AddWithValue("@ExisteAlcadaPendente", 0);
            cmd.Parameters.AddWithValue("@KM", orcamento.KM > 0 ? orcamento.KM : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdAreaNegocio", orcamento.IdAreaNegocio);
            cmd.Parameters.AddWithValue("@TipoOrcamento", orcamento.TipoOrcamento);
            cmd.Parameters.AddWithValue("@Xped", orcamento.Xped ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MensagemNF", orcamento.MensagemNF ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DataCriacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioCriacao", HttpContext.Current.User.Identity.Name.ToUpper());
            cmd.Parameters.AddWithValue("@ValorImpostos", 0.00);
            cmd.Parameters.AddWithValue("@RegistroInativo", 0);
            cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper());
            cmd.Parameters.AddWithValue("@VeiculoIdFraga", orcamento.VeiculoIdFraga ?? (object)DBNull.Value);
            try
            {
                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public void Update(Orcamento orcamento)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = " UPDATE ORCAMENTO " +
                         "    SET IsOrigemProtheus = @IsOrigemProtheus," +
                         "        IdConvenio = @IdConvenio," +
                         "        IdCliente = @IdCliente," +
                         "        DataValidade = @DataValidade," +
                         "        IdTabelaPreco = @IdTabelaPreco," +
                         "        Complemento = @Complemento," +
                         "        Telefone = @Telefone," +
                         "        IdVendedor = @IdVendedor," +
                         "        InformacoesCliente = @InformacoesCliente," +
                         "        Placa = @Placa," +
                         "        Ano = @Ano," +
                         "        TelefoneComercial = @TelefoneComercial," +
                         "        TelefoneCelular = @TelefoneCelular," +
                         "        IdLojaDellaVia = @IdLojaDellaVia," +
                         "        IdOperador = @IdOperador," +
                         "        IdTransportadora = @IdTransportadora," +
                         "        PossuiReservaEstoque = @PossuiReservaEstoque," +
                         "        StatusOrcamento = @StatusOrcamento," +
                         "        ExisteAlcadaPendente = @ExisteAlcadaPendente," +
                         "        KM = @KM," +
                         "        IdAreaNegocio = @IdAreaNegocio," +
                         "        TipoOrcamento = @TipoOrcamento," +
                         "        Xped = @Xped," +
                         "        MensagemNF = @MensagemNF," +
                         "        ValorImpostos = @ValorImpostos," +
                         "        CampoCodigo = @CampoCodigo," +
                         "        RegistroInativo = @RegistroInativo," +
                         "        DataAtualizacao = @DataAtualizacao," +
                         "        UsuarioAtualizacao = @UsuarioAtualizacao," +
                         "        VeiculoIdFraga = @VeiculoIdFraga" +
                         "  WHERE Id = @Id; ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IsOrigemProtheus", 0);
            cmd.Parameters.AddWithValue("@IdConvenio ", orcamento.IdConvenio);
            cmd.Parameters.AddWithValue("@IdCliente", orcamento.IdCliente);
            cmd.Parameters.AddWithValue("@DataValidade", orcamento.DataValidade);
            cmd.Parameters.AddWithValue("@IdTabelaPreco", orcamento.IdTabelaPreco);
            cmd.Parameters.AddWithValue("@Complemento", orcamento.Complemento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefone", orcamento.Telefone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdVendedor", orcamento.IdVendedor ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@InformacoesCliente", orcamento.InformacoesCliente ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Placa", orcamento.Placa ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ano", orcamento.Ano > 0 ? orcamento.Ano : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TelefoneComercial", orcamento.TelefoneComercial ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TelefoneCelular", orcamento.TelefoneCelular ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdLojaDellaVia", orcamento.IdLojaDellaVia ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdOperador", orcamento.IdOperador ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTransportadora", orcamento.IdTransportadora > 0 ? orcamento.IdTransportadora : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PossuiReservaEstoque", orcamento.PossuiReservaEstoque);
            cmd.Parameters.AddWithValue("@StatusOrcamento", orcamento.StatusOrcamento);
            cmd.Parameters.AddWithValue("@ExisteAlcadaPendente", 0);
            cmd.Parameters.AddWithValue("@KM", orcamento.KM > 0 ? orcamento.KM : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdAreaNegocio", orcamento.IdAreaNegocio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TipoOrcamento", orcamento.TipoOrcamento);
            cmd.Parameters.AddWithValue("@Xped", orcamento.Xped ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MensagemNF", orcamento.MensagemNF ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ValorImpostos", 0.00);
            cmd.Parameters.AddWithValue("@CampoCodigo", orcamento.CampoCodigo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RegistroInativo", 0);
            cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
            cmd.Parameters.AddWithValue("@UsuarioAtualizacao", HttpContext.Current.User.Identity.Name.ToUpper());
            cmd.Parameters.AddWithValue("@VeiculoIdFraga", orcamento.VeiculoIdFraga ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", orcamento.Id);
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

    public void Delete(Orcamento entity)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "DELETE Orcamento Where Id=@Id";
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
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "DELETE Orcamento Where Id=@Id";
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
    #endregion

    #region [ Itens Orcamento ]
    public ModalDetalhesProdutoDto ObterDadosModalDetalhesProduto(long idOrcamentoItemPai, long idOrcamento)
    {
        var orcamentoItens = _orcamentoItemRepository.GetOrcamentoItensByIdOrcamento(idOrcamento).ToList();
        var orcamentoItemPai = orcamentoItens.Where(x => x.Id == idOrcamentoItemPai).FirstOrDefault();
        var orcamentoFilhos = orcamentoItens.Where(x => x.NrItemProdutoPaiId == orcamentoItemPai.NrItem);
        var modalDetalhes = new ModalDetalhesProdutoDto();

        // Produto
        var produtoDto = modalDetalhes.ProdutoPaiDto = Mapper.Map<ProdutoDto>(orcamentoItemPai.Produto);
        modalDetalhes.IdOrcamento = orcamentoItemPai.OrcamentoId;
        modalDetalhes.IdOrcamentoItemPai = orcamentoItemPai.Id;
        modalDetalhes.QuantidadePai = (int)orcamentoItemPai.Quantidade;
        modalDetalhes.PrecoUnitarioPai = orcamentoItemPai.PrecoUnitario;
        modalDetalhes.TotalItemPai = orcamentoItemPai.TotalItem;
        modalDetalhes.ProdutoPaiDto.IdSubGrupoProduto = orcamentoItemPai.Produto.GrupoProduto.IdGrupoSubGrupo;
        // Busca todos os servicos agregados do produto.
        var grupoServicoAgregadoProdutoDtoLista = Mapper.Map<ICollection<GrupoServicoAgregadoProdutoDto>>(_produtoRepository.GetKitServico(orcamentoItemPai.Produto.IdGrupoServicoAgregado, orcamentoItemPai.Orcamento.TabelaPreco.CampoCodigo));
        var listaProdutoAgregados = new List<GrupoServicoAgregadoProdutoDto>();
        foreach (var itemOrcamento in _orcamentoItemRepository.GetOrcamentoItensByIdItemIdOrcamento(orcamentoItemPai.NrItem, orcamentoItemPai.OrcamentoId))
        {
            var itemServicoModalDto = grupoServicoAgregadoProdutoDtoLista.FirstOrDefault(a => a.IdProduto == itemOrcamento.ProdutoId);
            // Se já está persistido na orcamentoItem remove para não ser add no proximo for.
            grupoServicoAgregadoProdutoDtoLista.Remove(itemServicoModalDto);
            if (itemServicoModalDto != null)
            {
                itemServicoModalDto.Descricao = itemOrcamento.Produto.CampoCodigo + " - " + itemOrcamento.Produto.Descricao;
                itemServicoModalDto.Quantidade = (int)itemOrcamento.Quantidade;
                itemServicoModalDto.PrecoUnitario = itemOrcamento.PrecoUnitario;
                itemServicoModalDto.TotalItem = itemOrcamento.TotalItem;
                if (itemOrcamento.Id != 0)
                    itemServicoModalDto.IdOrcamentoItemFilho = itemOrcamento.Id;
                listaProdutoAgregados.Add(itemServicoModalDto);
            }
        }
        // Sugerindo os produtos que não foram add na primeira vez.
        foreach (GrupoServicoAgregadoProdutoDto produtoAgregadoNaoAdicionado in grupoServicoAgregadoProdutoDtoLista)
        {
            string idTabelaPreco = (orcamentoItemPai.Orcamento != null) ? orcamentoItemPai.Orcamento.IdTabelaPreco : GetOrcamentoById(orcamentoItemPai.OrcamentoId).IdTabelaPreco;
            var tabelaPrecoItem = _tabelaPrecoRepository.GetTabelaPrecoItem(idTabelaPreco, produtoAgregadoNaoAdicionado.IdProduto.ToString());
            var produto = _produtoRepository.GetById(produtoAgregadoNaoAdicionado.IdProduto);
            produtoAgregadoNaoAdicionado.Quantidade = 0;
            produtoAgregadoNaoAdicionado.PrecoUnitario = tabelaPrecoItem?.PrecoVenda ?? 0m;
            produtoAgregadoNaoAdicionado.TotalItem = produtoAgregadoNaoAdicionado.Quantidade * produtoAgregadoNaoAdicionado.PrecoUnitario;
            produtoAgregadoNaoAdicionado.Descricao = produto.CampoCodigo + " - " + produto.Descricao;
            var itemServicoFilho = orcamentoFilhos.FirstOrDefault(a => a.ProdutoId == produtoAgregadoNaoAdicionado.IdProduto);
            if (itemServicoFilho != null)
            {
                produtoAgregadoNaoAdicionado.IdOrcamentoItemFilho = (long)itemServicoFilho.Id > 0 ? (long)itemServicoFilho.Id : 0;
                produtoAgregadoNaoAdicionado.Quantidade = (int)itemServicoFilho.Quantidade > 0 ? (int)itemServicoFilho.Quantidade : 0;
            }
            listaProdutoAgregados.Add(produtoAgregadoNaoAdicionado);
        }
        // Servico agregado list
        modalDetalhes.ProdutosAgregadosModalList = listaProdutoAgregados;
        var produtoComplemento = _produtoRepository.GetProdutoComplementoByCampoCodigo(orcamentoItemPai.ProdutoId);
        if (produtoComplemento != null)
        {
            modalDetalhes.ProdutoComplementoPaiDto = Mapper.Map<ProdutoComplementoDto>(produtoComplemento);
            modalDetalhes.ProdutoComplementoPaiDto.hasCampoHTML = !string.IsNullOrEmpty(produtoComplemento.CampoHTML);
            produtoComplemento.CampoHTML = string.Empty; // Caso contrario o json ficaria mt grande.
        }
        return modalDetalhes;
    }

    public OrcamentoProdutoBuscaDto ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(long idOrcamento)
    {
        var orcamentoItens = _orcamentoItemRepository.GetOrcamentoItensByIdOrcamento(idOrcamento).ToList();
        var orcamentoItensSomentePais = orcamentoItens.Where((OrcamentoItem x) => !x.NrItemProdutoPaiId.HasValue);
        var produtoBuscaDto = new OrcamentoProdutoBuscaDto { IdOrcamento = idOrcamento };
        decimal total = default(decimal);
        foreach (OrcamentoItem itemPai in orcamentoItensSomentePais)
        {
            var produtoSacola = new SacolaProdutoDto
            {
                IdOrcamentoItem = itemPai.Id,
                NumeroItem = itemPai.NrItem,
                CampoCodigoProduto = itemPai.Produto.CampoCodigo,
                Descricao = itemPai.Produto.Descricao,
                Quantidade = (int)itemPai.Quantidade,
                Valor = itemPai.PercDescon > 0 ? decimal.Round(itemPai.PrecoUnitario - ((itemPai.PercDescon / 100m) * itemPai.PrecoUnitario), 2) : itemPai.PrecoUnitario,
                PercentualDesconto = itemPai.PercDescon,
                ValorDesconto = itemPai.ValorDesconto,
                ValorTotal = (itemPai.Quantidade * itemPai.PrecoUnitario) - itemPai.ValorDesconto,
                TipoItem = itemPai.Produto.IdGrupoProduto == "4" /*Serviço*/ ? TipoItemOrcamento.Servico : TipoItemOrcamento.Produto
            };
            if (produtoSacola.TipoItem == TipoItemOrcamento.Servico)
                produtoSacola.ProfissionaisMontagem = Mapper.Map<List<ProfissionalMontagemDto>>(_orcamentoItemRepository.ObterEquipeMontagemDto(itemPai.Id).Equipe);
            produtoSacola.SolicitacoesDescontoAlcada = Mapper.Map<List<SolicitacaoDescontoVendaAlcadaDto>>(_orcamentoItemRepository.GetSolicitacaoDescontoVendaAlcada(itemPai.Id).ToList());
            total += (produtoSacola.Quantidade * itemPai.PrecoUnitario) - produtoSacola.ValorDesconto;

            // Varredura nos itens filhos do orcamento (Servicos Agregados)
            foreach (var itemFilho in orcamentoItens.Where(x => x.NrItemProdutoPaiId == itemPai.NrItem))
            {
                var produtoSacolaFilho = new ServicoCorrelacionadoDto
                {
                    IdOrcamentoItem = itemFilho.Id,
                    NumeroItem = itemFilho.NrItem,
                    CampoCodigoProduto = itemFilho.Produto.CampoCodigo,
                    Descricao = itemFilho.Produto.Descricao,
                    Quantidade = (int)itemFilho.Quantidade,
                    Valor = itemFilho.PercDescon > 0 ? decimal.Round(itemFilho.PrecoUnitario - ((itemFilho.PercDescon / 100m) * itemFilho.PrecoUnitario), 2) : itemFilho.PrecoUnitario,
                    ValorDesconto = itemFilho.ValorDesconto,
                    PercentualDesconto = itemFilho.PercDescon,
                    ValorTotal = ((int)itemFilho.Quantidade * itemFilho.PrecoUnitario) - itemFilho.ValorDesconto,
                    TipoItem = itemFilho.Produto.IdGrupoProduto == "4" /*Serviço*/ ? TipoItemOrcamento.Servico : TipoItemOrcamento.Produto
                };
                total += (produtoSacolaFilho.Quantidade * itemFilho.PrecoUnitario) - produtoSacolaFilho.ValorDesconto;
                if (produtoSacolaFilho.TipoItem == TipoItemOrcamento.Servico)
                    produtoSacolaFilho.ProfissionaisMontagem = Mapper.Map<List<ProfissionalMontagemDto>>(_orcamentoItemRepository.ObterEquipeMontagemDto(itemFilho.Id).Equipe);
                produtoSacola.SolicitacoesDescontoAlcada = Mapper.Map<List<SolicitacaoDescontoVendaAlcadaDto>>(_orcamentoItemRepository.GetSolicitacaoDescontoVendaAlcada(itemFilho.Id).ToList());
                produtoSacola.Servicos.Add(produtoSacolaFilho);
            }
            produtoBuscaDto.Produtos.Add(produtoSacola);
        }
        produtoBuscaDto.Total = total;
        return produtoBuscaDto;
    }

    public void AtualizarReservaEstoque(long id, bool reservaEstoque)
    {
        var orcamento = GetOrcamentoById(id);
        orcamento.PossuiReservaEstoque = reservaEstoque;
        Update(orcamento);
    }

    public OrcamentoProdutoBuscaDto InserirItensOrcamento(ModalDetalhesProdutoDto modalDetalhesProdutoDto)
    {
        var orcamento = GetOrcamentoById(modalDetalhesProdutoDto.IdOrcamento);
        var produtoPai = _produtoRepository.GetById(modalDetalhesProdutoDto.ProdutoPaiDto.Id);
        var tabelaPrecoItemPai = _tabelaPrecoRepository.GetTabelaPrecoItem(orcamento.IdTabelaPreco, produtoPai?.IdProduto);
        var nrItemPai = 1;
        orcamento.OrcamentoItens = _orcamentoItemRepository.GetOrcamentoItensByIdOrcamento(orcamento.Id);
        if (orcamento.OrcamentoItens != null)
            nrItemPai = ((orcamento.OrcamentoItens.Count == 0) ? 1 : (orcamento.OrcamentoItens.Max((OrcamentoItem x) => x.NrItem) + 1));
        if (modalDetalhesProdutoDto.QuantidadePai == 0)
            throw new NegocioException("A quantidade do produto deve ser maior que zero.");
        var orcItemPai = new OrcamentoItem
        {
            OrcamentoId = modalDetalhesProdutoDto.IdOrcamento,
            ProdutoId = modalDetalhesProdutoDto.ProdutoPaiDto.Id,
            NrItem = nrItemPai,
            Quantidade = modalDetalhesProdutoDto.QuantidadePai,
            PrecoUnitario = tabelaPrecoItemPai.PrecoVenda,
            TotalItem = (decimal)modalDetalhesProdutoDto.QuantidadePai * tabelaPrecoItemPai.PrecoVenda,
            NrItemProdutoPaiId = null,
            PercDescon = 0m,
            ValorDesconto = 0m
        };
        // Insere o produto no orcamento
        _orcamentoItemRepository.Add(orcItemPai);

        // Add os serviços agregados com quantidade > 0
        int contadorItens = orcItemPai.NrItem;
        foreach (var prodServicoFilho in modalDetalhesProdutoDto.ProdutosAgregadosModalList)
        {
            if (prodServicoFilho.Quantidade > 0)
            {
                var tabelaPrecoItem = _tabelaPrecoRepository.GetTabelaPrecoItem(orcamento.IdTabelaPreco, prodServicoFilho.IdProduto);
                if (tabelaPrecoItem != null)
                {
                    contadorItens++;
                    var orcItem = new OrcamentoItem
                    {
                        OrcamentoId = modalDetalhesProdutoDto.IdOrcamento,
                        ProdutoId = prodServicoFilho.IdProduto,
                        NrItem = contadorItens,
                        Quantidade = prodServicoFilho.Quantidade,
                        PrecoUnitario = tabelaPrecoItem.PrecoVenda,
                        TotalItem = prodServicoFilho.Quantidade * tabelaPrecoItem.PrecoVenda,
                        NrItemProdutoPaiId = orcItemPai.NrItem,
                        PercDescon = 0m,
                        ValorDesconto = 0m
                    };
                    // Insere os serviços no orcamento
                    _orcamentoItemRepository.Add(orcItem);
                }
            }
        }
        if (orcamento.FormaPagamentos != null)
            RemoverPagamentos(orcamento.Id);
        return ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orcamento.Id);
    }

    public OrcamentoProdutoBuscaDto AtualizarItensOrcamento(ModalDetalhesProdutoDto modalDetalhesProdutoDto)
    {
        if (modalDetalhesProdutoDto.QuantidadePai <= 0)
            throw new NegocioException("A quantidade do produto deve ser maior que zero.");
        var orcItemPai = _orcamentoItemRepository.GetById(modalDetalhesProdutoDto.IdOrcamentoItemPai);
        if (orcItemPai.IdDescontoModeloVenda.HasValue)
        {
            orcItemPai.IdDescontoModeloVenda = null;
            orcItemPai.PercDescon = 0m;
            orcItemPai.ValorDesconto = 0m;
            orcItemPai.DescontoModeloVendaUtilizado = null;
        }
        orcItemPai.Quantidade = modalDetalhesProdutoDto.QuantidadePai;
        orcItemPai.TotalItem = (decimal)modalDetalhesProdutoDto.QuantidadePai * orcItemPai.PrecoUnitario;
        if (orcItemPai.PercDescon > 0m)
        {
            var valorDesconto2 = decimal.Round(orcItemPai.PercDescon / 100m * (orcItemPai.Quantidade * orcItemPai.PrecoUnitario), 2);
            orcItemPai.TotalItem = (orcItemPai.Quantidade * orcItemPai.PrecoUnitario) - valorDesconto2;
            orcItemPai.ValorDesconto = valorDesconto2;
        }
        // Atualiza o item no orcamento
        _orcamentoItemRepository.Update(orcItemPai);

        var orcamento = GetOrcamentoById(orcItemPai.OrcamentoId);
        orcItemPai.Orcamento.OrcamentoItens = _orcamentoItemRepository.GetOrcamentoItensByIdOrcamento(orcamento.Id);
        var idTabelaPreco = (orcItemPai.Orcamento != null) ? orcItemPai.Orcamento.IdTabelaPreco : orcamento.IdTabelaPreco;
        foreach (var prodServicoFilho in modalDetalhesProdutoDto.ProdutosAgregadosModalList)
        {
            // Se o item tiver qtd > 0 e id = 0, é um novo item.
            if (prodServicoFilho.IdOrcamentoItemFilho == 0L && prodServicoFilho.Quantidade > 0)
            {
                var produto = _produtoRepository.GetById(prodServicoFilho.IdProduto);
                var tabelaPrecoItem = _tabelaPrecoRepository.GetTabelaPrecoItem(idTabelaPreco, produto.IdProduto);
                if (tabelaPrecoItem != null)
                {
                    var orcItem = new OrcamentoItem
                    {
                        OrcamentoId = orcItemPai.OrcamentoId,
                        ProdutoId = produto.IdProduto,
                        NrItem = orcItemPai.Orcamento.OrcamentoItens.Max((OrcamentoItem x) => x.NrItem) + 1,
                        Quantidade = prodServicoFilho.Quantidade,
                        PrecoUnitario = tabelaPrecoItem.PrecoVenda,
                        TotalItem = prodServicoFilho.Quantidade * tabelaPrecoItem.PrecoVenda,
                        NrItemProdutoPaiId = orcItemPai.NrItem,
                        PercDescon = 0m,
                        ValorDesconto = 0m
                    };
                    _orcamentoItemRepository.Add(orcItem);
                }
            }
            else if (prodServicoFilho.IdOrcamentoItemFilho > 0)
            {
                var orcamentoItem = _orcamentoItemRepository.GetById(prodServicoFilho.IdOrcamentoItemFilho);
                if (prodServicoFilho.Quantidade == 0)
                    // Remove o item do orçamento
                    RemoverItensOrcamento(orcamento.Id, orcamentoItem.Id);
                    //_orcamentoItemRepository.Delete(orcamentoItem);
                else
                {
                    orcamentoItem.Quantidade = prodServicoFilho.Quantidade;
                    orcamentoItem.TotalItem = (decimal)prodServicoFilho.Quantidade * orcamentoItem.PrecoUnitario;
                    if (orcamentoItem.PercDescon > 0m)
                    {
                        var valorDesconto = decimal.Round(orcamentoItem.PercDescon / 100m * (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), 2);
                        orcamentoItem.TotalItem = (prodServicoFilho.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;
                        orcamentoItem.ValorDesconto = valorDesconto;
                    }
                    // Atualiza o item no orcamento
                    _orcamentoItemRepository.Update(orcamentoItem);
                }
            }
        }
        if (orcamento.FormaPagamentos != null)
            RemoverPagamentos(orcamento.Id);
        return ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orcItemPai.OrcamentoId);
    }

    public void RemoverItensOrcamento(long orcamentoId, long orcamentoItemId)
    {
        //var orcamento = GetOrcamentoById(orcamentoId);
        var orcItem = _orcamentoItemRepository.GetById(orcamentoItemId);
        // Removendo a equipe de montagem do serviço correlacionado.
        var equipe = _orcamentoItemRepository.ObterEquipeMontagemDto(orcamentoItemId).Equipe;
        if (equipe != null && equipe.Count > 0)
            _equipeMontagemRepository.DeleteByIdOrcamentoItem(orcamentoItemId);
        // Se o NrItemProdutoPaiId é nulo ele é um pai e pode ter serviços correlacionados
        if (!orcItem.NrItemProdutoPaiId.HasValue)
            foreach (var item in _orcamentoItemRepository.GetOrcamentoItensByIdItemIdOrcamento(orcItem.NrItem, orcamentoId))
            {
                _orcamentoItemRepository.DeleteSolicitacaoDescontoVendaAlcada(item.Id);
                _orcamentoItemRepository.Delete(item);
            }
        _orcamentoItemRepository.DeleteSolicitacaoDescontoVendaAlcada(orcItem.Id);
        _orcamentoItemRepository.Delete(orcItem);
        //if (orcamento.FormaPagamentos != null)
        RemoverPagamentos(orcamentoId);
        CalcularImpostos(orcamentoId);
    }

    public AplicarDescontoDto ObterItemOrcamentoDesconto(long orcamentoId, long orcamentoItemId)
    {
        var orcamento = GetOrcamentoById(orcamentoId);
        var orcamentoItem = _orcamentoItemRepository.GetById(orcamentoItemId);
        var aplicarDescontoDto = new AplicarDescontoDto();
        //Geral
        aplicarDescontoDto.IdDescontoModeloVenda = null;
        aplicarDescontoDto.DescricaoProduto = orcamentoItem.Produto.Descricao;
        aplicarDescontoDto.QuantidadeProduto = orcamentoItem.Quantidade;
        aplicarDescontoDto.ValorOriginal = orcamentoItem.TotalItem + orcamentoItem.ValorDesconto;
        aplicarDescontoDto.PercentualDesconto = orcamentoItem.PercDescon;
        aplicarDescontoDto.ValorDesconto = orcamentoItem.ValorDesconto;
        aplicarDescontoDto.ValorTotalComDesconto = orcamentoItem.TotalItem;
        aplicarDescontoDto.DescontoModeloVendaUtilizado = orcamentoItem.DescontoModeloVendaUtilizado;
        aplicarDescontoDto.IdOrcamentoItem = orcamentoItem.Id;
        aplicarDescontoDto.PrecoUnitario = orcamentoItem.PercDescon > 0 ? decimal.Round(orcamentoItem.PrecoUnitario - ((orcamentoItem.PercDescon / 100m) * orcamentoItem.PrecoUnitario), 2) : orcamentoItem.PrecoUnitario;
        // Desconto Alçada
        aplicarDescontoDto.ObservacaoGeral = null;
        aplicarDescontoDto.ObservacaoItem = null;
        var solicitacaoDescontoVendaAlcada = _orcamentoItemRepository.GetSolicitacaoDescontoVendaAlcada(orcamentoItemId, StatusSolicitacao.NaoEnviado);
        if (solicitacaoDescontoVendaAlcada != null)
        {
            aplicarDescontoDto.ObservacaoGeral = solicitacaoDescontoVendaAlcada.ObservacaoGeral;
            aplicarDescontoDto.ObservacaoItem = solicitacaoDescontoVendaAlcada.ObservacaoItem;
        }

        aplicarDescontoDto.PercentualDescontoAlcadaGerente = 0m;
        aplicarDescontoDto.PercentualLimiteDesconto = 0m;
        /* TODO Desabilitando temporariamente validação de desconto para o TMK até definição dessa opção - 2020/01/29
        if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
        {
            var codOperador = HttpContext.Current.User.Identity.GetIdOperador();
            var operador = _vendedorRepository.GetByUser(codOperador);
            //TODO ajuste no limite do operador 2020/01/09
            aplicarDescontoDto.PercentualLimiteDesconto = 30;// operador.PercLimiteDesconto;
        }
        else
        {*/
        // Desconto por Loja
        var descontosLoja = _orcamentoItemRepository.GetDescontosLoja(orcamentoItem.Orcamento.LojaDellaVia.CampoCodigo).Where(x => x.IdAreaNegocio == orcamento.AreaNegocio.IdArea).ToList();
        if (descontosLoja != null && descontosLoja.Count > 0)
        {
            aplicarDescontoDto.PercentualDescontoAlcadaGerente = (decimal)descontosLoja.FirstOrDefault().PercentualDescontoLojaGerente > 0 ? (decimal)descontosLoja.FirstOrDefault().PercentualDescontoLojaGerente : 0m;
            aplicarDescontoDto.PercentualLimiteDesconto = (decimal)descontosLoja.FirstOrDefault().PercentualDescontoLojaVendedor > 0 ? (decimal)descontosLoja.FirstOrDefault().PercentualDescontoLojaVendedor : 0m;
            var descontoGrupo = descontosLoja.Where(g => g.IdGrupo == orcamentoItem.Produto.GrupoProduto.IdGrupoSubGrupo).FirstOrDefault();
            if (descontoGrupo != null)
            {
                aplicarDescontoDto.PercentualDescontoAlcadaGerente = (decimal)descontoGrupo.PercentualDescontoGrupoGerente > 0 ? (decimal)descontoGrupo.PercentualDescontoGrupoGerente : 0m;
                aplicarDescontoDto.PercentualLimiteDesconto = (decimal)descontoGrupo.PercentualDescontoGrupoVendedor > 0 ? (decimal)descontoGrupo.PercentualDescontoGrupoVendedor : 0m;
            }
        }
        //}
        return aplicarDescontoDto;
    }

    public void AplicarDescontoItensOrcamento(AplicarDescontoDto aplicarDescontoDto)
    {
        if (aplicarDescontoDto.PercentualDesconto <= 0 && aplicarDescontoDto.ValorOriginal == aplicarDescontoDto.ValorTotalComDesconto)
            return;
        var orcamentoItem = _orcamentoItemRepository.GetById(aplicarDescontoDto.IdOrcamentoItem);
        var orcamento = ObterOrcamentoPorId(orcamentoItem.OrcamentoId);
        orcamentoItem.DescontoModeloVendaUtilizado = null;
        orcamentoItem.DescontoModeloVenda = null;
        var valorDesconto = CalculaAjuste((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
        orcamentoItem.ValorDesconto = aplicarDescontoDto.ValorDesconto > 0 ? aplicarDescontoDto.ValorDesconto : valorDesconto;
        orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
        orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - (aplicarDescontoDto.ValorDesconto > 0 ? aplicarDescontoDto.ValorDesconto : valorDesconto);
        // Desconto por Loja
        var descontosLoja = _orcamentoItemRepository.GetDescontosLoja(orcamentoItem.Orcamento.LojaDellaVia.CampoCodigo).Where(x => x.IdAreaNegocio == orcamento.AreaNegocio).ToList();
        if (descontosLoja != null && descontosLoja.Count > 0)
        {
            var descontoGrupo = descontosLoja.Where(g => g.IdGrupo == orcamentoItem.Produto.GrupoProduto.IdGrupoSubGrupo).FirstOrDefault();
            var PercentualDescontoGerente = (descontoGrupo != null && (decimal)descontoGrupo?.PercentualDescontoGrupoGerente > 0) ? (decimal)descontoGrupo.PercentualDescontoGrupoGerente : (decimal)descontosLoja.FirstOrDefault().PercentualDescontoLojaGerente;
            var PercentualDescontoVendedor = (descontoGrupo != null && (decimal)descontoGrupo?.PercentualDescontoGrupoVendedor > 0) ? (decimal)descontoGrupo.PercentualDescontoGrupoVendedor : (decimal)descontosLoja.FirstOrDefault().PercentualDescontoLojaVendedor;
            // Desconto excede limite do desconto do gerente.
            if (aplicarDescontoDto.PercentualDesconto > PercentualDescontoGerente)
                throw new NegocioException("O desconto de venda alçada excede o limite máximo de desconto do gerente.");

            /*var valorDesconto = CalculaDesconto((orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario), aplicarDescontoDto.PercentualDesconto)[1];
            orcamentoItem.ValorDesconto = valorDesconto;
            orcamentoItem.PercDescon = aplicarDescontoDto.PercentualDesconto;
            orcamentoItem.TotalItem = (orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - valorDesconto;*/
            var solicitacaoDescontoVendaAlcada = _orcamentoItemRepository.GetSolicitacaoDescontoVendaAlcada(orcamentoItem.Id, StatusSolicitacao.NaoEnviado);
            // Validar o percentual de desconto se é maior que PercLimiteDesconto do vendedor.
            if (aplicarDescontoDto.PercentualDesconto > PercentualDescontoVendedor)
            {
                if (aplicarDescontoDto.ObservacaoGeral.IsNullOrEmpty() || aplicarDescontoDto.ObservacaoItem.IsNullOrEmpty())
                    throw new NegocioException("É necessáro preencher as observações!");
                //var nome = HttpContext.Current.User.Identity.GetName();
                //var operador = _vendedorRepository.GetByUser(codOperador);
                if (solicitacaoDescontoVendaAlcada == null)
                {
                    var novaSolicitacaoDescontoVendaAlcada = new SolicitacaoDescontoVendaAlcada
                    {
                        IdOrcamentoItem = orcamentoItem.Id,
                        DataSolicitacao = DateTime.Now,
                        ObservacaoItem = aplicarDescontoDto.ObservacaoItem,
                        ObservacaoGeral = aplicarDescontoDto.ObservacaoGeral,
                        StatusSolicitacaoAlcada = StatusSolicitacao.NaoEnviado,
                        ValorDesconto = valorDesconto,
                        PercentualDesconto = orcamentoItem.PercDescon,
                        Situacao = SituacaoDescontoAlcada.Pendente
                    };
                    //Insere solicitação de venda alçada.
                    _orcamentoItemRepository.AddSolicitacaoDescontoVendaAlcada(novaSolicitacaoDescontoVendaAlcada);
                }
                else
                {
                    solicitacaoDescontoVendaAlcada.ObservacaoGeral = aplicarDescontoDto.ObservacaoGeral;
                    solicitacaoDescontoVendaAlcada.ObservacaoItem = aplicarDescontoDto.ObservacaoItem;
                    solicitacaoDescontoVendaAlcada.ValorDesconto = valorDesconto;
                    solicitacaoDescontoVendaAlcada.PercentualDesconto = orcamentoItem.PercDescon;
                    //Atualiza solicitação de venda alçada.
                    _orcamentoItemRepository.UpdateSolicitacaoDescontoVendaAlcada(solicitacaoDescontoVendaAlcada);
                }
            } 
            else
            {
                if (solicitacaoDescontoVendaAlcada != null)
                {
                    solicitacaoDescontoVendaAlcada.Situacao = SituacaoDescontoAlcada.Cancelado;
                    //Atualiza solicitação de venda alçada.
                    _orcamentoItemRepository.UpdateSolicitacaoDescontoVendaAlcada(solicitacaoDescontoVendaAlcada);
                }
            }
        }

        if (orcamento.FormaPagamento != null)
            RemoverPagamentos(orcamento.Id);
        //CalcularImpostos(orcamento);
        _orcamentoItemRepository.Update(orcamentoItem);
    }

    /// <summary>
    /// Calcula o ajuste com base nos parametros informados.
    /// </summary>
    /// <param name="total">Valor total do item</param>
    /// <param name="percentual">Ajuste percentual</param>
    /// <param name="valor">Valor do ajuste.</param>
    /// <returns>Array de decimals, primeiro elemento é o valor toal do item, segundo representa o percentual e terceiro o valor do ajuste.</returns>
    public static decimal[] CalculaAjuste(decimal total, decimal? percentual = null, decimal? valor = null)
    {
        if (!(percentual == null ^ valor == null))
            throw new NegocioException("Deve ser informado apenas um tipo de desconto.");
        decimal valorAjuste;
        decimal totalAjuste;
        if (percentual != null)
        {
            valorAjuste = (decimal)(percentual / 100 * total);
            totalAjuste = total - valorAjuste;
        }
        else
        {
            valorAjuste = (decimal)(valor / total * 100);
            totalAjuste = (decimal)(total - valor);
        }
        return new[] { decimal.Round(totalAjuste, 2), decimal.Round(valorAjuste, 4) };
    }

    #endregion

    #region [ Veiculo Cliente ]
    public void AtualizarDadosVeiculo(OrcamentoDto orcamentoDto)
    {
        var orcamento = GetOrcamentoById(orcamentoDto.Id);
        if (orcamento == null)
            throw new NegocioException("Orçamento não encontrado.");
        orcamento.TipoOrcamento = orcamentoDto.TipoOrcamento;
        orcamento.IdConvenio = orcamentoDto.Convenio;
        orcamento.IdTabelaPreco = orcamentoDto.TabelaPreco;
        orcamento.Placa = ((orcamentoDto.PlacaVeiculo == null) ? null : Regex.Replace(orcamentoDto.PlacaVeiculo, "-", ""));
        orcamento.Ano = orcamentoDto.AnoVeiculo;
        orcamento.KM = orcamentoDto.QuilometragemVeiculo;
        orcamento.IdCliente = orcamentoDto.IdCliente;
        orcamento.Telefone = orcamentoDto.TelefoneCliente;
        orcamento.TelefoneCelular = orcamentoDto.CelularCliente;
        orcamento.TelefoneComercial = orcamentoDto.TelefoneComercialCliente;
        orcamento.InformacoesCliente = orcamentoDto.InformacoesCliente;
        orcamento.DataAtualizacao = DateTime.Now;
        orcamento.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
        orcamento.VeiculoIdFraga = _veiculoRepository.GetVeiculo(orcamentoDto.MarcaVeiculoDescricao, orcamentoDto.ModeloVeiculoDescricao, orcamentoDto.VersaoVeiculoDescricao, orcamentoDto.VersaoMotorDescricao, orcamentoDto.AnoDescricao)?.VeiculoIdFraga;
        try
        {
            Update(orcamento);
        }
        catch (Exception excecao)
        {
            throw new NegocioException("Erro ao atualizar o orcamento", excecao);
        }
    }
    #endregion

    #region [ Forma de Pagamento do Orçamento ]
    public OrcamentoPagamentoDto ObterOrcamentoPagamentoDto(long idOrcamento)
    {
        var orcamentoFormaPagamentos = _orcamentoFormaPagamentoRepository.GetsByIdOrcamentoDto(idOrcamento).ToList();
        var orcamentoPagamentoDto = new OrcamentoPagamentoDto();
        orcamentoPagamentoDto.FormasPagamentos = Mapper.Map<List<OrcamentoFormaPagamentoDto>>(orcamentoFormaPagamentos);
        // Condição Pagamento default
        var orcamentoFormaPagamento = orcamentoFormaPagamentos.FirstOrDefault();
        if (orcamentoFormaPagamento != null)
            orcamentoPagamentoDto.CondicaoPagamento = orcamentoFormaPagamento.Id + ";" + orcamentoFormaPagamento.CondicaoPagamento;
        return orcamentoPagamentoDto;
    }

    public void RemoverPagamentos(long orcamentoId)
    {
        var formas = _orcamentoFormaPagamentoRepository.GetsByIdOrcamento(orcamentoId);
        if (formas == null)
            return;
        foreach (var pagamento in formas)
            _orcamentoFormaPagamentoRepository.DeleteById(pagamento.Id);
    }
    #endregion

    #region [ Análise de crédito ]
    public List<SolicitacaoAnaliseCredito> GetSolicitacaoAnaliseCreditoByOrcamento(long idOrcamento)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT DISTINCT Id, IdOrcamento, CampoCodigo " +
                         "  FROM SOLICITACAO_ANALISE_CREDITO " +
                         " WHERE IdOrcamento = @IdOrcamento " +
                         " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamento", idOrcamento);
            List<SolicitacaoAnaliseCredito> list = new List<SolicitacaoAnaliseCredito>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new SolicitacaoAnaliseCredito
                        {
                            Id = (long)reader["Id"],
                            IdOrcamento = (long)reader["IdOrcamento"],
                            DataSolicitacao = (DateTime)reader["DataSolicitacao"],
                            StatusSolicitacaoAnaliseCredito = (StatusSolicitacao)(int)reader["StatusSolicitacaoAnaliseCredito"],
                            SituacaoAnaliseCredito = (SituacaoAnaliseCredito)(int)reader["SituacaoAnaliseCredito"],
                            DataResposta = (DateTime)reader["DataResposta"],
                            RespostaSolicitacao = reader["RespostaSolicitacao"].ToString(),
                            NumeroContrato = reader["NumeroContrato"].ToString(),
                            RegistroInativo = (bool)reader["RegistroInativo"],
                            CampoCodigo = reader["CampoCodigo"].ToString(),
                            DataAtualizacao = (DateTime)reader["DataAtualizacao"],
                            UsuarioAtualizacao = reader["UsuarioAtualizacao"].ToString()
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }

    public void AddSolicitacaoAnaliseCredito(SolicitacaoAnaliseCredito solicitacao)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "INSERT INTO SOLICITACAO_ANALISE_CREDITO " +
                        "( IdOrcamento,  DataSolicitacao,  StatusSolicitacaoAnaliseCredito,  SituacaoAnaliseCredito,  DataResposta, " +
                        " RespostaSolicitacao,  NumeroContrato,  CampoCodigo,  RegistroInativo,  DataAtualizacao,  UsuarioAtualizacao) " +
                        "VALUES " +
                        "(@IdOrcamento, @DataSolicitacao, @StatusSolicitacaoAnaliseCredito, @SituacaoAnaliseCredito, @DataResposta, " +
                        "@RespostaSolicitacao, @NumeroContrato, @CampoCodigo, @RegistroInativo, @DataAtualizacao, @UsuarioAtualizacao); ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdOrcamento ", solicitacao.IdOrcamento);
            cmd.Parameters.AddWithValue("@DataSolicitacao", solicitacao.DataSolicitacao);
            cmd.Parameters.AddWithValue("@StatusSolicitacaoAnaliseCredito", solicitacao.StatusSolicitacaoAnaliseCredito);
            cmd.Parameters.AddWithValue("@SituacaoAnaliseCredito", solicitacao.SituacaoAnaliseCredito);
            cmd.Parameters.AddWithValue("@DataResposta", solicitacao.DataResposta ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RespostaSolicitacao", solicitacao.RespostaSolicitacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@NumeroContrato", solicitacao.NumeroContrato ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CampoCodigo", solicitacao.CampoCodigo);
            cmd.Parameters.AddWithValue("@RegistroInativo", 0);
            cmd.Parameters.AddWithValue("@DataAtualizacao", solicitacao.DataAtualizacao);
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
    #endregion

    #region [ Observação ]
    public List<OrcamentoObservacao> GetOrcamentoObservacaoByTipo(long tipo)
    {
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string sql = "SELECT DISTINCT Id, Titulo, Conteudo, TipoObservacao  " +
                         "  FROM ORCAMENTO_OBSERVACAO " +
                         " WHERE TipoObservacao = @TipoObservacao " +
                         "   AND RegistroInativo <> 1 " +
                         " ORDER BY 1";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TipoObservacao", tipo);
            List<OrcamentoObservacao> list = new List<OrcamentoObservacao>();
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (reader.Read())
                        list.Add(new OrcamentoObservacao
                        {
                            Id = (long)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Conteudo = reader["Conteudo"].ToString(),
                            TipoObservacao = (TipoObservacao)(int)reader["TipoObservacao"]
                        });
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }
    #endregion

    #region [ Impostos ]
    public void CalcularImpostos(Orcamento orcamento)
    {
        var valorImpostos = orcamento.ValorImpostos = _calculoImpostosApi.CalculoImpostos(orcamento);
        Update(orcamento);
    }

    public void CalcularImpostos(long idOrcamento)
    {
        var orc = GetOrcamentoById(idOrcamento);
        var valorImpostos = default(decimal);
        if (orc.OrcamentoItens != null)
            valorImpostos = _calculoImpostosApi.CalculoImpostos(orc);
        orc.ValorImpostos = valorImpostos;
        Update(orc);
    }
    #endregion

    #region [ Protheus ]
    public async Task<bool> EnviarOrcamentoProtheus(long idOrcamento)
    {
        var orcamento = GetOrcamentoById(idOrcamento);
        if (orcamento == null)
            throw new NegocioException("Orçamento não encontrado.");
        orcamento.OrcamentoItens = _orcamentoItemRepository.GetOrcamentoItensByIdOrcamento(idOrcamento);
        foreach (var item in orcamento.OrcamentoItens)
        {
            item.SolicitacaoDescontoVendaAlcadaList = _orcamentoItemRepository.GetSolicitacoesDescontoVendaAlcada(item.OrcamentoId, null);
            item.EquipeMontagemList = _equipeMontagemRepository.GetByIdOrcamentoItem(item.OrcamentoId);
        }
        orcamento.FormaPagamentos = _orcamentoFormaPagamentoRepository.GetsByIdOrcamento(idOrcamento).ToList();
        foreach (var forma in orcamento.FormaPagamentos)
            forma.CondicaoPagamento = _condicaoPagamentoRepository.GetCondicaoPagamentoById(forma.IdCondicaoPagamento, orcamento.IdAreaNegocio);
        orcamento.TabelaPreco = _tabelaPrecoRepository.GetById(orcamento.IdTabelaPreco);
        OrcamentoRetornoPostProtheusDto retornoPostProtheusDto;
        try
        {
            var codUsuario = HttpContext.Current.User.Identity.Name;
            CalcularImpostos(orcamento);
            retornoPostProtheusDto = await _orcamentoApi.EnviarOrcamento(orcamento, codUsuario);
        }
        catch (NegocioException n)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new NegocioException("Ocorreu um erro ao processar o orçamento no Protheus: " + e.Message);
        }

        if (retornoPostProtheusDto.ExisteAnaliseCredito)
        {
            //Verifica se já existe a solicitação
            var solicitacaoAnaliseCredito = GetSolicitacaoAnaliseCreditoByOrcamento(orcamento.Id).FirstOrDefault();
            if (solicitacaoAnaliseCredito == null || solicitacaoAnaliseCredito.SituacaoAnaliseCredito != SituacaoAnaliseCredito.EmAnalise)
            {
                solicitacaoAnaliseCredito = new SolicitacaoAnaliseCredito
                {
                    IdOrcamento = orcamento.Id,
                    SituacaoAnaliseCredito = retornoPostProtheusDto.SituacaoAnaliseCredito,
                    StatusSolicitacaoAnaliseCredito = StatusSolicitacao.PendenteRetorno,
                    NumeroContrato = retornoPostProtheusDto.NumeroContratoSolicitacaoAnaliseCredito,
                    DataSolicitacao = DateTime.Now
                };
                if (solicitacaoAnaliseCredito.SituacaoAnaliseCredito != SituacaoAnaliseCredito.EmAnalise)
                {
                    solicitacaoAnaliseCredito.DataResposta = DateTime.Now;
                    solicitacaoAnaliseCredito.StatusSolicitacaoAnaliseCredito = StatusSolicitacao.Retornado;
                    solicitacaoAnaliseCredito.RespostaSolicitacao = retornoPostProtheusDto.RespostaSolicitacao;
                }
                AddSolicitacaoAnaliseCredito(solicitacaoAnaliseCredito);
            }
        }
        /*
        // Espelhamento dos registros de Marca e Modelo com o protheus.
        if (orcamento.MarcaModeloVersao.MarcaModelo.CampoCodigo.IsNullOrEmpty()
            || orcamento.MarcaModeloVersao.MarcaModelo.CampoCodigo.IsNullOrEmpty())
        {
            if (!retornoPostProtheusDto.CampoCodigoMarca.IsNullOrEmpty())
            {
                var marca = _marcaRepositorio.GetSingle(x => x.Id == orcamento.MarcaModeloVersao.MarcaModelo.IdMarca);
                marca.CampoCodigo = retornoPostProtheusDto.CampoCodigoMarca;
            }

            if (!retornoPostProtheusDto.CampoCodigoModelo.IsNullOrEmpty())
            {
                var modelo = _marcaModeloRepositorio.GetSingle(x => x.Id == orcamento.MarcaModeloVersao.MarcaModelo.Id);
                modelo.CampoCodigo = retornoPostProtheusDto.CampoCodigoModelo;
            }
        }
        */
        orcamento.CampoCodigo = retornoPostProtheusDto.CampoCodigoOrcamento;
        Update(orcamento);
        return true;
    }
    #endregion
}
