using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Resources;
using sievis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    //[AppAuthorize(Roles = "AD")]
    public class UsuariosController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        private ApplicationUserManager _userManager;
        #endregion

        #region Methods
        private void LoadCombos(AppUsers entity) {            
            ViewBag.RoleList = AppEnum.GetRoles().ToSelectList(entity.RoleId);
            ViewBag.EstatusList = AppEnum.GetEstatusUsuario().ToSelectList();

            ViewBag.Gerencia_id = (entity.Gerencia_id != null) 
                ? new SelectList(db.Gerencia, "id", "nombre", entity.Gerencia_id)
                : new SelectList(db.Gerencia, "id", "nombre");

            ViewBag.Zona_id = (entity.Zona_id != null)
                ? new SelectList(db.Zona, "id", "nombre", entity.Zona_id)
                : new SelectList(db.Zona, "id", "nombre");

            ViewBag.Subestacion_id = (entity.Subestacion_id != null)
                ? new SelectList(db.Subestacion, "id", "nombre", entity.Subestacion_id)
                : new SelectList(db.Subestacion, "id", "nombre");
        }

        private RedirectToRouteResult RedirectToErrorList(string toActionName, string toControllerName, List<string> errorList) {
            //-- redirect from
            TempData["actionName"] = "Catalogos";
            TempData["controllerName"] = "Administracion";
            //-- error list
            TempData["ErrorList"] = errorList;
            return RedirectToAction(toActionName, toControllerName);
        }
        #endregion

        #region UserManager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        #region Http-Rest
        [HttpGet]
        public ActionResult Index()
        {
            var entity = db.AppUsers.Include(a => a.AspNetUsers).Include(a => a.AspNetRoles).Include(a => a.Gerencia).Include(a => a.Zona).Include(a => a.Subestacion);
            return View(entity.ToList());
        }

        [HttpGet]
        public ActionResult Detalle(string id)
        {
            AppUsers entity = db.AppUsers.Include(a => a.AspNetRoles).Include(a => a.Gerencia).Include(a => a.Zona).Include(a => a.Subestacion).Where(x => x.UserId == id).FirstOrDefault();
            return (entity != null) ? PartialView("Detalle", entity) : PartialView(Resources.AppResources.UrlViewNoEncontrado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Validate(AppUsers entity)
        {
            List<string> errorList = new List<string>();
            if (!ModelState.IsValid)
                errorList.AddRange(ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList());

            if (string.IsNullOrWhiteSpace(entity.NombreCompleto))
                errorList.Add("Campo Nombre Completo es obligatorio");
            if (string.IsNullOrWhiteSpace(entity.Email))
                errorList.Add("Campo Email es obligatorio");
            if (string.IsNullOrWhiteSpace(entity.Estatus))
                errorList.Add("Campo Estatus es obligatorio");

            bool continueVal = true;
            if (entity.Action == Resources.AppResources.Agregar) {
                if (string.IsNullOrWhiteSpace(entity.Password))
                {
                    errorList.Add("Campo Contraseña es obligatorio");
                    continueVal = false;
                }
                if (string.IsNullOrWhiteSpace(entity.RePassword)) {
                    errorList.Add("Campo Reescribir Contraseña es obligatorio");
                    continueVal = false;
                }

                if (continueVal) {
                    if (entity.Password != entity.RePassword)
                    {
                        errorList.Add("Contraseña y Reescribir Contraseña deben coincidir");
                    }
                    else
                    {
                        var resultValPsw = await UserManager.PasswordValidator.ValidateAsync(entity.Password);
                        if (!resultValPsw.Succeeded) {
                            errorList.Add("Contraseña No cumple con las Reglas de Seguridad");
                        }
                    }
                }                
            }

            if (errorList.Count == 0)
                return Json(new JsonResponse<string>() { Success = true });
            else
                return Json(new JsonResponse<string>() { Success = false, Message = AppResources.ValidationFailMessage, Data = JsonConvert.SerializeObject(errorList) });
        }

        [HttpGet]
        public ActionResult Agregar()
        {            
            AppUsers entity = new AppUsers() { Action = Resources.AppResources.Agregar };
            LoadCombos(entity);
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(AppUsers entity)
        {            
            var errorList = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    //-- saves in [AspNetUsers]
                    var user = new ApplicationUser { UserName = entity.Email, Email = entity.Email };
                    var result = UserManager.Create(user, entity.Password);
                    if (result.Succeeded)
                    {
                        //-- saves in [AspNetRoles]
                        var result2 = UserManager.AddToRole(user.Id, entity.RoleId);

                        if (result2.Succeeded)
                        {
                            AspNetUsers aspnetUser = db.AspNetUsers.Where(x => x.Email == entity.Email).FirstOrDefault();

                            //-- saves in [AppUsers]
                            entity.AspNetUsers = aspnetUser;
                            db.AppUsers.Add(entity);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            errorList.AddRange(result2.Errors.ToList());
                            return RedirectToErrorList("Index", "Error", errorList);
                        }
                    }
                    else {
                        errorList.AddRange(result.Errors.ToList());
                        return RedirectToErrorList("Index", "Error", errorList);
                    }                    
                }
            }
            catch (DbEntityValidationException entityException)
            {
                foreach (var eve in entityException.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        errorList.Add(ve.ErrorMessage);

                return RedirectToErrorList("Index", "Error", errorList);
            }
            catch (Exception ex) {
                errorList.Add(ex.Message);
                return RedirectToErrorList("Index", "Error", errorList);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Editar(string id)
        {
            AppUsers entity = db.AppUsers.Include(a => a.AspNetRoles).Include(a => a.Gerencia).Include(a => a.Zona).Include(a => a.Subestacion).Where(x => x.UserId == id).FirstOrDefault();            
            if (entity == null) return PartialView(Resources.AppResources.UrlViewNoEncontrado);
            entity.Email = entity.AspNetUsers.Email;
            entity.Action = Resources.AppResources.Editar;
            LoadCombos(entity);
            return PartialView("Editar", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(AppUsers entity)
        {            
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new EmptyResult();            
        }

        [HttpGet]
        public ActionResult Eliminar(string id)
        {
            AppUsers entity = db.AppUsers.Include(a => a.AspNetRoles).Include(a => a.Gerencia).Include(a => a.Zona).Include(a => a.Subestacion).Where(x => x.UserId == id).FirstOrDefault();
            return (entity != null) ? PartialView("Eliminar", entity) : PartialView(Resources.AppResources.UrlViewNoEncontrado);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmed(string id)
        {
            AppUsers entity = db.AppUsers.Where(x => x.UserId == id).FirstOrDefault();
            db.AppUsers.Remove(entity);
            AspNetUsers entityUsr = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(entityUsr);
            db.SaveChanges();
            return RedirectToAction("Index");
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
