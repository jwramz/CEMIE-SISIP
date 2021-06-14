using Resources;
using sievis.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    public class ModelosController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        #endregion

        #region HTTP-REST [CRUD]
        [HttpGet]
        public ActionResult Index()
        {
            var data = (from mm in db.Modelo
                        join m in db.Marca on mm.Marca_id equals m.id
                        select new { Marca = m, Marca_id = m.id, id = mm.id, nombre = mm.nombre }).ToList().
                        Select(x => new Modelo { Marca = x.Marca, Marca_id = x.Marca_id, id = x.id, nombre = x.nombre }).ToList();

            ViewBag.Modelo = data;
            return View(db.Modelo.ToList());
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            var entity = (from t in db.Modelo where t.id == id select t).First();
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Validate(Modelo entity)
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
            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre");
            return PartialView( );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Modelo entity)
        {
            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre");
            if (ModelState.IsValid)
            {
                db.Modelo.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            var entity = (from t in db.Modelo where t.id == id select t).First();
            return PartialView("Eliminar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var entity = (from t in db.Modelo where t.id == id select t).First();
            db.Modelo.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var entity = (from t in db.Modelo where t.id == id select t).First();
            ViewBag.Marca = (from t in db.Marca where t.id == entity.Marca_id select t).First();
            return PartialView("Editar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Modelo entity)
        {
            ViewBag.Marca = (from t in db.Marca where t.id == entity.Marca_id select t).First();
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
