using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sievis.Models;

namespace sievis.Controllers.sievis._TEMP
{
    public class PruebasTempController : Controller
    {
        private ModeloSievis db = new ModeloSievis();

        // GET: PruebasTemp
        public ActionResult Index()
        {
            var prueba = db.Prueba.Include(p => p.Equipo);
            return View(prueba.ToList());
        }

        // GET: PruebasTemp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prueba prueba = db.Prueba.Find(id);
            if (prueba == null)
            {
                return HttpNotFound();
            }
            return View(prueba);
        }

        // GET: PruebasTemp/Create
        public ActionResult Create()
        {
            ViewBag.Equipo_id = new SelectList(db.Equipo, "id", "bahia");
            return View();
        }

        // POST: PruebasTemp/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Equipo_id,fecha_prueba,fecha_inspeccion,instrumento_medicionSF6,existe_gabinete_centralizador,existe_gabinetectrl_xfase,evalBasica,evalExtendida")] Prueba prueba)
        {
            if (ModelState.IsValid)
            {
                db.Prueba.Add(prueba);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Equipo_id = new SelectList(db.Equipo, "id", "bahia", prueba.Equipo_id);
            return View(prueba);
        }

        // GET: PruebasTemp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prueba prueba = db.Prueba.Find(id);
            if (prueba == null)
            {
                return HttpNotFound();
            }
            ViewBag.Equipo_id = new SelectList(db.Equipo, "id", "bahia", prueba.Equipo_id);
            return View(prueba);
        }

        // POST: PruebasTemp/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Equipo_id,fecha_prueba,fecha_inspeccion,instrumento_medicionSF6,existe_gabinete_centralizador,existe_gabinetectrl_xfase,evalBasica,evalExtendida")] Prueba prueba)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prueba).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Equipo_id = new SelectList(db.Equipo, "id", "bahia", prueba.Equipo_id);
            return View(prueba);
        }

        // GET: PruebasTemp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prueba prueba = db.Prueba.Find(id);
            if (prueba == null)
            {
                return HttpNotFound();
            }
            return View(prueba);
        }

        // POST: PruebasTemp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prueba prueba = db.Prueba.Find(id);
            db.Prueba.Remove(prueba);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
