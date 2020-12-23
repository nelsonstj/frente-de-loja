using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Filters;
using DV.FrenteLoja.Util;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace DV.FrenteLoja.Controllers
{   [Authorize]
    [AuthorizationFilter(FlagPerfilAcessoUsuario = new[] { PerfilAcessoUsuario.Administrativo })]
    public class LogIntegracaoController : Controller
    {
        private readonly ILogIntegracaoServico _logIntegracaoServico;
        public LogIntegracaoController(ILogIntegracaoServico logIntegracaoServico)
        {
            _logIntegracaoServico = logIntegracaoServico;
        }
        // GET: LogIntegracao
        public ActionResult Index()
        {

            return View(new LogIntegracaoDto());
        }

        public ActionResult ObterLogIntegracao(LogIntegracaoDto model)
        {
            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = _logIntegracaoServico.ObterLogIntegracao(model);

            return jsonNetResult;
        }

        public ActionResult ObterTextoErro(long id)
        {
            var textoErro = _logIntegracaoServico.ObterTextoErro(id);
            return PartialView("_ModalLogErro", textoErro);
        }
    }
}