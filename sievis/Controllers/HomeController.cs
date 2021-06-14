using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sievis.Models;

namespace sievis.Controllers
{
    [SessionExpire]
    [AppAuthorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
           
            // if (HttpContext.Session. == null) RedirectToAction("Login", "Account");
        }
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Tablero()
        {
            ViewBag.Message = "Your dasboard page.";

            return View();
        }

        public ActionResult Reportes()
        {
            ViewBag.Message = "Your Reportes page.";

            return View();
        }             

    }
}