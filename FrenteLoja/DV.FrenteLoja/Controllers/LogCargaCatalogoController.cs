using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Util;
using Newtonsoft.Json;

namespace DV.FrenteLoja.Controllers
{
    public class LogCargaCatalogoController : Controller
    {
		private readonly ILogCargaCatalogoServico _logCargaCatalogoServico;
	    public LogCargaCatalogoController(ILogCargaCatalogoServico logCargaCatalogoServico)
	    {
		    _logCargaCatalogoServico = logCargaCatalogoServico;
	    }
	    // GET: LogIntegracao
	    public ActionResult Index()
	    {

		    return View(new LogCargaCatalogoDto());
	    }

	    public ActionResult ObterLogIntegracao(LogCargaCatalogoDto model)
	    {
		    JsonNetResult jsonNetResult = new JsonNetResult();
		    jsonNetResult.Formatting = Formatting.Indented;
		    jsonNetResult.Data = _logCargaCatalogoServico.ObterLogIntegracao(model);

		    return jsonNetResult;
	    }

	    public ActionResult ObterTextoErro(long id)
	    {
		    var textoErro = _logCargaCatalogoServico.ObterTextoErro(id);
		    return PartialView("_ModalLogErro", textoErro);
	    }
	}
}