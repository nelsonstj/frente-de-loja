using System.Web.Mvc;
using DV.FrenteLoja.Infra;
using DV.FrenteLoja.Filters;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Controllers
{
    [Authorize]
    [AuthorizationFilter(FlagPerfilAcessoUsuario = new[] { PerfilAcessoUsuario.Administrativo })]
    public class LogController : Controller
    {
        
        public ActionResult Index()
        {
            return new ElmahResult(string.Empty);
        }        
        
        public ActionResult Stylesheet() { return new ElmahResult("stylesheet"); }
       
        public ActionResult Rss() { return new ElmahResult("rss"); }
       
        public ActionResult DigestRss() { return new ElmahResult("digestrss"); }
        
        public ActionResult About() { return new ElmahResult("about"); }
        
        public ActionResult Detail() { return new ElmahResult("detail"); }
        
        public ActionResult Download() { return new ElmahResult("download"); }
        
        public ActionResult Json() { return new ElmahResult("json"); }
        
        public ActionResult Xml() { return new ElmahResult("xml"); }
    }
}