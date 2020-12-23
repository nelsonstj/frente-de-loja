using AutoMapper;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Filters;
using DV.FrenteLoja.Models;
using DV.FrenteLoja.Repository;
using DV.FrenteLoja.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

[Authorize]
[AuthorizationFilter(FlagPerfilAcessoUsuario = new PerfilAcessoUsuario[] { PerfilAcessoUsuario.TMK, PerfilAcessoUsuario.FrenteLoja })]
public class OrcamentoController : Controller
{
    private ClienteRepository _clienteRepository = new ClienteRepository();
    private ConvenioRepository _convenioRepository = new ConvenioRepository();
    private EquipeMontagemRepository _equipeMontagemRepository = new EquipeMontagemRepository();
    private LojaDellaViaRepository _lojaDellaViaRepository = new LojaDellaViaRepository();
    private LojaConfigRepository _lojaConfigRepository = new LojaConfigRepository();
    private TabelaPrecoRepository _tabelaPrecoRepository = new TabelaPrecoRepository();
    private TransportadoraRepository _transportadoraRepository = new TransportadoraRepository();
    private readonly IVeiculoServico _veiculoServico;
    private readonly IOrcamentoServico _orcamentoServico;
    private readonly IProdutoServico _produtoServico;
    private readonly IOrcamentoItemServico _orcamentoItemServico;
    private readonly IParametroGeralServico _parametroGeralServico;
    private readonly AreaNegocioRepository _areaNegocioRepository;
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrcamentoFormaPagamentoRepository _orcamentoFormaPagamentoRepository;
    private readonly ProdutoRepository _produtoRepository;
    private readonly VeiculoRepository _veiculoRepository;
    private readonly VendedorRepository _vendedorRepository;
    private readonly CondicaoPagamentoRepository _condicaoPagamentoRepository;
    private readonly OrcamentoItemRepository _orcamentoItemRepository;
    private readonly HomeRepository _homeRepository;
    public OrcamentoController(IVeiculoServico veiculoServico, 
        /*IConvenioServico convenioServico, IEstoqueProtheusApi estoqueProtheusServico,*/ 
        IOrcamentoServico orcamentoServico, 
        IProdutoServico produtoService, 
        IOrcamentoItemServico orcamentoItemServico, 
        IParametroGeralServico parametroGeralServico, 
        AreaNegocioRepository areaNegocioRepository,
        OrcamentoRepository orcamentoRepository, 
        OrcamentoFormaPagamentoRepository orcamentoFormaPagamentoRepository, 
        ProdutoRepository produtoRepository, 
        VeiculoRepository veiculoRepository, 
        VendedorRepository vendedorRepository, 
        CondicaoPagamentoRepository condicaoPagamentoRepository, 
        OrcamentoItemRepository orcamentoItemRepository,
        HomeRepository homeRepository)
    {
        _veiculoServico = veiculoServico;
        _orcamentoServico = orcamentoServico;
        _produtoServico = produtoService;
        _orcamentoItemServico = orcamentoItemServico;
        _parametroGeralServico = parametroGeralServico;
        _areaNegocioRepository = areaNegocioRepository;
        _orcamentoRepository = orcamentoRepository;
        _orcamentoFormaPagamentoRepository = orcamentoFormaPagamentoRepository;
        _produtoRepository = produtoRepository;
        _veiculoRepository = veiculoRepository;
        _vendedorRepository = vendedorRepository;
        _condicaoPagamentoRepository = condicaoPagamentoRepository;
        _orcamentoItemRepository = orcamentoItemRepository;
        _homeRepository = homeRepository;
    }

    #region [ Lista de Orçamentos ]
    public ActionResult Index(string vencidos)
    {
        return View();
    }

