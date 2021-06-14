using Resources;
using sievis.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    public class GerenciasController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        #endregion

        #region HTTP-REST [CRUD]
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Gerencia.ToList());
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            var entity = (from t in db.Gerencia where t.id == id select t).First();
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Validate(Gerencia entity)
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
            return PartialView();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Gerencia entity)
        {
            if (ModelState.IsValid)
            {
                db.Gerencia.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            var entity = (from t in db.Gerencia where t.id == id select t).First();
            return PartialView("Eliminar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var entity = (from t in db.Gerencia where t.id == id select t).First();
            db.Gerencia.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var entity = (from t in db.Gerencia where t.id == id select t).First();
            return PartialView("Editar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Gerencia entity)
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
