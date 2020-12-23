using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Exceptions;
using System.Diagnostics;
using System.Net.Mail;
using System.IO;
using System.Text;
using NReco.PdfGenerator;
using DV.FrenteLoja.Filters;
using DV.FrenteLoja.Repository;

namespace DV.FrenteLoja.Controllers
{
    [Authorize]
    [AuthorizationFilter(FlagPerfilAcessoUsuario = new[] { PerfilAcessoUsuario.TMK, PerfilAcessoUsuario.FrenteLoja })]
    public class CheckupController : Controller
    {
        //private OrcamentoRepository _orcamentoRepository = new OrcamentoRepository();
        private readonly ICheckupCarServico<CheckupCarDto> _checkupCarServico;
        private readonly ICheckupTruckServico<CheckupTruckDto> _checkupTruckServico;
        private readonly IVendedorServico _vendedorServico;
        private readonly IOrcamentoServico _orcamentoServico;
        private readonly ICheckupServico<CheckupDto> _checkupServico;

        private OrcamentoRepository _orcamentoRepository;

        public CheckupController(IVendedorServico veiculoServico, 
            ICheckupCarServico<CheckupCarDto> checkupCarServico,
            IOrcamentoServico orcamentoServico, 
            ICheckupTruckServico<CheckupTruckDto> checkupTruckServico,
            ICheckupServico<CheckupDto> checkupServico,

            OrcamentoRepository orcamentoRepository)
        {
            _vendedorServico = veiculoServico;
            _checkupCarServico = checkupCarServico;
            _orcamentoServico = orcamentoServico;
            _checkupTruckServico = checkupTruckServico;
            _checkupServico = checkupServico;

            _orcamentoRepository = orcamentoRepository;
        }

        [HttpGet]
        public ActionResult Consultar(string id, string tipo)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    CheckupDto checkup = new CheckupDto();
                    if (tipo == "car")
                        checkup.IsCheckupCar = true;
                    return View("StepCommon", checkup);
                }
                else
                {
                    CheckupDto checkup = _checkupServico.ObterPorId(Int64.Parse(id));
                    return View("StepCommon", checkup);
                }
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


        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                    _checkupServico.Excluir(Int64.Parse(id));
                else
                    throw new Exception("ID não informado.");

                return Index();
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return Index();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult Search(string id, string tipoPesquisa, string tipoCheckup)
        {
            try
            {
                ModalOrcamentosDto orcamentos = new ModalOrcamentosDto()
                {
                    Varejo = tipoCheckup.ToLower() == "varejo"
                };
                switch (tipoPesquisa.ToLower())
                {
                    case "codigo-cliente":
                        {
                            orcamentos.Orcamentos = _orcamentoServico.ObterOrcamentoPorCodigoCliente(id);
                        }
                        break;
                    case "cpf":
                        {
                            id = id.Replace(".", "").Replace("-", "").Replace("/", "");
                            orcamentos.Orcamentos = _orcamentoServico.ObterOrcamentoPorCPF(id);
                        }
                        break;
                    case "cnpj":
                        {
                            id = id.Replace(".", "").Replace("-", "").Replace("/", "");
                            orcamentos.Orcamentos = _orcamentoServico.ObterOrcamentoPorCNPJ(id);
                        }
                        break;
                    default:
                        return Json("houve um problema na busca.");
                }

                if (orcamentos.Orcamentos.Count > 1)
                {
                    return PartialView("_ModalOrcamentos", orcamentos);
                }
                else
                {
                    return Json(new { id = orcamentos.Orcamentos.FirstOrDefault().Id });
                }

            }
            catch (NegocioException ex)
            {
                TempData["Erro"] = ex.Message;
                return View("StepCommon");
            }
            catch (Exception e)
            {
                TempData["Erro"] = e.Message;
                return View("_ModalOrcamentos");
            }
        }

