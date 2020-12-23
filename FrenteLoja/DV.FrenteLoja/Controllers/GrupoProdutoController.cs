using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Controllers
{
    public class GrupoProdutoController : Controller
    {
        private readonly GrupoProdutoRepository _grupoProdutoRepository;

        public GrupoProdutoController(IRepositorioEscopo escopo, DellaviaContexto contexto, GrupoProdutoRepository grupoProdutoRepository)
        {
            _grupoProdutoRepository = grupoProdutoRepository;
        }

        // GET: GrupoProduto
        public ActionResult Index()
        {
            return View(_grupoProdutoRepository.GetAll());
        }

        // GET: GrupoProduto/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GrupoProduto grupoProduto = _grupoProdutoRepository.GetById(id);

            if (grupoProduto == null)
            {
                return HttpNotFound();
            }
            return View(grupoProduto);
        }

        // GET: GrupoProduto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrupoProduto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdGrupoProduto,IdGrupoSubGrupo,Descricao,RegistroInativo,CampoCodigo,DataAtualizacao,UsuarioAtualizacao")] GrupoProduto grupoProduto)
        {
            if (ModelState.IsValid)
            {
                _grupoProdutoRepository.Insert(grupoProduto);
                return RedirectToAction("Index");
            }

            return View(grupoProduto);
        }

        // GET: GrupoProduto/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GrupoProduto grupoProduto = _grupoProdutoRepository.GetById(id);

            if (grupoProduto == null)
            {
                return HttpNotFound();
            }
            return View(grupoProduto);
        }

        // POST: GrupoProduto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdGrupoProduto,IdGrupoSubGrupo,Descricao,RegistroInativo,CampoCodigo,DataAtualizacao,UsuarioAtualizacao")] GrupoProduto grupoProduto)
        {
            if (ModelState.IsValid)
            {
                _grupoProdutoRepository.Update(grupoProduto);
                return RedirectToAction("Index");
            }
            return View(grupoProduto);
        }

        // GET: GrupoProduto/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoProduto grupoProduto = _grupoProdutoRepository.GetById(id);
            if (grupoProduto == null)
            {
                return HttpNotFound();
            }
            return View(grupoProduto);
        }

        // POST: GrupoProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            _grupoProdutoRepository.DeleteById(id);
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