    public ActionResult ObterOrcamentos(TipoFiltroOrcamento? tipo, string termo, StatusOrcamento? statusOrcamento)
    {
        try
        {
            ICollection<OrcamentoConsultaDto> orcamentos = _orcamentoRepository.ObterListaOrcamento(tipo, termo, statusOrcamento);
            return new JsonResult
            {
                Data = orcamentos,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            base.Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult ObterOrcamentosVencidosHoje(TipoFiltroOrcamento? tipo, string termo, StatusOrcamento? statusOrcamento)
    {
        try
        {
            ICollection<OrcamentoConsultaDto> orcamentos = _orcamentoRepository.ObterListaOrcamentoVencidosHoje();
            return new JsonResult
            {
                Data = orcamentos,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public async Task<ActionResult> ObterOrcamentosProtheus(TipoFiltroOrcamento? tipo, string termo, StatusOrcamento? statusOrcamento)
    {
        try
        {
            ICollection<OrcamentoConsultaDto> orcamentos = await _orcamentoRepository.ObterListaOrcamentoProtheus(tipo, termo, statusOrcamento);
            return new JsonResult
            {
                Data = orcamentos,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult SincronizaOrcamentoProtheus(long id)
    {
        try
        {
            OrcamentoDto orc = _orcamentoRepository.ObterOrcamentoPorId(id);
            Task<string> retorno = _orcamentoServico.AtualizarOrcamentoComProtheus(orc);
            if (!string.IsNullOrEmpty(retorno.Result))
                throw new NegocioException(retorno.Result);
            return Json("ok");
        }
        catch (NegocioException ex)
        {
            base.Response.StatusCode = 500;
            return Json("Erro ao sincronizar com o Protheus: " + ex.Message);
        }
        catch (Exception ex)
        {
            base.Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }
    #endregion

    #region [ Veiculo Cliente ]
    public ActionResult VeiculoCliente(long? id, OrcamentoDto orcamentoDto)
    {
        try
        {
            if (orcamentoDto.Voltar)
                return View(orcamentoDto);
            if (id.HasValue)
            {
                OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(id.Value);
                return View(orcamento);
            }
            if (_homeRepository.ObterPerfilUsuarioLogado() == PerfilAcessoUsuario.TMK)
            {
                var consultor = _vendedorRepository.ObterConsultorLogado();
                if (consultor != null)
                {
                    orcamentoDto.Vendedor = consultor.IdConsultor;
                    orcamentoDto.VendedorDescricao = consultor.Nome;
                }
            }
            LojaDellaViaDto loja = _lojaDellaViaRepository.ObterLojaUsuarioLogado();
            if (loja != null)
            {
                orcamentoDto.LojaDestino = loja.CampoCodigo;
                orcamentoDto.LojaDestinoDescricao = loja.Descricao;
                orcamentoDto.Convenio = _lojaConfigRepository.GetById(loja.CampoCodigo)?.ConvenioPadrao ?? "";
            }
            ConvenioDto convenio = !string.IsNullOrEmpty(orcamentoDto.Convenio) ? _convenioRepository.ObterConvenioDto(orcamentoDto.Convenio) : _convenioRepository.ObterConvenioUsuarioLogado();
            if (convenio != null)
            {
                orcamentoDto.Convenio = convenio.IdConvenio;
                orcamentoDto.ConvenioDescricao = $"{convenio.IdConvenio} - {convenio.Descricao}";
                orcamentoDto.InformacaoConvenio = convenio.Observacoes;
                orcamentoDto.IdClienteLogado = convenio.IdCliente;
                if (convenio.IdTabelaPreco != null)
                {
                    orcamentoDto.TabelaPreco = convenio.IdTabelaPreco;
                    orcamentoDto.TabelaPrecoDescricao = _tabelaPrecoRepository.GetTabelaPrecoDto(convenio.IdTabelaPreco).Descricao;
                }
                orcamentoDto.TrocaCliente = convenio.TrocaCliente;
                orcamentoDto.TrocaProduto = convenio.TrocaProduto;
                orcamentoDto.TrocaTabelaPreco = convenio.TrocaTabelaPreco;
                orcamentoDto.TrocaPrecoConvenio = convenio.TrocaPreco;
            }
            AreaNegocioDto areaNegocio = _areaNegocioRepository.ObterAreaNegocioUsuarioLogado();
            if (areaNegocio != null)
            {
                orcamentoDto.AreaNegocio = areaNegocio.Id;
                orcamentoDto.AreaNegocioDescricao = areaNegocio?.Descricao;
            }
            orcamentoDto.AnoVeiculo = null;
            orcamentoDto.QuilometragemVeiculo = null;
            orcamentoDto.DataValidade = _parametroGeralServico.ObterDataVencimentoOrcamento();
            return View(orcamentoDto);
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    public ActionResult BuscaLojaPorId(string idLoja)
    {
        try
        {
            var lojaDestino = "";
            var lojaDestinoDescricao = "";
            ConvenioDto convenio = null;
            LojaDellaVia loja = null;
            if (idLoja != null)
                loja = _lojaDellaViaRepository.GetById_PD(idLoja);
            if (loja != null)
            {
                lojaDestino = loja.CampoCodigo;
                lojaDestinoDescricao = loja.Descricao;
                var convenioPadrao = _lojaConfigRepository.GetById(loja.CampoCodigo)?.ConvenioPadrao ?? "";
                convenio = !string.IsNullOrEmpty(convenioPadrao) ? _convenioRepository.ObterConvenioDto(convenioPadrao) : _convenioRepository.ObterConvenioUsuarioLogado();
            }
            return new JsonResult
            {
                Data = new
                {
                    lojaDestino,
                    lojaDestinoDescricao,
                    convenio
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult ObterVeiculoPorPlaca(string placa)
    {
        try
        {
            var placaNova = (placa.Contains(" ") ? placa.Remove(placa.IndexOf(' '), 1) : placa);
            if (placaNova.Length != 7)
                throw new NegocioException("Placa está em um formato incorreto. Formato esperado: XXX 0X00.");
            ClienteVeiculoDto data = _veiculoRepository.ObterVeiculoPorPlaca(placaNova);
            if (data == null)
                throw new NegocioException("Não foi encontrado dados do veículo para a placa informada. <br /> Por favor, preencha os dados manualmente. <br />");
            data.Placa = placa;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult ObterClientePorTipo(string tipoConsulta, string codigo)
    {
        try
        {
            ClienteDto cliente;
            switch (tipoConsulta)
            {
                case "id":
                    cliente = _clienteRepository.ObterClientePorTipo(codigo, tipoConsulta);
                    break;
                case "codigo":
                    if (!codigo.Contains("-") || codigo.Length != 9)
                        codigo += "-99";
                        //throw new NegocioException("Código está em um formato incorreto. Formato esperado: 000000-00.");
                    cliente = _clienteRepository.ObterClientePorTipo(codigo, tipoConsulta);
                    break;
                case "cpf":
                case "cnpj":
                    codigo = codigo.Replace(".", "").Replace("-", "").Replace("/", "");
                    cliente = /*(_clienteRepository.ObterClientePorTipo(codigo, tipoConsulta) ??*/ _clienteRepository.GetByCpfCnpj(codigo);
                    break;
                case "nome":
                    if (string.IsNullOrEmpty(codigo) || codigo.Length < 6)
                        throw new NegocioException("Formato incorreto.");
                    cliente = _clienteRepository.ObterClientePorTipo(codigo.ToUpper(), tipoConsulta);
                    break;
                default:
                    return Json(new
                    {
                        status = "error",
                        message = "Cliente não encontrado"
                    });
            }
            if (cliente == null)
                throw new NegocioException("Cliente não encontrado");
            return new JsonResult
            {
                Data = Mapper.Map<ClienteDto>(cliente),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    [HttpGet]
    public ActionResult ObterClientePorPlaca(string placa)
    {
        Cliente data = _veiculoServico.ObterClientePorPlaca(placa);
        return new JsonResult
        {
            Data = data,
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    public ActionResult VerificaConveniosPorCliente(string convenio, string cliente, string loja)
    {
        loja = (loja.Contains("-") ? loja.Split('-')[0].Trim() : loja);
        List<ConvenioDto> data = _convenioRepository.GetConveniosByCliente(cliente, loja);
        return new JsonResult
        {
            Data = data.Any(c => c.IdConvenio == convenio),
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    public ActionResult AbrirModalClienteVinculado(string idCLiente)
    {
        try
        {
            if (Request.IsAjaxRequest() && !string.IsNullOrEmpty(idCLiente))
            {
                ClienteDto cliente = _clienteRepository.ObterClientePorTipo(idCLiente, "id");
                return PartialView("_ModalClienteVinculado", cliente);
            }
            return Content("");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    //public async Task<ActionResult> AbrirModalVeiculoVinculado(string placa)
    public ActionResult AbrirModalVeiculoVinculado(string placa)
    {
        try
        {
            if (Request.IsAjaxRequest() && !string.IsNullOrEmpty(placa))
            {
                //ClienteVeiculoDto veiculo = await _veiculoRepository.ObterVeiculoPorPlaca(placa);
                ClienteVeiculoDto veiculo = _veiculoRepository.ObterVeiculoPorPlaca(placa);
                veiculo.Placa = placa.Insert(3, "-");
                return PartialView("_ModalSugestaoVeiculo", veiculo);
            }
            return Content("");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    #region [ Actions listas ]
    [HttpGet]
    public ActionResult ObterLojaDestino(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _lojaDellaViaRepository.TamanhoLojasPorTermo(termoBusca);
        List<LojaDellaViaDto> data = _lojaDellaViaRepository.ObterLojasPorNome(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterConvenios(int tamanhoPagina, int numeroPagina, string termoBusca, string cliente, string loja)
    {
        /*if (string.IsNullOrEmpty(cliente))
            cliente = _convenioRepository.ObterConvenioUsuarioLogado().IdCliente;
        if (string.IsNullOrEmpty(loja))
            loja = _lojaDellaViaRepository.ObterLojaUsuarioLogado().CampoCodigo;*/
        loja = (loja.Contains("-") ? loja.Split('-')[0].Trim() : loja);
        int count = _convenioRepository.TamanhoConveniosPorTermo(termoBusca, cliente, loja);
        List<ConvenioDto> data = _convenioRepository.ObterConveniosPorTermo(tamanhoPagina, numeroPagina, termoBusca, cliente, loja);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterTabelaPreco(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _tabelaPrecoRepository.TamanhoTabelaPrecoPorTermo(termoBusca);
        List<TabelaPrecoDto> data = _tabelaPrecoRepository.ObterTabelaPrecoPorTermo(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterMarcasVeiculos(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _veiculoRepository.QuantidadeMarcasPorTermo(termoBusca);
        List<VeiculoMarcaModel> data = _veiculoRepository.ObterMarcaPorTermo(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterModelosVeiculos(int tamanhoPagina, int numeroPagina, string marca, string versao, string motor, string ano, string buscaModelo)
    {
        try
        {
            if (string.IsNullOrEmpty(marca))
            {
                return new JsonResult
                {
                    Data = new { data = new object[0], count = 0 },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            int count = _veiculoRepository.QuantidadeModelosPorTermo(marca, versao, motor, ano, buscaModelo);
            List<VeiculoModeloModel> data = _veiculoRepository.ObterModelosPelaMarca(tamanhoPagina, numeroPagina, marca, versao, motor, ano, buscaModelo);
            return new JsonResult
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (Exception)
        {
            return new JsonResult
            {
                Data = new { data = new object[0], count = 0 },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    [HttpGet]
    public ActionResult ObterVersaoVeiculos(int tamanhoPagina, int numeroPagina, string marca, string modelo, string motor, string ano, string buscaVersao)
    {
        try
        {
            int count = _veiculoRepository.QuantidadeVersoesPorTermo(marca, modelo, motor, ano, buscaVersao);
            List<VeiculoVersaoModel> data = _veiculoRepository.ObterVersoesPeloModelo(tamanhoPagina, numeroPagina, marca, modelo, motor, ano, buscaVersao);
            return new JsonResult
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (Exception)
        {
            return new JsonResult
            {
                Data = new { data = new object[0], count = 0 },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    [HttpGet]
    public ActionResult ObterMotorVeiculos(int tamanhoPagina, int numeroPagina, string marca, string modelo, string versao, string ano, string buscaMotor)
    {
        try
        {
            int count = _veiculoRepository.QuantidadeMotoresPorTermo(marca, modelo, versao, ano, buscaMotor);
            List<VeiculoMotorModel> data = _veiculoRepository.ObterVersoesMotorPeloIdVersaoVeiculo(tamanhoPagina, numeroPagina, marca, modelo, versao, ano, buscaMotor);
            return new JsonResult
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (Exception)
        {
            return new JsonResult
            {
                Data = new { data = new object[0], count = 0 },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    [HttpGet]
    public ActionResult ObterAnos(int tamanhoPagina, int numeroPagina, string marca, string modelo, string versao, string motor, string buscaAno)
    {
        try
        {
            int count = _veiculoRepository.QuantidadeAnosPorTermo(marca, modelo, versao, motor, buscaAno);
            List<AnosModel> data = _veiculoRepository.ObterAnosVeiculo(tamanhoPagina, numeroPagina, marca, modelo, versao, motor, buscaAno);
            return new JsonResult
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (Exception)
        {
            return new JsonResult
            {
                Data = new { data = new object[0], count = 0 },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    [HttpGet]
    public ActionResult ObterNomeCliente(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _clienteRepository.TamanhoClientePorTermo(termoBusca);
        List<ClienteDto> data = _clienteRepository.ObterClientePorNome(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            MaxJsonLength = int.MaxValue
        };
    }
    #endregion
    #endregion

    #region [ Equipe Vendas ]
    public ActionResult EquipeVendas()
    {
        return RedirectToAction("VeiculoCliente");
    }

    [HttpPost]
    public ActionResult EquipeVendas(OrcamentoDto orcamentoDto)
    {
        try
        {
            if (orcamentoDto.Id == 0L)
            {
                AreaNegocioDto areaNegocio = _areaNegocioRepository.ObterAreaNegocioUsuarioLogado();
                if (areaNegocio != null)
                {
                    orcamentoDto.AreaNegocio = areaNegocio.Id;
                    orcamentoDto.AreaNegocioDescricao = areaNegocio?.Descricao;
                }
                LojaDellaViaDto loja = _lojaDellaViaRepository.ObterLojaUsuarioLogado();
                if (loja != null)
                {
                    orcamentoDto.LojaDestino = loja.CampoCodigo;
                    orcamentoDto.LojaDestinoDescricao = loja.Descricao;
                }
                orcamentoDto.DataValidade = _parametroGeralServico.ObterDataVencimentoOrcamento();
            }
            if (orcamentoDto.TipoOrcamento == TipoOrcamento.Retira)
            {
                VeiculoModel veiculo = _veiculoRepository.ObterVeiculoRetira();
                orcamentoDto.MarcaVeiculoDescricao = veiculo.Marca;
                orcamentoDto.ModeloVeiculoDescricao = veiculo.Modelo;
                orcamentoDto.VersaoVeiculoDescricao = veiculo.Versao;
                orcamentoDto.VersaoMotorDescricao = veiculo.Motor;
            }
            if (orcamentoDto.Id == 0)
                return View("EquipeVendas", orcamentoDto);

            _orcamentoRepository.AtualizarDadosVeiculo(orcamentoDto);
            return RedirectToAction("EditarEquipeVendas", new { id = orcamentoDto.Id });
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("VeiculoCliente");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ActionResult VoltarVeiculoCliente(OrcamentoDto orcamentoDto)
    {
        return RedirectToAction("VeiculoCliente", orcamentoDto);
    }

    public ActionResult EditarEquipeVendas(long id)
    {
        try
        {
            OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(id);
            return View("EquipeVendas", orcamento);
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public ActionResult BuscaConvenioPorId(string idConvenio, string idCliente)
    {
        try
        {
            Convenio convenio = _convenioRepository.GetById(idConvenio);
            convenio.TabelaPreco = _tabelaPrecoRepository.GetById(convenio.IdTabelaPreco);
            if (convenio.TabelaPreco == null)
                throw new NegocioException("Tabela de preço para esse convênio não foi encontrada.");
            if (!string.IsNullOrEmpty(idCliente) && !convenio.TrocaCliente)
                throw new NegocioException("Convênio não está associado ao cliente.");
            return new JsonResult
            {
                Data = convenio,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    #region [ Action listas ]
    [HttpGet]
    public ActionResult ObterAreaNegocio(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _areaNegocioRepository.QuantidadeAreaNegociosPorTermo(termoBusca);
        List<AreaNegocioDto> data = _areaNegocioRepository.ObterAreaNegocioPorTermo(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterVendedor(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _vendedorRepository.TamanhoVendedorPorTermo(termoBusca);
        List<VendedorDto> data = _vendedorRepository.ObterVendedorPorNome(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterTransportadora(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _transportadoraRepository.TamanhoTransportadoraPorTermo(termoBusca);
        List<TransportadoraDto> data = _transportadoraRepository.ObterTransportadorasPorNome(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }
    #endregion
    #endregion

    #region [ Busca de Produtos ]
    public ActionResult IniciarOrcamento(OrcamentoDto orcamento)
    {
        try
        {
            if (orcamento.TipoOrcamento == TipoOrcamento.Retira) 
            {
                //VeiculoModel veiculo = _veiculoRepository.ObterVeiculoRetira();
                orcamento.VersaoVeiculoDescricao = "RETIRA";
                orcamento.ModeloVeiculoDescricao = "RETIRA";
                orcamento.MarcaVeiculoDescricao = "RETIRA";
                orcamento.VersaoMotorDescricao = "RETIRA";
            }
            orcamento.Id = _orcamentoRepository.IniciarOrcamento(orcamento);
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("EquipeVendas", orcamento);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return RedirectToAction("BuscaProduto", new { id = orcamento.Id });
    }

    public ActionResult BuscaProduto(long id)
    {
        try
        {
            OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(id);
            if (orcamento.StatusSomenteLeitura)
                return RedirectToAction("Negociacao", new { id = orcamento.Id });
            return View(orcamento);
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public ActionResult PesquisarProdutos(string search)
    {
        try
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CatalogoFraga busca = serializer.Deserialize<CatalogoFraga>(search);
            if (search == "{}" || (busca.CodigoGrupo == "0" && busca.VeiculoIdFraga == null && busca.VeiculoMarca == null && busca.VeiculoModelo == null))
            {
                busca.CodigoGrupo = "1";       // PNEUS
                busca.CodigoSubGrupo = "0001"; // PNEU ORIGINAL = PIRELLI
            }
            string loja = busca.IdLojaDestino ?? _lojaDellaViaRepository.ObterLojaUsuarioLogado().CampoCodigo;
            string tabelaPreco = busca.TabelaPreco ?? _convenioRepository.ObterConvenioUsuarioLogado().IdTabelaPreco;
            List<CatalogoFraga> produto = busca.CodigoGrupo == "4" /*Serviço*/ ? _produtoRepository.GetByDescriptionServicoPD(busca, loja, tabelaPreco) : _produtoRepository.GetByDescription(busca, loja, tabelaPreco);
            long ok = 0L;
            if ((produto == null || produto.Count == 0) && busca.CodigoGrupo != "4" && long.TryParse(busca.ProdutoDescricao, out ok))
                produto = _produtoRepository.GetByDescriptionPD(search, loja, tabelaPreco);
            if (produto == null || produto.Count == 0)
                throw new Exception("Não foi encontrado produto para essa pesquisa.");
            return new JsonResult
            {
                Data = produto.OrderByDescending(a => a.Estoque).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult AbrirModalDetalhes(string campoCodigo, string numeroOrcamento, string estoque)
    {
        try
        {
            if (base.Request.IsAjaxRequest())
            {
                Orcamento orcamento = _orcamentoRepository.GetOrcamentoById(Convert.ToInt64(numeroOrcamento));
                var loja = _lojaDellaViaRepository.ObterLojaUsuarioLogado().CampoCodigo;
                //IEnumerable<LojaDellaVia> lojasProximas = _lojaDellaViaRepository.ObterLojasProximas(loja).Distinct();
                ModalDetalhesProdutoDto produto = _produtoRepository.ObterDadosModalDetalhesProduto(orcamento, campoCodigo, loja);
                return PartialView("_ModalDetalhesProduto", produto);
            }
            return Content("");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            base.Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult AbrirEditarOrcamentoItem(string idOrcamento, string idOrcamentoItem)
    {
        if (Request.IsAjaxRequest())
        {
            ModalDetalhesProdutoDto produto = _orcamentoRepository.ObterDadosModalDetalhesProduto(Convert.ToInt64(idOrcamentoItem), Convert.ToInt64(idOrcamento));
            return PartialView("_ModalEditarOrcamentoItem", produto);
        }
        return Content("");
    }

    public ActionResult AbrirModalEstoque(string codDellavia, string lojaDestino)
    {
        try
        {
            var loja = lojaDestino ?? _lojaDellaViaRepository.ObterLojaUsuarioLogado().CampoCodigo;
            List<LojaDellaVia> lojasProximas = _lojaDellaViaRepository.ObterLojasProximas(loja);
            string tabelaPreco = _convenioRepository.ObterConvenioUsuarioLogado().IdTabelaPreco;
            CatalogoFraga produto = _produtoRepository.GetByCodigoDellavia(codDellavia, loja, tabelaPreco) ?? _produtoRepository.GetByCodigoDellaviaPD(codDellavia, loja, tabelaPreco);
            ModalEstoqueDto resultAsync = _produtoRepository.ObterSaldoProdutoLojasDellaVia(produto, lojasProximas.Select(item => item.CampoCodigo).ToArray());
            return PartialView("_ModalEstoque", resultAsync);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult ObterEstoqueFilial(string codigoProduto, string lojaDestino, bool listarProximas)
    {
        try
        {
            var loja = lojaDestino ?? _lojaDellaViaRepository.ObterLojaUsuarioLogado().CampoCodigo;
            //if (!idFilial.HasValue)
            //    idFilial = loja.Id;
            string tabelaPreco = _convenioRepository.ObterConvenioUsuarioLogado().IdTabelaPreco;
            CatalogoFraga produto = _produtoRepository.GetByCodigoDellavia(codigoProduto, loja, tabelaPreco) ?? _produtoRepository.GetByCodigoDellaviaPD(codigoProduto, loja, tabelaPreco);
            if (listarProximas)
            {
                List<LojaDellaVia> filiaisProximas = _lojaDellaViaRepository.ObterLojasProximas(loja);
                ModalEstoqueDto result3 = _produtoRepository.ObterSaldoProdutoLojasDellaVia(produto, filiaisProximas.Select((LojaDellaVia item) => item.CampoCodigo).ToArray());
                return new JsonResult
                {
                    Data = result3,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            /*if (idFilial == _lojaDellaViaRepository.ObterLojaUsuarioLogado().Id)
            {
                ModalEstoqueDto result2 = _produtoRepository.ObterSaldoProdutoLojasDellaVia(produto, null);
                return new JsonResult
                {
                    Data = result2,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }*/
            List<LojaDellaViaDto> filial = _lojaDellaViaRepository.GetLojasById_PD(loja);
            ModalEstoqueDto result = _produtoRepository.ObterSaldoProdutoLojasDellaVia(produto, filial.Select((LojaDellaViaDto item) => item.CampoCodigo).ToArray());
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult BuscaEstoqueProdutos(BuscaEstoqueProtheusDto busca)
    {
        try
        {
            if (busca.Produtos == null)
                return null;
            List<ProdutoProtheusDto> resultAsync = _produtoServico.ObterSaldosProdutoLojaDellaVia(busca.Produtos.ToArray(), busca.IdFilial).Result;
            return Json(resultAsync);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult BuscaValoresProdutos(BuscaValoresProdutoDto busca)
    {
        try
        {
            Orcamento orcamento = _orcamentoRepository.GetOrcamentoById(busca.IdOrcamento);
            string loja = _lojaDellaViaRepository.ObterLojaUsuarioLogado().CampoCodigo;
            Dictionary<string, decimal> response = _produtoRepository.ObterPrecoProdutoPorOrcamento(orcamento, loja, busca.Produtos.ToArray());
            List<object> reponseAjustado = new List<object>();
            foreach (KeyValuePair<string, decimal> item in response)
            {
                reponseAjustado.Add(new
                {
                    CampoCodigo = item.Key,
                    Valor = $"{item.Value:C}"
                });
            }
            return Json(reponseAjustado);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult RemoverItemCarrinho(long? orcamentoId, long? orcamentoItemId)
    {
        try
        {
            _orcamentoRepository.RemoverItensOrcamento(orcamentoId.Value, orcamentoItemId.Value);
            OrcamentoProdutoBuscaDto orcamentoProduto = _orcamentoRepository.ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orcamentoId.Value);
            return PartialView("_Carrinho", orcamentoProduto);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult CriaSacolaCompras(ModalDetalhesProdutoDto produto)
    {
        try
        {
            OrcamentoProdutoBuscaDto carrinho = _orcamentoRepository.InserirItensOrcamento(produto);
            return PartialView("_Carrinho", carrinho);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult AtualizarOrcamentoItem(ModalDetalhesProdutoDto produto)
    {
        try
        {
            OrcamentoProdutoBuscaDto carinho = _orcamentoRepository.AtualizarItensOrcamento(produto);
            return PartialView("_Carrinho", carinho);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult DetalheProduto(string produtoPaiId)
    {
        var produtoComplemento = _produtoRepository.ObterHtmlMaisDetalhesProduto(produtoPaiId);
        return PartialView("_ModalComplementoProduto", produtoComplemento);
    }

    #region [ Action listas ]
    [HttpGet]
    public ActionResult ObterFabricantePeca(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        HashSet<object> data = new HashSet<object>();
        int count = _produtoRepository.TamanhoTermoFabricantePeca(termoBusca);
        List<string> dataFabricanteCatalogo = _produtoRepository.ObterFabricantePecaPorTermo(termoBusca, tamanhoPagina, numeroPagina).ToList();
        int itemJaAdicionadoCatalogo = 0;
        if (numeroPagina == 1)
        {
            foreach (string item2 in dataFabricanteCatalogo)
            {
                data.Add(dataFabricanteCatalogo[itemJaAdicionadoCatalogo]);
                itemJaAdicionadoCatalogo++;
            }
        }
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterFilialDellavia(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _lojaDellaViaRepository.TamanhoLojasPorTermo(termoBusca);
        List<LojaDellaViaDto> data = _lojaDellaViaRepository.ObterLojasPorNome(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult()
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }
    #endregion
    #endregion

    #region [ Negociação ]
    public ActionResult Negociacao(long id)
    {
        try
        {
            OrcamentoDto orc = _orcamentoRepository.ObterOrcamentoPorId(Convert.ToInt64(id));
            //_impostosServico.CalcularImpostos(Convert.ToInt64(id));
            return View("Negociacao", orc);
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ActionResult AbrirModalDesconto(long idOrcamento, long idOrcamentoItem)
    {
        try
        {
            AplicarDescontoDto descontos = _orcamentoRepository.ObterItemOrcamentoDesconto(idOrcamento, idOrcamentoItem);
            if (descontos.PercentualDescontoAlcadaGerente == 0 && descontos.PercentualLimiteDesconto == 0)
                throw new NegocioException("Item do orçamento não possui regras de descontos!");
            return PartialView("_ModalDesconto", descontos);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult AbrirModalParcelamento(long idPagamento)
    {
        try
        {
            ParcelamentoDto parcelamento = _orcamentoFormaPagamentoRepository.ObterParcelamento(idPagamento);
            return PartialView("_ModalParcelamento", parcelamento);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult AbrirModalEquipeMontagem(long idOrcamento, long idOrcamentoItem)
    {
        try
        {
            EquipeMontagemDto equipeMontagem = _orcamentoItemRepository.ObterEquipeMontagemDto(idOrcamentoItem);
            equipeMontagem.IdOrcamentoItem = idOrcamentoItem;
            return PartialView("_ModalEquipeMontagem", equipeMontagem);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult MontarEquipeMontagem(EquipeMontagemDto equipe)
    {
        try
        {
            if (equipe.Equipe.Count >= 1 && equipe.Equipe[0].Funcao != (ProfissionalMontagemFuncao)0 && !string.IsNullOrEmpty(equipe.Equipe[0].ProfissionalNome))
            {
                if (equipe.Equipe.Any(a => string.IsNullOrEmpty(a.ProfissionalNome)))
                    throw new NegocioException("Informe o profissional!");
                if (equipe.Equipe.Any(a => a.Funcao == (ProfissionalMontagemFuncao)0))
                    throw new NegocioException("Informe a função!");
                var lastItem = equipe.Equipe.Last();
                equipe.Equipe.Remove(lastItem);
                if (equipe.Equipe.Any((a => a.ProfissionalNome == lastItem.ProfissionalNome && a.Funcao == lastItem.Funcao)))
                    throw new NegocioException("Esse profissional já está na equipe de montagem com a mesma função!");
                equipe.Equipe.Add(lastItem);
            }
            _orcamentoItemRepository.InserirEquipeMontagem(equipe, equipe.IdOrcamentoItem);
            return Json("ok");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult AdicionarProfissionalMontagem(EquipeMontagemDto equipe)
    {
        try
        {
            if (equipe.Equipe.Any(a => string.IsNullOrEmpty(a.ProfissionalNome)))
                throw new NegocioException("Informe o profissional!");
            if (equipe.Equipe.Any(a => a.Funcao == (ProfissionalMontagemFuncao)0))
                throw new NegocioException("Informe a função!");
            var lastItem = equipe.Equipe.Last();
            equipe.Equipe.Remove(lastItem);
            if (equipe.Equipe.Any((a => a.ProfissionalNome == lastItem.ProfissionalNome && a.Funcao == lastItem.Funcao)))
                throw new NegocioException("Profissional já está na equipe de montagem com a mesma função!");
            equipe.Equipe.Add(lastItem);
            ModelState.Clear();
            equipe.Equipe.Add(new ProfissionalMontagemDto());
            return PartialView("_ModalEquipeMontagem", equipe);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            base.Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult RemoverProfissionalMontagem(EquipeMontagemDto equipe)
    {
        try
        {
            ModelState.Clear();
            equipe.Equipe.RemoveAt(equipe.IndexRemover);
            if (!equipe.Equipe.Any())
                equipe.Equipe.Add(new ProfissionalMontagemDto());
            return PartialView("_ModalEquipeMontagem", equipe);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult AdicionaPagamento(FormaPagamentoDto forma)
    {
        try
        {
            if (forma.Valor == 0)
                throw new NegocioException("Informe o valor!");
            Orcamento orcamento = _orcamentoRepository.GetOrcamentoById(forma.IdOrcamento);
            orcamento.OrcamentoItens = _orcamentoItemRepository.GetOrcamentoItensByIdOrcamento(forma.IdOrcamento);
            _orcamentoFormaPagamentoRepository.InserirFormaPagamento(forma, orcamento);
            return Json("ok");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult RemovePagamento(int idPagamento)
    {
        try
        {
            long idOrcamento = _orcamentoFormaPagamentoRepository.RemoverFormaPagamento(idPagamento);
            return Json("ok");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    /*[HttpPost] public ActionResult CalculaDescontoModeloVenda(string selecionado, long idItemOrcamento, long orcamentoId, long qtdeItem)
    {
        try
        {
            AplicarDescontoDto aplicarOrcamentoDto = _orcamentoRepository.ObterItemOrcamentoDesconto(orcamentoId, idItemOrcamento);
            long qtde = qtdeItem;
            decimal? desconto = 0m;
            if (!string.IsNullOrEmpty(selecionado))
            {
                qtde = Convert.ToInt32(selecionado.Replace("Quantidade", ""));
                long num = qtde - 1;
                if ((ulong)num <= 3uL)
                {
                    switch (num)
                    {
                        case 0L:
                            desconto = aplicarOrcamentoDto.DescontoModeloVendaQuantidade1;
                            break;
                        case 1L:
                            desconto = aplicarOrcamentoDto.DescontoModeloVendaQuantidade2;
                            break;
                        case 2L:
                            desconto = aplicarOrcamentoDto.DescontoModeloVendaQuantidade3;
                            break;
                        case 3L:
                            desconto = aplicarOrcamentoDto.DescontoModeloVendaQuantidade4;
                            break;
                    }
                }
            }
            if (qtde < qtdeItem)
                qtde = qtdeItem;
            decimal valorTotal = aplicarOrcamentoDto.PrecoUnitario * (decimal)qtdeItem; //qtde;
            decimal percentualDesconto = Convert.ToDecimal(desconto);
            decimal valorDesconto = percentualDesconto / 100m * valorTotal;
            decimal totalComDesconto = valorTotal - valorDesconto;
            //string retornoDesconto = $"{valorDesconto:C}";
            //string retornoTotalComDesconto = $"{totalComDesconto:C}";
            return new JsonResult
            {
                Data = new
                {
                    valorOriginal = $"{valorTotal:C}",
                    percentualDesconto,
                    valorDesconto = $"{valorDesconto:C}",
                    totalComDesconto = $"{totalComDesconto:C}",
                    qtdeModeloVenda = qtdeItem //qtde
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }
    */

    [HttpPost]
    public ActionResult CalcularAjuste(string tipo, string valor, string total)
    {
        try
        {
            decimal valorTotal = decimal.Parse(string.IsNullOrEmpty(total) ? "0" : total, NumberStyles.Currency);
            decimal ajuste = decimal.Parse(string.IsNullOrEmpty(valor) ? "0" : valor, NumberStyles.Currency);
            decimal percentual;
            decimal valorAjuste;
            decimal totalAjuste;
            switch (tipo)
            {
                case "acrescimo":
                case "desconto":
                    percentual = ajuste;
                    valorAjuste = decimal.Round(ajuste / 100m * valorTotal, 2);
                    totalAjuste = tipo == "desconto" ? valorTotal - valorAjuste : valorTotal + valorAjuste;
                    break;
                case "valor":
                default:
                    percentual = decimal.Round(ajuste / valorTotal * 100m, 4);
                    valorAjuste = ajuste;
                    totalAjuste = valorTotal - ajuste;
                    break;
            }
            return new JsonResult
            {
                Data = new
                {
                    percentual = percentual.ToString(),
                    valor = $"{valorAjuste:C}",
                    total = $"{totalAjuste:C}",
                    tipo
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult AplicarDesconto(AplicarDescontoDto model)
    {
        try
        {
            ModelState.Clear();
            _orcamentoRepository.AplicarDescontoItensOrcamento(model);
            return Json("ok");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult RemoverItemCarrinhoRefresh(long idOrcamento, long idItemOrcamento)
    {
        try
        {
            _orcamentoRepository.RemoverItensOrcamento(idOrcamento, idItemOrcamento);
            OrcamentoDto orc = _orcamentoRepository.ObterOrcamentoPorId(idOrcamento);
            ViewData["readonly"] = orc.StatusSomenteLeitura;
            return PartialView("_TabelaOrcamentoProduto", orc);
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    public ActionResult ObterSaldoPagamento(long idOrcamento)
    {
        try
        {
            OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(idOrcamento);
            return new JsonResult
            {
                Data = new
                {
                    subtotal = string.Format("<b>{0}</b>", $"{orcamento.OrcamentoProduto.Total:C}", (orcamento.ValorImpostos > 0m) ? $"<small>+{$"{orcamento.ValorImpostos:C}"} Imp.</small>" : string.Empty),
                    restante = $"{orcamento.FormaPagamento.ValorRestante:C}"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }

    #region [ Action listas ]
    [HttpGet]
    public ActionResult ObterProfissionalMontagem(string termoBusca, int tamanhoPagina, int numeroPagina, string idFilial)
    {
        int count = _equipeMontagemRepository.TamanhoProfissionalMontagemPorTermo(termoBusca, idFilial);
        List<ProfissionalMontagemDto> data = _equipeMontagemRepository.ObterProfissionalMontagemPorNome(termoBusca, tamanhoPagina, numeroPagina, idFilial);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterCondicoesPagamento(string termoBusca, int tamanhoPagina, int numeroPagina, string areaNegocio)
    {
        int count = _condicaoPagamentoRepository.TamanhoCondicoesPagamentoPorTermo(termoBusca, areaNegocio);
        List<FormaPagamentoDto> data = _condicaoPagamentoRepository.ObterCondicoesPagamentoPorNome(termoBusca, areaNegocio, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterBandeira(int tamanhoPagina, int numeroPagina, string termoBusca, string idCondPagto)
    {
        int count = _orcamentoFormaPagamentoRepository.TamanhoAdmFinanceiraPorTermo(termoBusca, idCondPagto);
        List<AdministradoraFinanceiraDto> data = _orcamentoFormaPagamentoRepository.ObterAdmFinanceiraPorNome(tamanhoPagina, numeroPagina, termoBusca, idCondPagto);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }

    [HttpGet]
    public ActionResult ObterBanco(string termoBusca, int tamanhoPagina, int numeroPagina)
    {
        int count = _orcamentoItemServico.TamanhoBancoPorTermo(termoBusca);
        List<BancoDto> data = _orcamentoItemServico.ObterBancoPorNome(termoBusca, tamanhoPagina, numeroPagina);
        return new JsonResult
        {
            Data = new { data, count },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        };
    }
    #endregion
    #endregion

    #region [ Finalização ]
    public ActionResult AtualizaOrcamento(OrcamentoDto model)
    {
        try
        {
            _orcamentoRepository.AtualizarReservaEstoque(model.Id, model.ReservaEstoque);
            return RedirectToAction("Finalizacao", new { id = model.Id });
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ActionResult Finalizacao(long id)
    {
        try
        {
            OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(id);
            return View(orcamento);
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public ActionResult EnviarProtheus(long id)
    {
        try
        {
            Task<bool> enviar = _orcamentoRepository.EnviarOrcamentoProtheus(id);
            if (enviar.Exception != null)
                throw enviar.Exception.GetBaseException();
            TempData["Envio"] = "Orçamento enviado com sucesso!";
            return Json("ok");
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }
    #endregion

    #region [ Relatório ]
    public ActionResult Relatorio(long id)
    {
        OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(id);
        return View(orcamento);
    }

    public async Task<ViewResult> RelatorioProtheus(string codOrcamento)
    {
        return View("Relatorio", await _orcamentoServico.ObterOrcamentoRelatorioProtheus(codOrcamento));
    }

    [HttpPost]
    public JsonResult EnviarPorEmail(string orcamentoId, string orcamento, string emailCliente, bool orcamentoAnexado)
    {
        try
        {
            StringBuilder html = new StringBuilder();
            Attachment anexo = null;
            if (orcamentoAnexado)
            {
                byte[] pdfBinary = Convert.FromBase64String(orcamento);
                Stream stream = new MemoryStream(pdfBinary);
                anexo = new Attachment(stream, "Orçamento-" + orcamentoId + "-DELLAVIA.pdf");
                html.AppendFormat("<p>Ol&aacute;,</p>\r\n " +
                                  "<p>O orçamento " + orcamentoId + " realizado na DELLA VIA se encontra no anexo deste e-mail.</p>\r\n " +
                                  "<p>Atenciosamente,</p>\r\n " +
                                  "<p>DELLA VIA</p> ");
            }
            else
            {
                html.AppendFormat("<html><body><p>Ol&aacute;,</p>\r\n " +
                                  "<p>Segue abaixo o orçamento " + orcamentoId + " realizado na DELLA VIA.</p>\r\n " +
                                  orcamento + "\r\n" +
                                  "<p>Atenciosamente,</p>\r\n " +
                                  "<p>DELLA VIA</p> </body></html>");
            }
            EnvioEmail.EnviarEmail(emailCliente, "Confira o orçamento " + orcamentoId + " realizado na DELLA VIA", html.ToString(), anexo);
            return new JsonResult
            {
                Data = "sucesso",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        catch (NegocioException ex)
        {
            Response.StatusCode = 500;
            return Json("Não enviado: " + ex.Message);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            return Json(ex.Message);
        }
    }
    #endregion

    #region [ Wizard ]
    public ActionResult RedirectWizard(string action, long id)
    {
        try
        {
            return RedirectToAction(action, new { id });
        }
        catch (NegocioException e)
        {
            TempData["Erro"] = e.Message;
            return View("Index");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}
