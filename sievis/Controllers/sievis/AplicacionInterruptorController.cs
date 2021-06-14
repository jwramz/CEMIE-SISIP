using Resources;
using sievis.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    public class AplicacionInterruptorController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        #endregion

        #region HTTP-REST [CRUD]
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.AplicacionInterruptor.ToList());
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            var entity = (from t in db.AplicacionInterruptor where t.id == id select t).First();
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Validate(AplicacionInterruptor entity)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(entity.descripcion))
            {
                return Json(new JsonResponse<string>() { Success = true });
            }
            return Json(new JsonResponse<string>() { Success = false, Message = AppResources.ValidationFailMessage });
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(AplicacionInterruptor entity)
        {
            if (ModelState.IsValid)
            {
                db.AplicacionInterruptor.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            var entity = (from t in db.AplicacionInterruptor where t.id == id select t).First();
            return PartialView("Eliminar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var entity = (from t in db.AplicacionInterruptor where t.id == id select t).First();
            db.AplicacionInterruptor.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var entity = (from t in db.AplicacionInterruptor where t.id == id select t).First();
            return PartialView("Editar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(AplicacionInterruptor entity)
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
