using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sievis.Models;

namespace sievis.Controllers
{
    public class EquipoTempController : Controller
    {
        private ModeloSievis db = new ModeloSievis();

        // GET: EquipoTemp
        public ActionResult Index()
        {
            var equipo = db.Equipo.Include(e => e.AplicacionInterruptor).Include(e => e.Modelo.Marca).Include(e => e.Mecanismo).Include(e => e.Modelo).Include(e => e.Subestacion);
            return View(equipo.ToList());
        }

        // GET: EquipoTemp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = db.Equipo.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        // GET: EquipoTemp/Create
        public ActionResult Create()
        {
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion");
            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre");
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion");
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre");
            ViewBag.Subestacion_id = new SelectList(db.Subestacion, "id", "nombre");
            return View();
        }

        // POST: EquipoTemp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Gerencia_id,Zona_id,Subestacion_id,Marca_id,Modelo_id,Mecanismo_id,AplicacionInterruptor_id,bahia,ns,anio_fabricacion,tension_nominal,corriente_nominal,corriente_cc,bil,disponibilidad_refaccion_st,presionSF6,presion_alarma,tipo_unidades_presion,altitud_operacion,altitud_instalacion,dis_estructural,conf_camaras,res_estatica_contactos,interruptor_contiene,interruptor_resistencia,interruptor_capacitor,fecha_puestaservicio,fultimo_mantenimiento,nivel_contaminacion,tipo_disparo,comando_cierre,distancia_fuga,clase_interruptor")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Equipo.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion", equipo.AplicacionInterruptor_id);
            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre", equipo.Modelo_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion, "id", "nombre", equipo.Subestacion_id);
            return View(equipo);
        }

        // GET: EquipoTemp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = db.Equipo.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion", equipo.AplicacionInterruptor_id);
            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre", equipo.Modelo_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion, "id", "nombre", equipo.Subestacion_id);
            return View(equipo);
        }

        // POST: EquipoTemp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Gerencia_id,Zona_id,Subestacion_id,Marca_id,Modelo_id,Mecanismo_id,AplicacionInterruptor_id,bahia,ns,anio_fabricacion,tension_nominal,corriente_nominal,corriente_cc,bil,disponibilidad_refaccion_st,presionSF6,presion_alarma,tipo_unidades_presion,altitud_operacion,altitud_instalacion,dis_estructural,conf_camaras,res_estatica_contactos,interruptor_contiene,interruptor_resistencia,interruptor_capacitor,fecha_puestaservicio,fultimo_mantenimiento,nivel_contaminacion,tipo_disparo,comando_cierre,distancia_fuga,clase_interruptor")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion", equipo.AplicacionInterruptor_id);
            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre", equipo.Modelo_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion, "id", "nombre", equipo.Subestacion_id);
            return View(equipo);
        }

        // GET: EquipoTemp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = db.Equipo.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        // POST: EquipoTemp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipo equipo = db.Equipo.Find(id);
            db.Equipo.Remove(equipo);
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
