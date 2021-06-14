using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sievis.Models;

namespace sievis.Controllers
{
    [SessionExpire]
    [AppAuthorize(Roles = "AD,AZ")]
    public class AdministracionController : Controller
    {
        // GET: Administracion
        public ActionResult Catalogos()
        {
            return View();
        }
    }
}