        private CheckupDto BuscaOrcamentoCheckup(long? id)
        {
            long idOrcamento;
            Int64.TryParse(id.ToString(), out idOrcamento);
            OrcamentoDto orcamento = _orcamentoRepository.ObterOrcamentoPorId(idOrcamento);
            CheckupDto checkupDto = new CheckupDto()
            {
                OrcamentoId = orcamento.Id.ToString(),
                Cliente = orcamento.NomeCliente,
                CPFCNPJ = orcamento.CPFCNPJCliente,
                Phone = orcamento.TelefoneCliente,
                License = orcamento.PlacaVeiculo,
                Marca = orcamento.MarcaVeiculoDescricao,
                Car = orcamento.ModeloVeiculoDescricao,
                Versao = orcamento.VersaoVeiculoDescricao,
                Year = orcamento.AnoVeiculo,
                KM = orcamento.QuilometragemVeiculo,
                ProdutoServico = orcamento.InformacoesCliente,
                VendedorNome = orcamento.VendedorDescricao
            };
            var vendedor = _vendedorServico.ObterVendedorPorNome(orcamento.VendedorDescricao, 10, 1).FirstOrDefault();
            if (vendedor != null)
            {
                checkupDto.Vendedor = vendedor.Id.ToString();
            }
            return checkupDto;
        }
        public ActionResult CheckupVarejo(long? id)
        {
            try
            {
                if (id == null)
                    throw new NegocioException("Código do Orçamento não informado.");
                var checkupDto = BuscaOrcamentoCheckup(id);
                checkupDto.IsCheckupCar = true;
                return View("StepCommon", checkupDto);
            }
            catch (NegocioException ex)
            {
                TempData["Erro"] = ex.Message;
                CheckupDto checkup = new CheckupDto() { IsCheckupCar = true };
                return View("StepCommon", checkup);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult CheckupTruck(long? id)
        {
            try
            {
                if (id == null)
                    throw new NegocioException("Código do Orçamento não informado.");
                var checkupDto = BuscaOrcamentoCheckup(id);
                checkupDto.IsCheckupCar = false;
                return View("StepCommon", checkupDto);
            }
            catch (NegocioException ex)
            {
                TempData["Erro"] = ex.Message;
                return View("StepCommon", new CheckupDto());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult StepOne(CheckupDto checkupDTO)
        {
            try
            {
                if (String.IsNullOrEmpty(checkupDTO.OrcamentoId))
                    throw new NegocioException("Orçamento não informado.");

                if (checkupDTO.IsCheckupCar)
                {
                    CheckupCarDto checkupCar = (CheckupCarDto)checkupDTO;
                    if (checkupDTO.CheckupId != 0)
                        checkupCar = _checkupCarServico.ObterPorId(checkupDTO.CheckupId);
                    return View("StepOneCar", checkupCar);
                }
                else
                {
                    CheckupTruckDto checkupTruck = (CheckupTruckDto)checkupDTO;
                    if (checkupDTO.CheckupId != 0)
                        checkupTruck = _checkupTruckServico.ObterPorId(checkupDTO.CheckupId);
                    return View("StepOneTruck", checkupTruck);
                }
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepCommon", checkupDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        #region [CAR]
        [HttpPost]
        public ActionResult StepTwoCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepTwoCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepOneCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepThreeCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepThreeCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepTwoCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepFourCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepFourCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepThreeCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepFiveCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepFiveCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepFourCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepSixCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepSixCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepFiveCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepSevenCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepSevenCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepSixCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepEightCar(CheckupCarDto checkupCarDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepEightCar", checkupCarDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepSevenCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveCheckupCar(CheckupCarDto checkupCarDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }
                if (checkupCarDTO.CheckupId == 0)
                {
                    checkupCarDTO.CheckupId = _checkupCarServico.Cadastrar(checkupCarDTO);
                }
                else
                {
                    _checkupCarServico.Atualizar(checkupCarDTO);
                }
                return Index();
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepEightCar", checkupCarDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        #endregion

        #region [TRUCK]
        [HttpPost]
        public ActionResult StepOneTruck(CheckupTruckDto checkupTruckDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepTwoTruck", checkupTruckDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepOneTruck", checkupTruckDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult StepTwoTruck(CheckupTruckDto checkupTruckDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepThreeTruck", checkupTruckDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepTwoTruck", checkupTruckDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        public ActionResult StepThreeTruck(CheckupTruckDto checkupTruckDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                return View("StepThreeTruck", checkupTruckDTO);
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepTwoTruck", checkupTruckDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveCheckupTruck(CheckupTruckDto checkupTruckDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NegocioException("Não foi possível prosseguir");
                }

                if (checkupTruckDTO.CheckupId == 0)
                {
                    checkupTruckDTO.CheckupId = _checkupTruckServico.Cadastrar(checkupTruckDTO);
                }
                else
                {
                    _checkupTruckServico.Atualizar(checkupTruckDTO);
                }
                return Index();
            }
            catch (NegocioException e)
            {
                TempData["Erro"] = e.Message;
                return View("StepEightCar", checkupTruckDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        #endregion

        [HttpGet]
        public ActionResult ObterVendedor(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            var count = _vendedorServico.TamanhoVendedorPorTermo(termoBusca);
            List<VendedorDto> data = _vendedorServico.ObterVendedorPorNome(termoBusca, tamanhoPagina, numeroPagina);

            return new JsonResult()
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ObterTecnico(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            var count = _vendedorServico.TamanhoVendedorPorTermo(termoBusca);
            List<VendedorDto> data = _vendedorServico.ObterVendedorPorNome(termoBusca, tamanhoPagina, numeroPagina);

            return new JsonResult()
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ObterEspecificacao(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            var count = _checkupCarServico.TamanhoModelosPorTermo(termoBusca);
            List<string> data = _checkupCarServico.ObterEspecificacao(termoBusca, tamanhoPagina, numeroPagina);

            return new JsonResult()
            {
                Data = new { data, count },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult ObterListaCheckupsPorTipo(string tipoConsulta, string codigo)
        {
            try
            {
                List<CheckupDto> checkups;
                switch (tipoConsulta)
                {
                    case "nomecliente":
                        {
                            checkups = _checkupServico.ObterCheckupsNomeCliente(codigo);
                        }
                        break;
                    case "cpf":
                        {
                            codigo = codigo.Replace(".", "").Replace("-", "").Replace("/", "");
                            checkups = _checkupServico.ObterCheckupsCPF(codigo);
                        }
                        break;
                    case "cnpj":
                        {
                            codigo = codigo.Replace(".", "").Replace("-", "").Replace("/", "");
                            checkups = _checkupServico.ObterCheckupsCNPJ(codigo);
                        }
                        break;
                    case "placa":
                        {
                            codigo = codigo.Replace("-", "");
                            checkups = _checkupServico.ObterCheckupsPlaca(codigo);
                        }
                        break;
                    case "veiculo":
                        {
                            checkups = _checkupServico.ObterCheckupsVeiculo(codigo);
                        }
                        break;
                    case "truck":
                        {
                            checkups = _checkupServico.ObterCheckupsTruck();
                        }
                        break;
                    case "car":
                        {
                            checkups = _checkupServico.ObterCheckupsCar();
                        }
                        break;
                    case "idcheckup":
                        {
                            long id = Int64.Parse(codigo);
                            checkups = new List<CheckupDto>() { _checkupServico.ObterPorId(id) };
                        }
                        break;
                    case "idorcamento":
                        {
                            checkups = _checkupServico.ObterCheckupsIdOrcamento(codigo);
                        }
                        break;
                    case "usuario":
                        {
                            checkups = _checkupServico.ObterCheckupsUsuario();
                        }
                        break;
                    default:
                        {
                            checkups = _checkupServico.ObterCheckupsUsuario();
                        }
                        break;

                }
                return new JsonResult()
                {
                    Data = checkups,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            catch (NegocioException)
            {
                return new JsonResult()
                {
                    Data = new List<CheckupDto>(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult Report(long? id)
        {
            try
            {
                var checkupDto = _checkupServico.ObterPorId(id);
                if (checkupDto.IsCheckupCar)
                {
                    var checkupCarDTO = _checkupCarServico.ObterPorId(id);
                    return View("ReportCar", (CheckupCarDto)checkupCarDTO);
                }
                else
                {
                    var checkupTruckDTO = _checkupTruckServico.ObterPorId(id);
                    return View("ReportTruck", (CheckupTruckDto)checkupTruckDTO);
                }
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


        #region [PDF]


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ReportPdf(long? id)
        {
            var checkupDto = _checkupServico.ObterPorId(id);
            if (checkupDto.IsCheckupCar)
            {
                var checkupCarDTO = _checkupCarServico.ObterPorId(id);
                return View("ReportCarPdf", (CheckupCarDto)checkupCarDTO);
            }
            else
            {
                var checkupTruckDTO = _checkupTruckServico.ObterPorId(id);
                return View("ReportTruckPdf", (CheckupTruckDto)checkupTruckDTO);
            }
        }

        public ActionResult EnviarPorEmail(string EmailCliente, long? Id)
        {
            try
            {
                if (Id == null)
                    throw new NegocioException("Checkup não informado.");

                string url = string.Format("http://" + HttpContext.Request.Url.Authority + "/checkup/reportpdf/" + Id);

                var htmlToPdf = new HtmlToPdfConverter() { LowQuality = false };
                var pdfBytesFromUrl = htmlToPdf.GeneratePdfFromFile(url, null);

                Stream stream = new MemoryStream(pdfBytesFromUrl);
                Attachment anexo = new Attachment(stream, "Checkup-" + Id + ".pdf");
                StringBuilder html = new StringBuilder();
                html.AppendFormat(@"<p>Ol&aacute;,</p>
                                        <p>O checkup realizado na DELLA VIA se encontra no anexo deste e-mail.</ p >
                                        <p>Atenciosamente,</ p >
                                        <p>DELLA VIA</p > ");
                Util.EnvioEmail.EnviarEmail(EmailCliente, "Confira o checkup realizado na DELLA VIA",
                    html.ToString(), anexo);
                TempData["sucesso"] = "E-mail enviado com sucesso!";
                return Report(Id);
            }
            catch (NegocioException e)
            {
                TempData["erro"] = e.Message;
                return Report(Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        public ActionResult Imprimir(long? Id)
        {
            try
            {
                if (Id == null)
                    throw new NegocioException("Checkup não informado.");

                string url = string.Format("http://" + HttpContext.Request.Url.Authority + "/checkup/reportpdf/" + Id);
                var htmlToPdf = new HtmlToPdfConverter() { LowQuality = false };
                var pdfBytesFromUrl = htmlToPdf.GeneratePdfFromFile(url, null);
                Stream stream = new MemoryStream(pdfBytesFromUrl);
                return File(stream, "application/pdf", "Checkup-" + Id + ".pdf");
            }
            catch (NegocioException e)
            {
                TempData["Retorno"] = e.Message;
                return Report(Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }

        }

        #endregion
    }
}