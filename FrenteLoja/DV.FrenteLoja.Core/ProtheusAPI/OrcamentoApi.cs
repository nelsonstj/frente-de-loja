using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Core.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DV.FrenteLoja.Core.Servicos;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
    public class OrcamentoApi : IOrcamentoApi
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly DellaviaContexto _contexto;
        private readonly IRepositorio<Produto> _produtoRepository;
        private readonly IRepositorio<Vendedor> _vendedorRepository;
        private readonly HttpClient _client;
        //private readonly IRepositorio<Convenio> _convenioRepositorio;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        private readonly IRepositorio<TabelaPreco> _tabelaPrecoRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
        private readonly IRepositorio<Banco> _bancoRepositorio;
        private readonly IRepositorio<Transportadora> _transportadoraRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<OrcamentoItem> _orcamentoItemRepositorio;
        private readonly IRepositorio<DescontoModeloVenda> _descontoModeloVendaRepositorio;
        private readonly IRepositorio<OrcamentoItemEquipeMontagem> _orcamentoItemEquipeMontagemRepositorio;
        private readonly IRepositorio<OrcamentoFormaPagamento> _orcamentoFormaPagamentoRepositorio;
        private readonly IRepositorio<Operador> _operadorRepositorio;
        private readonly IRepositorio<TipoVenda> _tipoVendaRepositorio;
        private readonly IRepositorio<CondicaoPagamento> _condicaoPagamentoRepositorio;
        private readonly IRepositorio<AdministradoraFinanceira> _admFinanceiraRepositorio;
        private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
        private readonly IImpostosServico _impostosServico;


        public OrcamentoApi(IRepositorioEscopo escopo, DellaviaContexto contexto, ICalculoImpostosApi calculoImpostosApi)
        {
            _escopo = escopo;
            _contexto = contexto;
            _client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _orcamentoRepositorio = escopo.GetRepositorio<Orcamento>();
            _produtoRepository = escopo.GetRepositorio<Produto>();
            _vendedorRepository = escopo.GetRepositorio<Vendedor>();
            //_convenioRepositorio = escopo.GetRepositorio<Convenio>();
            _clienteRepositorio = escopo.GetRepositorio<Cliente>();
            _tabelaPrecoRepositorio = escopo.GetRepositorio<TabelaPreco>();
            _vendedorRepositorio = escopo.GetRepositorio<Vendedor>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _bancoRepositorio = escopo.GetRepositorio<Banco>();
            _transportadoraRepositorio = escopo.GetRepositorio<Transportadora>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _orcamentoItemRepositorio = escopo.GetRepositorio<OrcamentoItem>();
            _descontoModeloVendaRepositorio = escopo.GetRepositorio<DescontoModeloVenda>();
            _orcamentoItemEquipeMontagemRepositorio = escopo.GetRepositorio<OrcamentoItemEquipeMontagem>();
            _orcamentoFormaPagamentoRepositorio = escopo.GetRepositorio<OrcamentoFormaPagamento>();
            _operadorRepositorio = escopo.GetRepositorio<Operador>();
            _tipoVendaRepositorio = escopo.GetRepositorio<TipoVenda>();
            _condicaoPagamentoRepositorio = escopo.GetRepositorio<CondicaoPagamento>();
            _admFinanceiraRepositorio = escopo.GetRepositorio<AdministradoraFinanceira>();
            _logIntegracaoRepositorio = escopo.GetRepositorio<LogIntegracao>();
            _impostosServico = new ImpostosServico(escopo, calculoImpostosApi);
        }

        /// <summary>
        /// Realiza o envio do orçamento especificado para o Protheus através de um http post.
        /// </summary>
        /// <param name="orc">Objeto orçamento.</param>
        /// <param name="codUsuario">Código do usuário logado.</param>
        /// <returns></returns>
        public async Task<OrcamentoRetornoPostProtheusDto> EnviarOrcamento(Orcamento orc, string codUsuario)
        {
            OrcamentoProtheusDto orcamentoProtheus = new OrcamentoProtheusDto();
            orcamentoProtheus.Numero = orc.Id.ToString();
            orcamentoProtheus.CampoCodigoOrcamento = orc.CampoCodigo;
            orcamentoProtheus.CodUsuario = codUsuario;
            orcamentoProtheus.Ano = orc.Ano.ToString();
            orcamentoProtheus.CodLojaCliente = orc.Cliente.Loja;
            orcamentoProtheus.CodBanco = orc.FormaPagamentos.FirstOrDefault(x => x.IdBanco != null)?.Banco?.CampoCodigo;
            orcamentoProtheus.CodCliente = orc.IdCliente;
            orcamentoProtheus.CodCondicaoPagamento = orc.TabelaPreco.CodCondicaoPagamento;
            orcamentoProtheus.CodConvenio = orc.IdConvenio;
            orcamentoProtheus.CodDellaVia = orc.LojaDellaVia.CampoCodigo;
            orcamentoProtheus.CodMarca = ""; //orc.Veiculo.MarcaModeloVersao.MarcaModelo.Marca.CampoCodigo;
            orcamentoProtheus.CodModelo = ""; //orc.Veiculo.MarcaModeloVersao.MarcaModelo.CampoCodigo;
            orcamentoProtheus.CodOperador = orc.IdOperador;
            orcamentoProtheus.CodVendedor = orc.IdVendedor;
            orcamentoProtheus.CodTabelaPreco = orc.IdTabelaPreco;
            orcamentoProtheus.Km = orc.KM.ToString();
            orcamentoProtheus.ObsVeiculo = orc.InformacoesCliente;
            orcamentoProtheus.Complemento = orc.Complemento;
            orcamentoProtheus.Placa = orc.Placa;
            orcamentoProtheus.Xped = orc.Xped;
            orcamentoProtheus.CodTransportadora = orc.Transportadora?.CampoCodigo;
            orcamentoProtheus.MensagemNF = orc.MensagemNF;
            orcamentoProtheus.DataValidade = orc.DataValidade;
            orcamentoProtheus.DataCriacao = orc.DataCriacao;
            orcamentoProtheus.Valor = orc.OrcamentoItens.Sum(x => x.TotalItem);
            orcamentoProtheus.Itens = ObterOrcItensProtheusDto(orc.OrcamentoItens);
            orcamentoProtheus.FormasPagamento = ObterOrcFormasPagamentoProtheusDto(orc.FormaPagamentos);
            orcamentoProtheus.CodTipoVenda = orc.IdAreaNegocio;
            orcamentoProtheus.DescricaoMarca = ""; // orc.Veiculo.MarcaModeloVersao?.MarcaModelo?.Marca?.Descricao;
            orcamentoProtheus.DescricaoModelo = ""; // orc.Veiculo.MarcaModeloVersao?.MarcaModelo?.Descricao;
            orcamentoProtheus.VeiculoIdFraga = orc.VeiculoIdFraga;
            var json = JsonConvert.SerializeObject(orcamentoProtheus);
            var _logIntegracao = new LogIntegracao
            {
                DataAtualizacao = DateTime.Now,
                UsuarioAtualizacao = orcamentoProtheus.CodUsuario,
                RequestIntegracaoJson = json,
            };
            try
            {
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
                var resourcePath = "ORCAMENTO";
                var uri = new Uri(string.Concat(baseAddress, resourcePath));
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    content.Headers.Add("tenantId", "01," + orcamentoProtheus.CodDellaVia);
                    var response = _client.PostAsync(uri, content).Result;
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    JObject responseObj = JObject.Parse(contentResponse);
                    _logIntegracao.ResponseIntegracaoJson = responseObj.ToString();
                    if (!response.IsSuccessStatusCode)
                        throw new NegocioException($"Orçamento { orc.Id } não foi integrado: { responseObj["errorMessage"]}.\n");
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                    _logIntegracao.Log = $"Orçamento { orc.Id } integrado com sucesso.";
                    return JsonConvert.DeserializeObject<OrcamentoRetornoPostProtheusDto>(contentResponse);
                }
            }
            catch (Exception e)
            {
                _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                _logIntegracao.Log = e.Message;
                _logIntegracao.ResponseIntegracaoJson = _logIntegracao.ResponseIntegracaoJson ?? e.Message;
                Debug.WriteLine(e.ToString());
                throw new ServicoIntegracaoException($"Erro no método {nameof(EnviarOrcamento)}. Descrição: " + e.Message);
            }
            finally
            {
                _logIntegracaoRepositorio.Add(_logIntegracao);
            }
        }

        public async Task<List<OrcamentoProtheusDto>> ObterOrcamentos()
        {
            try
            {
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
                var resourcePath = "ALLORCAMENTOS";
                var uri = new Uri(string.Concat(baseAddress, resourcePath));
                var response = await _client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Api respondeu com: {response}.\n");

                if (response.StatusCode == HttpStatusCode.NoContent)
                    // Não existem novos dados na base para integrar.
                    return new List<OrcamentoProtheusDto>();

                var content = await response.Content.ReadAsStringAsync();
                var orcamentoProtheusDtos = JsonConvert.DeserializeObject<OrcamentoRetornoProtheusDto>(content);
                return orcamentoProtheusDtos.OrcamentosEnvio.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new ServicoIntegracaoException($"Erro no método {nameof(ObterOrcamentos)}. Descrição: " + e.Message);
            }
        }

        public async Task<List<OrcamentoConsultaDto>> ObterOrcamentos(OrcamentoFiltroProtheusDto orcamentoFiltroProtheusDto)
        {
            var json = JsonConvert.SerializeObject(orcamentoFiltroProtheusDto);
            try
            {
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
                var resourcePath = "ORCAMENTOSGRID";
                var uri = new Uri(string.Concat(baseAddress, resourcePath));
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await _client.PostAsync(uri, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        JObject responseObj = JObject.Parse(errorResponse);
                        throw new NegocioException($"Não foi possivel obter orçamentos do Protheus: {responseObj["errorMessage"]}.\n");
                    }
                    var contentResponsiveed = await response.Content.ReadAsStringAsync();
                    if (contentResponsiveed.IsNullOrEmpty())
                        return new List<OrcamentoConsultaDto>();

                    JObject objFromRequest = JObject.Parse(contentResponsiveed);
                    var result = JsonConvert.DeserializeObject<List<OrcamentoConsultaDto>>(objFromRequest["ORCAMENTOSENVIO"].ToString());
                    return result;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new NegocioException($"Erro na comunicação com Protheus no metodo: {nameof(ObterOrcamentos)}. Descrição: " + e.Message);
            }
        }

        public async Task<OrcamentoProtheusDto> ObterOrcamentoPorCodProtheus(OrcamentoDto orcDto)
        {
            try
            {
                var parametros = new
                {
                    CodOrcamento = orcDto.CampoCodigo,
                    CodFilial = orcDto.LojaDestinoCampoCodigo
                };
                var json = JsonConvert.SerializeObject(parametros);
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
                var resourcePath = "ORCAMENTOSBYID";
                var uri = new Uri(string.Concat(baseAddress, resourcePath));
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = _client.PostAsync(uri, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        JObject responseObj = JObject.Parse(errorResponse);
                        throw new NegocioException($"Não foi possivel obter orçamentos do Protheus: {responseObj["errorMessage"]}.\n");
                    }
                    var contentResponsiveed = await response.Content.ReadAsStringAsync();
                    JObject objFromRequest = JObject.Parse(contentResponsiveed);
                    var orc = JsonConvert.DeserializeObject<OrcamentoProtheusDto[]>(objFromRequest["ORCAMENTOSENVIO"].ToString());
                    return orc.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new ServicoIntegracaoException($"Erro no método {nameof(ObterOrcamentoPorCodProtheus)}. Descrição: " + e.Message);
            }
        }

        public async Task<OrcamentoDto> ObterOrcamentoRelatorio(string codigoOrcamento)
        {
            try
            {
                var parametros = new
                {
                    CodOrcamento = codigoOrcamento.Substring(0, codigoOrcamento.Length - 2),
                    CodFilial = codigoOrcamento.Substring(codigoOrcamento.Length - 2)
                };
                var json = JsonConvert.SerializeObject(parametros);
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
                var resourcePath = "ORCAMENTOSREPORT";
                var uri = new Uri(string.Concat(baseAddress, resourcePath));
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await _client.PostAsync(uri, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        JObject responseObj = JObject.Parse(errorResponse);
                        throw new NegocioException($"Não foi possivel obter orçamentos do Protheus: {responseObj["errorMessage"]}.\n");
                    }
                    var contentResponsiveed = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(contentResponsiveed);
                    DateTime dateValue;
                    var intAux = 0;
                    var decAux = 0.0M;
                    var orc = new OrcamentoDto();
                    orc.CampoCodigo = obj["CAMPOCODIGO"].ToString();
                    orc.AnoVeiculo = int.TryParse(obj["ANOVEICULO"].ToString(), out intAux) ? intAux : 0;
                    orc.CelularCliente = obj["CELULARCLIENTE"].ToString();
                    orc.CodigoCliente = obj["CODIGOCLIENTE"].ToString();
                    orc.CPFCNPJCliente = obj["CPFCNPJCLIENTE"].ToString();
                    orc.DataCriacao = DateTime.TryParse(obj["DATACRIACAO"].ToString(), out dateValue) ? dateValue : DateTime.MinValue;
                    orc.DataValidade = DateTime.TryParse(obj["DATAVENCIMENTO"].ToString(), out dateValue) ? dateValue : DateTime.MinValue;
                    orc.EmailCliente = obj["EMAILCLIENTE"].ToString();
                    orc.FormaPagamento = new OrcamentoPagamentoDto();
                    orc.FormaPagamento.FormasPagamentos = JsonConvert.DeserializeObject<List<OrcamentoFormaPagamentoDto>>(obj["FORMASPAGAMENTO"].ToString());
                    orc.OrcamentoProduto = new OrcamentoProdutoBuscaDto();
                    var orcItensJArray = JArray.Parse(obj["ITENS"].ToString());
                    foreach (var jObj in orcItensJArray)
                    {
                        var prod = new SacolaProdutoDto();
                        prod.CampoCodigoProduto = jObj["CODPRODUTO"].ToString();
                        prod.NumeroItem = int.TryParse(jObj["NRITEM"].ToString(), out intAux) ? intAux : 0;
                        prod.Valor = decimal.TryParse(jObj["PRECOUNITARIO"].ToString(), out decAux) ? decAux : 0;
                        prod.Descricao = jObj["PRODUTODESCRICAO"].ToString();
                        prod.Quantidade = int.TryParse(jObj["QUANTIDADE"].ToString(), out intAux) ? intAux : 0;
                        prod.ValorTotal = decimal.TryParse(jObj["VALORTOTALITEM"].ToString(), out decAux) ? decAux : 0;
                        orc.OrcamentoProduto.Produtos.Add(prod);
                    }
                    orc.LojaDestinoCampoCodigo = obj["LOJADESTINOCAMPOCODIGO"].ToString();
                    orc.MarcaVeiculoDescricao = obj["MARCAVEICULO"].ToString();
                    orc.ModeloVeiculoDescricao = obj["MODELOVEICULO"].ToString();
                    orc.NomeCliente = obj["NOMECLIENTE"].ToString();
                    orc.PlacaVeiculo = obj["PLACAVEICULO"].ToString();
                    orc.QuilometragemVeiculo = int.TryParse(obj["QUILOMETRAGEMVEICULO"].ToString(), out intAux) ? intAux : 0;
                    orc.TelefoneCliente = obj["TELEFONECLIENTE"].ToString();
                    return orc;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new ServicoIntegracaoException($"Erro no método {nameof(ObterOrcamentos)}. Descrição: " + e.Message);
            }
        }

        public string AtualizarOrcamento(OrcamentoProtheusDto orcProtheus, long idOrc = 0)
        {
            var errosBuilder = new StringBuilder();
           // var commitDone = false;
            Orcamento orc;
            // Quando vem do Protheus não temos o id do sql server.
            orc = idOrc == 0
                ? _orcamentoRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.Numero
                && x.LojaDellaVia.CampoCodigo == orcProtheus.CodDellaVia)
                : _orcamentoRepositorio.GetSingle(x => x.Id == idOrc);

            if (orc == null)
            {
                errosBuilder.AppendLine($"Orçamento não encontrado");
                return errosBuilder.ToString();
            }

            using (var dbContext = _contexto.Database.BeginTransaction())
            {
                try
                {
                    #region Convenio
                    //TO DO
                    /*
                    if (orcProtheus.CodConvenio != orc.Convenio.CampoCodigo)
                    {
                        var convenio = _convenioRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodConvenio);
                        if (convenio == null)
                        {
                            errosBuilder.AppendLine($"Convenio não encontrada. Código: {orcProtheus.CodConvenio}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();
                        }

                        orc.IdConvenio = convenio.IdConvenio;
                    }
                    */
                    #endregion

                    #region Cliente

                    if (orcProtheus.CodCliente != orc.Cliente.CampoCodigo || orcProtheus.CodLojaCliente != orc.Cliente.Loja)
                    {
                        var cliente = _clienteRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodCliente && x.Loja == orcProtheus.CodLojaCliente);

                        if (cliente == null)
                        {
                            errosBuilder.AppendLine($"Cliente não encontrado. Código: {orcProtheus.CodCliente} Código Loja {orcProtheus.CodLojaCliente}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();
                        }

                        orc.IdCliente = cliente.IdCliente;
                    }

                    #endregion

                    #region TabelaPreço


                    if (orcProtheus.CodTabelaPreco != orc.TabelaPreco.CampoCodigo && !string.IsNullOrEmpty(orcProtheus.CodTabelaPreco))
                    {
                        var tabelaPreco = _tabelaPrecoRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodTabelaPreco);
                        if (tabelaPreco == null)
                        {
                            errosBuilder.AppendLine($"Tabela Preco não encontrada. Código: {orcProtheus.CodTabelaPreco}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();

                        }

                        orc.IdTabelaPreco = tabelaPreco.CampoCodigo;
                    }

                    #endregion

                    #region Vendedor

                    if (orcProtheus.CodVendedor != orc.IdVendedor)
                    {
                        var vendedor = _vendedorRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodVendedor);
                        if (vendedor == null)
                        {
                            errosBuilder.AppendLine($"Vendedor não encontrado. Código: {orcProtheus.CodVendedor}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();
                        }

                        orc.IdVendedor = vendedor.IdConsultor;
                    }

                    #endregion

                    #region MarcaModeloVersao

                    if (orcProtheus.CodMarca != "" //orc.Veiculo.MarcaModeloVersao.MarcaModelo.Marca.CampoCodigo
                        || orcProtheus.CodModelo != "") //orc.Veiculo.MarcaModeloVersao.MarcaModelo.CampoCodigo)
                    {
                        var marcaModeloVersao = _marcaModeloVersaoRepositorio.GetSingle(x =>
                            x.MarcaModelo.Marca.CampoCodigo == orcProtheus.CodMarca && x.MarcaModelo.CampoCodigo == orcProtheus.CodModelo &&
                            x.Descricao == Constants.VERSAO_DEFAULT);

                        if (marcaModeloVersao == null)
                        {
                            errosBuilder.AppendLine(
                                $"Marca e Modelo não encontrado. Código Marca: {orcProtheus.CodMarca} - Código Modelo {orcProtheus.CodModelo}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();
                        }

                        //orc.Veiculo.IdMarcaModeloVersao = marcaModeloVersao.Id;
                    }

                    #endregion

                    #region Banco

                    if (orcProtheus.CodBanco != orc.Banco?.CampoCodigo)
                    {
                        var banco = _bancoRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodBanco);
                        orc.IdBanco = banco?.Id;
                    }


                    #endregion

                    #region Transportadora

                    if (orcProtheus.CodTransportadora != orc.Transportadora?.CampoCodigo)
                    {
                        var transportadora = _transportadoraRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodTransportadora);


                        orc.IdTransportadora = transportadora?.Id;
                    }

                    #endregion

                    #region Operador

                    if (orcProtheus.CodOperador != orc.IdOperador && !string.IsNullOrEmpty(orcProtheus.CodOperador))
                    {
                        var operador = _operadorRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodOperador);
                        if (operador == null)
                        {
                            errosBuilder.AppendLine($"Operador não encontrado. Código: {orcProtheus.CodOperador}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();
                        }

                        orc.IdOperador = operador.Id.ToString();
                    }

                    #endregion

                    #region Tipo Venda

                    if (orcProtheus.CodTipoVenda != orc.IdAreaNegocio && !string.IsNullOrEmpty(orcProtheus.CodTipoVenda))
                    {
                        var tipoVenda = _tipoVendaRepositorio.GetSingle(x => x.CampoCodigo == orcProtheus.CodTipoVenda);
                        if (tipoVenda == null)
                        {
                            errosBuilder.AppendLine($"Tipo Venda não encontrado. Código: {orcProtheus.CodTipoVenda}");
                            dbContext.Rollback();
                            return errosBuilder.ToString();
                        }
                        //TODO
                        //orc.IdTipoVenda = tipoVenda.Id;
                    }

                    #endregion

                    #region Orcamento Forma Pagamento

                    // Recupero todos os itens da base
                    var orcFormaPagamentosFromDb = _orcamentoFormaPagamentoRepositorio.Get(x => x.IdOrcamento == idOrc);

                    // Crio uma Lista aux para armazenar os registros que estão na base mas não estão mais sendo retornados do Protheus
                    var orcFormaPagamentoNaoPresentesNoProtheus = new List<OrcamentoFormaPagamento>();

                    foreach (var fp in orcFormaPagamentosFromDb)
                    {
                        var formaPagamentoProtheus = orcProtheus.FormasPagamento.FirstOrDefault(x => x.CodCondicaoPagamento == fp.CampoCodigo);
                        if (formaPagamentoProtheus == null)
                        {
                            // Se a forma de pagamento for null, isso nos diz que esse registro foi apagado do lado do Protheus...
                            // Então o armazenamos para ser deletado depois.
                            orcFormaPagamentoNaoPresentesNoProtheus.Add(fp);
                            continue;
                        }


                        /* Verifico se houve mudança no registros que compõe a entidade.*/

                        // CondicaoPagamento
                        if (fp.CondicaoPagamento?.IdFormaPagamento != formaPagamentoProtheus.CodCondicaoPagamento)
                        {
                            var condicaoPagamentoAtualizado = _condicaoPagamentoRepositorio.GetSingle(x => x.CampoCodigo == formaPagamentoProtheus.CodCondicaoPagamento);
                            if (condicaoPagamentoAtualizado == null)
                                throw new NegocioException($"Condição de pagamento não encontrada. Cod adm financeira {formaPagamentoProtheus.CodAdministradora}");
                            //TO DO
                            //fp.IdCondicaoPagamento = condicaoPagamentoAtualizado.Id;
                        }

                        // AdmFinanceira
                        if (fp.AdministradoraFinanceira?.CampoCodigo != formaPagamentoProtheus.CodAdministradora)
                        {
                            var administradoraFinanceiraAtualizado = _admFinanceiraRepositorio.GetSingle(x => x.CampoCodigo == formaPagamentoProtheus.CodAdministradora);
                            if (administradoraFinanceiraAtualizado == null)
                                throw new NegocioException($"Adm financeira não encontrada. Cod adm financeira {formaPagamentoProtheus.CodAdministradora}");

                            fp.IdAdministradoraFinanceira = administradoraFinanceiraAtualizado.IdAdminFinanceira;
                        }

                        // Banco
                        if (fp.Banco?.CampoCodigo != formaPagamentoProtheus.Banco)
                        {
                            var bancoAtualizado = _condicaoPagamentoRepositorio.GetSingle(x => x.CampoCodigo == formaPagamentoProtheus.CodCondicaoPagamento);
                            if (bancoAtualizado == null)
                                throw new NegocioException($"Banco não encontrado. Cod banco {formaPagamentoProtheus.CodCondicaoPagamento}");

                            //TO DO
                            //fp.IdCondicaoPagamento = bancoAtualizado.Id;
                        }


                        fp.TotalValorForma = formaPagamentoProtheus.ValorTotal;
                    }

                    // Excluir os registros que não estão no protheus.
                    foreach (var orcFormaPagamentoNaoPresentesNoProtheu in orcFormaPagamentoNaoPresentesNoProtheus)
                    {
                        _orcamentoFormaPagamentoRepositorio.Remove(orcFormaPagamentoNaoPresentesNoProtheu);
                    }

                    #endregion

                    #region Itens

                    // Recupero todos os itens da base
                    var orcItensFromDb = _orcamentoItemRepositorio.Get(x => x.OrcamentoId == idOrc);

                    // Crio uma Lista aux para armazenar os registros que estão na base mas não estão mais sendo retornados do Protheus
                    var orcItensNaoPresentesNoProtheus = new List<OrcamentoItem>();

                    foreach (var oItem in orcItensFromDb)
                    {
                        var itemProtheus = orcProtheus.Itens.FirstOrDefault(x => x.CodProduto == oItem.Produto.CampoCodigo);
                        if (itemProtheus == null)
                        {
                            // Se registro for null, isso nos diz que esse registro foi apagado do lado do Protheus...
                            // Então o armazenamos para ser deletado depois.

                            orcItensNaoPresentesNoProtheus.Add(oItem);
                            continue;
                        }


                        /* Verifico se houve mudança no registros que compõe a entidade.*/

                        #region Desconto Modelo Venda

                        if (oItem.DescontoModeloVenda != null && oItem.DescontoModeloVenda.CampoCodigo != itemProtheus.CodDescontoModeloVenda)
                        {
                            var modeloVenda = _descontoModeloVendaRepositorio.GetSingle(x => x.CampoCodigo == itemProtheus.CodDescontoModeloVenda);
                            if (modeloVenda == null)
                            {
                                dbContext.Rollback();
                                throw new NegocioException($"Desconto modelo de venda não encontrado. Código: {itemProtheus.CodDescontoModeloVenda}");
                            }

                            oItem.IdDescontoModeloVenda = modeloVenda.Id;
                        }

                        #endregion

                        #region Produto

                        if (oItem.Produto.CampoCodigo != itemProtheus.CodProduto)
                        {
                            var produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == itemProtheus.CodProduto);
                            if (produto == null)
                            {
                                dbContext.Rollback();
                                throw new NegocioException($"Produto não encontrado. Código: {itemProtheus.CodProduto}");
                            }

                            //TODO
                            oItem.ProdutoId = produto.Id.ToString();
                        }

                        #endregion

                        #region EquipeMontagem

                        var codVendedoresProtheus = itemProtheus.Equipe.Select(x => x.CodVendedor).ToList();
                        var codVendedoresDb = oItem.EquipeMontagemList.Select(x => x.Vendedor.CampoCodigo).ToList();

                        // Verifico se ambas as coleções são iguais
                        var isEqual = codVendedoresDb.All(codVendedoresProtheus.Contains);

                        if (!isEqual)
                        {
                            // Caso não forem, removo todos os itens...
                            oItem.EquipeMontagemList.ToList().ForEach(x => _orcamentoItemEquipeMontagemRepositorio.Remove(x));

                            // e adiciono novamente.
                            foreach (var montagemProtheusDto in itemProtheus.Equipe)
                            {
                                OrcamentoItemEquipeMontagem itemEquipeMontagem = null;


                                // Se não existe então criamos o registro.
                                itemEquipeMontagem = new OrcamentoItemEquipeMontagem();

                                var vendedorOperador = _vendedorRepositorio.GetSingle(x => x.CampoCodigo == montagemProtheusDto.CodVendedor);
                                if (vendedorOperador == null)
                                {
                                    errosBuilder.AppendLine(
                                        $"Vendedor (equipe de montagem) não encontrado. Código: {montagemProtheusDto.CodVendedor}");
                                    dbContext.Rollback();
                                    return errosBuilder.ToString();
                                }

                                if (!itemProtheus.CodDescontoModeloVenda.IsNullOrEmpty())
                                {
                                    var modeloVenda = _descontoModeloVendaRepositorio.GetSingle(x => x.CampoCodigo == itemProtheus.CodDescontoModeloVenda);
                                    if (modeloVenda == null)
                                    {
                                        errosBuilder.AppendLine($"Desconto modelo de venda não encontrado. Código: {itemProtheus.CodDescontoModeloVenda}");
                                        dbContext.Rollback();
                                        return errosBuilder.ToString();
                                    }

                                    oItem.IdDescontoModeloVenda = modeloVenda.Id;
                                }

                                itemEquipeMontagem.IdVendedor = vendedorOperador.IdConsultor;
                                itemEquipeMontagem.IdOrcamentoItem = oItem.Id;
                                itemEquipeMontagem.Funcao = montagemProtheusDto.Funcao;
                                oItem.EquipeMontagemList.Add(itemEquipeMontagem);
                            }
                        }

                        #endregion

                        #region SolicitacaoDescontoVendaAlcada

                        var descontoVendaAlcadaProtheusDto = itemProtheus.DescontoVendaAlcadaRetorno.FirstOrDefault();
                        if (descontoVendaAlcadaProtheusDto != null)
                        {
                            SolicitacaoDescontoVendaAlcada solicitacaoDescontoVendaAlcada = oItem.SolicitacaoDescontoVendaAlcadaList.FirstOrDefault(x =>
                                x.StatusSolicitacaoAlcada == StatusSolicitacao.PendenteRetorno);
                            if (solicitacaoDescontoVendaAlcada != null)
                            {
                                solicitacaoDescontoVendaAlcada.ObservacaoItem = descontoVendaAlcadaProtheusDto.ObservacaoItem;
                                solicitacaoDescontoVendaAlcada.ObservacaoGeral = descontoVendaAlcadaProtheusDto.ObservacaoGeral;
                                solicitacaoDescontoVendaAlcada.ValorDesconto = descontoVendaAlcadaProtheusDto.ValorDesconto;
                                solicitacaoDescontoVendaAlcada.PercentualDesconto = descontoVendaAlcadaProtheusDto.PercentualDesconto;
                                solicitacaoDescontoVendaAlcada.StatusSolicitacaoAlcada = StatusSolicitacao.Retornado;
                                solicitacaoDescontoVendaAlcada.Situacao = solicitacaoDescontoVendaAlcada.Situacao;
                                solicitacaoDescontoVendaAlcada.DataResposta = DateTime.Now;
                                solicitacaoDescontoVendaAlcada.RespostaSolicitacao = descontoVendaAlcadaProtheusDto.RespostaSolitacao;
                            }
                        }
                        #endregion

                        oItem.PercDescon = itemProtheus.PercDesconto;
                        oItem.PrecoUnitario = itemProtheus.PrecoUnitario;
                        oItem.Quantidade = itemProtheus.Quantidade;
                        oItem.TipoOperacao = itemProtheus.TipoOperacao;
                        oItem.ValorDesconto = itemProtheus.ValorDesconto;
                        oItem.NrItem = itemProtheus.NrItem;
                        oItem.DataAtualizacao = DateTime.Now;
                        oItem.UsuarioAtualizacao =
                            HttpContext.Current.User.Identity.GetName();
                        oItem.NrItemProdutoPaiId = itemProtheus.NrItemProdutoPaiId;
                        oItem.OrcamentoId = orc.Id;
                        oItem.ReservaEstoque = itemProtheus.ReservaEstoque;
                        oItem.TotalItem = (itemProtheus.Quantidade * itemProtheus.PrecoUnitario) - itemProtheus.ValorDesconto;
                        oItem.RegistroInativo = itemProtheus.RegistroInativo;

                    }

                    foreach (var orcItensNaoPresentesNoProtheu in orcItensNaoPresentesNoProtheus)
                    {
                        _orcamentoItemRepositorio.Remove(orcItensNaoPresentesNoProtheu);
                    }
                    #endregion



                    int intAux;

                    orc.DataAtualizacao = DateTime.Now;
                    orc.UsuarioAtualizacao = HttpContext.Current.User.Identity.GetName();
                    orc.Ano = int.TryParse(orcProtheus.Ano, out intAux) ? intAux : 0;
                    orc.DataValidade = orcProtheus.DataValidade;
                    orc.MensagemNF = orcProtheus.MensagemNF;
                    orc.PossuiReservaEstoque = orcProtheus.PossuiReservaEstoque;
                    orc.Placa = orcProtheus.Placa;
                    orc.StatusOrcamento = (StatusOrcamento)orcProtheus.StatusOrcamento;
                    orc.KM = int.TryParse(orcProtheus.Km, out intAux) ? intAux : 0;
                    orc.Telefone = orcProtheus.Telefone;
                    orc.TelefoneCelular = orcProtheus.TelefoneCelular;
                    orc.Xped = orcProtheus.Xped;

                    dbContext.Commit();
                    _escopo.Finalizar();
                   // commitDone = true;
                }
                catch (Exception e)
                {
                    //if (!commitDone)
                    //{
                    //    dbContext.Rollback();
                    //}
                    Debug.WriteLine(e.ToString());
                    throw;
                }
                try
                {
                    _impostosServico.CalcularImpostos(orc);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine("Erro no calculo de imposto " + e + " " + e.InnerException?.Message);
                    return errosBuilder.ToString();
                }
            }

            return errosBuilder.ToString();
        }

        private List<OrcamentoItemProtheusDto> ObterOrcItensProtheusDto(ICollection<OrcamentoItem> orcOrcamentoItens)
        {
            var orcItensProtheus = new List<OrcamentoItemProtheusDto>();
            foreach (var orcamentoItem in orcOrcamentoItens)
            {
                var descontoVendaAlcada = orcamentoItem.SolicitacaoDescontoVendaAlcadaList.OrderByDescending(x => x.DataSolicitacao).FirstOrDefault();
                //TODO
                //var produto = _produtoRepository.GetSingle(x => x.CampoCodigo == orcamentoItem.ProdutoId);
                //if (produto == null)
                //    continue;

                var orcItemProtheusDto = new OrcamentoItemProtheusDto
                {
                    NrItem = orcamentoItem.NrItem,
                    Quantidade = orcamentoItem.Quantidade,
                    CodProduto = orcamentoItem.ProdutoId,
                    PrecoUnitario = orcamentoItem.PrecoUnitario,
                    TipoOperacao = orcamentoItem.TipoOperacao,
                    PercDesconto = orcamentoItem.PercDescon,
                    ValorDesconto = orcamentoItem.ValorDesconto,
                    Equipe = ObterEquipeMontagemProtheusDto(orcamentoItem.EquipeMontagemList),
                    DescontoVendaAlcadaEnvio = descontoVendaAlcada == null
                    ? null
                    : new DescontoVendaAlcadaProtheusDto
                    {
                        ObservacaoGeral = descontoVendaAlcada.ObservacaoGeral,
                        PercentualDesconto = descontoVendaAlcada.PercentualDesconto,
                        ValorDesconto = descontoVendaAlcada.ValorDesconto,
                        ObservacaoItem = descontoVendaAlcada.ObservacaoItem
                    }
                };
                orcItensProtheus.Add(orcItemProtheusDto);
            }
            return orcItensProtheus;
        }

        private List<OrcamentoFormaPagamentoProtheusDto> ObterOrcFormasPagamentoProtheusDto(ICollection<OrcamentoFormaPagamento> orcFormasPagamento)
        {
            var orcFormasPagamentoProtheus = new List<OrcamentoFormaPagamentoProtheusDto>();
            foreach (var orcamentoForma in orcFormasPagamento)
            {
                orcFormasPagamentoProtheus.Add(new OrcamentoFormaPagamentoProtheusDto
                {
                    CodCondicaoPagamento = orcamentoForma.CondicaoPagamento.IdCondicaoPagamento,
                    CodAdministradora = orcamentoForma.IdAdministradoraFinanceira,
                    //Banco = orcamentoForma.IdBanco,
                    Descricao = orcamentoForma.CondicaoPagamento.DescricaoCondicaoPagamento,
                    Forma = orcamentoForma.CondicaoPagamento.IdFormaPagamento,
                    ValorTotal = orcamentoForma.TotalValorForma
                });
            }
            return orcFormasPagamentoProtheus;
        }

        private List<OrcamentoItemEquipeMontagemProtheusDto> ObterEquipeMontagemProtheusDto(ICollection<OrcamentoItemEquipeMontagem> orcamentoItemEquipeMontagemList)
        {
            var equipeMontagemProtheusDtos = new List<OrcamentoItemEquipeMontagemProtheusDto>();
            foreach (var itemEquipeMontagem in orcamentoItemEquipeMontagemList)
            {
                var vendedor = _vendedorRepository.GetSingle(x => x.IdConsultor == itemEquipeMontagem.IdVendedor);
                if (vendedor == null)
                    continue;

                equipeMontagemProtheusDtos.Add(new OrcamentoItemEquipeMontagemProtheusDto
                {
                    Funcao = itemEquipeMontagem.Funcao,
                    CodVendedor = vendedor.CampoCodigo
                });
            }

            return equipeMontagemProtheusDtos;
        }
    }
}