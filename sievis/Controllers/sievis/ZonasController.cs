using Resources;
using sievis.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace sievis.Controllers
{
    [SessionExpire]
    [AppAuthorize]
    public class ZonasController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        #endregion

        #region HTTP-REST [CRUD]
        [HttpGet]
        public ActionResult Index()
        {
            var data = (from z in db.Zona
                        join g in db.Gerencia on z.Gerencia_id equals g.id
                        select new { Gerencia = g, Gerencia_id = g.id, id = z.id, nombre = z.nombre }).ToList().
                        Select(x => new Zona { Gerencia = x.Gerencia, Gerencia_id = x.Gerencia_id, id = x.id, nombre = x.nombre }).ToList();

            ViewBag.Zonas = data;
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            var entity = (from t in db.Zona where t.id == id select t).First();
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Validate(Zona entity)
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
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Zona entity)
        {
            ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre");
            if (ModelState.IsValid)
            {
                db.Zona.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            var entity = (from t in db.Zona where t.id == id select t).First();
            return PartialView("Eliminar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var entity = (from t in db.Zona where t.id == id select t).First();
            db.Zona.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var entity = (from t in db.Zona where t.id == id select t).First();
            ViewBag.Gerencia = (from t in db.Gerencia where t.id == entity.Gerencia_id select t).First();
            return PartialView("Editar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Zona entity)
        {
            ViewBag.Gerencia = (from t in db.Gerencia where t.id == entity.Gerencia_id select t).First();
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
