using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Repository;

namespace DV.FrenteLoja.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
	    private readonly IHomeServico _homeServico;
        private readonly HomeRepository _homeRepository;

        public HomeController(IHomeServico homeServico, HomeRepository homeRepository)
	    {
		    _homeServico = homeServico;
            _homeRepository = homeRepository;

        }

        public ActionResult Index()
        {
            //var model = _homeServico.ObterInformacoesIniciais();
            var model = _homeRepository.ObterInformacoesIniciais();
            return View("Index", model);
        }

     
    }
}