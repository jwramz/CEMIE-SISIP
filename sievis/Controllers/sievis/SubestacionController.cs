using Resources;
using sievis.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    public class SubestacionController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        #endregion

        #region HTTP-REST [CRUD]
        [HttpGet]
        public ActionResult Index()
        {
            var data = (from s in db.Subestacion
                        join z in db.Zona on s.Zona_id equals z.id
                        join g in db.Gerencia on z.Gerencia_id equals g.id
                        select new { Zona = z, Gerencia = g, Zona_id = z.id, id = s.id, nombre = s.nombre }).ToList().
                        Select(x => new Subestacion { id=x.id, nombre=x.nombre,
                                                      Zona = new Zona() { id=x.Zona.id, nombre=x.Zona.nombre,
                                                      Gerencia = new Gerencia() { id=x.Gerencia.id, nombre=x.Gerencia.nombre }}}).ToList();

            ViewBag.Subestaciones = data;
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            var entity = (from t in db.Subestacion where t.id == id select t).First();
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Validate(Subestacion entity)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(entity.nombre))
            {
                return Json(new JsonResponse<string>() { Success = true });
            }
            return Json(new JsonResponse<string>() { Success = false, Message = AppResources.ValidationFailMessage });
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre");
            ViewBag.Zona_id = new SelectList(db.Zona, "id", "nombre");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Subestacion entity)
        {
            if (ModelState.IsValid)
            {
                db.Subestacion.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            var entity = (from t in db.Subestacion where t.id == id select t).First();
            return PartialView("Eliminar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var entity = (from t in db.Subestacion where t.id == id select t).First();
            db.Subestacion.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var entity = (from t in db.Subestacion where t.id == id select t).First();
            ViewBag.Gerencia = (from t in db.Gerencia where t.id == entity.Gerencia_id select t).First();
            ViewBag.Zona = (from t in db.Zona where t.id == entity.Zona_id select t).First();            

            return PartialView("Editar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Subestacion entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
