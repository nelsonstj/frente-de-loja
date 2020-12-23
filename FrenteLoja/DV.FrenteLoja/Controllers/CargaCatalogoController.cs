using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Filters;

namespace DV.FrenteLoja.Controllers
{

    [Authorize]
    [AuthorizationFilter(FlagPerfilAcessoUsuario = new[] { PerfilAcessoUsuario.Administrativo })]
    public class CargaCatalogoController : Controller
    {
        private readonly ICatalogoServico _catalogoServico;
        private readonly ICargaProdutoServico _cargaProdutoServico;
        private readonly ICargaCadastrosBasicosService _cargaCadastrosBasicosService;
        private readonly ICargaDescontosServico _cargaDescontosServico;
        private readonly ICargaClienteServico _cargaClienteServico;
        private readonly ICargaConvenioServico _cargaConvenioServico;
        private readonly ICargaPrecoServico _cargaPrecoServico;
        private readonly ICargaOrcamentoServico _cargaOrcamentoServico;
        private readonly ICargaVeiculoServico _cargaVeiculoServico;
        private readonly ICargaVeiculoProdutosServico _cargaVeiculoProdutosServico;

        public CargaCatalogoController(ICatalogoServico catalogoServico, ICargaProdutoServico cargaProdutoServico, ICargaCadastrosBasicosService cargaCadastrosBasicosService, ICargaDescontosServico cargaDescontosServico, ICargaClienteServico cargaClienteServico, ICargaConvenioServico cargaConvenioServico, ICargaPrecoServico cargaPrecoServico, ICargaOrcamentoServico cargaOrcamentoServico, ICargaVeiculoServico cargaVeiculoServico, ICargaVeiculoProdutosServico cargaVeiculoProdutosServico)
        {
            _catalogoServico = catalogoServico;
            _cargaProdutoServico = cargaProdutoServico;
            _cargaCadastrosBasicosService = cargaCadastrosBasicosService;
            _cargaDescontosServico = cargaDescontosServico;
            _cargaClienteServico = cargaClienteServico;
            _cargaConvenioServico = cargaConvenioServico;
            _cargaPrecoServico = cargaPrecoServico;
            _cargaOrcamentoServico = cargaOrcamentoServico;
            _cargaVeiculoServico = cargaVeiculoServico;
            _cargaVeiculoProdutosServico = cargaVeiculoProdutosServico;
        }

        public ActionResult Index(CargaCatalogoDto arquivo, bool godMode = false)
        {
            ViewBag.IsGodMode = godMode;
            return View(arquivo);
        }

        // GET: api/Integracao
        public ActionResult Get()
        {
            return Json(_catalogoServico.Obter(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult UploadCargaCatalogo()
        {

            var catalogoProtheusApi = BaseConfig.Container.GetInstance<ICatalogoProtheusApi>();
            var arquivo = new CargaCatalogoDto();
            try
            {
                if (Request.Files == null)
                {
                    arquivo.LogImportacao += "O arquivo não pode ser vazio ou nulo. \n";
                }
                else
                {
                    var target = new MemoryStream();
                    var file = Request.Files[0];
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    arquivo.Nome = file.FileName;
                    arquivo.Arquivo = data;
                    arquivo.Tamanho = file.ContentLength;
                    _catalogoServico.UploadArquivoCatalogo(arquivo, catalogoProtheusApi);
                }

            }
            catch (Exception ex)
            {
                arquivo.LogImportacao = ex.Message;

            }

            return View("Index", arquivo);
        }

        public ActionResult List()
        {
            return View(_catalogoServico.Obter());
        }

     
        public async Task ImportacaoCadastrosBasicos(bool isFirstLoad = false)
        {
            try
            {
                await _cargaCadastrosBasicosService.SyncMarcas(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncModelos(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncBancos(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncTipoVenda(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncVendedores(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncOperador(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncTransportadoras(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncLojasDellaVia(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncParametroGeral(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaCadastrosBasicosService.SyncAdministracaoFinanceira(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ImportacaoDesconto(bool isFirstLoad = false)
        {
            try
            {
                await _cargaDescontosServico.SyncDescontoModeloDeVenda(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaDescontosServico.SyncDescontoVendaAlcada(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaDescontosServico.SyncDescontoVendaAlcadaGrupoProduto(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task ImportacaoVeiculos()
        {
            try
            {
                await _cargaVeiculoServico.SyncVeiculo();
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaVeiculoProdutosServico.SyncProdutos();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ImportarProduto(bool isFirstLoad = false)
        {
            try
            {
                await _cargaProdutoServico.SyncGrupoProduto(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaProdutoServico.SyncProduto(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaProdutoServico.SyncGrupoServicoAgregadosProdutos(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaProdutoServico.SyncProdutoComplemento(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaVeiculoProdutosServico.SyncProdutos();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ImportacaoCliente(bool isFirstLoad = false)
        {
            try
            {
                await _cargaClienteServico.SyncCliente(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaClienteServico.SyncClienteVeiculo(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ImportarPreco(bool isFirstLoad = false)
        {
            try
            {
                await _cargaPrecoServico.SyncTabelaPreco(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                await _cargaPrecoServico.SyncTabelaPrecoItem(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task ImportarConvenio(bool isFirstLoad = false)
        {
            try
            {
                await _cargaConvenioServico.SyncConvenio(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaConvenioServico.SyncCondicaoPagamento(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaConvenioServico.SyncConvenioCondicaoPagamento(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaConvenioServico.SyncConvenioCliente(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaConvenioServico.SyncConvenioProduto(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ImportacaoOrcamentos(bool isFirstLoad = false)
        {

            try
            {
                await _cargaOrcamentoServico.SyncOrcamento(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _cargaOrcamentoServico.SyncSolicitacaoAnaliseCredito(isFirstLoad);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /**
         * Importação de produtos diferentes de pneu
         * Produtos importados da tabela VW_VEICULO_PRODUTOS
         */
        public async Task ImportacaoProdutosFraga()
        {
            try
            {
                await _cargaVeiculoProdutosServico.SyncProdutos();
            } catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ProcessaCargaCatalogoHistorica(bool isFirstLoad = false)
        {

            try
            {
                _catalogoServico.ProcessaCatalogoHistoricoProtheus();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}