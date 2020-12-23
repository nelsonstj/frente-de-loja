using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Contratos.Validator;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Infra.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace DV.FrenteLoja.Core.Servicos
{
    public class OrcamentoServico : IOrcamentoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IOrcamentoItemServico _orcamentoItemServico;
        private readonly IOrcamentoApi _orcamentoApi;
        private readonly ClienteValidator _clienteValidator;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly OrcamentoValidator _orcamentoValidator;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
        private readonly IRepositorio<Veiculo> _veiculoRepositorio;
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        //private readonly IRepositorio<Convenio> _convenioRepositorio;
        private readonly IRepositorio<LojaDellaVia> _lojaDellaViaRepositorio;
        private readonly IRepositorio<TipoVenda> _tipoVendaRepositorio;
        private readonly IRepositorio<TabelaPreco> _tabelaPrecoRepositorio;
        private readonly IRepositorio<Operador> _operadorRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;
        private readonly IRepositorio<ClienteVeiculo> _clienteVeiculoRepositorio;
        private readonly IRepositorio<OrcamentoItem> _orcamentoItemRepositorio;
        private readonly IRepositorio<Transportadora> _transportadoraRepositorio;
        private readonly IRepositorio<SolicitacaoDescontoVendaAlcada> _solicitacaoDescontoVendaAlcadaRepositorio;
        private readonly IRepositorio<SolicitacaoAnaliseCredito> _solicitacaoAnaliseCreditoRepositorio;
        private readonly IRepositorio<OrcamentoObservacao> _orcamentoObservacaoRepositorio;
        private readonly IRepositorio<Marca> _marcaRepositorio;
        private readonly IRepositorio<MarcaModelo> _marcaModeloRepositorio;
        private readonly IImpostosServico _impostosServico;

        public OrcamentoServico(IRepositorioEscopo escopo, IOrcamentoItemServico orcamentoItemServico, IOrcamentoApi orcamentoApi, IImpostosServico impostosServico)
        {
            _escopo = escopo;
            _orcamentoItemServico = orcamentoItemServico;
            _orcamentoApi = orcamentoApi;
            _impostosServico = impostosServico;
            _orcamentoValidator = new OrcamentoValidator();
            _orcamentoRepositorio = _escopo.GetRepositorio<Orcamento>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _veiculoRepositorio = escopo.GetRepositorio<Veiculo>();
            _clienteRepositorio = escopo.GetRepositorio<Cliente>();
            //_convenioRepositorio = escopo.GetRepositorio<Convenio>();
            _lojaDellaViaRepositorio = escopo.GetRepositorio<LojaDellaVia>();
            _tipoVendaRepositorio = escopo.GetRepositorio<TipoVenda>();
            _tabelaPrecoRepositorio = escopo.GetRepositorio<TabelaPreco>();
            _operadorRepositorio = escopo.GetRepositorio<Operador>();
            _vendedorRepositorio = escopo.GetRepositorio<Vendedor>();
            _clienteVeiculoRepositorio = escopo.GetRepositorio<ClienteVeiculo>();
            _orcamentoItemRepositorio = escopo.GetRepositorio<OrcamentoItem>();
            _transportadoraRepositorio = escopo.GetRepositorio<Transportadora>();
            _solicitacaoDescontoVendaAlcadaRepositorio = escopo.GetRepositorio<SolicitacaoDescontoVendaAlcada>();
            _solicitacaoAnaliseCreditoRepositorio = escopo.GetRepositorio<SolicitacaoAnaliseCredito>();
            _orcamentoObservacaoRepositorio = escopo.GetRepositorio<OrcamentoObservacao>();
            _marcaRepositorio = escopo.GetRepositorio<Marca>();
            _marcaModeloRepositorio = escopo.GetRepositorio<MarcaModelo>();
            _clienteValidator = new ClienteValidator();
        }

        private OrcamentoProdutoBuscaDto ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(long idOrcamento)
        {
            var orcamentoItens = _orcamentoItemRepositorio.Get(x => x.OrcamentoId == idOrcamento).ToList();

            var orcamentoItensSomentePais = orcamentoItens.Where(x => x.NrItemProdutoPaiId == null);

            var produtoBuscaDto = new OrcamentoProdutoBuscaDto { IdOrcamento = idOrcamento };

            decimal total = 0;

            foreach (var orcamenteItenPai in orcamentoItensSomentePais)
            {
                var produtoSacola = new SacolaProdutoDto
                {
                    IdOrcamentoItem = orcamenteItenPai.Id,
                    Descricao = orcamenteItenPai.Produto.Descricao,
                    Quantidade = (int)orcamenteItenPai.Quantidade,
                    Valor = orcamenteItenPai.PrecoUnitario,
                    ValorDesconto = orcamenteItenPai.ValorDesconto,
                    CampoCodigoProduto = orcamenteItenPai.Produto.CampoCodigo,
                    NumeroItem = orcamenteItenPai.NrItem,
                    PercentualDesconto = orcamenteItenPai.PercDescon,
                    ValorTotal = (orcamenteItenPai.Quantidade * orcamenteItenPai.PrecoUnitario) - orcamenteItenPai.ValorDesconto
                };

                produtoSacola.SolicitacoesDescontoAlcada = Mapper.Map<List<SolicitacaoDescontoVendaAlcadaDto>>(_solicitacaoDescontoVendaAlcadaRepositorio.Get(a => a.IdOrcamentoItem == orcamenteItenPai.Id).ToList());

                total += (produtoSacola.Quantidade * produtoSacola.Valor) - produtoSacola.ValorDesconto;

                // Varredura nos orcamento itens filhos (Servicos Agregados)
                foreach (var orcamentoItem in orcamentoItens.Where(x => x.NrItemProdutoPaiId == orcamenteItenPai.NrItem))
                {
                    var produtoSacolaFilho = new ServicoCorrelacionadoDto
                    {
                        IdOrcamentoItem = orcamentoItem.Id,
                        Descricao = orcamentoItem.Produto.Descricao,
                        Quantidade = (int)orcamentoItem.Quantidade,
                        Valor = orcamentoItem.PrecoUnitario,
                        ValorDesconto = orcamentoItem.ValorDesconto,
                        CampoCodigoProduto = orcamentoItem.Produto.CampoCodigo,
                        NumeroItem = orcamentoItem.NrItem,
                        PercentualDesconto = orcamentoItem.PercDescon,
                        ProfissionaisMontagem = Mapper.Map<List<ProfissionalMontagemDto>>(orcamentoItem.EquipeMontagemList),
                        ValorTotal = ((int)orcamentoItem.Quantidade * orcamentoItem.PrecoUnitario) - orcamentoItem.ValorDesconto
                    };

                    total += (produtoSacolaFilho.Quantidade * produtoSacolaFilho.Valor) - produtoSacolaFilho.ValorDesconto;
                    produtoSacola.SolicitacoesDescontoAlcada = Mapper.Map<List<SolicitacaoDescontoVendaAlcadaDto>>(_solicitacaoDescontoVendaAlcadaRepositorio.Get(a => a.IdOrcamentoItem == orcamentoItem.Id).ToList());
                    produtoSacola.Servicos.Add(produtoSacolaFilho);
                }
                produtoBuscaDto.Produtos.Add(produtoSacola);
            }
            produtoBuscaDto.Total = total;
            return produtoBuscaDto;
        }

        public void AtualizarReservaEstoque(long id, bool reservaEstoque)
        {
            var orcamento = _orcamentoRepositorio.GetSingle(a => a.Id == id);
            orcamento.PossuiReservaEstoque = reservaEstoque;
            _escopo.Finalizar();
        }

/*     public long IniciarOrcamento(OrcamentoDto novoOrcamentoDto)
        {

            var cliente = _clienteRepositorio.GetSingle(a => a.Id == novoOrcamentoDto.IdCliente);

            if (cliente == null)
            {
                throw new NegocioException("Cliente não encontrado");
            }

            var convenio = _convenioRepositorio.GetSingle(a => a.Id == novoOrcamentoDto.Convenio);

            if (convenio == null)
            {
                throw new NegocioException("Convênio não encontrado");
            }

            long? lojaOrcamento;

            if (novoOrcamentoDto.TipoOrcamento == TipoOrcamento.Telemarketing)
            {
                lojaOrcamento = novoOrcamentoDto.LojaDestino;
            }
            else
            {
                lojaOrcamento = HttpContext.Current.User.Identity.GetLojaPadrao().First();
            }

            var lojaDellaVia = _lojaDellaViaRepositorio.GetSingle(a => a.Id == lojaOrcamento);

            if (lojaDellaVia == null)
            {
                throw new NegocioException("Loja não encontrada");
            }

            var marcaModeloVersao = _marcaModeloVersaoRepositorio.GetSingle(a => a.Id == novoOrcamentoDto.VersaoVeiculo);

            var veiculo = _veiculoRepositorio.GetSingle(a => a.IdMarcaModeloVersao == marcaModeloVersao.Id);

            if (marcaModeloVersao == null || veiculo == null)
            {
                throw new NegocioException("Veículo não encontrado");
            }

            var transportadora = _transportadoraRepositorio.GetSingle(x => x.Id == novoOrcamentoDto.Transportadora);

            var idOperador = HttpContext.Current.User.Identity.GetIdOperador();
            var operador = _operadorRepositorio.GetSingle(a => a.CampoCodigo == idOperador);

            if (operador == null)
            {
                throw new NegocioException("Operador não encontrado");
            }

            var tabelaPreco = _tabelaPrecoRepositorio.GetSingle(a => a.Id == novoOrcamentoDto.TabelaPreco);

            if (tabelaPreco == null)
            {
                throw new NegocioException("Tabela de Preço não encontrada");
            }

            var tipoVenda = _tipoVendaRepositorio.GetSingle(a => a.Id == novoOrcamentoDto.TipoVenda);

            if (tipoVenda == null)
            {
                throw new NegocioException("Tipo de Venda não encontrada");
            }

            var vendedor = _vendedorRepositorio.GetSingle(a => a.Id == novoOrcamentoDto.Vendedor);

            if (vendedor == null)
            {
                throw new NegocioException("Vendedor não encontrado");
            }


            Orcamento orcamento;

            if (novoOrcamentoDto.Id > 0)
            {
                orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == novoOrcamentoDto.Id);

                if (orcamento == null)
                    throw new NegocioException("Orçamento não encontrado.");


                // Chamar rotina de calcular impostos quando houver mudança de cliente e/ou loja destino.
                if (cliente.Id != orcamento.Cliente.Id || lojaDellaVia.Id != orcamento.LojaDellaVia.Id)
                    _impostosServico.CalcularImpostos(orcamento);

                orcamento.Ano = novoOrcamentoDto.AnoVeiculo;
                orcamento.Complemento = novoOrcamentoDto.Observacao;
                orcamento.DataAtualizacao = DateTime.Now;
                orcamento.DataValidade = novoOrcamentoDto.DataValidade;
                orcamento.IdCliente = cliente.Id;
                orcamento.IdConvenio = convenio.Id;
                orcamento.IdLojaDellaVia = lojaDellaVia.Id;
                //orcamento.Veiculo.IdMarcaModeloVersao = marcaModeloVersao.Id;
                orcamento.IdOperador = operador.Id;
                orcamento.IdTabelaPreco = tabelaPreco.Id;
                orcamento.IdTipoVenda = tipoVenda.Id;
                orcamento.IdVendedor = vendedor.Id;
                orcamento.IdTransportadora = transportadora?.Id;
                orcamento.KM = novoOrcamentoDto.QuilometragemVeiculo;
                orcamento.Placa = string.IsNullOrEmpty(novoOrcamentoDto.PlacaVeiculo) ? string.Empty : Regex.Replace(novoOrcamentoDto.PlacaVeiculo, "-", "");
                orcamento.Telefone = novoOrcamentoDto.TelefoneCliente;
                orcamento.TelefoneCelular = novoOrcamentoDto.CelularCliente;
                orcamento.TelefoneComercial = novoOrcamentoDto.TelefoneComercialCliente;
                orcamento.InformacoesCliente = novoOrcamentoDto.InformacoesCliente;
                orcamento.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
                orcamento.MensagemNF = novoOrcamentoDto.MensagemNF;
                orcamento.Xped = novoOrcamentoDto.Xped;
            }
            else
            {

                orcamento = new Orcamento
                {
                    Ano = novoOrcamentoDto.AnoVeiculo,
                    Complemento = novoOrcamentoDto.Observacao,
                    DataAtualizacao = DateTime.Now,
                    DataValidade = novoOrcamentoDto.DataValidade,
                    IdCliente = cliente.Id,
                    IdConvenio = convenio.Id,
                    IdLojaDellaVia = lojaDellaVia.Id,
                    VeiculoIdFraga = veiculo.IdFraga,
                    //IdMarcaModeloVersao = marcaModeloVersao.Id,
                    IdOperador = operador.Id,
                    IdTabelaPreco = tabelaPreco.Id,
                    IdTipoVenda = tipoVenda.Id,
                    IdVendedor = vendedor.Id,
                    IdTransportadora = transportadora?.Id,
                    KM = novoOrcamentoDto.QuilometragemVeiculo,
                    Placa = string.IsNullOrEmpty(novoOrcamentoDto.PlacaVeiculo) ? string.Empty : Regex.Replace(novoOrcamentoDto.PlacaVeiculo, "-", ""),
                    Telefone = novoOrcamentoDto.TelefoneCliente,
                    TelefoneCelular = novoOrcamentoDto.CelularCliente,
                    TelefoneComercial = novoOrcamentoDto.TelefoneComercialCliente,
                    InformacoesCliente = novoOrcamentoDto.InformacoesCliente,
                    UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                    MensagemNF = novoOrcamentoDto.MensagemNF,
                    Xped = novoOrcamentoDto.Xped,
                    DataCriacao = DateTime.Now,
                    StatusOrcamento = StatusOrcamento.EmAberto
                };

                novoOrcamentoDto.Id = _orcamentoRepositorio.Add(orcamento).Id;
            }

            // Validação do orçamento, cliente.
            var clienteDto = Mapper.Map<ClienteDto>(cliente);
            var resultado = _clienteValidator.Validate(clienteDto);
            if (!resultado.IsValid)
            {
                throw new NegocioException(string.Empty, resultado.Errors);
            }

            if (!_clienteVeiculoRepositorio.Any(a => a.ClienteId == orcamento.IdCliente && a.VeiculoIdFraga == orcamento.VeiculoIdFraga))
            {
                _clienteVeiculoRepositorio.Add(new ClienteVeiculo
                {
                    ClienteId = orcamento.IdCliente,
                    DataAtualizacao = DateTime.Now,
                    VeiculoIdFraga = orcamento.VeiculoIdFraga,
                    Placa = orcamento.Placa,
                    Ano = orcamento.Ano
                });
            }

            novoOrcamentoDto.MarcaVeiculo = marcaModeloVersao.MarcaModelo.Marca.Id;
            novoOrcamentoDto.MarcaVeiculoDescricao = marcaModeloVersao.MarcaModelo.Marca.Descricao;
            novoOrcamentoDto.ModeloVeiculo = marcaModeloVersao.MarcaModelo.Id;
            novoOrcamentoDto.ModeloVeiculoDescricao = marcaModeloVersao.MarcaModelo.Descricao;
            novoOrcamentoDto.VersaoVeiculo = marcaModeloVersao.Id;
            novoOrcamentoDto.VersaoVeiculoDescricao = marcaModeloVersao.Descricao;
            novoOrcamentoDto.VersaoMotor = veiculo.IdVersaoMotor;
            novoOrcamentoDto.VersaoMotorDescricao = veiculo.VersaoMotor.Descricao;

            _escopo.Finalizar();

            return orcamento.Id;

        }
*/
/*      public void AtualizarDadosVeiculo(OrcamentoDto orcamentoDto)
        {

            var orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == orcamentoDto.Id);

            if (orcamento == null)
                throw new NegocioException("Orçamento não encontrado.");

            orcamento.TipoOrcamento = orcamentoDto.TipoOrcamento;
            orcamento.Placa = Regex.Replace(orcamentoDto.PlacaVeiculo, "-", "");
            //orcamento.IdMarcaModeloVersao = orcamentoDto.Id;
            orcamento.Ano = orcamentoDto.AnoVeiculo;
            orcamento.KM = orcamentoDto.QuilometragemVeiculo;
            orcamento.IdCliente = orcamentoDto.IdCliente;
            orcamento.Telefone = orcamentoDto.TelefoneCliente;
            orcamento.TelefoneCelular = orcamentoDto.CelularCliente;
            orcamento.TelefoneComercial = orcamentoDto.TelefoneComercialCliente;
            orcamento.InformacoesCliente = orcamentoDto.InformacoesCliente;

            orcamento.DataAtualizacao = DateTime.Now;
            orcamento.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
            _escopo.Finalizar();
        }
*/
/*       public OrcamentoDto ObterOrcamentoPorId(long idOrcamento)
        {
            var orcamentoDto = new OrcamentoDto();
            var orc = _orcamentoRepositorio.GetSingle(x => !x.RegistroInativo && x.Id == idOrcamento);
            if (orc == null)
            {
                throw new NegocioException($"Orçamento {idOrcamento} não encontrado.");
            }
            if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
            {
                var usuario = HttpContext.Current.User.Identity.Name.ToUpper();
                if (orc.UsuarioAtualizacao != usuario)
                    throw new NegocioException($"Usuário não tem permissão para acessar o orçamento.");
            }
            else
            {
                var lojasUsuario = HttpContext.Current.User.Identity.GetLojaPadrao();
                if (!lojasUsuario.Contains(orc.LojaDellaVia.Id))
                    throw new NegocioException($"Filial não tem permissão para acessar o orçamento.");
            }

            #region Inicio
            orcamentoDto.Id = idOrcamento;
            orcamentoDto.PlacaVeiculo = orc.Placa;
            //orcamentoDto.MarcaVeiculo = orc.MarcaModeloVersao.MarcaModelo.Marca.Id;
            //orcamentoDto.MarcaVeiculoDescricao = orc.MarcaModeloVersao.MarcaModelo.Marca.Descricao;
            //orcamentoDto.ModeloVeiculo = orc.MarcaModeloVersao.MarcaModelo.Id;
            //orcamentoDto.ModeloVeiculoDescricao = orc.MarcaModeloVersao.MarcaModelo.Descricao;
            //orcamentoDto.VersaoVeiculo = orc.MarcaModeloVersao.Id;
            //orcamentoDto.VersaoVeiculoDescricao = orc.MarcaModeloVersao.Descricao;
            //orcamentoDto.VersaoMotor = orc.Veiculo.IdVersaoMotor;
            //orcamentoDto.VersaoMotorDescricao = orc.Veiculo.VersaoMotor.Descricao;
            orcamentoDto.AnoVeiculo = orc.Ano;
            orcamentoDto.QuilometragemVeiculo = orc.KM;
            //orcamentoDto.IdCliente = orc.Cliente.Id;
            orcamentoDto.CodigoCliente = orc.Cliente.CampoCodigo + "-" + orc.Cliente.Loja;
            orcamentoDto.CPFCNPJCliente = orc.Cliente.CNPJCPF;
            orcamentoDto.NomeCliente = orc.Cliente.Nome;
            orcamentoDto.ClassificacaoCliente = orc.Cliente.ClassificacaoCliente;
            orcamentoDto.ScoreCliente = orc.Cliente.Score;
            orcamentoDto.EmailCliente = orc.Cliente.Email;
            orcamentoDto.CelularCliente = orc.TelefoneCelular;
            orcamentoDto.TelefoneComercialCliente = orc.TelefoneComercial;
            orcamentoDto.TelefoneCliente = orc.Telefone;
            orcamentoDto.InformacoesCliente = orc.InformacoesCliente;
            orcamentoDto.ConvenioDescricao = orc.Convenio.Descricao;
            orcamentoDto.TrocaTabelaPreco = orc.Convenio.TrocaTabelaPreco;
            orcamentoDto.TrocaPrecoConvenio = orc.Convenio.TrocaPreco;
            orcamentoDto.CampoCodigo = orc.CampoCodigo;
            orcamentoDto.LojaDestinoCampoCodigo = orc.LojaDellaVia.CampoCodigo;
            #endregion

            #region Complemento

            orcamentoDto.Vendedor = orc.Vendedor.Id;
            orcamentoDto.TipoVenda = orc.TipoVenda.Id;
            orcamentoDto.TabelaPreco = orc.TabelaPreco.Id;
            orcamentoDto.Convenio = orc.Convenio.Id;
            orcamentoDto.LojaDestino = orc.LojaDellaVia.Id;
            orcamentoDto.VendedorDescricao = orc.Vendedor.Nome;
            orcamentoDto.TipoVendaDescricao = orc.TipoVenda.Descricao;
            orcamentoDto.TabelaPrecoDescricao = orc.TabelaPreco.CampoCodigo;
            orcamentoDto.ConvenioDescricao = $"{orc.Convenio.CampoCodigo} - {orc.Convenio.Descricao}";
            orcamentoDto.LojaDestinoDescricao = orc.LojaDellaVia.Descricao;
            orcamentoDto.Observacao = orc.Complemento;
            orcamentoDto.InformacaoConvenio = orc.Convenio.Observacoes;
            orcamentoDto.StatusCliente = orc.Cliente.StatusCliente;
            orcamentoDto.DataValidade = orc.DataValidade;
            orcamentoDto.Transportadora = orc.IdTransportadora;
            orcamentoDto.Xped = orc.Xped;
            orcamentoDto.MensagemNF = orc.MensagemNF;
            orcamentoDto.DataCriacao = orc.DataCriacao;
            orcamentoDto.StatusSomenteLeitura = !((orc.StatusOrcamento == StatusOrcamento.EmAberto || orc.StatusOrcamento == StatusOrcamento.Reserva) && orc.DataValidade >= DateTime.Today);
            #endregion

            #region Busca Produto

            orcamentoDto.CampoCodigo = orc.CampoCodigo;
            orcamentoDto.OrcamentoProduto.IdOrcamento = orc.Id;

            #endregion

            #region Itens Orcamento

            orcamentoDto.OrcamentoProduto = ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(orc.Id);
            #endregion

            #region Negociação

            orcamentoDto.ReservaEstoque = orc.PossuiReservaEstoque;
            orcamentoDto.FormaPagamento = _orcamentoItemServico.ObterOrcamentoPagamentoDto(orc.Id);
            orcamentoDto.ValorImpostos = orc.ValorImpostos;
            #endregion

            #region Finalização

            orcamentoDto.SolicitacoesAnaliseCredito = Mapper.Map<List<SolicitacaoAnaliseCreditoDto>>(_solicitacaoAnaliseCreditoRepositorio.Get(x => x.IdOrcamento == orc.Id));
            #endregion

            #region Relatório

            orcamentoDto.ObservacoesRelatorio = new ObservacaoDto();

            foreach (var obs in _orcamentoObservacaoRepositorio.Get(a => !a.RegistroInativo && a.TipoObservacao == TipoObservacao.Observacao))
            {
                orcamentoDto.ObservacoesRelatorio.Informacoes.Add(obs.Conteudo);
            }

            orcamentoDto.AtividadesDellaViaRelatorio = new List<ObservacaoDto>();

            var listaAtividadeDellaVia = _orcamentoObservacaoRepositorio.Get(a => !a.RegistroInativo && a.TipoObservacao == TipoObservacao.AtividadeDellaVia).ToList();

            foreach (var titulo in listaAtividadeDellaVia.GroupBy(a => a.Titulo))
            {
                var titObs = new ObservacaoDto { Titulo = titulo.Key };
                foreach (var obs in listaAtividadeDellaVia.Where(a => a.Titulo == titulo.Key))
                {
                    titObs.Informacoes.Add(obs.Conteudo);
                }
                orcamentoDto.AtividadesDellaViaRelatorio.Add(titObs);
            }

            orcamentoDto.LogradouroLoja = orc.LojaDellaVia.Logradouro;
            orcamentoDto.BairroLoja = orc.LojaDellaVia.Bairro;
            orcamentoDto.CidadeLoja = orc.LojaDellaVia.Cidade;
            orcamentoDto.EstadoLoja = orc.LojaDellaVia.Estado;
            orcamentoDto.CepLoja = orc.LojaDellaVia.Cep;
            orcamentoDto.CnpjLoja = orc.LojaDellaVia.Cnpj;
            orcamentoDto.InscricaoEstadualLoja = orc.LojaDellaVia.InscricaoEstadual;
            orcamentoDto.TelefoneLoja = orc.LojaDellaVia.Telefone;
            orcamentoDto.Complemento = orc.Complemento;

            #endregion
            return orcamentoDto;
        }
*/
        public async Task<OrcamentoDto> ObterOrcamentoRelatorioProtheus(string campoCodigo)
        {
            var orc = await _orcamentoApi.ObterOrcamentoRelatorio(campoCodigo);
            var loja = _lojaDellaViaRepositorio.GetSingle(x => x.CampoCodigo == orc.LojaDestinoCampoCodigo);
            if (loja != null)
            {
                orc.LojaDestino = loja.CampoCodigo;
                orc.LogradouroLoja = loja.Logradouro;
                orc.BairroLoja = loja.Bairro;
                orc.CepLoja = loja.Cep;
                orc.CnpjLoja = loja.Cnpj;
                orc.InscricaoEstadualLoja = loja.InscricaoEstadual;
                orc.TelefoneLoja = loja.Telefone;
            }
            else
                throw new NegocioException($"Não foi possível encontrar a loja {orc.LojaDestinoCampoCodigo} no banco de dados.");

            orc.ObservacoesRelatorio = new ObservacaoDto();

            foreach (var obs in _orcamentoObservacaoRepositorio.Get(a => !a.RegistroInativo && a.TipoObservacao == TipoObservacao.Observacao))
            {
                orc.ObservacoesRelatorio.Informacoes.Add(obs.Conteudo);
            }

            orc.AtividadesDellaViaRelatorio = new List<ObservacaoDto>();

            var listaAtividadeDellaVia = _orcamentoObservacaoRepositorio.Get(a => !a.RegistroInativo && a.TipoObservacao == TipoObservacao.AtividadeDellaVia).ToList();

            foreach (var titulo in listaAtividadeDellaVia.GroupBy(a => a.Titulo))
            {
                var titObs = new ObservacaoDto { Titulo = titulo.Key };
                foreach (var obs in listaAtividadeDellaVia.Where(a => a.Titulo == titulo.Key))
                {
                    titObs.Informacoes.Add(obs.Conteudo);
                }
                orc.AtividadesDellaViaRelatorio.Add(titObs);
            }

            orc.MarcaVeiculoDescricao = _marcaRepositorio.GetSingle(x => x.CampoCodigo == orc.MarcaVeiculoDescricao)?.Descricao;
            orc.ModeloVeiculoDescricao = _marcaModeloRepositorio.GetSingle(x => x.CampoCodigo == orc.ModeloVeiculoDescricao)?.Descricao;

            return orc;
        }

        /// <summary>
        /// Atualiza as informações do orçamento frente de loja com as informações contidas no protheus.
        /// </summary>
        /// <param name="orcDto"></param>
        /// <returns></returns>
        public async Task<string> AtualizarOrcamentoComProtheus(OrcamentoDto orcDto)
        {
            if (orcDto.CampoCodigo.IsNullOrEmpty())
                return string.Empty;

            var orcamentoDto = await _orcamentoApi.ObterOrcamentoPorCodProtheus(orcDto);
            var resultado = _orcamentoApi.AtualizarOrcamento(orcamentoDto, orcDto.Id);
            return resultado;
        }

        public string ValidaBuscaOrcamentos(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento)
        {
            var loginUsuario = HttpContext.Current.User.Identity.Name.ToUpper();

            try
            {
                switch (tipoFiltro)
                {
                    case TipoFiltroOrcamento.IdOrcamento:
                        {
                            long idOrcamento = 0;
                            Int64.TryParse(termoBusca, out idOrcamento);

                            if (idOrcamento == 0)
                                throw new NegocioException("Informe o ID do Orçamento.");
                        }
                        break;
                    case TipoFiltroOrcamento.NumeroProtheus:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe o número Protheus - Loja. Formato esperado: 000000-00.");
                            }

                            if (!termoBusca.Contains("-"))
                            {
                                termoBusca += "-" + HttpContext.Current.User.Identity.GetLojaPadraoCampoCodigo().Split(',').FirstOrDefault();
                            }

                            if (termoBusca.Length != 9)
                            {
                                throw new NegocioException("O filtro número Protheus - Loja está em um formato incorreto. Formato esperado: 000000-00 ou 000000.");
                            }
                        }
                        break;
                    case TipoFiltroOrcamento.Placa:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe a placa.");
                            }
                            else
                            {
                                termoBusca = termoBusca.Replace("-", "").ToUpper();
                            }
                        }
                        break;
                    case TipoFiltroOrcamento.CPFCLiente:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe o CPF do Cliente.");
                            }
                            else
                            {
                                termoBusca = termoBusca.Replace(".", "").Replace("-", "").Replace("/", "");
                            }
                        }
                        break;
                    case TipoFiltroOrcamento.CNPJCliente:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe o CNPJ do Cliente.");
                            }
                            else
                            {
                                termoBusca = termoBusca.Replace(".", "").Replace("-", "").Replace("/", "");
                            }
                        }
                        break;
                    case TipoFiltroOrcamento.CodigoCliente:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe o código do cliente - Loja. Formato esperado: 000000-00 ou 000000.");
                            }
                            if (termoBusca.Length != 9 && termoBusca.Length != 6)
                            {
                                throw new NegocioException("O filtro código do cliente - Loja está em um formato incorreto. Formato esperado: 000000-00 ou 000000.");
                            }
                        }
                        break;
                    case TipoFiltroOrcamento.Vendedor:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe o código do vendedor.");
                            }
                        }
                        break;
                    case TipoFiltroOrcamento.LojaDestino:
                        {
                            if (string.IsNullOrEmpty(termoBusca))
                            {
                                throw new NegocioException("Informe o código da Loja.");
                            }
                        }
                        break;
                    default:
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

