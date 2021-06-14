using System.Collections.Generic;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.actionName = TempData["actionName"];
            ViewBag.controllerName = TempData["controllerName"];
            ViewBag.ErrorList = TempData["ErrorList"] as List<string>;
            return View();
        }
    }
}