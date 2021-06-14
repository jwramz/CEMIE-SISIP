using sievis.Models;
using sievis.Service;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Diagnostics;

namespace sievis.Controllers.sievis
{
    [SessionExpire]
    [AppAuthorize]
    public class EquipoController : Controller
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        //private AppServices appService = null;
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
            ViewBag.ListaFuncionaNoFunciona = AppEnum.ToSelectList(AppEnum.GetFuncionaNoFunciona());
            ViewBag.ListaEstadoBMNA = AppEnum.ToSelectList(AppEnum.GetEstadoBMNA());

            //-- PruebaDeRutina
            ViewBag.ListaCumpleNoCumple = AppEnum.ToSelectList(AppEnum.GetCumpleNoCumple());
            ViewBag.UnidadPresion = AppEnum.ToSelectList(AppEnum.GetTipoUnidadPresion());
            ViewBag.UnidadMedicionHumedad = AppEnum.ToSelectList(AppEnum.GetUnidadMedicionHumedad());
            ViewBag.ListaExisteNoExiste = AppEnum.ToSelectList(AppEnum.GetExisteNoExiste());
            ViewBag.ListaClaseInterruptor = AppEnum.ToSelectList(AppEnum.GetClaseInterruptor());
            ViewBag.ListaFrecuenciaLlenado = AppEnum.ToSelectList(AppEnum.GetFrecuenciaLlenado());
            ViewBag.ListaVoltajeNominalBobina = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());

        }
        #endregion

        #region Constructor
        public EquipoController()
        {
            
           // this.appService = new AppServices();
        }
        #endregion

        #region ActionResult
        // GET: Equipo
        public ActionResult Index()
        {
            var usuario = AppServices.AppUser;
            var equipo = db.Equipo.Include(e => e.Modelo.Marca).Include(e => e.Mecanismo).Include(e => e.Subestacion);
            if (usuario.Zona_id != null)
            {
                equipo = db.Equipo.Include(e => e.Modelo.Marca).Include(e => e.Mecanismo).Include(e => e.Subestacion).Where(x => x.Zona_id == usuario.Zona_id);
                
            }else if (usuario.Gerencia_id != null && usuario.Gerencia_id != 15)
                equipo = db.Equipo.Include(e => e.Modelo.Marca).Include(e => e.Mecanismo).Include(e => e.Subestacion).Where(x => x.Gerencia_id == usuario.Gerencia_id);
            return View(equipo.ToList());
        }

        // GET: Equipo
        public ActionResult ListaEquipos()
        {
            var usuario = AppServices.AppUser;
            using (ModeloSievis dbt = new ModeloSievis())
            {
                //var vPEIndice = dbt.vPruebasEquipoIndice.Where(l=>l.Gerencia_id>0);
                //if (usuario.Zona_id != null)
                //    vPEIndice = dbt.vPruebasEquipoIndice.Where(x => x.Zona_id == usuario.Zona_id);
                //else if (usuario.Gerencia_id != null && usuario.Gerencia_id != 15)
                //    vPEIndice = dbt.vPruebasEquipoIndice.Where(x => x.Gerencia_id == usuario.Gerencia_id);


                var vPEIndice =
                from EquipoMax in dbt.vPruebasEquipoIndice.Where(l => l.Gerencia_id > 0 && l.indiceConfiabilidad >=75)
                group EquipoMax by EquipoMax.equipoId into playerGroup
                select new
                {

                    IdEquipo = playerGroup.Key,
                    MaxFecha = playerGroup.Max(x => x.fecha_prueba),
                };

                var vPEIndiceFinal =
                    from Equipo in dbt.vPruebasEquipoIndice
                    join EquipoMax in vPEIndice on new { mun = Equipo.equipoId, est = Equipo.fecha_prueba }
                equals new { mun = EquipoMax.IdEquipo, est = EquipoMax.MaxFecha }
                    select Equipo;

                return Json(vPEIndiceFinal.ToList(), JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult ListaEquiposAlarmas()
        {
            var usuario = AppServices.AppUser;
            using (ModeloSievis db = new ModeloSievis())
            {
                //var vPEIndice = db.vPruebasEquipoIndice.Where(l => (l.indiceSaludBasica < 75 || l.indiceSaludExtendida < 75) && l.indiceConfiabilidad < 75);
                //if (usuario.Zona_id != null)
                //   vPEIndice = db.vPruebasEquipoIndice.Where(l => (l.indiceSaludBasica < 75 || l.indiceSaludExtendida < 75) && l.indiceConfiabilidad < 75).Where(x =>x.Zona_id == usuario.Zona_id);
                //else if (usuario.Gerencia_id != null && usuario.Gerencia_id != 15)
                //    vPEIndice = db.vPruebasEquipoIndice.Where(l => (l.indiceSaludBasica < 75 || l.indiceSaludExtendida < 75) && l.indiceConfiabilidad < 75).Where(x=>x.Gerencia_id == usuario.Gerencia_id);
                var vPEIndice = db.vPruebasEquipoIndice.Where(l => (l.indiceSaludBasica < 75 || l.indiceSaludExtendida < 75) && l.indiceConfiabilidad < 75);
                return Json(vPEIndice.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaInterruptores(int? take,int? skip,int? page,int? pageSize)
        {
            var usuario = AppServices.AppUser;
            using (ModeloSievis db = new ModeloSievis())
            {
                var vPEIndice = db.vDatosEquipo.OrderBy(o=>o.id);
                if (usuario.Zona_id != null)
                    vPEIndice = db.vDatosEquipo.Where(x => x.Zona_id == usuario.Zona_id).OrderBy(o => o.id);
                else if (usuario.Gerencia_id != null && usuario.Gerencia_id != 15)
                    vPEIndice = db.vDatosEquipo.Where(x => x.Gerencia_id == usuario.Gerencia_id).OrderBy(o => o.id);

                var total = vPEIndice.Count();
                
                pageSize = total;
               
                JsonResult jsonResult = Json(new { data = vPEIndice.ToList(), total = total }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

      
        // GET: Equipo/Details/5
        public ActionResult Details(int? id)
        {
            Equipo equipo = db.Equipo.Find(id);

            ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre", equipo.Gerencia_id);
            ViewBag.Zona_id = new SelectList(db.Zona.Where(x => x.id == 0), "id", "nombre", equipo.Zona_id);
            ViewBag.Zona_id = new SelectList(db.Zona.Where(x => x.id == 0), "id", "nombre", equipo.Zona_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion.Where(x => x.id == 0), "id", "nombre", equipo.Subestacion_id);

            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre", equipo.Modelo_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion", equipo.AplicacionInterruptor_id);

            ViewBag.DisenioEstructural = AppEnum.ToItemSelected(AppEnum.GetDisenioEstructural(), equipo.dis_estructural);
            ViewBag.ConfiguracionCamaras = AppEnum.ToItemSelected(AppEnum.GetConfiguracionCamaras(), equipo.conf_camaras);
            ViewBag.InterruptorContiene = AppEnum.ToItemSelected(AppEnum.GetInterruptorContiene(), equipo.interruptor_contiene);
            ViewBag.TipoDisparo = AppEnum.ToItemSelected(AppEnum.GetTipoDisparo(), equipo.tipo_disparo);
            ViewBag.ComandoCierre = AppEnum.ToItemSelected(AppEnum.GetComandoCierre(), equipo.comando_cierre);
            ViewBag.UnidadPresion = AppEnum.ToItemSelected(AppEnum.GetTipoUnidadPresion(), equipo.tipo_unidades_presion);
            ViewBag.NivelContaminacion = AppEnum.ToItemSelected(AppEnum.GetNivelContaminacion(), equipo.nivel_contaminacion);
            ViewBag.ListaVoltajeNominalBobina = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());
            ViewBag.instrumento_medicionSF6_list = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicionSF6());

            return View(equipo);
        }

        // GET: Equipo/Create
        public ActionResult Create()
        {
            Equipo equipo = new Equipo();

            ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre", equipo.Gerencia_id);
            ViewBag.Zona_id = new SelectList(db.Zona.Where(x => x.id == 0), "id", "nombre", equipo.Zona_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion.Where(x => x.id == 0), "id", "nombre", equipo.Subestacion_id);
            ViewBag.ListaDisponibilidad = AppEnum.ToSelectList(AppEnum.GetDisponibilidad());
            ViewBag.instrumento_medicionSF6_list = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicionSF6());

            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre");
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre");
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion");
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion");
            var ListTensionNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Tensión nominal (kV)")
                            .Select(s => new
                            {
                                descripcion = s.descripcion,
                                valorFlotante = s.valorFlotante
                            }).ToList()
                            .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });


            ViewBag.tension_nominal = new SelectList(ListTensionNominal, "descripcion", "valor", equipo.tension_nominal);

            var ListCorrienteNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente nominal (A)")
               .Select(s => new
               {
                   descripcion = s.descripcion,
                   valorFlotante = s.valorFlotante
               }).ToList()
               .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });


            ViewBag.corriente_nominal = new SelectList(ListCorrienteNominal, "descripcion", "valor", equipo.corriente_nominal);
            var ListCorrienteCC = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente de interrupción de corto circuito (kA)")
              .Select(s => new
              {
                  descripcion = s.descripcion,
                  valorFlotante = s.valorFlotante
              }).ToList()
              .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });

            ViewBag.corriente_cc = new SelectList(ListCorrienteCC, "descripcion", "valor", equipo.corriente_cc);
            ViewBag.ListaDisenioEstructural = AppEnum.ToSelectList(AppEnum.GetDisenioEstructural());
            ViewBag.ConfiguracionCamaras = AppEnum.ToSelectList(AppEnum.GetConfiguracionCamaras());
            ViewBag.ListaInterruptorContiene = AppEnum.ToSelectList(AppEnum.GetInterruptorContiene());
            ViewBag.ListaTipoDisparo = AppEnum.ToSelectList(AppEnum.GetTipoDisparo());
            ViewBag.ListaComandoCierre = AppEnum.ToSelectList(AppEnum.GetComandoCierre());
            ViewBag.ListaUnidadPresion = AppEnum.ToSelectList(AppEnum.GetTipoUnidadPresion());
            ViewBag.ListaNivelContaminacion = AppEnum.ToSelectList(AppEnum.GetNivelContaminacion());
            ViewBag.ListaVoltajeNominalBobina = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());
            ViewBag.ListaNBAI = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());
            var ListBill = db.vCatalogoPuntuaciones.Where(c => c.nombre == "NBAI" && c.valorFlotante == equipo.tension_nominal)
            .Select(s => new
            {
                descripcion = s.descripcion,
                abreviatura = s.abreviatura
            }).ToList()
            .Select(d => new { d.descripcion, abreviatura = String.Format("{0:0.##}", d.abreviatura) });
            ViewBag.bil = new SelectList(ListBill, "descripcion", "abreviatura", equipo.bil);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipo equipo)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
             .Select(x => new { x.Key, x.Value.Errors })
             .ToArray();

            if (!ModelState.IsValid)
            {
                db.Equipo.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
        }


        ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre", equipo.Gerencia_id);
            ViewBag.Zona_id = new SelectList(db.Zona.Where(x => x.id == 0), "id", "nombre",equipo.Zona_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion.Where(x => x.id == 0), "id", "nombre", equipo.Subestacion_id);
            ViewBag.instrumento_medicionSF6_list = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicionSF6());

            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre",equipo.Modelo_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion",equipo.AplicacionInterruptor_id);
            ViewBag.ListaDisponibilidad = AppEnum.ToSelectList(AppEnum.GetDisponibilidad());
            
            var ListTensionNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Tensión nominal (kV)")
                .Select(s => new
                {
                    descripcion = s.descripcion,
                    valorFlotante = s.valorFlotante
                }).ToList()
                .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });
            ViewBag.tension_nominal = new SelectList(ListTensionNominal, "descripcion", "valor", equipo.tension_nominal);

            var ListCorrienteNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente nominal (A)")
               .Select(s => new
               {
                   descripcion = s.descripcion,
                   valorFlotante = s.valorFlotante
               }).ToList()
               .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });
            ViewBag.corriente_nominal = new SelectList(ListCorrienteNominal, "descripcion", "valor", equipo.corriente_nominal);

            var ListCorrienteCC = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente de interrupción de corto circuito (kA)")
              .Select(s => new
              {
                  descripcion = s.descripcion,
                  valorFlotante = s.valorFlotante
              }).ToList()
              .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });
            ViewBag.corriente_cc = new SelectList(ListCorrienteCC, "descripcion", "valor", equipo.corriente_cc);
            ViewBag.ListaDisenioEstructural = AppEnum.ToSelectList(AppEnum.GetDisenioEstructural(), equipo.dis_estructural);
            ViewBag.ConfiguracionCamaras = AppEnum.ToSelectList(AppEnum.GetConfiguracionCamaras(), equipo.conf_camaras);
            ViewBag.ListaInterruptorContiene = AppEnum.ToSelectList(AppEnum.GetInterruptorContiene(), equipo.interruptor_contiene);
            ViewBag.ListaTipoDisparo = AppEnum.ToSelectList(AppEnum.GetTipoDisparo(), equipo.tipo_disparo);
            ViewBag.ListaComandoCierre = AppEnum.ToSelectList(AppEnum.GetComandoCierre(), equipo.comando_cierre);
            ViewBag.ListaUnidadPresion = AppEnum.ToSelectList(AppEnum.GetTipoUnidadPresion(), equipo.tipo_unidades_presion);
            ViewBag.ListaNivelContaminacion = AppEnum.ToSelectList(AppEnum.GetNivelContaminacion(), equipo.nivel_contaminacion);
            ViewBag.ListaVoltajeNominalBobina = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());

            var ListBill = db.vCatalogoPuntuaciones.Where(c => c.nombre == "NBAI" && c.valorFlotante == equipo.tension_nominal)
                       .Select(s => new
                       {
                           descripcion = s.descripcion,
                           abreviatura = s.abreviatura
                       }).ToList()
                       .Select(d => new { d.descripcion, abreviatura = String.Format("{0:0.##}", d.abreviatura) });
            ViewBag.bil = new SelectList(ListBill, "descripcion", "abreviatura", equipo.bil);

            return View(equipo);
        }

        // GET: Equipo/Edit/5
        public ActionResult Edit(int? id)
        {
            Equipo equipo = db.Equipo.Find(id);

            ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre", equipo.Gerencia_id);
            ViewBag.Zona_id = new SelectList(db.Zona.Where(z => z.Gerencia_id == equipo.Gerencia_id), "id", "nombre", equipo.Zona_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion.Where(s => s.Zona_id == equipo.Zona_id), "id", "nombre", equipo.Subestacion_id);
            //ViewBag.Zona_id = new SelectList(db.Zona, "id", "nombre", equipo.Zona_id);
            //ViewBag.Subestacion_id = new SelectList(db.Subestacion, "id", "nombre", equipo.Subestacion_id);
            ViewBag.instrumento_medicionSF6_list = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicionSF6());

            ViewBag.ListaDisponibilidad = AppEnum.ToSelectList(AppEnum.GetDisponibilidad());

            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            //ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre", equipo.Modelo_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo.Where(m => m.Marca_id == equipo.Marca_id), "id", "nombre", equipo.Modelo_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion", equipo.AplicacionInterruptor_id);
            var ListTensionNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Tensión nominal (kV)")
                .Select(s => new
                {
                    descripcion = s.descripcion,
                    valorFlotante = s.valorFlotante
                }).ToList()
                .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });


            ViewBag.tension_nominal = new SelectList(ListTensionNominal, "descripcion", "valor", equipo.tension_nominal);

            var ListCorrienteNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente nominal (A)")
               .Select(s => new
               {
                   descripcion = s.descripcion,
                   valorFlotante = s.valorFlotante
               }).ToList()
               .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });


            ViewBag.corriente_nominal = new SelectList(ListCorrienteNominal, "descripcion", "valor", equipo.corriente_nominal);
            var ListCorrienteCC = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente de interrupción de corto circuito (kA)")
              .Select(s => new
              {
                  descripcion = s.descripcion,
                  valorFlotante = s.valorFlotante
              }).ToList()
              .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });
                
            ViewBag.corriente_cc = new SelectList(ListCorrienteCC, "descripcion", "valor", equipo.corriente_cc);
                             

            ViewBag.ListaDisenioEstructural = AppEnum.ToSelectList(AppEnum.GetDisenioEstructural(), equipo.dis_estructural);
            ViewBag.ConfiguracionCamaras = AppEnum.ToSelectList(AppEnum.GetConfiguracionCamaras(), equipo.conf_camaras);
            ViewBag.ListaInterruptorContiene = AppEnum.ToSelectList(AppEnum.GetInterruptorContiene(), equipo.interruptor_contiene);
            ViewBag.ListaTipoDisparo = AppEnum.ToSelectList(AppEnum.GetTipoDisparo(), equipo.tipo_disparo);
            ViewBag.ListaComandoCierre = AppEnum.ToSelectList(AppEnum.GetComandoCierre(), equipo.comando_cierre);
            ViewBag.ListaUnidadPresion = AppEnum.ToSelectList(AppEnum.GetTipoUnidadPresion(), equipo.tipo_unidades_presion);
            ViewBag.ListaNivelContaminacion = AppEnum.ToSelectList(AppEnum.GetNivelContaminacion(), equipo.nivel_contaminacion);
            ViewBag.ListaVoltajeNominalBobina = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());

            var ListBill = db.vCatalogoPuntuaciones.Where(c => c.nombre == "NBAI" && c.valorFlotante == equipo.tension_nominal)
            .Select(s => new
            {
                descripcion = s.descripcion,
                abreviatura = s.abreviatura
            }).ToList()
            .Select(d => new { d.descripcion, abreviatura = String.Format("{0:0.##}", d.abreviatura) });

            ViewBag.bil = new SelectList(ListBill, "descripcion", "abreviatura", equipo.bil);


            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                if (equipo.dis_estructural == "TM") equipo.conf_camaras = "C";
                db.Entry(equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Gerencia_id = new SelectList(db.Gerencia, "id", "nombre", equipo.Gerencia_id);
            //ViewBag.Zona_id = new SelectList(db.Zona, "id", "nombre", equipo.Zona_id);
            //ViewBag.Subestacion_id = new SelectList(db.Subestacion, "id", "nombre", equipo.Subestacion_id);
            ViewBag.Zona_id = new SelectList(db.Zona.Where(z => z.Gerencia_id == equipo.Gerencia_id), "id", "nombre", equipo.Zona_id);
            ViewBag.Subestacion_id = new SelectList(db.Subestacion.Where(s => s.Zona_id == equipo.Zona_id), "id", "nombre", equipo.Subestacion_id);
            ViewBag.ListaDisponibilidad = AppEnum.ToSelectList(AppEnum.GetDisponibilidad());
            ViewBag.instrumento_medicionSF6_list = AppEnum.ToSelectList(AppEnum.GetInstrumentoMedicionSF6());

            ViewBag.Marca_id = new SelectList(db.Marca, "id", "nombre", equipo.Marca_id);
            //ViewBag.Modelo_id = new SelectList(db.Modelo, "id", "nombre", equipo.Modelo_id);
            ViewBag.Modelo_id = new SelectList(db.Modelo.Where(m => m.Marca_id == equipo.Marca_id), "id", "nombre", equipo.Modelo_id);
            ViewBag.Mecanismo_id = new SelectList(db.Mecanismo, "id", "descripcion", equipo.Mecanismo_id);
            ViewBag.AplicacionInterruptor_id = new SelectList(db.AplicacionInterruptor, "id", "descripcion", equipo.AplicacionInterruptor_id);
            var ListTensionNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Tensión nominal (kV)")
                .Select(s => new
                {
                    descripcion = s.descripcion,
                    valorFlotante = s.valorFlotante
                }).ToList()
                .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });


            ViewBag.tension_nominal = new SelectList(ListTensionNominal, "descripcion", "valor", equipo.tension_nominal);

            var ListCorrienteNominal = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente nominal (A)")
               .Select(s => new
               {
                   descripcion = s.descripcion,
                   valorFlotante = s.valorFlotante
               }).ToList()
               .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });


            ViewBag.corriente_nominal = new SelectList(ListCorrienteNominal, "descripcion", "valor", equipo.corriente_nominal);
            var ListCorrienteCC = db.vCatalogoPuntuaciones.Where(c => c.nombre == "Corriente de interrupción de corto circuito (kA)")
              .Select(s => new
              {
                  descripcion = s.descripcion,
                  valorFlotante = s.valorFlotante
              }).ToList()
              .Select(d => new { d.descripcion, valor = String.Format("{0:0.##}", d.valorFlotante) });

            ViewBag.corriente_cc = new SelectList(ListCorrienteCC, "descripcion", "valor", equipo.corriente_cc);
            ViewBag.ListaDisenioEstructural = AppEnum.ToSelectList(AppEnum.GetDisenioEstructural(), equipo.dis_estructural);
            ViewBag.ConfiguracionCamaras = AppEnum.ToSelectList(AppEnum.GetConfiguracionCamaras(), equipo.conf_camaras);
            ViewBag.ListaInterruptorContiene = AppEnum.ToSelectList(AppEnum.GetInterruptorContiene(), equipo.interruptor_contiene);
            ViewBag.ListaTipoDisparo = AppEnum.ToSelectList(AppEnum.GetTipoDisparo(), equipo.tipo_disparo);
            ViewBag.ListaComandoCierre = AppEnum.ToSelectList(AppEnum.GetComandoCierre(), equipo.comando_cierre);
            ViewBag.ListaUnidadPresion = AppEnum.ToSelectList(AppEnum.GetTipoUnidadPresion(), equipo.tipo_unidades_presion);
            ViewBag.ListaNivelContaminacion = AppEnum.ToSelectList(AppEnum.GetNivelContaminacion(), equipo.nivel_contaminacion);
            ViewBag.ListaVoltajeNominalBobina = AppEnum.ToSelectList(AppEnum.GetVoltajeNominalBobina());

           var ListBill = db.vCatalogoPuntuaciones.Where(c => c.nombre == "NBAI" && c.valorFlotante == equipo.tension_nominal)
              .Select(s => new
               {
                   descripcion = s.descripcion,
                  abreviatura = s.abreviatura
               }).ToList()
              .Select(d => new { d.descripcion, abreviatura = String.Format("{0:0.##}", d.abreviatura) });

            ViewBag.bil = new SelectList(ListBill, "descripcion", "abreviatura", equipo.bil);
            return View(equipo);
        }

        // GET: Equipo/Delete/5
        public ActionResult Delete(int? id)
        {
            Equipo equipo = db.Equipo.Find(id);
            return View(equipo);
        }

        // POST: Equipo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            db.EliminarEquipo(id);
            
                return RedirectToAction("Index");
        }

       
        #endregion

        #region JsonResult
        public JsonResult GetZonaByGerenciaId(int Gerencia_id)
        {
            ModeloSievis dbt = new ModeloSievis();
            dbt.Configuration.ProxyCreationEnabled = false;
            return Json(dbt.Zona.Where(z => z.Gerencia_id == Gerencia_id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubestacionByGIdZId(int Gerencia_id, int Zona_id)
        {
            ModeloSievis dbt = new ModeloSievis();
            dbt.Configuration.ProxyCreationEnabled = false;
            return Json(dbt.Subestacion.Where(s => s.Gerencia_id == Gerencia_id && s.Zona_id == Zona_id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModeloByMarcaId(int Marca_id)
        {
            ModeloSievis dbt = new ModeloSievis();
            dbt.Configuration.ProxyCreationEnabled = false;
            return Json(dbt.Modelo.Where(m => m.Marca_id == Marca_id), JsonRequestBehavior.AllowGet);
        }

        
       public JsonResult GetbilByTensionNominal(Decimal? tension_nominal)
        {
            ModeloSievis dbt = new ModeloSievis();
            dbt.Configuration.ProxyCreationEnabled = false;
            return Json(dbt.vCatalogoPuntuaciones.Where(c => c.nombre == "NBAI" && c.valorFlotante == tension_nominal), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNumSerieDuplicado(string ns)
        {
            ModeloSievis dbt = new ModeloSievis();
            dbt.Configuration.ProxyCreationEnabled = false;
            var ListNumSerie = dbt.vDatosEquipo.Where(c => c.ns == ns).Count();
                      
            return Json(dbt.vDatosEquipo.Where(c => c.ns == ns).Count(), JsonRequestBehavior.AllowGet);
            
        }


        public JsonResult GetIdEquipoSAPDuplicado(string Id_EquipoSAP)
        {
            ModeloSievis dbt = new ModeloSievis();
            dbt.Configuration.ProxyCreationEnabled = false;
            var ListNumSerie = dbt.vDatosEquipo.Where(c => c.Id_EquipoSAP == Id_EquipoSAP).Count();

            return Json(dbt.vDatosEquipo.Where(c => c.Id_EquipoSAP == Id_EquipoSAP).Count(), JsonRequestBehavior.AllowGet);

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
