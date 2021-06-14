using sievis.Calculos;
using sievis.Common;
using sievis.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    public class PruebasController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        private DBContext dbContext = new DBContext();
        #endregion

        #region Dropdown
        private void LoadDropDownPruebas()
        {

            //-- Prueba
            ViewBag.instrumento_medicionSF6_list = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicionSF6());

            //-- InspeccionVisual
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion");
            ViewBag.ListaInstrumentoMedicion = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicion());
            ViewBag.ListaEstadoCondicion = AppEnum.ToSelectList(AppEnum.GetEstado());
            ViewBag.ListaEstadoCondicion1 = AppEnum.GetEstado();
            ViewBag.ListaFuncionaNoFunciona = AppEnum.ToSelectList(AppEnum.GetFuncionaNoFunciona());
            ViewBag.ListaEstadoBMNA = AppEnum.ToSelectList(AppEnum.GetEstadoBMNA());

            //-- PruebaDeRutina
            ViewBag.ListaCumpleNoCumple = AppEnum.ToSelectList(AppEnum.GetCumpleNoCumple());
            ViewBag.ListaCumpleNoCumplePR = AppEnum.ToSelectList(AppEnum.GetCumpleNoCumplePR());
            ViewBag.UnidadPresion = AppEnum.ToSelectList(AppEnum.GetTipoUnidadPresion());
            ViewBag.UnidadMedicionHumedad = AppEnum.ToSelectList(AppEnum.GetUnidadMedicionHumedad());
            ViewBag.ListaExisteNoExiste = AppEnum.ToSelectList(AppEnum.GetExisteNoExiste());
            ViewBag.ListaClaseInterruptor = AppEnum.ToSelectList(AppEnum.GetClaseInterruptor());
            ViewBag.ListaFrecuenciaLlenado = AppEnum.ToSelectList(AppEnum.GetFrecuenciaLlenado());
            ViewBag.ListaUbicaTempMax = AppEnum.ToSelectList(AppEnum.GetUbicaTempMax());
            ViewBag.ListaSiNo = AppEnum.ToSelectList(AppEnum.GetSiNo());
            ViewBag.ListaSiNoSD = AppEnum.ToSelectList(AppEnum.GetSiNoSD());
            ViewBag.ListaSiNoBool = AppEnum.ToSelectList(AppEnum.GetSiNoBool());
            ViewBag.ListaNumeroMotores = AppEnum.ToSelectList(AppEnum.GetNumeroMotores());
        }
        #endregion

        #region Pruebas
        // GET: Pruebas
        [HttpGet]
        public ActionResult Index(int? EquipoId)
        {
            var prueba = db.Prueba.Where(pba => pba.Equipo_id == EquipoId).Include(p => p.Equipo);
            return View(prueba.OrderBy(x=>x.fecha_prueba).ToList());
        }

        // GET: Pruebas/Datos
        [HttpGet]
        public ActionResult Datos(int? equipoId, int? pruebaId)
        {
            LoadDropDownPruebas();

            Prueba prueba = dbContext.Get(db, equipoId, pruebaId);
            prueba.Equipo_id = (int)equipoId;
            Equipo equipo = db.Equipo.Find(equipoId);
            prueba.Equipo = equipo;
            ViewBag.Equipo = equipo;

            return View(prueba);
        }

        // POST: Pruebas/Datos
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Datos(Prueba prueba)
        {
            try
            {
                Equipo equipo = db.Equipo.Find(prueba.Equipo_id);
                ViewBag.Equipo = equipo;
                //prueba.Equipo = equipo;

                
                if (ModelState.IsValid && CustomValidation(prueba))
                {
                    int equipoId = Convert.ToInt32(Request["EquipoId"]);
                    int pruebaId = Convert.ToInt32(Request["PruebaId"]);

                    //int equipoId = _equipoId;
                    //int pruebaId = _pruebaId;


                    dbContext.Save(db, equipoId, pruebaId, prueba);

                    return RedirectToAction("Datos", "Pruebas", new { EquipoId = equipoId, PruebaId = prueba.id });
                }
            }
            catch (DbEntityValidationException entityException)
                {
                foreach (var eve in entityException.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        ModelState.AddModelError(string.Empty, ve.ErrorMessage);
            }
            
            LoadDropDownPruebas();
            return View(prueba);
        }

        // GET: Prueba/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Prueba prueba = db.Prueba.Find(id);
            return View(prueba);
        }

        // POST: Prueba/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Prueba prueba = db.Prueba.Find(id);

            db.EliminarPrueba(prueba.id);


            return RedirectToAction("Index", "Pruebas", new {EquipoId = prueba.Equipo_id});

        }

        #region Reporte
        // GET: Pruebas/Reporte
        [HttpGet]
        public ActionResult Reporte(int? equipoId, int? pruebaId, bool? pMostrarPesoValor)
        {
            vDatosEquipo equipo = db.vDatosEquipo.SingleOrDefault(de => de.id == equipoId);
            ViewBag.Equipo = equipo;
            Prueba prueba = dbContext.Get(db, equipoId, pruebaId);
            ViewBag.Prueba = prueba;
            //
            CalculoIndSaludInsVisual unIndiceIV = new CalculoIndSaludInsVisual((int)equipoId, (int)pruebaId);
            double vlIndiceSaludIV = unIndiceIV.IndSaludInsVisual();
            //
            CalculoIndSaludPruebasRutina unIndicePR = new CalculoIndSaludPruebasRutina((int)equipoId, (int)pruebaId);
            double vlIndiceSaludPR = unIndicePR.IndicesSaludPruebaRutina();
            //
            CalculoIndSaludPruebasEspeciales unIndicePE = new CalculoIndSaludPruebasEspeciales((int)equipoId, (int)pruebaId);
            double vlIndiceSaludPE = 0;
            //
            double vlIndiceSaludPB = vlIndiceSaludIV * 0.05 + vlIndiceSaludPR * 0.95;
            double vlIndiceConfiabilidadPB = unIndicePR.IndiceConfiabilidadPruebaRutina();
            double vlIndiceSaludPbaExtendida = 0;
            double vlIndiceConfiabilidadPE = 0;
            //
            ViewBag.indiceSaludPB = vlIndiceSaludPB;
            ViewBag.indiceConfPB = vlIndiceConfiabilidadPB;
            prueba.indiceSaludBasica = (decimal)vlIndiceSaludPB;
            prueba.indiceConfiabilidad = (decimal)vlIndiceConfiabilidadPB;
            if (prueba.evalExtendida)
            {
                vlIndiceSaludPE = unIndicePE.IndicesSaludPruebaEspecial();
                vlIndiceSaludPbaExtendida = vlIndiceSaludIV * 0.05 + vlIndiceSaludPR * 0.45 + vlIndiceSaludPE * 0.50;
                ViewBag.indiceSaludPE = vlIndiceSaludPbaExtendida;
                vlIndiceConfiabilidadPE = unIndicePE.IndiceConfiabilidadPruebaEspecial();
                ViewBag.indiceConfPE = vlIndiceConfiabilidadPE;
                prueba.indiceConfiabilidad = (decimal)vlIndiceConfiabilidadPE;
            }
            //

            ViewBag.inspeccionVisual = unIndiceIV.pDatosCPCms;
            ViewBag.PruebaRutina = unIndicePR.vlListadoPuntuaciones;
            ViewBag.PruebaEspecial = unIndicePE.vlListadoPuntuaciones;
            ViewBag.MostrarPesoValor = pMostrarPesoValor;
            //
            try
            {
                prueba.indiceSaludExtendida = (decimal)vlIndiceSaludPbaExtendida;
                db.Entry(prueba).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            //
            return View();
        }
        #endregion Reporte

        public JsonResult ReporteIndices()
        {
            var equipo = db.Equipo;

            dynamic Reporte = new ExpandoObject();
                               

            return Json(equipo ,JsonRequestBehavior.AllowGet);
        }
        public JsonResult PruebasIndices(int? equipoId)
        {
            var equipo = db.vPruebasEquipoIndice.Where(e=>e.equipoId == equipoId).Select(s=> new {fecha = SqlFunctions.DateName("day", s.fecha_inspeccion).Trim() + "/" +
                   SqlFunctions.StringConvert((double)s.fecha_inspeccion.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", s.fecha_inspeccion),
                s.indiceSaludBasica,s.indiceSaludExtendida  });
            return Json(equipo, JsonRequestBehavior.AllowGet);
        }
        //----------------
        #endregion

        #region Validation
        private bool CustomValidation(Prueba prueba)
        {
            bool result = true;

            return result;
        }
        #endregion

        #region Archivos
        // GET: Pruebas/Archivos
        public ActionResult Archivos(int? equipoId, int? pruebaId)
        {
            var req = Request;
            CargarArchivos(equipoId, pruebaId);
            return View();
        }

        // GET: Pruebas/DescargarArchivo
        public FileResult DescargarArchivo(int? archivoId)
        {
            var file = (from a in db.Archivo where a.id == archivoId select a).First();
            byte[] filedata = file.archivo_soporte;
            return File(filedata, MimeTypeProvider.GetContent(file.extension), file.nombre_archivo);
        }

       
        public ActionResult EliminarArchivo(int? archivoId)
        {
            Archivo archivo = db.Archivo.Find(archivoId);
            var equipoId = archivo.Prueba.Equipo_id;
            var pruebaId = archivo.Prueba_id;
            db.Archivo.Remove(archivo);
            db.SaveChanges();
            return RedirectToAction("Archivos", "Pruebas", new { EquipoId = equipoId, PruebaId = pruebaId });
        }
        // POST: Pruebas/Archivos
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Archivos(Prueba prueba)
        {
            var postedFiles = Request.Files;
            var equipoId = Request["EquipoId"];
            var pruebaId = Request["PruebaId"];
            var descripcion = Request["nombre_prueba"];

            if (postedFiles != null && postedFiles.Count >= 1 && equipoId != null && pruebaId != null)
            {
                var file = postedFiles[0];
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);

                Archivo archivo = new Archivo()
                {
                    fecha = DateTime.Now,
                    Prueba_id = Convert.ToInt32(pruebaId),
                    archivo_soporte = bytes,
                    nombre_archivo = file.FileName,
                    extension = MimeTypeProvider.GetExtension(file.FileName),
                    nombre_prueba = descripcion
                };
                db.Archivo.Add(archivo);
                db.SaveChanges();
            }
            if (equipoId != null && pruebaId != null)
            {
                CargarArchivos(Convert.ToInt32(equipoId), Convert.ToInt32(pruebaId));
            }
            return View();
        }
        #endregion

        #region private
        void CargarArchivos(int? equipoId, int? pruebaId)
        {
            var archivos = (from e in db.Equipo
                            join p in db.Prueba on e.id equals p.Equipo_id
                            join a in db.Archivo on p.id equals a.Prueba_id
                            where e.id == equipoId && p.id == pruebaId
                            select a).ToList();
            ViewBag.Archivos = archivos;
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
