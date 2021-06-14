using sievis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static sievis.Models.AppEnum;


namespace sievis.Calculos
{
    public class CalculoIndSaludPruebasRutina
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        private DBContext dbContext = new DBContext();
        /* Objetos de apoyo extraer datos de cabecera de prueba */
        private Equipo vlEquipo;
        private Prueba vlPrueba;
        private Mecanismo vlMecanismo;
        //private Variables vlVariablesPR;
        private IQueryable<vCatParamVarRango> vlParamVarRango;
        /* Datos de la prueba de rutina */
        private IQueryable<vdPrConfInterruptor> vlPRConfInt;
        private IQueryable<vdPrMecOperacion> vlPRMecOper;
        private IQueryable<vdPrSecVerifProtecciones> vlPRSecVerProtec;
        private IQueryable<vdPrUnidadRuptora> vlPRUniRuptora;
        private IQueryable<vCatalogoPuntuaciones> vlPuntuacionesNum;
        private List<PuntuacionFase> vlListaPuntConfInterruptor { get; set; }
        private List<PuntuacionFase> vlListaPuntMecOperacion { get; set; }
        private List<PuntuacionFase> vlListaPuntVerifProtecciones { get; set; }
        private List<PuntuacionFase> vlListaPuntUnidadRuptora { get; set; }
        // Atributos de instancia para el calculo de Indice de Salud y Confiabilidad
        //private IQueryable<vParametrosVariablesPeso> vlParamVarPesos;
        private decimal viCPCmConfInterruptor;
        private decimal viCPCmMecOperacion;
        private decimal viCPCmVerifProtecciones;
        private decimal viCPCmUnidadRuptora;
        #endregion
        public List<PuntuacionFase> vlListadoPuntuaciones { get; set; }

        public CalculoIndSaludPruebasRutina(int equipoId, int pruebaId)
        {
            vlEquipo = db.Equipo.SingleOrDefault(de => de.id == equipoId);
            vlPrueba = dbContext.Get(db, equipoId, pruebaId);
            vlMecanismo = db.Mecanismo.SingleOrDefault(me => me.id == vlEquipo.Mecanismo_id);
            vlParamVarRango = db.vCatParamVarRango.Where(cpvr => cpvr.catalogo == "Pruebas de rutina");
            /* Datos de la prueba de rutina */
            // Configuracion del interruptor
            vlPRConfInt = db.vdPrConfInterruptor.AsNoTracking().Where(vprci => vprci.pruebaId == pruebaId);
            // Mecanismo de operación
            vlPRMecOper = db.vdPrMecOperacion.AsNoTracking().Where(vprmo => vprmo.prueba_id == pruebaId);
            // Secuencias para verificación de protecciones
            vlPRSecVerProtec = db.vdPrSecVerifProtecciones.AsNoTracking().Where(vprsvp => vprsvp.Prueba_id == pruebaId);
            // Unidad ruptora
            vlPRUniRuptora = db.vdPrUnidadRuptora.AsNoTracking().Where(vprur => vprur.pruebaId == pruebaId);
            //
            vlPuntuacionesNum = db.vCatalogoPuntuaciones.AsNoTracking().Where(cp => cp.nombre == "Puntuación");
            // Para obtener los pesos de cada variable
            // vlParamVarPesos = db.vParametrosVariablesPeso.Where(vpvp => vpvp.catalogo == "Pruebas de rutina");
            // Lista de calificaciones de cada variable evaluada
            vlListadoPuntuaciones = new List<PuntuacionFase>();
            //
            vlListaPuntConfInterruptor = new List<PuntuacionFase>();
            vlListaPuntMecOperacion = new List<PuntuacionFase>();
            vlListaPuntVerifProtecciones = new List<PuntuacionFase>();
            vlListaPuntUnidadRuptora = new List<PuntuacionFase>();
        }

        public double IndiceConfiabilidadPruebaRutina()
        {
            double vlTotalPeso = (double)ObtenSumaPesosVar();
            double vlPesoVarDisponibles = (double)ObtenSumaPesosVarPrueba();
            return (vlPesoVarDisponibles / vlTotalPeso) * 100;
        }

        #region IndicesSalud
        public double IndicesSaludPruebaRutina()
        {
            indSaludPRConfInterruptor();
            indSaludPRMecOperacion();
            indSaludPRSecVerifProtecciones();
            IndSaludPRUnidadRuptora();
            return (double)obtenISPR();
        }

