using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sievis.Models;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    public class ParametrosFabricanteController : Controller
    {
        private ModeloSievis db = new ModeloSievis();

        // GET: ParametrosFabricante
        public async Task<ActionResult> Index()
        {
            var parametrosFabricantePE = db.ParametrosFabricantePE.Include(p => p.Modelo);
            return View(await parametrosFabricantePE.ToListAsync());
        }

        // GET: ParametrosFabricante/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParametrosFabricantePE parametrosFabricantePE = await db.ParametrosFabricantePE.FindAsync(id);
            if (parametrosFabricantePE == null)
            {
                return HttpNotFound();
            }
            return View(parametrosFabricantePE);
        }

        // GET: ParametrosFabricante/Create
        public ActionResult Create()
        {
            ViewBag.MarcaId = new SelectList(db.Marca, "id", "nombre");
            ViewBag.ModeloId = new SelectList(db.Modelo, "id", "nombre");
            return View();
        }

        // POST: ParametrosFabricante/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,MarcaId,ModeloId,toTAperturaD1LimInf,toTAperturaD1LimSup,toTAperturaD2LimInf,toTAperturaD2LimSup,toTCierreLimInf,toTCierreLimSup,toTCierreApeCALimInf,toTCierreApeCALimSup,toTEntResPreLimInf,toTEntResPreLimSup,pdCarrTotalLimInf,pdCarrTotalLimSup,pdAnguloGiro,pdFactoConversion,daVelocidadLimInf,daVelocidadLimSup,daSobreviaje,daRebote,dcVelocidadLimInf,dcVelocidadLimSup,dcSobreviaje,dcRebote,dcPenetracionLimInf,dcPenetracionLimSup,boTensionNominal,boAperturaIPico,boCierreIPico,mtrTensionNominal,mtrCorrienteArranque,mtrTCargaResorteLimInf,mtrTCargaResorteLimSup")] ParametrosFabricantePE parametrosFabricantePE)
        {
            if (ModelState.IsValid)
            {
                db.ParametrosFabricantePE.Add(parametrosFabricantePE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MarcaId = new SelectList(db.Marca, "id", "nombre", parametrosFabricantePE.MarcaId);
            ViewBag.ModeloId = new SelectList(db.Modelo, "id", "nombre", parametrosFabricantePE.ModeloId);
            return View(parametrosFabricantePE);
        }

        // GET: ParametrosFabricante/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParametrosFabricantePE parametrosFabricantePE = await db.ParametrosFabricantePE.FindAsync(id);
            if (parametrosFabricantePE == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarcaId = new SelectList(db.Marca, "id", "nombre", parametrosFabricantePE.MarcaId);
            ViewBag.ModeloId = new SelectList(db.Modelo, "id", "nombre", parametrosFabricantePE.ModeloId);
            return View(parametrosFabricantePE);
        }

        // POST: ParametrosFabricante/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,MarcaId,ModeloId,toTAperturaD1LimInf,toTAperturaD1LimSup,toTAperturaD2LimInf,toTAperturaD2LimSup,toTCierreLimInf,toTCierreLimSup,toTCierreApeCALimInf,toTCierreApeCALimSup,toTEntResPreLimInf,toTEntResPreLimSup,pdCarrTotalLimInf,pdCarrTotalLimSup,pdAnguloGiro,pdFactoConversion,daVelocidadLimInf,daVelocidadLimSup,daSobreviaje,daRebote,dcVelocidadLimInf,dcVelocidadLimSup,dcSobreviaje,dcRebote,dcPenetracionLimInf,dcPenetracionLimSup,boTensionNominal,boAperturaIPico,boCierreIPico,mtrTensionNominal,mtrCorrienteArranque,mtrTCargaResorteLimInf,mtrTCargaResorteLimSup")] ParametrosFabricantePE parametrosFabricantePE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametrosFabricantePE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MarcaId = new SelectList(db.Marca, "id", "nombre", parametrosFabricantePE.MarcaId);
            ViewBag.ModeloId = new SelectList(db.Modelo, "id", "nombre", parametrosFabricantePE.ModeloId);
            return View(parametrosFabricantePE);
        }

        // GET: ParametrosFabricante/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParametrosFabricantePE parametrosFabricantePE = await db.ParametrosFabricantePE.FindAsync(id);
            if (parametrosFabricantePE == null)
            {
                return HttpNotFound();
            }
            return View(parametrosFabricantePE);
        }

        // POST: ParametrosFabricante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParametrosFabricantePE parametrosFabricantePE = await db.ParametrosFabricantePE.FindAsync(id);
            db.ParametrosFabricantePE.Remove(parametrosFabricantePE);
            await db.SaveChangesAsync();
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
