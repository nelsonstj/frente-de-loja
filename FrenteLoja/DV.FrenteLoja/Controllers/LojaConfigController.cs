using System.Net;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.Contratos.Interfaces;

namespace DV.FrenteLoja.Controllers
{
    public class LojaConfigController : Controller
    {
        private readonly LojaConfigRepository _LojaConfigRepository;
        private readonly ConvenioRepository _ConvenioRepository;

        public LojaConfigController(IRepositorioEscopo escopo, DellaviaContexto contexto, LojaConfigRepository LojaConfigRepository)
        {
            _LojaConfigRepository = LojaConfigRepository;
            _ConvenioRepository = new ConvenioRepository();
        }

        // GET: LojaConfig
        public ActionResult Index()
        {
            return View(_LojaConfigRepository.GetAll());
        }

        // GET: LojaConfig/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LojaConfig LojaConfig = _LojaConfigRepository.GetById(id);

            if (LojaConfig == null)
            {
                return HttpNotFound();
            }
            return View(LojaConfig);
        }

        // GET: LojaConfig/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LojaConfig/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idLoja,ConvenioPadrao,DescontoMax")] LojaConfig LojaConfig)
        {
            if (ModelState.IsValid)
            {
                if (_LojaConfigRepository.GetById(LojaConfig.idLoja) != null)
                {
                    ModelState.AddModelError("idLoja", "Config de loja já existente.");
                }
                else if (_ConvenioRepository.GetConvenio(LojaConfig.ConvenioPadrao) == null)
                {
                    ModelState.AddModelError("ConvenioPadrao", "Convenio inválido");
                }
                else
                {
                    _LojaConfigRepository.Insert(LojaConfig);
                    return RedirectToAction("Index");
                }
            }

            return View(LojaConfig);
        }

        // GET: LojaConfig/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LojaConfig LojaConfig = _LojaConfigRepository.GetById(id);

            if (LojaConfig == null)
            {
                return HttpNotFound();
            }
            return View(LojaConfig);
        }

        // POST: LojaConfig/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idLoja,ConvenioPadrao,DescontoMax")] LojaConfig LojaConfig)
        {
            if (ModelState.IsValid)
            {
                if (_ConvenioRepository.GetConvenio(LojaConfig.ConvenioPadrao) == null)
                {
                    ModelState.AddModelError("ConvenioPadrao", "Convenio inválido");
                }
                else
                {
                    _LojaConfigRepository.Update(LojaConfig);
                    return RedirectToAction("Index");
                }
            }
            return View(LojaConfig);
        }

        // GET: LojaConfig/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LojaConfig LojaConfig = _LojaConfigRepository.GetById(id);
            if (LojaConfig == null)
            {
                return HttpNotFound();
            }
            return View(LojaConfig);
        }

        // POST: LojaConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _LojaConfigRepository.DeleteById(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_contexto.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