        public void indSaludPRConfInterruptor()
        {
            /* Confiabilidad del interruptor -> Fecha de puesta en servicio */
            IQueryable<vCatParamVarRango> vlCIFPuestaServicio = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Confiabilidad del interruptor") && r.variable.Equals("Fecha de puesta en servicio")).OrderBy(r => r.rango);
            /* Confiabilidad del interruptor -> Fecha del último mantenimiento mayor */
            IQueryable<vCatParamVarRango> vlCIFMantoMayor = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Confiabilidad del interruptor") && r.variable.Equals("Fecha del último mantenimiento mayor")).OrderBy(r => r.rango);
            /* Confiabilidad del interruptor -> Número de operaciones */
            IQueryable<vCatParamVarRango> vlCINumOperaciones = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Confiabilidad del interruptor") && r.variable.Equals("Número de operaciones clase M1")).OrderBy(r => r.rango);
            /*  Confiabilidad del interruptor -> Tipo de mecanismo de operación (resorte, hidráulico y neumático) */
            IQueryable<vCatParamVarRango> vlCITipoMecanismo = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Confiabilidad del interruptor") && r.variable.Equals("Tipo de mecanismo de operación (resorte, hidráulico y neumático)")).OrderBy(r => r.rango);
            /*  Confiabilidad del interruptor -> Disponibilidad de refacciones y soporte técnico */
            IQueryable<vCatParamVarRango> vlCIDispRefacSopTec = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Confiabilidad del interruptor") && r.variable.Equals("Disponibilidad de refacciones y soporte técnico")).OrderBy(r => r.rango);
            /* Confiabilidad del interruptor -> Altitud/Contaminación */
            IQueryable<vCatParamVarRango> vlCIAltContaminacion = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Confiabilidad del interruptor") && r.variable.Equals("Altitud/Contaminación")).OrderBy(r => r.rango);

            /* Para cada registro se obtiene su puntuación */
            foreach (vdPrConfInterruptor vConfInt in vlPRConfInt)
            {
                /* Obtener puntuación de variable Fecha de puesta en servicio */
                if (vConfInt.aPuestaServicio != null)
                {
                    decimal aniosPuestaServicio = (decimal)vConfInt.aPuestaServicio / 12;
                    PuntuacionFase vlPuntuacionPuestaServicio = ObtenPuntuacionComNums(aniosPuestaServicio, vlCIFPuestaServicio);
                    vlListadoPuntuaciones.Add(vlPuntuacionPuestaServicio);
                    vlListaPuntConfInterruptor.Add(vlPuntuacionPuestaServicio);
                }
                else
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlCIFPuestaServicio.First().varPeso;
                    nuevaPuntuacion.variable = vlCIFPuestaServicio.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                /* Obtener puntuación de variable Fecha de último mantenimiento mayor */
                if (vConfInt.aUltimoManto != null)
                {
                    decimal aniosMantoMayor = (decimal)vConfInt.aUltimoManto / 12;
                    PuntuacionFase vlPuntuacionMantoMayor = ObtenPuntuacionComNums(aniosMantoMayor, vlCIFMantoMayor);
                    vlListadoPuntuaciones.Add(vlPuntuacionMantoMayor);
                    vlListaPuntConfInterruptor.Add(vlPuntuacionMantoMayor);
                }
                else
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlCIFMantoMayor.First().varPeso;
                    nuevaPuntuacion.variable = vlCIFMantoMayor.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                /* Obtener puntuación de variable Número de operaciones */
                if (vConfInt.numOperaciones != null)
                {
                    decimal numOperaciones = (decimal)vConfInt.numOperaciones;
                    PuntuacionFase vlPuntuacionNumOper = ObtenPuntuacionComNums(numOperaciones, vlCINumOperaciones);
                    vlListadoPuntuaciones.Add(vlPuntuacionNumOper);
                    vlListaPuntConfInterruptor.Add(vlPuntuacionNumOper);
                }
                else
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlCINumOperaciones.First().varPeso;
                    nuevaPuntuacion.variable = vlCINumOperaciones.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                /* Obtener puntuación de variable Tipo de mecanismo */
                if (vlMecanismo.descripcion != null)
                {
                    String tipoMecanismo = vlMecanismo.descripcion;
                    PuntuacionFase vlPuntuacionTipoMecanismo = ObtenPuntuacionTipoMecanismo(tipoMecanismo, vlCITipoMecanismo);
                    vlListadoPuntuaciones.Add(vlPuntuacionTipoMecanismo);
                    vlListaPuntConfInterruptor.Add(vlPuntuacionTipoMecanismo);
                }
                else
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlCITipoMecanismo.First().varPeso;
                    nuevaPuntuacion.variable = vlCITipoMecanismo.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                /* Obtener puntuación de variable Disponibilidad de refacciones */
                if (vConfInt.disponibilidad_refaccion_st != null)
                {
                    String disRefacciones = vConfInt.disponibilidad_refaccion_st;
                    PuntuacionFase vlPuntuacionDisRefacciones = ObtenPuntuacionComAltaMediaBaja(disRefacciones, vlCIDispRefacSopTec);
                    vlListadoPuntuaciones.Add(vlPuntuacionDisRefacciones);
                    vlListaPuntConfInterruptor.Add(vlPuntuacionDisRefacciones);
                }
                else
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlCIDispRefacSopTec.First().varPeso;
                    nuevaPuntuacion.variable = vlCIDispRefacSopTec.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                /* Altitud/Contaminación */
                if (vlEquipo.altitud_operacion != null && vlEquipo.altitud_instalacion != null && vConfInt.nivel_contaminacion != null)
                {
                    PuntuacionFase vlPuntuacionAltitudContaminacion = new PuntuacionFase();
                    vCatParamVarRango vlRangoAC = vlCIAltContaminacion.First();
                    vlPuntuacionAltitudContaminacion.peso = (decimal)vlRangoAC.varPeso;
                    vlPuntuacionAltitudContaminacion.variable = vlRangoAC.variable;

                    if (vlEquipo.altitud_instalacion <= vlEquipo.altitud_operacion)
                    {
                        String vlNivelContaminacion = vConfInt.nivel_contaminacion;
                        int vlDistanciaFuga = (int)vlEquipo.distancia_fuga;
                        switch (vlNivelContaminacion)
                        {
                            case "A":
                                if (vlDistanciaFuga >= 25)
                                    vlPuntuacionAltitudContaminacion.puntuacionLetra = "A";
                                else
                                    vlPuntuacionAltitudContaminacion.puntuacionLetra = "E";
                                break;
                            case "B":
                            case "C":
                            case "M": //C
                                if (vlDistanciaFuga >= 20)
                                    vlPuntuacionAltitudContaminacion.puntuacionLetra = "A";
                                else
                                    vlPuntuacionAltitudContaminacion.puntuacionLetra = "E";
                                break;

                        }
                    }
                    else
                    {
                        vlPuntuacionAltitudContaminacion.peso = (decimal)vlRangoAC.varPeso;
                        vlPuntuacionAltitudContaminacion.puntuacionLetra = "E";
                    }
                    vlPuntuacionAltitudContaminacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacionAltitudContaminacion.puntuacionLetra);
                    vlListadoPuntuaciones.Add(vlPuntuacionAltitudContaminacion);
                    vlListaPuntConfInterruptor.Add(vlPuntuacionAltitudContaminacion);
                }
                else
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlCIAltContaminacion.First().varPeso;
                    nuevaPuntuacion.variable = vlCIAltContaminacion.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
            }

        }

        public void indSaludPRMecOperacion()
        {
            /* Mecanismo de operación -> Tiempos de apertura (TA) disparo 1 en ms */
            IQueryable<vCatParamVarRango> vlMOTAperturaD1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Tiempos de apertura (TA) disparo 1 en ms")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Tiempos de apertura (TA) disparo 2 en ms */
            IQueryable<vCatParamVarRango> vlMOTAperturaD2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Tiempos de apertura (TA) disparo 2 en ms")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Tiempos de cierre (TC) */
            IQueryable<vCatParamVarRango> vlMOTCierre = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Tiempos de cierre (TC)")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad (SO) de operación de polos en apertura (entre polos) disparo 1 */
            IQueryable<vCatParamVarRango> vlMOSimPolosAperturaD1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad (SO) de operación de polos en apertura (entre polos) disparo 1")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad (SO) de operación de polos en apertura (entre polos) disparo 2 */
            IQueryable<vCatParamVarRango> vlMOSimPolosAperturaD2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad (SO) de operación de polos en apertura (entre polos) disparo 2")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad (SC) de operación de polos en cierre (entre polos) */
            IQueryable<vCatParamVarRango> vlMOSimPolosCierre = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad (SC) de operación de polos en cierre (entre polos)")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad de contactos en apertura (del mismo polo en interruptores de 2 cámaras) disparo 1 */
            IQueryable<vCatParamVarRango> vlMOSimConAperturaD1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad de contactos en apertura (del mismo polo en interruptores de 2 cámaras) disparo 1")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad de contactos en apertura (del mismo polo en interruptores de 2 cámaras) disparo 2 */
            IQueryable<vCatParamVarRango> vlMOSimConAperturaD2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad de contactos en apertura (del mismo polo en interruptores de 2 cámaras) disparo 2")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad de contactos en cierre (del mismo polo en interruptores de 2 cámaras) */
            IQueryable<vCatParamVarRango> vlMOSimConCierre = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad de contactos en cierre (del mismo polo en interruptores de 2 cámaras)")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad de contactos en cierre con resistencia de preinserción (del mismo polo en interruptores multicámara) */
            IQueryable<vCatParamVarRango> vlMOSimConCierreRPMismoPoloMulCam = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad de contactos en cierre con resistencia de preinserción (del mismo polo en interruptores multicámara)")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Simultaneidad de contactos en cierre con resistencia de preinserción (entre polos) */
            IQueryable<vCatParamVarRango> vlMOSimConCierreRPEntrePolos = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Simultaneidad de contactos en cierre con resistencia de preinserción (entre polos)")).OrderBy(r => r.rango);
            /* Mecanismo de operación -> Tiempo de entrada de la resistencia de preinserción */
            IQueryable<vCatParamVarRango> vlMOTEntradaRP = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Tiempo de entrada de la resistencia de preinserción")).OrderBy(r => r.rango);

            if (vlEquipo.conf_camaras == "C")
            {
                try
                {
                    PuntuacionFase vlPuntuacionTiempoApD1 = ObtenPuntuacionComNums(ObtenTiempoApeD1Con1Camara(), vlMOTAperturaD1);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoApD1);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoApD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOTAperturaD1.First().varPeso;
                    nuevaPuntuacion.variable = vlMOTAperturaD1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoApD2 = ObtenPuntuacionComNums(ObtenTiempoApeD2Con1Camara(), vlMOTAperturaD2);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoApD2);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoApD2);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOTAperturaD2.First().varPeso;
                    nuevaPuntuacion.variable = vlMOTAperturaD2.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoCierre = ObtenPuntuacionComNums(ObtenTiempoCierre1Camara(), vlMOTCierre);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoCierre);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoCierre);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOTCierre.First().varPeso;
                    nuevaPuntuacion.variable = vlMOTCierre.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                //
                try
                {
                    PuntuacionFase vlSimultaneidadD1 = ObtenPuntuacionComNums(ObtenSimultaneidadD1(), vlMOSimPolosAperturaD1);
                    vlListadoPuntuaciones.Add(vlSimultaneidadD1);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimPolosAperturaD1.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimPolosAperturaD1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlSimultaneidadD2 = ObtenPuntuacionComNums(ObtenSimultaneidadD2(), vlMOSimPolosAperturaD2);
                    vlListadoPuntuaciones.Add(vlSimultaneidadD2);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadD2);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimPolosAperturaD2.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimPolosAperturaD2.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlSimultaneidadCierre = ObtenPuntuacionComNums(ObtenSimultaneidadCierre(), vlMOSimPolosCierre);
                    vlListadoPuntuaciones.Add(vlSimultaneidadCierre);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadCierre);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimPolosCierre.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimPolosCierre.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }

                if (vlEquipo.interruptor_contiene == "R" || vlEquipo.interruptor_contiene == "A")
                {
                    try
                    {
                        PuntuacionFase vlSimultContCierrePIREntrePolos = ObtenPuntuacionComNums(ObtenSimulContCierrePIREntrePolos1Camara(), vlMOSimConCierreRPEntrePolos);
                        vlListadoPuntuaciones.Add(vlSimultContCierrePIREntrePolos);
                        vlListaPuntMecOperacion.Add(vlSimultContCierrePIREntrePolos);
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlMOSimConCierreRPEntrePolos.First().varPeso;
                        nuevaPuntuacion.variable = vlMOSimConCierreRPEntrePolos.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                    try
                    {
                        PuntuacionFase vlSimultContCierrePIRMismoPolo = ObtenPuntuacionComNums(ObtenSimulContCierrePIRMismoPolo1Camara(), vlMOSimConCierreRPMismoPoloMulCam);
                        vlListadoPuntuaciones.Add(vlSimultContCierrePIRMismoPolo);
                        vlListaPuntMecOperacion.Add(vlSimultContCierrePIRMismoPolo);
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlMOSimConCierreRPMismoPoloMulCam.First().varPeso;
                        nuevaPuntuacion.variable = vlMOSimConCierreRPMismoPoloMulCam.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                    try
                    {
                        PuntuacionFase vlTiempoEntradaPIR = ObtenPuntuacionComNums(ObtenTiempoEntradaPIR1Camara(), vlMOTEntradaRP);
                        vlListadoPuntuaciones.Add(vlTiempoEntradaPIR);
                        vlListaPuntMecOperacion.Add(vlTiempoEntradaPIR);
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlMOTEntradaRP.First().varPeso;
                        nuevaPuntuacion.variable = vlMOTEntradaRP.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                }
            }
            else
            {
                try
                {
                    PuntuacionFase vlPuntuacionTiempoIntD1 = ObtenPuntuacionComNums(ObtenTiempoApeD1Con2Camaras(), vlMOTAperturaD1);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoIntD1);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoIntD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOTAperturaD1.First().varPeso;
                    nuevaPuntuacion.variable = vlMOTAperturaD1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoIntD2 = ObtenPuntuacionComNums(ObtenTiempoApeD2Con2Camaras(), vlMOTAperturaD2);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoIntD2);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoIntD2);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOTAperturaD2.First().varPeso;
                    nuevaPuntuacion.variable = vlMOTAperturaD2.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoCierre = ObtenPuntuacionComNums(ObtenTiempoCierre2Camaras(), vlMOTCierre);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoCierre);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoCierre);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOTCierre.First().varPeso;
                    nuevaPuntuacion.variable = vlMOTCierre.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                //
                try
                {
                    PuntuacionFase vlSimultaneidadD1 = ObtenPuntuacionComNums(ObtenSimultaneidadD1(), vlMOSimPolosAperturaD1);
                    vlListadoPuntuaciones.Add(vlSimultaneidadD1);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimPolosAperturaD1.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimPolosAperturaD1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlSimultaneidadD2 = ObtenPuntuacionComNums(ObtenSimultaneidadD2(), vlMOSimPolosAperturaD2);
                    vlListadoPuntuaciones.Add(vlSimultaneidadD2);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadD2);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimPolosAperturaD2.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimPolosAperturaD2.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlSimultaneidadCierre = ObtenPuntuacionComNums(ObtenSimultaneidadCierre(), vlMOSimPolosCierre);
                    vlListadoPuntuaciones.Add(vlSimultaneidadCierre);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadCierre);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimPolosCierre.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimPolosCierre.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                //
                try
                {
                    PuntuacionFase vlSimultaneidadContD1 = ObtenPuntuacionComNums(ObtenSimultaneidadContD12Camaras(), vlMOSimConAperturaD1);
                    vlListadoPuntuaciones.Add(vlSimultaneidadContD1);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadContD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimConAperturaD1.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimConAperturaD1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlSimultaneidadContD2 = ObtenPuntuacionComNums(ObtenSimultaneidadContD22Camaras(), vlMOSimConAperturaD2);
                    vlListadoPuntuaciones.Add(vlSimultaneidadContD2);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadContD2);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimConAperturaD2.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimConAperturaD2.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlSimultaneidadContCierre = ObtenPuntuacionComNums(ObtenSimultaneidadContCierre2Camaras(), vlMOSimConCierre);
                    vlListadoPuntuaciones.Add(vlSimultaneidadContCierre);
                    vlListaPuntMecOperacion.Add(vlSimultaneidadContCierre);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlMOSimConCierre.First().varPeso;
                    nuevaPuntuacion.variable = vlMOSimConCierre.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                if (vlEquipo.interruptor_contiene == "R" || vlEquipo.interruptor_contiene == "A")
                {
                    try
                    {
                        PuntuacionFase vlSimultContCierrePIREntrePolos = ObtenPuntuacionComNums(ObtenSimulContCierrePIREntrePolos2Camaras(), vlMOSimConCierreRPEntrePolos);
                        vlListadoPuntuaciones.Add(vlSimultContCierrePIREntrePolos);
                        vlListaPuntMecOperacion.Add(vlSimultContCierrePIREntrePolos);
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlMOSimConCierreRPEntrePolos.First().varPeso;
                        nuevaPuntuacion.variable = vlMOSimConCierreRPEntrePolos.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                    try
                    {
                        PuntuacionFase vlSimultContCierrePIRMismoPolo = ObtenPuntuacionComNums(ObtenSimulContCierrePIRMismoPolo2Camaras(), vlMOSimConCierreRPMismoPoloMulCam);
                        vlListadoPuntuaciones.Add(vlSimultContCierrePIRMismoPolo);
                        vlListaPuntMecOperacion.Add(vlSimultContCierrePIRMismoPolo);
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlMOSimConCierreRPMismoPoloMulCam.First().varPeso;
                        nuevaPuntuacion.variable = vlMOSimConCierreRPMismoPoloMulCam.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                    try
                    {
                        PuntuacionFase vlTiempoEntradaPIR = ObtenPuntuacionComNums(ObtenTiempoEntradaPIR2Camaras(), vlMOTEntradaRP);
                        vlListadoPuntuaciones.Add(vlTiempoEntradaPIR);
                        vlListaPuntMecOperacion.Add(vlTiempoEntradaPIR);
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlMOTEntradaRP.First().varPeso;
                        nuevaPuntuacion.variable = vlMOTEntradaRP.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                }
            }

        }

        public void indSaludPRSecVerifProtecciones()
        {
            /* Secuencias para verificación de protecciones -> Tiempo de Cierre/Apertura (CA) Prueba de disparo libre, comando C-A enviado a partir de interruptor abierto */
            IQueryable<vCatParamVarRango> vlSVPTCierreAperturaDispLibre = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Secuencias para verificación de protecciones") && r.variable.Equals("Tiempo de Cierre/Apertura en disparo libre")).OrderBy(r => r.rango);
            /* Secuencias para verificación de protecciones -> Prueba de antibombeo (con interruptor cerrado, se manda secuencia C-A. El interruptor al final de la secuencia debe permanecer abierto) */
            IQueryable<vCatParamVarRango> vlSVPPbaAntibombeo = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Secuencias para verificación de protecciones") && r.variable.Equals("Prueba de antibombeo (con interruptor cerrado, se manda secuencia C-A. El interruptor al final de la secuencia debe permanecer abierto)")).OrderBy(r => r.rango);
            /* Secuencias para verificación de protecciones -> Discrepancia de polos (con interruptor abierto se manda comando de cierre a un solo polo; como los otros polos permanecen abiertos, el polo que recibe la señal debe regresar a la posición abierto. Repetir en los 3 polos)*/
            IQueryable<vCatParamVarRango> vlSVPDisPolos = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Secuencias para verificación de protecciones") && r.variable.Equals("Discrepancia de polos (con interruptor abierto se manda comando de cierre a un solo polo; como los otros polos permanecen abiertos, el polo que recibe la señal debe regresar a la posición abierto. Repetir en los 3 polos)")).OrderBy(r => r.rango);
            /* Secuencias para verificación de protecciones -> Discrepancia de polos (Cumple/No cumple)*/
            IQueryable<vCatParamVarRango> vlSVPDiscrepanciaPolosCNC = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Secuencias para verificación de protecciones") && r.variable.Equals("Secuencia de discrepancia de polos")).OrderBy(r => r.rango);
            /* Secuencias para verificación de protecciones -> Secuencia nominal de operación: Apertura-t-CierreApertura-t´-CierreApertura (A-CA-CA). Donde t=0.3seg o 3 min y t´=3min */
            IQueryable<vCatParamVarRango> vlSVPSecNominal = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Secuencias para verificación de protecciones") && r.variable.Equals("Secuencia de disparo libre")).OrderBy(r => r.rango);
            //
            String vlValorEvaluar;
            if (vlEquipo.conf_camaras == "C")
            {
                //PuntuacionFase vlPuntuacionPbaDisparoLibre = evaluaPbaDispLibre1Camara(vlSVPTCierreAperturaDispLibre);
                vlValorEvaluar = ObtenSecDisparoLibre();
                if (vlValorEvaluar != null)
                {
                    try
                    {
                        PuntuacionFase vlCumpleDisparoLibre = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlSVPSecNominal);
                        vlListadoPuntuaciones.Add(vlCumpleDisparoLibre);
                        vlListaPuntVerifProtecciones.Add(vlCumpleDisparoLibre);
                        if (vlValorEvaluar == "CU")
                        {
                            try
                            {
                                PuntuacionFase vlTiempoDisparoLibre = ObtenPuntuacionComNums(ObtenTiempoCierreApertura1Camara(), vlSVPTCierreAperturaDispLibre);
                                vlListadoPuntuaciones.Add(vlTiempoDisparoLibre);
                                vlListaPuntVerifProtecciones.Add(vlTiempoDisparoLibre);
                            }
                            catch
                            {
                                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                                nuevaPuntuacion.peso = (decimal)vlSVPTCierreAperturaDispLibre.First().varPeso;
                                nuevaPuntuacion.variable = vlSVPTCierreAperturaDispLibre.First().variable;
                                vlListadoPuntuaciones.Add(nuevaPuntuacion);
                            }
                        }
                        else
                        {
                            PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                            nuevaPuntuacion.peso = (decimal)vlSVPTCierreAperturaDispLibre.First().varPeso;
                            nuevaPuntuacion.variable = vlSVPTCierreAperturaDispLibre.First().variable;
                            vlListadoPuntuaciones.Add(nuevaPuntuacion);
                        }
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlSVPSecNominal.First().varPeso;
                        nuevaPuntuacion.variable = vlSVPSecNominal.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }                    
                }
            }
            else
            {
                vlValorEvaluar = ObtenSecDisparoLibre();
                if (vlValorEvaluar != null)
                {
                    try
                    {
                        PuntuacionFase vlCumpleDisparoLibre = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlSVPSecNominal);
                        vlListadoPuntuaciones.Add(vlCumpleDisparoLibre);
                        vlListaPuntVerifProtecciones.Add(vlCumpleDisparoLibre);
                        if (vlValorEvaluar == "CU")
                        {
                            try
                            {
                                PuntuacionFase vlTiempoDisparoLibre = ObtenPuntuacionComNums(ObtenTiempoCierreApertura2Camaras(), vlSVPTCierreAperturaDispLibre);
                                vlListadoPuntuaciones.Add(vlTiempoDisparoLibre);
                                vlListaPuntVerifProtecciones.Add(vlTiempoDisparoLibre);
                            }
                            catch
                            {
                                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                                nuevaPuntuacion.peso = (decimal)vlSVPTCierreAperturaDispLibre.First().varPeso;
                                nuevaPuntuacion.variable = vlSVPTCierreAperturaDispLibre.First().variable;
                                vlListadoPuntuaciones.Add(nuevaPuntuacion);
                            }

                        }
                        else
                        {
                            PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                            nuevaPuntuacion.peso = (decimal)vlSVPTCierreAperturaDispLibre.First().varPeso;
                            nuevaPuntuacion.variable = vlSVPTCierreAperturaDispLibre.First().variable;
                            vlListadoPuntuaciones.Add(nuevaPuntuacion);
                        }
                    }
                    catch
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlSVPSecNominal.First().varPeso;
                        nuevaPuntuacion.variable = vlSVPSecNominal.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                    
                }
            }
            vlValorEvaluar = ObtenPbaAntibombeo();
            if (vlValorEvaluar != null)
            {
                try
                {
                    PuntuacionFase vlPuntuacionPbaAntibombeo = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlSVPPbaAntibombeo);
                    vlListadoPuntuaciones.Add(vlPuntuacionPbaAntibombeo);
                    vlListaPuntVerifProtecciones.Add(vlPuntuacionPbaAntibombeo);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlSVPPbaAntibombeo.First().varPeso;
                    nuevaPuntuacion.variable = vlSVPPbaAntibombeo.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
            }

            vlValorEvaluar = ObtenDiscrepanciaPolos();
            if (vlValorEvaluar != null)
            {
                try
                {
                    PuntuacionFase vlDisPolos = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlSVPDiscrepanciaPolosCNC);
                    vlListadoPuntuaciones.Add(vlDisPolos);
                    vlListaPuntVerifProtecciones.Add(vlDisPolos);
                    if (vlValorEvaluar == "CU")
                    {
                        try
                        {
                            PuntuacionFase vlTTotalDisPolos = ObtenPuntuacionComNums(ObtenTTotalDiscrepanciaPolos(), vlSVPDisPolos);
                            vlListadoPuntuaciones.Add(vlTTotalDisPolos);
                            vlListaPuntVerifProtecciones.Add(vlTTotalDisPolos);
                        }
                        catch
                        {
                            PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                            nuevaPuntuacion.peso = (decimal)vlSVPDisPolos.First().varPeso;
                            nuevaPuntuacion.variable = vlSVPDisPolos.First().variable;
                            vlListadoPuntuaciones.Add(nuevaPuntuacion);
                        }
                    }
                    else
                    {
                        PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                        nuevaPuntuacion.peso = (decimal)vlSVPDisPolos.First().varPeso;
                        nuevaPuntuacion.variable = vlSVPDisPolos.First().variable;
                        vlListadoPuntuaciones.Add(nuevaPuntuacion);
                    }
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlSVPDiscrepanciaPolosCNC.First().varPeso;
                    nuevaPuntuacion.variable = vlSVPDiscrepanciaPolosCNC.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                
            }
        }

        public void IndSaludPRUnidadRuptora()
        {
            /* Unidad ruptora -> Resistencia estática de contactos (REC) */
            IQueryable<vCatParamVarRango> vlURResEstContactos = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Resistencia estática de contactos (REC)")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Resistencia óhmica de la resistencia de pre inserción */
            IQueryable<vCatParamVarRango> vlURResOhmica = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Resistencia óhmica de la resistencia de pre inserción")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Capacitancia de condensadores */
            IQueryable<vCatParamVarRango> vlURCapCondensadores = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Capacitancia de condensadores")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Humedad de SF6 (100 kPa y 20°C) */
            IQueryable<vCatParamVarRango> vlURHumedad = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Humedad de SF6 (100 kPa y 20°C)")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Pureza de SF6 (a 100 kPa y 20°C) */
            IQueryable<vCatParamVarRango> vlURPureza = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Pureza de SF6 (a 100 kPa y 20°C)")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Diferencial de presión del gas SF6 @20°C */
            IQueryable<vCatParamVarRango> vlURDifPresion = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Diferencial de presión del gas SF6 @20°C")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Aire (%). Se calcula como 100-pureza */
            IQueryable<vCatParamVarRango> vlURAire = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Aire (%). Se calcula como 100-pureza")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Frecuencia de llenado de SF6 (Se refiere al número de veces al año en q se recupera la presión del gas) */
            IQueryable<vCatParamVarRango> vlURFrecLlenado = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Frecuencia de llenado de SF6 (Se refiere al número de veces al año en q se recupera la presión del gas)")).OrderBy(r => r.rango);
            //
            try
            {
                PuntuacionFase vlPuntuacionREC = new PuntuacionFase();
                if (vlEquipo.conf_camaras == "C")
                    vlPuntuacionREC = ObtenPuntuacionComNums(ObtenREC1Camara(), vlURResEstContactos);
                else
                    vlPuntuacionREC = ObtenPuntuacionComNums(ObtenREC2Camaras(), vlURResEstContactos);
                vlListadoPuntuaciones.Add(vlPuntuacionREC);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionREC);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURResEstContactos.First().varPeso;
                nuevaPuntuacion.variable = vlURResEstContactos.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            if (vlEquipo.interruptor_contiene == "R" || vlEquipo.interruptor_contiene == "A")
            {
                try
                {
                    PuntuacionFase vlResOhmPIR = ObtenPuntuacionComNums(ObtenResOhmicaPIR(), vlURResOhmica);
                    vlListadoPuntuaciones.Add(vlResOhmPIR);
                    vlListaPuntUnidadRuptora.Add(vlResOhmPIR);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlURResOhmica.First().varPeso;
                    nuevaPuntuacion.variable = vlURResOhmica.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
            }
            if (vlEquipo.interruptor_contiene == "C" || vlEquipo.interruptor_contiene == "A")
            {
                try
                {
                    PuntuacionFase vlCapacitanciaCondensadores = new PuntuacionFase();

                    if (vlEquipo.conf_camaras == "C")
                        vlCapacitanciaCondensadores = ObtenPuntuacionComNums(ObtenCapacitanciaCond1Camara(), vlURCapCondensadores);
                    else
                        vlCapacitanciaCondensadores = ObtenPuntuacionComNums(ObtenCapacitanciaCond2Camaras(), vlURCapCondensadores);

                    vlListadoPuntuaciones.Add(vlCapacitanciaCondensadores);
                    vlListaPuntUnidadRuptora.Add(vlCapacitanciaCondensadores);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlURCapCondensadores.First().varPeso;
                    nuevaPuntuacion.variable = vlURCapCondensadores.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
            }
            try
            {
                PuntuacionFase vlHumedad = ObtenPuntuacionComNums(ObtenHumedadSF6(), vlURHumedad);
                vlListadoPuntuaciones.Add(vlHumedad);
                vlListaPuntUnidadRuptora.Add(vlHumedad);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURHumedad.First().varPeso;
                nuevaPuntuacion.variable = vlURHumedad.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPureza = ObtenPuntuacionComNums(ObtenPurezaSF6(), vlURPureza);
                vlListadoPuntuaciones.Add(vlPureza);
                vlListaPuntUnidadRuptora.Add(vlPureza);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURPureza.First().varPeso;
                nuevaPuntuacion.variable = vlURPureza.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            //
            try
            {
                evaluaDiferencialPresion();
            }
            catch { }
            //
            try
            {
                PuntuacionFase vlAire = ObtenPuntuacionComNums(ObtenAire(), vlURAire);
                vlListadoPuntuaciones.Add(vlAire);
                vlListaPuntUnidadRuptora.Add(vlAire);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURAire.First().varPeso;
                nuevaPuntuacion.variable = vlURAire.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlFrecuenciaLlenado = ObtenPuntuacionComNums(ObtenFrecuenciaLlenado(), vlURFrecLlenado);
                vlListadoPuntuaciones.Add(vlFrecuenciaLlenado);
                vlListaPuntUnidadRuptora.Add(vlFrecuenciaLlenado);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURFrecLlenado.First().varPeso;
                nuevaPuntuacion.variable = vlURFrecLlenado.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
        }

        #endregion IndicesSalud

        #region MetodosAuxiliaresMecOperacion

        public decimal ObtenTiempoApeD1Con1Camara()
        {
            vdPrMecOperacion vTmp_MecOpeD1C1 = vlPRMecOper.Where(prmo => prmo.tapertura_d1c1 == vlPRMecOper.Max(mo => mo.tapertura_d1c1)).First();
            decimal vlTiempoInterruptor = (decimal)vTmp_MecOpeD1C1.tapertura_d1c1;
            return vlTiempoInterruptor;
        }

        public decimal ObtenTiempoApeD2Con1Camara()
        {
            vdPrMecOperacion vTmpMecOpeD2C1 = vlPRMecOper.Where(prmo => prmo.tapertura_d2c1 == vlPRMecOper.Max(mo => mo.tapertura_d2c1)).First();
            decimal vlTiempoInterruptor = (decimal)vTmpMecOpeD2C1.tapertura_d2c1;
            return vlTiempoInterruptor;
        }

        public decimal ObtenTiempoCierre1Camara()
        {
            vdPrMecOperacion vTmp_MecOpeCierre = vlPRMecOper.Where(prmo => prmo.tcierre_c1 == vlPRMecOper.Max(mo => mo.tcierre_c1)).First();
            decimal vlTiempoCierre = (decimal)vTmp_MecOpeCierre.tcierre_c1;
            return vlTiempoCierre;
        }

        public decimal ObtenTiempoApeD1Con2Camaras()
        {
            vdPrMecOperacion vTmp_MecOpeD1C1 = vlPRMecOper.Where(prmo => prmo.tapertura_d1c1 == vlPRMecOper.Max(mo => mo.tapertura_d1c1)).First();
            vdPrMecOperacion vTmp_MecOpeD1C2 = vlPRMecOper.Where(prmo => prmo.tapertura_d1c2 == vlPRMecOper.Max(mo => mo.tapertura_d1c2)).First();
            decimal vlTiempoInterruptor = 0;
            if (vTmp_MecOpeD1C1.tapertura_d1c1 > vTmp_MecOpeD1C2.tapertura_d1c2)
            {
                if (vTmp_MecOpeD1C1.tapertura_d1c1 > vTmp_MecOpeD1C1.tapertura_d1c2)
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD1C1.tapertura_d1c2;
                }
                else
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD1C1.tapertura_d1c1;
                }
            }
            else
            {
                if (vTmp_MecOpeD1C2.tapertura_d1c1 > vTmp_MecOpeD1C2.tapertura_d1c2)
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD1C2.tapertura_d1c2;
                }
                else
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD1C2.tapertura_d1c1;
                }
            }
            return vlTiempoInterruptor;
        }

        public decimal ObtenTiempoApeD2Con2Camaras()
        {
            vdPrMecOperacion vTmp_MecOpeD2C1 = vlPRMecOper.Where(prmo => prmo.tapertura_d2c1 == vlPRMecOper.Max(mo => mo.tapertura_d2c1)).First();
            vdPrMecOperacion vTmp_MecOpeD2C2 = vlPRMecOper.Where(prmo => prmo.tapertura_d2c2 == vlPRMecOper.Max(mo => mo.tapertura_d2c2)).First();
            decimal vlTiempoInterruptor = 0;
            if (vTmp_MecOpeD2C1.tapertura_d2c1 > vTmp_MecOpeD2C2.tapertura_d2c2)
            {
                if (vTmp_MecOpeD2C1.tapertura_d2c1 > vTmp_MecOpeD2C1.tapertura_d2c2)
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD2C1.tapertura_d2c2;
                }
                else
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD2C1.tapertura_d2c1;
                }
            }
            else
            {
                if (vTmp_MecOpeD2C2.tapertura_d2c1 > vTmp_MecOpeD2C2.tapertura_d2c2)
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD2C2.tapertura_d2c2;
                }
                else
                {
                    vlTiempoInterruptor = (decimal)vTmp_MecOpeD2C2.tapertura_d2c1;
                }
            }
            return vlTiempoInterruptor;
        }

        public decimal ObtenTiempoCierre2Camaras()
        {
            vdPrMecOperacion vTmp_MecOpeCierreC1 = vlPRMecOper.Where(prmo => prmo.tcierre_c1 == vlPRMecOper.Max(mo => mo.tcierre_c1)).First();
            vdPrMecOperacion vTmp_MecOpeCierreC2 = vlPRMecOper.Where(prmo => prmo.tcierre_c2 == vlPRMecOper.Max(mo => mo.tcierre_c2)).First();
            decimal vlTiempoInterruptor = 0;
            if (vTmp_MecOpeCierreC1.tcierre_c1 > vTmp_MecOpeCierreC2.tcierre_c2)
            {
                vlTiempoInterruptor = (decimal)vTmp_MecOpeCierreC1.tcierre_c1;
            }
            else
            {
                vlTiempoInterruptor = (decimal)vTmp_MecOpeCierreC2.tcierre_c2;
            }
            return vlTiempoInterruptor;
        }


        public decimal ObtenSimultaneidadD1()
        {
            decimal vlSimultaneidadD1, vMinimoValorFA, vMinimoValorFB, vMinimoValorFC, vMinimoValor, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();

            vMinimoValorFA = MinimoDeDosNumeros((decimal)vMecOpeSFA.tapertura_d1c1, vMecOpeSFA.tapertura_d1c2);
            vMinimoValorFB = MinimoDeDosNumeros((decimal)vMecOpeSFB.tapertura_d1c1, vMecOpeSFB.tapertura_d1c2);
            vMinimoValorFC = MinimoDeDosNumeros((decimal)vMecOpeSFC.tapertura_d1c1, vMecOpeSFC.tapertura_d1c2);
            //
            vMinimoValor = MinimoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinimoValorFC);
            //
            vMaximoValor = MaximoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMinimoValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vlSimultaneidadD1 = vMaximoValor - vMinimoValor;
            return vlSimultaneidadD1;
        }

        public decimal ObtenSimultaneidadD2()
        {
            decimal vlSimultaneidadD2, vMinimoValorFA, vMinimoValorFB, vMinimoValorFC, vMinimoValor, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vMinimoValorFA = MinimoDeDosNumeros((decimal)vMecOpeSFA.tapertura_d2c1, vMecOpeSFA.tapertura_d2c2);
            vMinimoValorFB = MinimoDeDosNumeros((decimal)vMecOpeSFB.tapertura_d2c1, vMecOpeSFB.tapertura_d2c2);
            vMinimoValorFC = MinimoDeDosNumeros((decimal)vMecOpeSFC.tapertura_d2c1, vMecOpeSFC.tapertura_d2c2);
            //
            vMinimoValor = MinimoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinimoValorFC);
            //
            vMaximoValor = MaximoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMinimoValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidadD2 = vMaximoValor - vMinimoValor;
            //
            return vlSimultaneidadD2;
        }

        public decimal ObtenSimultaneidadCierre()
        {
            decimal vlSimultaneidadCierre, vMaximoValorFA, vMaximoValorFB, vMaximoValorFC, vMinimoValor, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValorFA = MaximoDeDosNumeros((decimal)vMecOpeSFA.tcierre_c1, vMecOpeSFA.tcierre_c2);
            vMaximoValorFB = MaximoDeDosNumeros((decimal)vMecOpeSFB.tcierre_c1, vMecOpeSFB.tcierre_c2);
            vMaximoValorFC = MaximoDeDosNumeros((decimal)vMecOpeSFC.tcierre_c1, vMecOpeSFC.tcierre_c2);
            //
            vMinimoValor = MinimoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMaximoValorFC);
            //
            vMaximoValor = MaximoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaximoValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidadCierre = vMaximoValor - vMinimoValor;
            //
            return vlSimultaneidadCierre;
        }

        public decimal ObtenSimultaneidadContD12Camaras()
        {
            decimal vlSimultaneidad, vValorFA, vValorFB, vValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vValorFA = Math.Abs((decimal)(vMecOpeSFA.tapertura_d1c1 - vMecOpeSFA.tapertura_d1c2));
            vValorFB = Math.Abs((decimal)(vMecOpeSFB.tapertura_d1c1 - vMecOpeSFB.tapertura_d1c2));
            vValorFC = Math.Abs((decimal)(vMecOpeSFC.tapertura_d1c1 - vMecOpeSFC.tapertura_d1c2));
            //
            vMaximoValor = MaximoDeDosNumeros(vValorFA, vValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidad = vMaximoValor;
            //
            return vlSimultaneidad;
        }

        public decimal ObtenSimultaneidadContD22Camaras()
        {
            decimal vlSimultaneidad, vValorFA, vValorFB, vValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vValorFA = Math.Abs((decimal)(vMecOpeSFA.tapertura_d2c1 - vMecOpeSFA.tapertura_d2c2));
            vValorFB = Math.Abs((decimal)(vMecOpeSFB.tapertura_d2c1 - vMecOpeSFB.tapertura_d2c2));
            vValorFC = Math.Abs((decimal)(vMecOpeSFC.tapertura_d2c1 - vMecOpeSFC.tapertura_d2c2));
            //
            vMaximoValor = MaximoDeDosNumeros(vValorFA, vValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidad = vMaximoValor;
            //
            return vlSimultaneidad;
        }

        public decimal ObtenSimultaneidadContCierre2Camaras()
        {
            decimal vlSimultaneidad, vValorFA, vValorFB, vValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vValorFA = Math.Abs((decimal)(vMecOpeSFA.tcierre_c1 - vMecOpeSFA.tcierre_c2));
            vValorFB = Math.Abs((decimal)(vMecOpeSFB.tcierre_c1 - vMecOpeSFB.tcierre_c2));
            vValorFC = Math.Abs((decimal)(vMecOpeSFC.tcierre_c1 - vMecOpeSFC.tcierre_c2));
            //
            vMaximoValor = MaximoDeDosNumeros(vValorFA, vValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidad = vMaximoValor;
            //
            return vlSimultaneidad;
        }


        public decimal ObtenSimulContCierrePIREntrePolos1Camara()
        {
            decimal vlSimultaneidad, vMinimoValorFA, vMinimoValorFB, vMinimoValorFC, vMinimoValor, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();

            vMinimoValorFA = (decimal)vMecOpeSFA.tent_resprein_c1;
            vMinimoValorFB = (decimal)vMecOpeSFB.tent_resprein_c1;
            vMinimoValorFC = (decimal)vMecOpeSFC.tent_resprein_c1;
            //
            vMinimoValor = MinimoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinimoValorFC);
            //
            vMaximoValor = MaximoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMinimoValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vlSimultaneidad = vMaximoValor - vMinimoValor;
            return vlSimultaneidad;
        }

        public decimal ObtenSimulContCierrePIRMismoPolo1Camara()
        {
            decimal vlSimultaneidad, vValorFA, vValorFB, vValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vValorFA = Math.Abs((decimal)vMecOpeSFA.tent_resprein_c1);
            vValorFB = Math.Abs((decimal)vMecOpeSFB.tent_resprein_c1);
            vValorFC = Math.Abs((decimal)vMecOpeSFC.tent_resprein_c1);
            //
            vMaximoValor = MaximoDeDosNumeros(vValorFA, vValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidad = vMaximoValor;
            //
            return vlSimultaneidad;
        }

        public decimal ObtenTiempoEntradaPIR1Camara()
        {
            decimal vlTiempoEntradaResisPrein, vMaximoValorFA, vMaximoValorFB, vMaximoValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValorFA = (decimal)vMecOpeSFA.tent_resprein_c1;
            vMaximoValorFB = (decimal)vMecOpeSFB.tent_resprein_c1;
            vMaximoValorFC = (decimal)vMecOpeSFC.tent_resprein_c1;
            //
            vMaximoValor = MaximoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaximoValorFC);
            // Tiempo de entrada de la resistencia de preinserción
            vlTiempoEntradaResisPrein = vMaximoValor;
            //
            return vlTiempoEntradaResisPrein;
        }


        public decimal ObtenSimulContCierrePIREntrePolos2Camaras()
        {
            decimal vlSimultaneidad, vMinimoValorFA, vMinimoValorFB, vMinimoValorFC, vMinimoValor, vMaximoValor;
            decimal vMaximoValorFA, vMaximoValorFB, vMaximoValorFC;
            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();

            vMinimoValorFA = MinimoDeDosNumeros((decimal)vMecOpeSFA.tent_resprein_c1, vMecOpeSFA.tent_resprein_c2);
            vMinimoValorFB = MinimoDeDosNumeros((decimal)vMecOpeSFB.tent_resprein_c1, vMecOpeSFB.tent_resprein_c2);
            vMinimoValorFC = MinimoDeDosNumeros((decimal)vMecOpeSFC.tent_resprein_c1, vMecOpeSFC.tent_resprein_c2);
            vMaximoValorFA = MaximoDeDosNumeros((decimal)vMecOpeSFA.tent_resprein_c1, vMecOpeSFA.tent_resprein_c2);
            vMaximoValorFB = MaximoDeDosNumeros((decimal)vMecOpeSFB.tent_resprein_c1, vMecOpeSFB.tent_resprein_c2);
            vMaximoValorFC = MaximoDeDosNumeros((decimal)vMecOpeSFC.tent_resprein_c1, vMecOpeSFC.tent_resprein_c2);
            //
            vMinimoValor = MinimoDeDosNumeros(vMinimoValorFA, vMinimoValorFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinimoValorFC);
            //
            vMaximoValor = MaximoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaximoValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vlSimultaneidad = vMaximoValor - vMinimoValor;
            return vlSimultaneidad;
        }

        public decimal ObtenSimulContCierrePIRMismoPolo2Camaras()
        {
            decimal vlSimultaneidad, vValorFA, vValorFB, vValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vValorFA = Math.Abs((decimal)(vMecOpeSFA.tent_resprein_c1 - vMecOpeSFA.tent_resprein_c2));
            vValorFB = Math.Abs((decimal)(vMecOpeSFB.tent_resprein_c1 - vMecOpeSFB.tent_resprein_c2));
            vValorFC = Math.Abs((decimal)(vMecOpeSFC.tent_resprein_c1 - vMecOpeSFC.tent_resprein_c2));
            //
            vMaximoValor = MaximoDeDosNumeros(vValorFA, vValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vValorFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 2
            vlSimultaneidad = vMaximoValor;
            //
            return vlSimultaneidad;
        }

        public decimal ObtenTiempoEntradaPIR2Camaras()
        {
            decimal vlTiempoEntradaResisPrein, vMaximoValorFA, vMaximoValorFB, vMaximoValorFC, vMaximoValor;

            vdPrMecOperacion vMecOpeSFA = vlPRMecOper.Where(prmo => prmo.fase == "A").First();
            vdPrMecOperacion vMecOpeSFB = vlPRMecOper.Where(prmo => prmo.fase == "B").First();
            vdPrMecOperacion vMecOpeSFC = vlPRMecOper.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValorFA = MaximoDeDosNumeros((decimal)vMecOpeSFA.tent_resprein_c1, vMecOpeSFA.tent_resprein_c2);
            vMaximoValorFB = MaximoDeDosNumeros((decimal)vMecOpeSFB.tent_resprein_c1, vMecOpeSFB.tent_resprein_c2);
            vMaximoValorFC = MaximoDeDosNumeros((decimal)vMecOpeSFC.tent_resprein_c1, vMecOpeSFC.tent_resprein_c2);
            //
            vMaximoValor = MaximoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaximoValorFC);
            // Tiempo de entrada de la resistencia de preinserción
            vlTiempoEntradaResisPrein = vMaximoValor;
            //
            return vlTiempoEntradaResisPrein;
        }

        #endregion

        #region MetodosAuxiliaresVerificacionProtecciones

        private PuntuacionFase evaluaPbaDispLibre1Camara(IQueryable<vCatParamVarRango> pVlSVPDispLibre)
        {
            PuntuacionFase vlPuntuacionDisparoLibre = new PuntuacionFase();
            String vlValorEvaluar = ObtenSecDisparoLibre();
            if (vlValorEvaluar != null)
            {
                PuntuacionFase vlCumpleDisparoLibre = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, pVlSVPDispLibre);
                PuntuacionFase vlTiempoDisparoLibre = ObtenPuntuacionComNums(ObtenTiempoCierreApertura1Camara(), pVlSVPDispLibre);

                if (vlCumpleDisparoLibre.puntuacionLetra == "A" && vlTiempoDisparoLibre.puntuacionLetra != "A")
                {
                    vlPuntuacionDisparoLibre = vlCumpleDisparoLibre;
                    vlPuntuacionDisparoLibre.valorNumero = vlTiempoDisparoLibre.valorNumero;
                }
                else
                {
                    vlPuntuacionDisparoLibre = vlTiempoDisparoLibre;
                    vlPuntuacionDisparoLibre.valorLetra = vlCumpleDisparoLibre.valorLetra;
                }
            }
            return vlPuntuacionDisparoLibre;
        }

        private PuntuacionFase evaluaPbaDispLibre2Camaras(IQueryable<vCatParamVarRango> pVlSVPDispLibre)
        {
            PuntuacionFase vlPuntuacionDisparoLibre = new PuntuacionFase();
            String vlValorEvaluar = ObtenSecDisparoLibre();
            if (vlValorEvaluar != null)
            {
                PuntuacionFase vlCumpleDisparoLibre = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, pVlSVPDispLibre);
                PuntuacionFase vlTiempoDisparoLibre = ObtenPuntuacionComNums(ObtenTiempoCierreApertura2Camaras(), pVlSVPDispLibre);
                if (vlCumpleDisparoLibre.puntuacionLetra == "A" && vlTiempoDisparoLibre.puntuacionLetra != "A")
                {
                    vlPuntuacionDisparoLibre = vlCumpleDisparoLibre;
                    vlPuntuacionDisparoLibre.valorNumero = vlTiempoDisparoLibre.valorNumero;
                }
                else
                {
                    vlPuntuacionDisparoLibre = vlTiempoDisparoLibre;
                    vlPuntuacionDisparoLibre.valorLetra = vlCumpleDisparoLibre.valorLetra;
                }
            }
            return vlPuntuacionDisparoLibre;
        }

        private string ObtenSecDisparoLibre()
        {
            vdPrSecVerifProtecciones vSecVerifProtec = vlPRSecVerProtec.Where(svp => svp.fase == "A").First();
            String vSecDisLibre = vSecVerifProtec.seccuencia_displibre;

            return vSecDisLibre;
        }

        private decimal ObtenTiempoCierreApertura1Camara()
        {
            decimal vMaximoValor = 0;
            vdPrSecVerifProtecciones vSecVerifProtecFA = vlPRSecVerProtec.Where(svp => svp.fase == "A").First();
            vdPrSecVerifProtecciones vSecVerifProtecFB = vlPRSecVerProtec.Where(svp => svp.fase == "B").First();
            vdPrSecVerifProtecciones vSecVerifProtecFC = vlPRSecVerProtec.Where(svp => svp.fase == "C").First();

            if (vSecVerifProtecFA.tcierreapertura_c1 != null &&
                vSecVerifProtecFB.tcierreapertura_c1 != null &&
                vSecVerifProtecFC.tcierreapertura_c1 != null)
            {
                vMaximoValor = MaximoDeDosNumeros((decimal)vSecVerifProtecFA.tcierreapertura_c1, vSecVerifProtecFB.tcierreapertura_c1);
                vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vSecVerifProtecFC.tcierreapertura_c1);
            }

            return vMaximoValor;
        }

        private decimal ObtenTiempoCierreApertura2Camaras()
        {
            decimal vMaximoValor = 0, vMaximoValorFA, vMaximoValorFB, vMaximoValorFC;
            vdPrSecVerifProtecciones vSecVerifProtecFA = vlPRSecVerProtec.Where(svp => svp.fase == "A").First();
            vdPrSecVerifProtecciones vSecVerifProtecFB = vlPRSecVerProtec.Where(svp => svp.fase == "B").First();
            vdPrSecVerifProtecciones vSecVerifProtecFC = vlPRSecVerProtec.Where(svp => svp.fase == "C").First();

            if (vSecVerifProtecFA.tcierreapertura_c1 != null && vSecVerifProtecFA.tcierreapertura_c2 != null &&
                vSecVerifProtecFB.tcierreapertura_c1 != null && vSecVerifProtecFB.tcierreapertura_c2 != null &&
                vSecVerifProtecFC.tcierreapertura_c1 != null && vSecVerifProtecFC.tcierreapertura_c2 != null)
            {
                vMaximoValorFA = MaximoDeDosNumeros((decimal)vSecVerifProtecFA.tcierreapertura_c1, vSecVerifProtecFA.tcierreapertura_c2);
                vMaximoValorFB = MaximoDeDosNumeros((decimal)vSecVerifProtecFB.tcierreapertura_c1, vSecVerifProtecFB.tcierreapertura_c2);
                vMaximoValorFC = MaximoDeDosNumeros((decimal)vSecVerifProtecFC.tcierreapertura_c1, vSecVerifProtecFC.tcierreapertura_c2);

                vMaximoValor = MaximoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
                vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaximoValorFC);
            }

            return vMaximoValor;
        }

        private string ObtenPbaAntibombeo()
        {
            vdPrSecVerifProtecciones vSecVerifProtec = vlPRSecVerProtec.Where(svp => svp.fase == "A").First();
            String vPruebaAntibombeo = vSecVerifProtec.pba_antibombeo;

            return vPruebaAntibombeo;
        }

        private string ObtenDiscrepanciaPolos()
        {
            vdPrSecVerifProtecciones vSecVerifProtec = vlPRSecVerProtec.Where(svp => svp.fase == "A").First();
            String vDiscrepanciaPolos = vSecVerifProtec.discrepancia_polos;

            return vDiscrepanciaPolos;
        }

        private decimal ObtenTTotalDiscrepanciaPolos()
        {
            vdPrSecVerifProtecciones vSecVerifProtec = vlPRSecVerProtec.Where(svp => svp.fase == "A").First();
            decimal vTTotalDiscrepanciaPolos = (decimal)vSecVerifProtec.ttotal_discrepancia;

            return vTTotalDiscrepanciaPolos;
        }

        #endregion

        #region MetodosAuxiliaresUnidadRuptora
        public decimal ObtenREC1Camara()
        {
            decimal vlREC, vMaximoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValor = MaximoDeDosNumeros((decimal)vUniRuptoraFA.res_estat_contactos_d1c1, vUniRuptoraFB.res_estat_contactos_d1c1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vUniRuptoraFC.res_estat_contactos_d1c1);
            vlREC = (decimal)vlEquipo.res_estatica_contactos;
            // Porcentaje de tiempo de entrada de la resistencia estatica de contactos
            vlREC = (vMaximoValor / vlREC) * 100;
            //
            return vlREC;
        }

        public decimal ObtenREC2Camaras()
        {
            decimal vlREC, vMaximoValorFA, vMaximoValorFB, vMaximoValorFC, vMaximoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValorFA = MaximoDeDosNumeros((decimal)vUniRuptoraFA.res_estat_contactos_d1c1, vUniRuptoraFA.res_estat_contactos_d1c2);
            vMaximoValorFB = MaximoDeDosNumeros((decimal)vUniRuptoraFB.res_estat_contactos_d1c1, vUniRuptoraFB.res_estat_contactos_d1c2);
            vMaximoValorFC = MaximoDeDosNumeros((decimal)vUniRuptoraFC.res_estat_contactos_d1c1, vUniRuptoraFC.res_estat_contactos_d1c2);
            //
            vMaximoValor = MaximoDeDosNumeros(vMaximoValorFA, vMaximoValorFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaximoValorFC);
            vlREC = (decimal)vlEquipo.res_estatica_contactos;
            // Tiempo de entrada de la resistencia de preinserción
            vlREC = (vMaximoValor / vlREC) * 100;
            //
            return vlREC;
        }

        public decimal ObtenResOhmicaPIR()
        {
            decimal vlResOhmicaPIR = 0, vMaximoValor = 0, vMinimoValor = 0;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            if (vlEquipo.conf_camaras == "C")
            {
                if (vUniRuptoraFA.resitencia_ohmica_rpi_c1 != null && vUniRuptoraFB.resitencia_ohmica_rpi_c1 != null && vUniRuptoraFC.resitencia_ohmica_rpi_c1 != null)
                {
                    vMaximoValor = MaximoDeDosNumeros((decimal)vUniRuptoraFA.resitencia_ohmica_rpi_c1, vUniRuptoraFB.resitencia_ohmica_rpi_c1);
                    vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vUniRuptoraFC.resitencia_ohmica_rpi_c1);

                    vMinimoValor = MinimoDeDosNumeros((decimal)vUniRuptoraFA.resitencia_ohmica_rpi_c1, vUniRuptoraFB.resitencia_ohmica_rpi_c1);
                    vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vUniRuptoraFC.resitencia_ohmica_rpi_c1);
                }
            }
            else
            {
                if (vUniRuptoraFA.resitencia_ohmica_rpi_c1 != null && vUniRuptoraFB.resitencia_ohmica_rpi_c1 != null && vUniRuptoraFC.resitencia_ohmica_rpi_c1 != null &&
                    vUniRuptoraFA.resitencia_ohmica_rpi_c2 != null && vUniRuptoraFB.resitencia_ohmica_rpi_c2 != null && vUniRuptoraFC.resitencia_ohmica_rpi_c2 != null)
                {
                    decimal vMaxFA = MaximoDeDosNumeros((decimal)vUniRuptoraFA.resitencia_ohmica_rpi_c1, vUniRuptoraFA.resitencia_ohmica_rpi_c2);
                    decimal vMaxFB = MaximoDeDosNumeros((decimal)vUniRuptoraFB.resitencia_ohmica_rpi_c1, vUniRuptoraFB.resitencia_ohmica_rpi_c2);
                    decimal vMaxFC = MaximoDeDosNumeros((decimal)vUniRuptoraFC.resitencia_ohmica_rpi_c1, vUniRuptoraFC.resitencia_ohmica_rpi_c2);

                    vMaximoValor = MaximoDeDosNumeros(vMaxFA, vMaxFB);
                    vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaxFC);

                    decimal vMinFA = MinimoDeDosNumeros((decimal)vUniRuptoraFA.resitencia_ohmica_rpi_c1, vUniRuptoraFA.resitencia_ohmica_rpi_c2);
                    decimal vMinFB = MinimoDeDosNumeros((decimal)vUniRuptoraFB.resitencia_ohmica_rpi_c1, vUniRuptoraFB.resitencia_ohmica_rpi_c2);
                    decimal vMinFC = MinimoDeDosNumeros((decimal)vUniRuptoraFC.resitencia_ohmica_rpi_c1, vUniRuptoraFC.resitencia_ohmica_rpi_c2);

                    vMinimoValor = MinimoDeDosNumeros(vMinFA, vMinFB);
                    vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinFC);
                }
            }
            //
            var vlRO = (decimal)vlEquipo.interruptor_resistencia;

            vMaximoValor = vMaximoValor / vlRO * 100;
            vMinimoValor = vMinimoValor / vlRO * 100;
            var vResMax = "";
            var vResMin = "";

            if (vMaximoValor >= 95 && vMaximoValor <= 105)
            {
                vResMax = "MB";
                vlResOhmicaPIR = vMaximoValor;
            }
            else if (vMaximoValor < 95 || vMaximoValor > 105)
                vResMax = "MM";

            if (vMinimoValor >= 95 && vMinimoValor <= 105)
            {
                vResMin = "MB";
                vlResOhmicaPIR = vMinimoValor;
            }
            else if (vMinimoValor < 95 || vMinimoValor > 105)
                vResMin = "MM";


            if (vResMax == "MM")
                vlResOhmicaPIR = vMaximoValor;
            else if (vResMin == "MM")
                vlResOhmicaPIR = vMinimoValor;
            //
            return vlResOhmicaPIR;
        }

        public decimal ObtenCapacitanciaCond1Camara()
        {
            decimal vlCapacitanciaCondensador, vMaximoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValor = MaximoDeDosNumeros((decimal)vUniRuptoraFA.capacitancia_condesadores_c1, vUniRuptoraFB.capacitancia_condesadores_c1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vUniRuptoraFC.capacitancia_condesadores_c1);
            //
            var vlCap = (decimal)vlEquipo.interruptor_capacitor;
            vlCapacitanciaCondensador = (vMaximoValor / vlCap) * 100;
            //
            return vlCapacitanciaCondensador;
        }

        public decimal ObtenCapacitanciaCond2Camaras()
        {
            decimal vlCapacitanciaCondensador, vMaximoValor, vMaxFA, vMaxFB, vMaxFC;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            vMaxFA = MaximoDeDosNumeros((decimal)vUniRuptoraFA.capacitancia_condesadores_c1, vUniRuptoraFA.capacitancia_condesadores_c2);
            vMaxFB = MaximoDeDosNumeros((decimal)vUniRuptoraFB.capacitancia_condesadores_c1, vUniRuptoraFB.capacitancia_condesadores_c2);
            vMaxFC = MaximoDeDosNumeros((decimal)vUniRuptoraFC.capacitancia_condesadores_c1, vUniRuptoraFC.capacitancia_condesadores_c2);

            vMaximoValor = MaximoDeDosNumeros(vMaxFA, vMaxFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaxFC);
            //
            var vlCap = (decimal)vlEquipo.interruptor_capacitor;
            vlCapacitanciaCondensador = (vMaximoValor / vlCap) * 100;
            //
            return vlCapacitanciaCondensador;
        }

        public decimal ObtenHumedadSF6()
        {
            decimal vlHumedadSF6, vMaximoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            vMaximoValor = MaximoDeDosNumeros((decimal)vUniRuptoraFA.humedad, vUniRuptoraFB.humedad);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vUniRuptoraFC.humedad);
            //
            vlHumedadSF6 = vMaximoValor;
            //
            return vlHumedadSF6;
        }

        public decimal ObtenPurezaSF6()
        {
            decimal vlPurezaSF6, vMinimoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            vMinimoValor = MinimoDeDosNumeros((decimal)vUniRuptoraFA.purezasf6, vUniRuptoraFB.purezasf6);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vUniRuptoraFC.purezasf6);
            //
            vlPurezaSF6 = vMinimoValor;
            //
            return vlPurezaSF6;
        }

        #region DiferencialPresion

        public decimal obtenPresionOperacion()
        {
            decimal vPresionOperacion = 0;
            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            // Presión de gas del equipo
            if (vUniRuptoraFA.unidadPresionSF6 != null)
            {
                if (vUniRuptoraFA.unidadPresionSF6 == "M")
                    vPresionOperacion = ConvierteMPaBar(vUniRuptoraFA.presionSF6);
                else
                    vPresionOperacion = vUniRuptoraFA.presionSF6;
            }

            return vPresionOperacion;
        }

        public decimal obtenPresionAlarma()
        {
            decimal vPresionAlarma = 0;
            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            // Presión de gas del equipo
            vPresionAlarma = ConvierteMPaBar((decimal)vUniRuptoraFA.presion_alarma);
            return vPresionAlarma;
        }

        private decimal obtenPromedioPresionOperacion(decimal pPresionOperacion, decimal pPresionAlarma)
        {
            decimal promedio = 0;
            promedio = (pPresionOperacion + pPresionAlarma) / 2;
            return promedio;
        }

        private void evaluaDiferencialPresion()
        {
            /* Unidad ruptora -> Diferencial de presión del gas SF6 @20°C */
            IQueryable<vCatParamVarRango> vlURDifPresion = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas de rutina") &&
            r.parametro.Equals("Unidad ruptora") && r.variable.Equals("Diferencial de presión del gas SF6 @20°C")).OrderBy(r => r.rango);
            vCatParamVarRango vRango = vlURDifPresion.First();

            // Datos iniciales
            decimal vlPresionOperacion = obtenPresionOperacion();
            decimal vlPresionAlarma = obtenPresionAlarma();

            // Datos para Bueno
            decimal vlRangoInferiorBueno = vlPresionOperacion - (decimal)0.2;
            decimal vlRangoSuperiorBueno = vlPresionOperacion + (decimal)0.2;
            // Datos para Regular
            decimal vlPresionPromedio = obtenPromedioPresionOperacion(vlPresionOperacion, vlPresionAlarma);
            decimal vlRangoInferiorRegular = vlPresionPromedio;
            decimal vlRangoSuperiorRegular = vlPresionOperacion + (decimal)0.8;
            //Datos para Malo
            //decimal vlRangoInferiorMalo = vlPresionOperacion + (decimal)0.8;
            //decimal vlRangoSuperiorMalo = vlPresionOperacion + (decimal)0.8;
            //
            decimal vlMinimoDiferencialPresion = ObtenMinDifPresionSF6();
            decimal vlMaximoDiferencialPresion = ObtenMaxDifPresionSF6();

            PuntuacionFase puntuacionVariableMin = new PuntuacionFase();
            // Evaluar Minimo
            if (vlMinimoDiferencialPresion >= vlRangoInferiorBueno && vlMinimoDiferencialPresion <= vlRangoSuperiorBueno) // Evaluar Rango bueno
            {
                puntuacionVariableMin.valorNumero = vlMinimoDiferencialPresion;
                puntuacionVariableMin.puntuacionLetra = "A";
            }
            else if (vlMinimoDiferencialPresion >= vlRangoInferiorRegular && vlMinimoDiferencialPresion < vlRangoInferiorBueno) // Regular inferior
            {
                puntuacionVariableMin.valorNumero = vlMinimoDiferencialPresion;
                puntuacionVariableMin.puntuacionLetra = "C";
            }
            else if (vlMinimoDiferencialPresion > vlRangoSuperiorBueno && vlMinimoDiferencialPresion <= vlRangoSuperiorRegular) // Regular superior
            {
                puntuacionVariableMin.valorNumero = vlMinimoDiferencialPresion;
                puntuacionVariableMin.puntuacionLetra = "C";
            }
            else if (vlMinimoDiferencialPresion > vlRangoSuperiorRegular) // Malo superior
            {
                puntuacionVariableMin.valorNumero = vlMinimoDiferencialPresion;
                puntuacionVariableMin.puntuacionLetra = "E";
            }
            else if (vlMinimoDiferencialPresion < vlRangoInferiorRegular) // Malo inferior
            {
                puntuacionVariableMin.valorNumero = vlMinimoDiferencialPresion;
                puntuacionVariableMin.puntuacionLetra = "E";
            }
            puntuacionVariableMin.puntuacionNumero = UnaPuntuacionLetraNumero(puntuacionVariableMin.puntuacionLetra);
            // Evauar Máximo
            PuntuacionFase puntuacionVariableMax = new PuntuacionFase();
            if (vlMaximoDiferencialPresion >= vlRangoInferiorBueno && vlMaximoDiferencialPresion <= vlRangoSuperiorBueno) // Evaluar Rango bueno
            {
                puntuacionVariableMax.valorNumero = vlMaximoDiferencialPresion;
                puntuacionVariableMax.puntuacionLetra = "A";
            }
            else if (vlMaximoDiferencialPresion >= vlRangoInferiorRegular && vlMaximoDiferencialPresion < vlRangoInferiorBueno) // Regular inferior
            {
                puntuacionVariableMax.valorNumero = vlMaximoDiferencialPresion;
                puntuacionVariableMax.puntuacionLetra = "C";
            }
            else if (vlMaximoDiferencialPresion > vlRangoSuperiorBueno && vlMaximoDiferencialPresion <= vlRangoSuperiorRegular) // Regular superior
            {
                puntuacionVariableMax.valorNumero = vlMaximoDiferencialPresion;
                puntuacionVariableMax.puntuacionLetra = "C";
            }
            else if (vlMaximoDiferencialPresion > vlRangoSuperiorRegular) // Malo superior
            {
                puntuacionVariableMax.valorNumero = vlMaximoDiferencialPresion;
                puntuacionVariableMax.puntuacionLetra = "E";
            }
            //else if (vlMaximoDiferencialPresion > vlPresionAlarma && vlMaximoDiferencialPresion < vlRangoInferiorRegular) // Malo inferior
            else if (vlMaximoDiferencialPresion < vlRangoInferiorRegular) // Malo inferior
            {
                puntuacionVariableMax.valorNumero = vlMaximoDiferencialPresion;
                puntuacionVariableMax.puntuacionLetra = "E";
            }
            else puntuacionVariableMax.puntuacionLetra = "E";

            puntuacionVariableMax.puntuacionNumero = UnaPuntuacionLetraNumero(puntuacionVariableMax.puntuacionLetra);

            PuntuacionFase puntuacionVariable;
            if (puntuacionVariableMin.puntuacionNumero < puntuacionVariableMax.puntuacionNumero)
                puntuacionVariable = puntuacionVariableMin;
            else
                puntuacionVariable = puntuacionVariableMax;

            puntuacionVariable.fase = "";
            puntuacionVariable.peso = (decimal)vRango.varPeso;
            puntuacionVariable.variable = vRango.variable;
            try
            {
                puntuacionVariable.puntuacionNumero = UnaPuntuacionLetraNumero(puntuacionVariable.puntuacionLetra);
                vlListadoPuntuaciones.Add(puntuacionVariable);
                vlListaPuntUnidadRuptora.Add(puntuacionVariable);
            }
            catch {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vRango.varPeso;
                nuevaPuntuacion.variable = vRango.variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
        }

        private decimal ObtenMaxDifPresionSF6()
        {
            decimal vMaximoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();

            // Presión de gas para cada fase
            if (vUniRuptoraFA.unidadPresionSF6Fase == "M") vUniRuptoraFA.presionsf6_fase = ConvierteMPaBar((decimal)vUniRuptoraFA.presionsf6_fase);
            if (vUniRuptoraFB.unidadPresionSF6Fase == "M") vUniRuptoraFB.presionsf6_fase = ConvierteMPaBar((decimal)vUniRuptoraFB.presionsf6_fase);
            if (vUniRuptoraFC.unidadPresionSF6Fase == "M") vUniRuptoraFC.presionsf6_fase = ConvierteMPaBar((decimal)vUniRuptoraFC.presionsf6_fase);
            vMaximoValor = MaximoDeDosNumeros((decimal)vUniRuptoraFA.presionsf6_fase, vUniRuptoraFB.presionsf6_fase);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vUniRuptoraFC.presionsf6_fase);
            //
            return vMaximoValor;
        }

        private decimal ObtenMinDifPresionSF6()
        {
            decimal vMinimoValor, vPresionOperacion = 0;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            // Presión de gas del equipo
            if (vUniRuptoraFA.unidadPresionSF6 != null)
            {
                if (vUniRuptoraFA.unidadPresionSF6 == "M") vPresionOperacion = ConvierteMPaBar(vUniRuptoraFA.presionSF6);
            }
            vPresionOperacion = (vPresionOperacion - (decimal)0.2);
            // Presión de gas para cada fase
            if (vUniRuptoraFA.unidadPresionSF6Fase == "M") vUniRuptoraFA.presionsf6_fase = ConvierteMPaBar((decimal)vUniRuptoraFA.presionsf6_fase);
            if (vUniRuptoraFB.unidadPresionSF6Fase == "M") vUniRuptoraFB.presionsf6_fase = ConvierteMPaBar((decimal)vUniRuptoraFB.presionsf6_fase);
            if (vUniRuptoraFC.unidadPresionSF6Fase == "M") vUniRuptoraFC.presionsf6_fase = ConvierteMPaBar((decimal)vUniRuptoraFC.presionsf6_fase);
            vMinimoValor = MinimoDeDosNumeros((decimal)vUniRuptoraFA.presionsf6_fase, vUniRuptoraFB.presionsf6_fase);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vUniRuptoraFC.presionsf6_fase);
            //
            return vMinimoValor;
        }

        #endregion

        public decimal ObtenAire()
        {
            decimal vlAire, vMaximoValor; //vMinimoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vdPrUnidadRuptora vUniRuptoraFB = vlPRUniRuptora.Where(prmo => prmo.fase == "B").First();
            vdPrUnidadRuptora vUniRuptoraFC = vlPRUniRuptora.Where(prmo => prmo.fase == "C").First();
            //
            /*vMinimoValor = MinimoDeDosNumeros((decimal)(vUniRuptoraFA.aire), (vUniRuptoraFB.aire));
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, (vUniRuptoraFC.aire));
            vlAire = vMinimoValor;*/
            //
            vMaximoValor = MaximoDeDosNumeros((decimal)(vUniRuptoraFA.aire), (vUniRuptoraFB.aire));
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, (vUniRuptoraFC.aire));
            vlAire = vMaximoValor;
            //
            return vlAire;
        }

        public decimal ObtenFrecuenciaLlenado()
        {
            decimal vlFrecuenciaLlenado, vMaximoValor;

            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            vMaximoValor = (decimal)vUniRuptoraFA.frecuencia_llenado_sf6;
            if (vMaximoValor == 2) // Valor 2= Hasta 3 veces al año
                vMaximoValor = 3;
            else if (vMaximoValor == 3) // Valor 3= más de tres veces al año
                vMaximoValor = 4;
            vlFrecuenciaLlenado = vMaximoValor;
            //
            return vlFrecuenciaLlenado;
        }

        #endregion

        #region MetodosUtilerias

        #region IndiceSalud
        private decimal obtenISPR()
        {
            bool vlConfIntIncluirEnOperacion = true;
            bool vlMecOperacionIncluirEnOperacion = true;
            bool vlVerifProteccionesIncluirEnOperacion = true;
            bool vlUnidadRuptoraIncluirEnOperacion = true;
            try
            {
               viCPCmConfInterruptor = obtenCPCms(vlListaPuntConfInterruptor);
            }
            catch {
                vlConfIntIncluirEnOperacion = false;
                viCPCmConfInterruptor = 0;
            }
            try
            {
                viCPCmMecOperacion = obtenCPCms(vlListaPuntMecOperacion);
            }
            catch {
                vlMecOperacionIncluirEnOperacion = false;
                viCPCmMecOperacion = 0;
            }
            try
            {
                viCPCmVerifProtecciones = obtenCPCms(vlListaPuntVerifProtecciones);
            }
            catch {
                vlVerifProteccionesIncluirEnOperacion = false;
                viCPCmVerifProtecciones = 0;
            }
            try
            {
                viCPCmUnidadRuptora = obtenCPCms(vlListaPuntUnidadRuptora);
            }
            catch {
                vlUnidadRuptoraIncluirEnOperacion = false;
                viCPCmUnidadRuptora = 0;
            }
            vCatParamVarRango vlParamConInterruptor = vlParamVarRango.Where(vpr => vpr.parametro == "Confiabilidad del interruptor").First();
            vCatParamVarRango vlParamMecOperacion = vlParamVarRango.Where(vpr => vpr.parametro == "Mecanismo de operación").First();
            vCatParamVarRango vlParamVerifProtecciones = vlParamVarRango.Where(vpr => vpr.parametro == "Secuencias para verificación de protecciones").First();
            vCatParamVarRango vlParamUnidadRuptora = vlParamVarRango.Where(vpr => vpr.parametro == "Unidad ruptora").First();
            decimal vlSumaMultiplicaciones = (viCPCmConfInterruptor * vlParamConInterruptor.parPeso) + (viCPCmMecOperacion * vlParamMecOperacion.parPeso) + 
                                             (viCPCmVerifProtecciones * vlParamVerifProtecciones.parPeso) + (viCPCmUnidadRuptora * vlParamUnidadRuptora.parPeso);
            //decimal vlSumaCPCms = viCPCmConfInterruptor + viCPCmMecOperacion + viCPCmVerifProtecciones + viCPCmUnidadRuptora;
            //decimal vlSumaPPCms = vlParamConInterruptor.parPeso + vlParamMecOperacion.parPeso + vlParamVerifProtecciones.parPeso + vlParamUnidadRuptora.parPeso;

            decimal vlSumaPPCms = 0;
            if (vlConfIntIncluirEnOperacion) vlSumaPPCms += vlParamConInterruptor.parPeso;
            if (vlMecOperacionIncluirEnOperacion) vlSumaPPCms += vlParamMecOperacion.parPeso;
            if (vlVerifProteccionesIncluirEnOperacion) vlSumaPPCms += vlParamVerifProtecciones.parPeso;
            if (vlUnidadRuptoraIncluirEnOperacion) vlSumaPPCms += vlParamUnidadRuptora.parPeso;
            
            return (vlSumaMultiplicaciones / (vlSumaPPCms * 4)) * 100;
        }

        private decimal obtenCPCms(List<PuntuacionFase> pPuntuaciones)
        {
            decimal vlCPC = 0;
            decimal vlSumaMultiplicaciones = 0, vlSumaPuntuaciones = 0;

            foreach (PuntuacionFase unaPuntuacion in pPuntuaciones)
            {
                if (unaPuntuacion.puntuacionLetra != null && unaPuntuacion.puntuacionLetra != "")
                {
                    vlSumaMultiplicaciones += unaPuntuacion.puntuacionNumero * unaPuntuacion.peso;
                    vlSumaPuntuaciones += unaPuntuacion.peso;
                }
            }
            vlCPC = (vlSumaMultiplicaciones / (4 * vlSumaPuntuaciones)) * 4;
            return vlCPC;
        }
        #endregion IndiceSalud

        #region IndiceConfiabilidad
        private decimal ObtenSumaPesosVar()
        {
            decimal vlSumaPesos = 0;
            
            //foreach (vParametrosVariablesPeso vPeso in vlParamVarPesos)
            foreach (PuntuacionFase vPeso in vlListadoPuntuaciones)
            {
                vlSumaPesos += vPeso.peso;
            }
            return vlSumaPesos;
        }

        private decimal ObtenSumaPesosVarPrueba()
        {
            decimal vlPesosConfInterruptor = ObtenSumaPesosVarParametro(vlListaPuntConfInterruptor);
            decimal vlPesosMecOperacion = ObtenSumaPesosVarParametro(vlListaPuntMecOperacion);
            decimal vlPesosVerifProtecciones = ObtenSumaPesosVarParametro(vlListaPuntVerifProtecciones);
            decimal vlPesosUnidadRuptora = ObtenSumaPesosVarParametro(vlListaPuntUnidadRuptora);
           
            return (vlPesosConfInterruptor + vlPesosMecOperacion + vlPesosVerifProtecciones + vlPesosUnidadRuptora);
        }

        private decimal ObtenSumaPesosVarParametro(List<PuntuacionFase> pPuntuaciones)
        {
            decimal vlSumaPesos = 0;

            foreach (PuntuacionFase unaPuntuacion in pPuntuaciones)
            {
                if (unaPuntuacion.puntuacionLetra != null && unaPuntuacion.puntuacionLetra != "")
                    vlSumaPesos += unaPuntuacion.peso;
            }
            return vlSumaPesos;
        }
        #endregion IndiceConfiabilidad

        private decimal MinimoDeDosNumeros(decimal pValor1, decimal? pValor2)
        {
            decimal minimoValor;
            if (pValor2 == null)
            {
                minimoValor = pValor1;
            }
            else
            {
                if (pValor1 < pValor2)
                    minimoValor = pValor1;
                else
                    minimoValor = (decimal)pValor2;
            }
            return minimoValor;
        }

        private decimal MaximoDeDosNumeros(decimal pValor1, decimal? pValor2)
        {
            decimal maximoValor;
            if (pValor2 == null)
            {
                maximoValor = pValor1;
            }
            else
            {
                if (pValor1 > pValor2)
                    maximoValor = pValor1;
                else
                    maximoValor = (decimal)pValor2;
            }
            return maximoValor;
        }

        private decimal UnaPuntuacionLetraNumero(String pPuntuacionLetra)
        {
            decimal vlPuntuacionNumero = 0;
       //    if (pPuntuacionLetra != null) //PIAH 29.10.2018
         //   { 
            foreach (vCatalogoPuntuaciones regCatPuntuacion in vlPuntuacionesNum)
            {
                if (pPuntuacionLetra.Equals(regCatPuntuacion.abreviatura))
                    vlPuntuacionNumero = (decimal)regCatPuntuacion.valorFlotante;
            }
           //}
            return vlPuntuacionNumero;
        }

        private decimal ConvierteMPaBar(decimal pValorMPa)
        {
            return pValorMPa * 10;
        }    

        private PuntuacionFase ObtenPuntuacionComNums(decimal pValorComparar, IQueryable<vCatParamVarRango> pVaribleRangos)
        {
            PuntuacionFase puntuacionVariable = new PuntuacionFase();
            var vPuntuacion = "";
            foreach (vCatParamVarRango vRango in pVaribleRangos)
            {
                if (vRango.valorMenor != null && vRango.valorMayor != null)
                {
                    switch (vRango.observacion)
                    {
                        case "<,>":
                            if (pValorComparar < vRango.valorMenor || pValorComparar > vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case ">,<":
                            if (pValorComparar > vRango.valorMenor && pValorComparar < vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "<=,>=":
                            if (pValorComparar <= vRango.valorMenor || pValorComparar >= vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case ">=,<=":
                            if (pValorComparar >= vRango.valorMenor && pValorComparar <= vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case ">,<=":
                            if (pValorComparar >= vRango.valorMenor && pValorComparar <= vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case ">=,<":
                            if (pValorComparar >= vRango.valorMenor && pValorComparar <= vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                    }

                }
                else if (vRango.valorMenor != null && vRango.valorMayor == null)
                {
                    switch (vRango.observacion)
                    {
                        case ">":
                            if (pValorComparar > vRango.valorMenor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case ">=":
                            if (pValorComparar >= vRango.valorMenor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "<":
                            if (pValorComparar < vRango.valorMenor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "<=":
                            if (pValorComparar <= vRango.valorMenor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "=":
                            if (pValorComparar == vRango.valorMenor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                    }
                }
                else if (vRango.valorMenor == null && vRango.valorMayor != null)
                {
                    switch (vRango.observacion)
                    {
                        case ">":
                            if (pValorComparar > vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case ">=":
                            if (pValorComparar >= vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "<":
                            if (pValorComparar < vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "<=":
                            if (pValorComparar <= vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                        case "=":
                            if (pValorComparar == vRango.valorMayor)
                            {
                                vPuntuacion = vRango.rango;
                            }
                            break;
                    }
                }

                puntuacionVariable.fase = "";
                puntuacionVariable.peso = (decimal)vRango.varPeso;
                puntuacionVariable.valorNumero = pValorComparar;
                puntuacionVariable.puntuacionLetra = vPuntuacion;
                puntuacionVariable.variable = vRango.variable;
                puntuacionVariable.recomendacion = vRango.recomendacion;

                if (vPuntuacion != "")
                {
                    break;
                }
            }
            puntuacionVariable.puntuacionNumero = UnaPuntuacionLetraNumero(vPuntuacion);
            return puntuacionVariable;
        }

        private PuntuacionFase ObtenPuntuacionComAltaMediaBaja(String pValorComparar, IQueryable<vCatParamVarRango> pVaribleRangos)
        {
            PuntuacionFase puntuacionVariable = new PuntuacionFase();
            var vPuntuacion = "";
            foreach (vCatParamVarRango vRango in pVaribleRangos)
            {
                if (vRango.observacion != null)
                {
                    if (pValorComparar.Equals("A") && vRango.observacion.Equals("ALTA"))
                    {
                        vPuntuacion = vRango.rango;
                    }
                    if (pValorComparar.Equals("M") && vRango.observacion.Equals("MEDIA"))
                    {
                        vPuntuacion = vRango.rango;
                    }
                    if (pValorComparar.Equals("B") && vRango.observacion.Equals("BAJA"))
                    {
                        vPuntuacion = vRango.rango;
                    }
                }

                puntuacionVariable.fase = "";
                puntuacionVariable.peso = (decimal)vRango.varPeso;
                puntuacionVariable.valorLetra = pValorComparar;
                puntuacionVariable.puntuacionLetra = vPuntuacion;
                puntuacionVariable.variable = vRango.variable;
                puntuacionVariable.recomendacion = vRango.recomendacion;

                if (vPuntuacion != "")
                {
                    break;
                }
            }
            puntuacionVariable.puntuacionNumero = UnaPuntuacionLetraNumero(vPuntuacion);
            return puntuacionVariable;
        }

        private PuntuacionFase ObtenPuntuacionTipoMecanismo(String pValorComparar, IQueryable<vCatParamVarRango> pVaribleRangos)
        {
            PuntuacionFase puntuacionVariable = new PuntuacionFase();
            var vPuntuacion = "";
            foreach (vCatParamVarRango vRango in pVaribleRangos)
            {
                if (vRango.observacion != null)
                {
                    if (pValorComparar.Equals(vRango.observacion))
                    {
                        vPuntuacion = vRango.rango;
                    }
                }

                puntuacionVariable.fase = "";
                puntuacionVariable.peso = (decimal)vRango.varPeso;
                puntuacionVariable.valorLetra = pValorComparar;
                puntuacionVariable.puntuacionLetra = vPuntuacion;
                puntuacionVariable.variable = vRango.variable;
                puntuacionVariable.recomendacion = vRango.recomendacion;

                if (vPuntuacion != "")
                {
                    break;
                }
            }
            puntuacionVariable.puntuacionNumero = UnaPuntuacionLetraNumero(vPuntuacion);
            return puntuacionVariable;
        }

        private PuntuacionFase ObtenPuntuacionCumpleNoCumple(String pValorComparar, IQueryable<vCatParamVarRango> pVaribleRangos)
        {
            PuntuacionFase puntuacionVariable = new PuntuacionFase();
            var vPuntuacion = "";
            foreach (vCatParamVarRango vRango in pVaribleRangos)
            {
                if (vRango.observacion != null)
                {
                    if (pValorComparar.Equals("CU") && vRango.observacion.Equals("Si lo hace"))
                    {
                        vPuntuacion = vRango.rango;
                    }
                    if (pValorComparar.Equals("NC") && vRango.observacion.Equals("No lo hace"))
                    {
                        vPuntuacion = vRango.rango;
                    }
                }

                puntuacionVariable.fase = "";
                puntuacionVariable.peso = (decimal)vRango.varPeso;
                puntuacionVariable.valorLetra = pValorComparar;
                puntuacionVariable.puntuacionLetra = vPuntuacion;
                puntuacionVariable.variable = vRango.variable;
                puntuacionVariable.recomendacion = vRango.recomendacion;

                if (vPuntuacion != "")
                {
                    break;
                }
            }
            puntuacionVariable.puntuacionNumero = UnaPuntuacionLetraNumero(vPuntuacion);
            return puntuacionVariable;
        }

        private PuntuacionFase ObtenPuntuacionComPorcentaje(decimal pValorComparar, IQueryable<vCatParamVarRango> pVaribleRangos)
        {
            vdPrUnidadRuptora vUniRuptoraFA = vlPRUniRuptora.Where(prmo => prmo.fase == "A").First();
            // Presión de gas del equipo
            decimal vlPresionOperacion = 0, vlPresionOpeMaxima, vlPresionOpeMinima, vlPorcentaje;
            if (vUniRuptoraFA.unidadPresionSF6 == "M")
                vlPresionOperacion = ConvierteMPaBar(vUniRuptoraFA.presionSF6);
            vlPorcentaje = 100 - ((100 * pValorComparar) / vlPresionOperacion);
            vlPorcentaje = vlPorcentaje * -1;
            // Presión de gas para cada fase
            PuntuacionFase puntuacionVariable = new PuntuacionFase();
            var vPuntuacion = "";
            foreach (vCatParamVarRango vRango in pVaribleRangos)
            {
                if (vRango.valorMenor != null && vRango.valorMayor != null)
                {
                    vlPresionOpeMaxima = vlPresionOperacion * (1 + ((decimal)vRango.valorMayor / 100));
                    vlPresionOpeMinima = vlPresionOperacion * (1 + ((decimal)vRango.valorMenor / 100));
                    if (pValorComparar > vlPresionOpeMinima && pValorComparar < vlPresionOpeMaxima)
                    {
                        vPuntuacion = vRango.rango;
                    }
                }
                else if (vRango.valorMenor != null && vRango.valorMayor == null)
                {
                    if (vRango.observacion == "<,>")
                    {
                        vlPresionOpeMaxima = vlPresionOperacion * (1 + ((decimal)vRango.valorMenor / 100));
                        vlPresionOpeMinima = vlPresionOperacion * (1 - ((decimal)vRango.valorMenor / 100));
                        if (pValorComparar < vlPresionOpeMinima || pValorComparar > vlPresionOpeMaxima)
                        {
                            vPuntuacion = vRango.rango;
                        }
                    }
                }
                else if (vRango.valorMenor == null && vRango.valorMayor != null)
                {
                    if (vRango.observacion == "+-")
                    {
                        vlPresionOpeMaxima = vlPresionOperacion * (1 + ((decimal)vRango.valorMayor / 100));
                        vlPresionOpeMinima = vlPresionOperacion * (1 - ((decimal)vRango.valorMayor / 100));
                        if (pValorComparar >= vlPresionOpeMinima && pValorComparar <= vlPresionOpeMaxima)
                        {
                            vPuntuacion = vRango.rango;
                        }
                    }
                    else
                    {
                        vlPresionOpeMaxima = vlPresionOperacion * (1 + ((decimal)vRango.valorMayor / 100));
                        if (pValorComparar < vlPresionOpeMaxima)
                        {
                            vPuntuacion = vRango.rango;
                        }
                    }
                }

                puntuacionVariable.fase = "";
                puntuacionVariable.peso = (decimal)vRango.varPeso;
                puntuacionVariable.valorNumero = pValorComparar;
                puntuacionVariable.valorLetra = String.Format("{0:0.00}", vlPorcentaje);
                puntuacionVariable.puntuacionLetra = vPuntuacion;
                puntuacionVariable.variable = vRango.variable;
                puntuacionVariable.recomendacion = vRango.recomendacion;

                if (vPuntuacion != "")
                {
                    break;
                }
            }

            puntuacionVariable.puntuacionNumero = UnaPuntuacionLetraNumero(vPuntuacion);
            return puntuacionVariable;
        }

        #endregion
    }
}