/*        public ICollection<OrcamentoConsultaDto> ObterListaOrcamento(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento)
        {
            IQueryable<Orcamento> query;
            if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
            {
                var loginUsuario = HttpContext.Current.User.Identity.Name.ToUpper();
                query = _orcamentoRepositorio.Get(a => !a.RegistroInativo && a.UsuarioAtualizacao == loginUsuario);
            }
            else
            {
                var lojas = HttpContext.Current.User.Identity.GetLojaPadrao();
                query = _orcamentoRepositorio.Get(a => !a.RegistroInativo && lojas.Contains(a.IdLojaDellaVia));
            }

            if (statusOrcamento != null && statusOrcamento != StatusOrcamento.Todos)
            {
                switch (statusOrcamento)
                {
                    case StatusOrcamento.EmAberto:
                        {
                            query = query.Where(a => a.StatusOrcamento == statusOrcamento &&
                            System.Data.Entity.DbFunctions.TruncateTime(a.DataValidade) >=
                                System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));
                        }
                        break;
                    case StatusOrcamento.EmAbertoVencido:
                        {
                            query = query.Where(a => (a.StatusOrcamento == statusOrcamento
                            || a.StatusOrcamento == StatusOrcamento.EmAberto) &&
                           System.Data.Entity.DbFunctions.TruncateTime(a.DataValidade) <
                               System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));
                        }
                        break;
                    default:
                        query = query.Where(a => a.StatusOrcamento == statusOrcamento);
                        break;
                }
            }
            try
            {
                termoBusca = ValidaBuscaOrcamentos(tipoFiltro, termoBusca, statusOrcamento);
                switch (tipoFiltro)
                {
                    case TipoFiltroOrcamento.IdOrcamento:
                        {
                            long idOrcamento = 0;
                            Int64.TryParse(termoBusca, out idOrcamento);
                            query = query.Where(a => a.Id == idOrcamento);
                            break;
                        }
                    case TipoFiltroOrcamento.NumeroProtheus:
                        {
                            var orcamentoCampoCodigo = termoBusca.Split('-')[0];
                            var lojaDellaviaCampoCodigo = termoBusca.Split('-')[1];
                            query = query.Where(a => a.CampoCodigo == orcamentoCampoCodigo && a.LojaDellaVia.CampoCodigo == lojaDellaviaCampoCodigo);
                            break;
                        }
                    case TipoFiltroOrcamento.Placa:
                        {
                            query = query.Where(a => a.Placa.Equals(termoBusca, StringComparison.InvariantCultureIgnoreCase));
                            break;
                        }
                    case TipoFiltroOrcamento.CPFCLiente:
                        {
                            query = query.Where(a => a.Cliente.CNPJCPF.Equals(termoBusca, StringComparison.InvariantCultureIgnoreCase));
                            break;
                        }
                    case TipoFiltroOrcamento.CNPJCliente:
                        {
                            query = query.Where(a => a.Cliente.CNPJCPF.Equals(termoBusca, StringComparison.InvariantCultureIgnoreCase));
                            break;
                        }
                    case TipoFiltroOrcamento.CodigoCliente:
                        {
                            if (termoBusca.Length == 9)
                            {
                                var clienteCampoCodigo = termoBusca.Split('-')[0];
                                var clienteLoja = termoBusca.Split('-')[1];

                                query = query.Where(a => a.Cliente.Loja.Equals(clienteLoja, StringComparison.InvariantCultureIgnoreCase) && a.Cliente.CampoCodigo.Equals(clienteCampoCodigo, StringComparison.InvariantCultureIgnoreCase));
                                break;
                            }
                            else
                            {
                                var clienteCampoCodigo = termoBusca;
                                query = query.Where(a => a.Cliente.CampoCodigo.Equals(clienteCampoCodigo, StringComparison.InvariantCultureIgnoreCase));
                                break;
                            }
                        }
                    case TipoFiltroOrcamento.Vendedor:
                        {
                            var vendedorCampoCodigo = termoBusca;
                            query = query.Where(a => a.IdVendedor.Equals(vendedorCampoCodigo, StringComparison.InvariantCultureIgnoreCase));
                            break;
                        }
                    case TipoFiltroOrcamento.LojaDestino:
                        {
                            var lojaDellaViaCampoCodigo = termoBusca;
                            query = query.Where(a => a.LojaDellaVia.CampoCodigo.Equals(lojaDellaViaCampoCodigo, StringComparison.InvariantCultureIgnoreCase));
                            break;
                        }
                    default:
                        break;
                }
                return Mapper.Map<ICollection<OrcamentoConsultaDto>>(query.OrderByDescending(a => a.DataCriacao).Take(100).ToList());
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
*/
/*        public ICollection<OrcamentoConsultaDto> ObterListaOrcamentoVencidosHoje()
        {
            try
            {
                var loginUsuario = HttpContext.Current.User.Identity.Name.ToUpper();
                var query = from orcamento in _orcamentoRepositorio.GetAll()
                            where System.Data.Entity.DbFunctions.TruncateTime(orcamento.DataValidade) ==
                            System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                            && orcamento.UsuarioAtualizacao == loginUsuario
                            && !orcamento.RegistroInativo
                            select orcamento;
                return Mapper.Map<ICollection<OrcamentoConsultaDto>>(query.OrderByDescending(a => a.DataCriacao).ToList());
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
*/
/*        public async Task<ICollection<OrcamentoConsultaDto>> ObterListaOrcamentoProtheus(TipoFiltroOrcamento? tipoFiltro, string termoBusca,
            StatusOrcamento? statusOrcamento)
        {
            if (tipoFiltro == TipoFiltroOrcamento.IdOrcamento)
                return new List<OrcamentoConsultaDto>();

            termoBusca = ValidaBuscaOrcamentos(tipoFiltro, termoBusca, statusOrcamento);
            var orcamentoFiltro = new OrcamentoFiltroProtheusDto()
            {
                Status = statusOrcamento == StatusOrcamento.Todos ? null : (int?)statusOrcamento.GetHashCode(),
                CodVendedor = tipoFiltro == TipoFiltroOrcamento.Vendedor ? termoBusca : null,
                CodCliente = tipoFiltro == TipoFiltroOrcamento.CodigoCliente ? termoBusca : null,
                CodCnpj = tipoFiltro == TipoFiltroOrcamento.CNPJCliente ? termoBusca : null,
                CodCpf = tipoFiltro == TipoFiltroOrcamento.CPFCLiente ? termoBusca : null,
                CodOrcamento = tipoFiltro == TipoFiltroOrcamento.NumeroProtheus ? termoBusca : null,
                CodPlaca = tipoFiltro == TipoFiltroOrcamento.Placa ? termoBusca : null
            };

            if (!orcamentoFiltro.CodOrcamento.IsNullOrEmpty())
            {
                orcamentoFiltro.CodFilial = null;
            }
            else
            {
                if (tipoFiltro == TipoFiltroOrcamento.LojaDestino)
                {
                    orcamentoFiltro.CodFilial = termoBusca;
                }
                else
                {
                    orcamentoFiltro.CodFilial = HttpContext.Current.User.Identity.GetLojaPadraoCampoCodigo();
                }
                if (tipoFiltro == TipoFiltroOrcamento.CodigoCliente)
                {
                    if (termoBusca.Length == 9)
                    {
                        orcamentoFiltro.CodCliente = termoBusca.Replace("-", "");
                    }
                    else
                    {
                        orcamentoFiltro.CodCliente = termoBusca.Replace("-", "") + orcamentoFiltro.CodFilial;
                    }
                }
            }

            var result = await _orcamentoApi.ObterOrcamentos(orcamentoFiltro);

            return result;
        }
*/
/*        public async Task<bool> EnviarOrcamentoProtheus(long idOrcamento)
        {
            var orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento);
            if (orcamento == null)
                throw new NegocioException("Orçamento não encontrado.");

            OrcamentoRetornoPostProtheusDto retornoPostProtheusDto;

            try
            {
                var codUsuario = HttpContext.Current.User.Identity.Name;
                _impostosServico.CalcularImpostos(orcamento);
                retornoPostProtheusDto = await _orcamentoApi.EnviarOrcamento(orcamento, codUsuario);
            }
            catch (NegocioException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new NegocioException("Ocorreu um erro ao processar o orçamento no Protheus: " + e.Message);
            }

            orcamento.CampoCodigo = retornoPostProtheusDto.CampoCodigoOrcamento;

            if (retornoPostProtheusDto.ExisteAnaliseCredito)
            {

                //Verifica se já existe a solicitação
                var solicitacaoAnaliseCredito = _solicitacaoAnaliseCreditoRepositorio.Get(a => a.IdOrcamento == orcamento.Id && a.SituacaoAnaliseCredito == SituacaoAnaliseCredito.EmAnalise).FirstOrDefault();
                if (solicitacaoAnaliseCredito == null)
                {
                    solicitacaoAnaliseCredito = new SolicitacaoAnaliseCredito()
                    {
                        IdOrcamento = orcamento.Id,
                        SituacaoAnaliseCredito = retornoPostProtheusDto.SituacaoAnaliseCredito
                    };
                    if (solicitacaoAnaliseCredito.SituacaoAnaliseCredito != SituacaoAnaliseCredito.EmAnalise)
                    {
                        solicitacaoAnaliseCredito.DataResposta = DateTime.Now;
                        solicitacaoAnaliseCredito.StatusSolicitacaoAnaliseCredito = StatusSolicitacao.Retornado;
                        solicitacaoAnaliseCredito.RespostaSolicitacao = retornoPostProtheusDto.RespostaSolicitacao;
                    }
                    else
                    {
                        solicitacaoAnaliseCredito.StatusSolicitacaoAnaliseCredito = StatusSolicitacao.PendenteRetorno;
                    }
                    solicitacaoAnaliseCredito.NumeroContrato = retornoPostProtheusDto.NumeroContratoSolicitacaoAnaliseCredito;

                    solicitacaoAnaliseCredito.DataSolicitacao = DateTime.Now;
                    _solicitacaoAnaliseCreditoRepositorio.Add(solicitacaoAnaliseCredito);
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
            *//*
            _escopo.Finalizar();

            return true;
        }
*/

        public List<OrcamentoDto> ObterOrcamentoPorCPF(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterOrcamentoPorCPFCNPJQuery(termoBusca, "CPF")
                .ProjectTo<OrcamentoDto>()
                .ToList();
        }

        public List<OrcamentoDto> ObterOrcamentoPorCNPJ(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterOrcamentoPorCPFCNPJQuery(termoBusca, "CNPJ")
                .ProjectTo<OrcamentoDto>()
                .ToList();
        }

        private IQueryable<Orcamento> ObterOrcamentoPorCPFCNPJQuery(string termoBusca, string tipo)
        {
            var lojasUsuario = HttpContext.Current.User.Identity.GetLojaPadrao();
            return
             from orcamento in _orcamentoRepositorio.GetAll()
             where orcamento.Cliente.CNPJCPF.Equals(termoBusca, StringComparison.InvariantCultureIgnoreCase)
             && lojasUsuario.Contains(orcamento.LojaDellaVia.CampoCodigo)
             && !orcamento.RegistroInativo
             orderby orcamento.CampoCodigo
             select orcamento;
        }

        public List<OrcamentoDto> ObterOrcamentoPorCodigoCliente(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterOrcamentoPorCodigoClienteQuery(termoBusca).OrderByDescending(a => a.DataValidade)
                .ProjectTo<OrcamentoDto>()
                .ToList();
        }

        private IQueryable<Orcamento> ObterOrcamentoPorCodigoClienteQuery(string termoBusca)
        {
            var clienteCampoCodigo = string.Empty;
            var clienteLoja = string.Empty;
            if (termoBusca.Length == 9)
            {
                clienteCampoCodigo = termoBusca.Split('-')[0];
                clienteLoja = termoBusca.Split('-')[1];
            }
            else
            {
                clienteCampoCodigo = termoBusca;
            }
            var lojasUsuario = HttpContext.Current.User.Identity.GetLojaPadrao();
            IQueryable<Orcamento> query =
             from orcamento in _orcamentoRepositorio.GetAll()
             where orcamento.Cliente.CampoCodigo.Equals(clienteCampoCodigo, StringComparison.InvariantCultureIgnoreCase)
              && lojasUsuario.Contains(orcamento.LojaDellaVia.CampoCodigo)
              && !orcamento.RegistroInativo
             orderby orcamento.CampoCodigo
             select orcamento;

            if (!string.IsNullOrEmpty(clienteLoja))
                query = query.Where(a => a.Cliente.Loja.Equals(clienteLoja, StringComparison.InvariantCultureIgnoreCase));

            return query;
        }

    }
}