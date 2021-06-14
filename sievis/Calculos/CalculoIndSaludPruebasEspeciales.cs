using sievis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static sievis.Models.AppEnum;


namespace sievis.Calculos
{
    public class CalculoIndSaludPruebasEspeciales
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        private DBContext dbContext = new DBContext();
        private Equipo vlEquipo;
        private Prueba vlPrueba;
        private Mecanismo vlMecanismo;
        private IQueryable<vCatParamVarRango> vlParamVarRango;
        /* Datos de la prueba especiales */
        private IQueryable<vPeMecanismoOperacion> vlPEMecOper;
        private IQueryable<vPeCondAislamientoUR> vlPEUniRuptora;
        private IQueryable<vCatalogoPuntuaciones> vlPuntuacionesNum;
        
        private const string SINDATOSFABRICANTE = "Sin datos del fabricante para realizar la evaluación.";
        private const string SINDATOSSUFICIENTES = "Sin suficientes datos para realizar la evaluación.";
        private List<PuntuacionFase> vlListaPuntUnidadRuptora { get; set; }
        private List<PuntuacionFase> vlListaPuntMecOperacion { get; set; }
        // vParametrosVariablesPeso
        private IQueryable<vParametrosVariablesPeso> vlParamVarPesos;
        private decimal viCPCmUnidadRuptora;
        private decimal viCPCmMecOperacion;
        #endregion
        public List<PuntuacionFase> vlListadoPuntuaciones { get; set; }


        public CalculoIndSaludPruebasEspeciales(int equipoId, int pruebaId)
        {
            vlEquipo = db.Equipo.SingleOrDefault(de => de.id == equipoId);
            vlPrueba = dbContext.Get(db, equipoId, pruebaId);
            vlMecanismo = db.Mecanismo.SingleOrDefault(me => me.id == vlEquipo.Mecanismo_id);
            vlParamVarRango = db.vCatParamVarRango.Where(cpvr => cpvr.catalogo == "Pruebas especiales");
            /* Datos de la prueba de rutina */
            // Mecanismo de operación
            vlPEMecOper = db.vPeMecanismoOperacion.AsNoTracking().Where(vprmo => vprmo.Prueba_id == pruebaId);
            // Unidad ruptora
            vlPEUniRuptora = db.vPeCondAislamientoUR.AsNoTracking().Where(vprur => vprur.Prueba_id == pruebaId);
            // Para obtener los pesos de cada variable
            vlParamVarPesos = db.vParametrosVariablesPeso.Where(vpvp => vpvp.catalogo == "Pruebas especiales");
            // Puntuaciones para convertir letras a numeros
            vlPuntuacionesNum = db.vCatalogoPuntuaciones.AsNoTracking().Where(cp => cp.nombre == "Puntuación");
            // Para mantener las puntuaciones por prueba y parametro
            vlListadoPuntuaciones = new List<PuntuacionFase>();
            vlListaPuntUnidadRuptora = new List<PuntuacionFase>();
            vlListaPuntMecOperacion = new List<PuntuacionFase>();
            //
        }

        public double IndiceConfiabilidadPruebaEspecial()
        {
            double vlTotalPeso = (double)ObtenSumaPesosVar();
            double vlPesoVarDisponibles = (double)ObtenSumaPesosVarPrueba();
            return (vlPesoVarDisponibles / vlTotalPeso) * 100;
        }

        public double IndicesSaludPruebaEspecial()
        {
            IndSaludPEUnidadRuptora();
            IndSaludPEMecanismoOper();
            return (double)ObtenISPE();
        }

        #region UnidadRuptora
        public void IndSaludPEUnidadRuptora()
        {
            /* Diferencial de longitud de contactos de arqueo (RDC). Sin referencia del fabricante */
            IQueryable<vCatParamVarRango> vlURResEstContactosSRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Diferencial de longitud de contactos de arqueo (RDC). Sin referencia del fabricante")).OrderBy(r => r.rango);
            /* Diferencial de longitud de contactos de arqueo (RDC). Con referencia del fabricante */
            IQueryable<vCatParamVarRango> vlURResEstContactosCRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Diferencial de longitud de contactos de arqueo (RDC). Con referencia del fabricante")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Termografía a terminales superiores */
            IQueryable<vCatParamVarRango> vlURResTerSuperiores = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Termografía a terminales superiores")).OrderBy(r => r.rango);
            /* Unidad ruptora -> Termografía a terminales inferiores */
            IQueryable<vCatParamVarRango> vlURTerInferiores = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Termografía a terminales inferiores")).OrderBy(r => r.rango);
            /* Unidad ruptora ->Acidez de SF6 (SO2 + SOF2 ó HF) 01 */
            IQueryable<vCatParamVarRango> vlURAciSF6HF = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Acidez de SF6 (SO2 + SOF2 ó HF) 01")).OrderBy(r => r.rango);
            /* Unidad ruptora ->Acidez de SF6 (SO2 + SOF2 ó HF) 02 */
            IQueryable<vCatParamVarRango> vlURAciSF6SO2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Acidez de SF6 (SO2 + SOF2 ó HF) 02")).OrderBy(r => r.rango);
            /* Unidad ruptora ->Tetrafloruro de Carbono (CF4) */
            IQueryable<vCatParamVarRango> vlURTetraCarbono = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Condición del aislamiento/ Unidad ruptora") && r.variable.Equals("Tetrafloruro de Carbono (CF4)")).OrderBy(r => r.rango);
            try
            {
                PuntuacionFase vlPuntuacionDifLongConArqueo = ObtenPuntuacionComNums(ObtenDifLongContArqueoSR(), vlURResEstContactosSRef);
                vlListadoPuntuaciones.Add(vlPuntuacionDifLongConArqueo);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionDifLongConArqueo);
            }
            catch { }
            //
            try
            {
                PuntuacionFase vlPuntuacionTermografiaSuperior = ObtenPuntuacionComNums(ObtenDifTermografiaSuperior(), vlURResTerSuperiores);
                vlListadoPuntuaciones.Add(vlPuntuacionTermografiaSuperior);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionTermografiaSuperior);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURResTerSuperiores.First().varPeso;
                nuevaPuntuacion.variable = vlURResTerSuperiores.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            //
            try
            {
                PuntuacionFase vlPuntuacionTermografiaInferior = ObtenPuntuacionComNums(ObtenDifTermografiaInferior(), vlURTerInferiores);
                vlListadoPuntuaciones.Add(vlPuntuacionTermografiaInferior);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionTermografiaInferior);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURTerInferiores.First().varPeso;
                nuevaPuntuacion.variable = vlURTerInferiores.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            //
            try
            {
                PuntuacionFase vlPuntuacionSF6HF = ObtenPuntuacionComNums(ObtenHF(), vlURAciSF6HF);
                vlListadoPuntuaciones.Add(vlPuntuacionSF6HF);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionSF6HF);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURAciSF6HF.First().varPeso;
                nuevaPuntuacion.variable = vlURAciSF6HF.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            //
            try
            {
                PuntuacionFase vlPuntuacionSF6SO2 = ObtenPuntuacionComNums(ObtenSO2(), vlURAciSF6SO2);
                vlListadoPuntuaciones.Add(vlPuntuacionSF6SO2);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionSF6SO2);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURAciSF6SO2.First().varPeso;
                nuevaPuntuacion.variable = vlURAciSF6SO2.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            //
            try
            {
                PuntuacionFase vlPuntuacionSF6CF4 = ObtenPuntuacionComNums(ObtenCF4(), vlURTetraCarbono);
                vlListadoPuntuaciones.Add(vlPuntuacionSF6CF4);
                vlListaPuntUnidadRuptora.Add(vlPuntuacionSF6CF4);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURTetraCarbono.First().varPeso;
                nuevaPuntuacion.variable = vlURTetraCarbono.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
        }
        #endregion UnidadRuptora

        #region MetodosUnidadRuptora

        public decimal ObtenDifLongContArqueoSR()
        {
            decimal vDiferencialc1, vDiferencialc2, vMaximoValorc1, vMinimoValorc1, vMaximoValorc2, vMinimoValorc2, vDiferencial, vMaximoValor, vMinimoValor;


            vPeCondAislamientoUR vTmp_DifLonCAFA = vlPEUniRuptora.Where(pemoip => pemoip.fase == "A").First();
            vPeCondAislamientoUR vTmp_DifLonCAFB = vlPEUniRuptora.Where(pemoip => pemoip.fase == "B").First();
            vPeCondAislamientoUR vTmp_DifLonCAFC = vlPEUniRuptora.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValorc1 = MaximoDeDosNumeros((decimal)vTmp_DifLonCAFA.long_contac_arqueo_c1, vTmp_DifLonCAFB.long_contac_arqueo_c1);
            vMaximoValorc1 = MaximoDeDosNumeros(vMaximoValorc1, vTmp_DifLonCAFC.long_contac_arqueo_c1);

            vMinimoValorc1 = MinimoDeDosNumeros((decimal)vTmp_DifLonCAFA.long_contac_arqueo_c1, vTmp_DifLonCAFB.long_contac_arqueo_c1);
            vMinimoValorc1 = MinimoDeDosNumeros(vMinimoValorc1, vTmp_DifLonCAFC.long_contac_arqueo_c1);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialc1 = vMaximoValorc1 - vMinimoValorc1;
            vDiferencial = vDiferencialc1;

            if (vlEquipo.conf_camaras != "C")
            {
                vMaximoValorc2 = MaximoDeDosNumeros((decimal)vTmp_DifLonCAFA.long_contac_arqueo_c2, vTmp_DifLonCAFB.long_contac_arqueo_c2);
                vMaximoValorc2 = MaximoDeDosNumeros(vMaximoValorc2, vTmp_DifLonCAFC.long_contac_arqueo_c2);


                vMinimoValorc2 = MinimoDeDosNumeros((decimal)vTmp_DifLonCAFA.long_contac_arqueo_c2, vTmp_DifLonCAFB.long_contac_arqueo_c2);
                vMinimoValorc2 = MinimoDeDosNumeros(vMinimoValorc2, vTmp_DifLonCAFC.long_contac_arqueo_c2);
                // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
                vDiferencialc2 = vMaximoValorc2 - vMinimoValorc2;

                vMaximoValor = MaximoDeDosNumeros(vMaximoValorc1, vMaximoValorc2);
                vMinimoValor = MinimoDeDosNumeros(vMinimoValorc1, vMinimoValorc2);

                vDiferencial = vMaximoValor - vMinimoValor;
            }


            return vDiferencial;
        }

        public decimal ObtenDifTermografiaSuperior()
        {
            decimal vDiferencial, vMaximoValor, vMinimoValor;
            decimal vMaxFA, vMaxFB, vMaxFC, vMinFA, vMinFB, vMinFC;

            vPeCondAislamientoUR vTmp_DifTerSupFA = vlPEUniRuptora.Where(pemoip => pemoip.fase == "A").First();
            vPeCondAislamientoUR vTmp_DifTerSupFB = vlPEUniRuptora.Where(pemoip => pemoip.fase == "B").First();
            vPeCondAislamientoUR vTmp_DifTerSupFC = vlPEUniRuptora.Where(pemoip => pemoip.fase == "C").First();

            vMaxFA = MaximoDeDosNumeros((decimal)vTmp_DifTerSupFA.temMaxTermSuperior_c1, vTmp_DifTerSupFA.temMaxTermSuperior_c2);
            vMaxFB = MaximoDeDosNumeros((decimal)vTmp_DifTerSupFB.temMaxTermSuperior_c1, vTmp_DifTerSupFB.temMaxTermSuperior_c2);
            vMaxFC = MaximoDeDosNumeros((decimal)vTmp_DifTerSupFC.temMaxTermSuperior_c1, vTmp_DifTerSupFC.temMaxTermSuperior_c2);

            vMaximoValor = MaximoDeDosNumeros(vMaxFA, vMaxFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaxFC);

            vMinFA = MinimoDeDosNumeros((decimal)vTmp_DifTerSupFA.temMaxTermSuperior_c1, vTmp_DifTerSupFA.temMaxTermSuperior_c2);
            vMinFB = MinimoDeDosNumeros((decimal)vTmp_DifTerSupFB.temMaxTermSuperior_c1, vTmp_DifTerSupFB.temMaxTermSuperior_c2);
            vMinFC = MinimoDeDosNumeros((decimal)vTmp_DifTerSupFC.temMaxTermSuperior_c1, vTmp_DifTerSupFC.temMaxTermSuperior_c2);

            vMinimoValor = MinimoDeDosNumeros(vMinFA, vMinFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencial = vMaximoValor - vMinimoValor;
            return vDiferencial;
        }

        public decimal ObtenDifTermografiaInferior()
        {
            decimal vDiferencial, vMaximoValor, vMinimoValor;
            decimal vMaxFA, vMaxFB, vMaxFC, vMinFA, vMinFB, vMinFC;

            vPeCondAislamientoUR vTmp_DifTerSupFA = vlPEUniRuptora.Where(pemoip => pemoip.fase == "A").First();
            vPeCondAislamientoUR vTmp_DifTerSupFB = vlPEUniRuptora.Where(pemoip => pemoip.fase == "B").First();
            vPeCondAislamientoUR vTmp_DifTerSupFC = vlPEUniRuptora.Where(pemoip => pemoip.fase == "C").First();

            vMaxFA = MaximoDeDosNumeros((decimal)vTmp_DifTerSupFA.temMaxTermInferior_c1, vTmp_DifTerSupFA.temMaxTermInferior_c2);
            vMaxFB = MaximoDeDosNumeros((decimal)vTmp_DifTerSupFB.temMaxTermInferior_c1, vTmp_DifTerSupFB.temMaxTermInferior_c2);
            vMaxFC = MaximoDeDosNumeros((decimal)vTmp_DifTerSupFC.temMaxTermInferior_c1, vTmp_DifTerSupFC.temMaxTermInferior_c2);

            vMaximoValor = MaximoDeDosNumeros(vMaxFA, vMaxFB);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vMaxFC);

            vMinFA = MinimoDeDosNumeros((decimal)vTmp_DifTerSupFA.temMaxTermInferior_c1, vTmp_DifTerSupFA.temMaxTermInferior_c2);
            vMinFB = MinimoDeDosNumeros((decimal)vTmp_DifTerSupFB.temMaxTermInferior_c1, vTmp_DifTerSupFB.temMaxTermInferior_c2);
            vMinFC = MinimoDeDosNumeros((decimal)vTmp_DifTerSupFC.temMaxTermInferior_c1, vTmp_DifTerSupFC.temMaxTermInferior_c2);

            vMinimoValor = MinimoDeDosNumeros(vMinFA, vMinFB);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vMinFC);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencial = vMaximoValor - vMinimoValor;
            return vDiferencial;
        }

        public decimal ObtenHF()
        {
            vPeCondAislamientoUR vTmp_SF6HFA = vlPEUniRuptora.Where(pemoip => pemoip.fase == "A").First();
            vPeCondAislamientoUR vTmp_SF6HFB = vlPEUniRuptora.Where(pemoip => pemoip.fase == "B").First();
            vPeCondAislamientoUR vTmp_SF6HFC = vlPEUniRuptora.Where(pemoip => pemoip.fase == "C").First();

            var vMax = MaximoDeDosNumeros((decimal)vTmp_SF6HFA.subprodsf6_hf, vTmp_SF6HFB.subprodsf6_hf);
            vMax = MaximoDeDosNumeros(vMax, vTmp_SF6HFC.subprodsf6_hf);
            decimal vHF = vMax;

            return vHF;
        }

        public decimal ObtenSO2()
        {
            vPeCondAislamientoUR vTmp_SF6SO2A = vlPEUniRuptora.Where(pemoip => pemoip.fase == "A").First();
            vPeCondAislamientoUR vTmp_SF6SO2B = vlPEUniRuptora.Where(pemoip => pemoip.fase == "B").First();
            vPeCondAislamientoUR vTmp_SF6SO2C = vlPEUniRuptora.Where(pemoip => pemoip.fase == "C").First();

            var vMax = MaximoDeDosNumeros((decimal)vTmp_SF6SO2A.subprodsf6_so2, vTmp_SF6SO2B.subprodsf6_so2);
            vMax = MaximoDeDosNumeros(vMax, vTmp_SF6SO2C.subprodsf6_so2);
            decimal vSO2 = vMax;

            return vSO2;
        }

        public decimal ObtenCF4()
        {
            vPeCondAislamientoUR vTmp_SF6CF4A = vlPEUniRuptora.Where(pemoip => pemoip.fase == "A").First();
            vPeCondAislamientoUR vTmp_SF6CF4B = vlPEUniRuptora.Where(pemoip => pemoip.fase == "B").First();
            vPeCondAislamientoUR vTmp_SF6CF4C = vlPEUniRuptora.Where(pemoip => pemoip.fase == "C").First();

            var vMax = MaximoDeDosNumeros((decimal)vTmp_SF6CF4A.subprodsf6_cf4, vTmp_SF6CF4B.subprodsf6_cf4);
            vMax = MaximoDeDosNumeros(vMax, vTmp_SF6CF4C.subprodsf6_cf4);

            decimal vCF4 = vMax;

            return vCF4;
        }
        #endregion

        #region MecanismoDeOperacion
        public void IndSaludPEMecanismoOper()
        {
            /*Mecanismo de operación*/
            IQueryable<vCatParamVarRango> vlURDifPicoBobinaA1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de corriente pico de la bobina de apertura 1 (entre fases)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo en que los contactos auxiliares cortan la corriente de la bobina de apertura 1*/
            IQueryable<vCatParamVarRango> vlURDifTiempoBobinaA1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo en que los contactos auxiliares cortan la corriente de la bobina de apertura 1")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo total de la bobina de apertura 1 (de 0 a 0)*/
            IQueryable<vCatParamVarRango> vlURDifTiempoTotalA1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo total de la bobina de apertura 1 (de 0 a 0)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo de liberación del trinquete de la bobina de apertura 1*/
            IQueryable<vCatParamVarRango> vlURDifLibTrinqueteA1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo de liberación del trinquete de la bobina de apertura 1")).OrderBy(r => r.rango);

            /*Mecanismo de operación>Diferencial de corriente pico de la bobina de apertura 2 (entre fases)*/
            IQueryable<vCatParamVarRango> vlURDifPicoBobinaA2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de corriente pico de la bobina de apertura 2 (entre fases)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo en que los contactos auxiliares cortan la corriente de la bobina de apertura 2*/
            IQueryable<vCatParamVarRango> vlURDifTiempoBobinaA2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo en que los contactos auxiliares cortan la corriente de la bobina de apertura 2")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo total de la bobina de apertura 2 (de 0 a 0)*/
            IQueryable<vCatParamVarRango> vlURDifTiempoTotalA2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo total de la bobina de apertura 2 (de 0 a 0)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo de liberación del trinquete de la bobina de apertura 2*/
            IQueryable<vCatParamVarRango> vlURDifLibTrinqueteA2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo de liberación del trinquete de la bobina de apertura 2")).OrderBy(r => r.rango);

            /*Mecanismo de operación>Funcionamiento de bobina de apertura 1 al 70% Vn (Tensión mínima)*/
            IQueryable<vCatParamVarRango> vlURFuncAper1_170 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Funcionamiento de bobina de apertura 1 al 70% Vn (Tensión mínima)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Funcionamiento de bobina de apertura 2 al 70% Vn (Tensión mínima)*/
            IQueryable<vCatParamVarRango> vlURFuncAper2_270 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Funcionamiento de bobina de apertura 2 al 70% Vn (Tensión mínima)")).OrderBy(r => r.rango);

            /*Mecanismo de operación>Diferencial de corriente pico de la bobina de cierre*/
            IQueryable<vCatParamVarRango> vlURDifPicoBobinaC = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de corriente pico de la bobina de cierre")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo en que los contactos auxiliares cortan la corriente de la bobina de cierre*/
            IQueryable<vCatParamVarRango> vlURDifTiempoBobinaC = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo en que los contactos auxiliares cortan la corriente de la bobina de cierre")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo total de la bobina de cierre (de 0 a 0)*/
            IQueryable<vCatParamVarRango> vlURDifTiempoTotalBC = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo total de la bobina de cierre (de 0 a 0)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de tiempo de liberación del trinquete de la bobina de cierre*/
            IQueryable<vCatParamVarRango> vlURDifLibTrinqueteBC = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de tiempo de liberación del trinquete de la bobina de cierre")).OrderBy(r => r.rango);

            /*Mecanismo de operación>Funcionamiento de bobina de cierre al 85% Vn (Tensión mínima)*/
            IQueryable<vCatParamVarRango> vlURFuncBC_85 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Funcionamiento de bobina de cierre al 85% Vn (Tensión mínima)")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Prueba de arranque mínimo de bobina de apertura 1*/
            IQueryable<vCatParamVarRango> vlURPruebaArranqueBA1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Prueba de arranque mínimo de bobina de apertura 1")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Prueba de arranque mínimo de bobina de apertura 2*/
            IQueryable<vCatParamVarRango> vlURPruebaArranqueBA2 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Prueba de arranque mínimo de bobina de apertura 2")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Prueba de arranque mínimo  de bobina de cierre*/
            IQueryable<vCatParamVarRango> vlURPruebaArranqueBC = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Prueba de arranque mínimo  de bobina de cierre")).OrderBy(r => r.rango);

            /*Mecanismo de operación>Tiempo de carga de resorte*/
            IQueryable<vCatParamVarRango> vlURTiempoCargaR = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Tiempo de carga de resorte")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de corriente del motor*/
            IQueryable<vCatParamVarRango> vlURDifCorrienteMotor = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de corriente del motor")).OrderBy(r => r.rango);


            /*Mecanismo de operación>Secuencia nominal de operación: Apertura-t-CierreApertura-t´-CierreApertura (A-CA-CA). Donde t=0.3seg o 3 min y t´=3min.*/
            IQueryable<vCatParamVarRango> vlURSecNominalOpeA_CA_CA = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Secuencia nominal de operación: Apertura-t-CierreApertura-t´-CierreApertura (A-CA-CA). Donde t=0.3seg o 3 min y t´=3min.")).OrderBy(r => r.rango);

            if (vlEquipo.comando_cierre == "M") {
                // Apertura 1
                try
                {
                    PuntuacionFase vlPuntuacionPicoApD1 = ObtenPuntuacionComNums(ObtenDifCorrientePicoBobinaA1(), vlURDifPicoBobinaA1);
                    vlListadoPuntuaciones.Add(vlPuntuacionPicoApD1);
                    vlListaPuntMecOperacion.Add(vlPuntuacionPicoApD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlURDifPicoBobinaA1.First().varPeso;
                    nuevaPuntuacion.variable = vlURDifPicoBobinaA1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoApD1 = ObtenPuntuacionComNums(ObtenDifTiempoBobinaA1(), vlURDifTiempoBobinaA1);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoApD1);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoApD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlURDifTiempoBobinaA1.First().varPeso;
                    nuevaPuntuacion.variable = vlURDifTiempoBobinaA1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoTotalApD1 = ObtenPuntuacionComNums(ObtenDifTiempoTotalBobinaA1(), vlURDifTiempoTotalA1);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoTotalApD1);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoTotalApD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlURDifTiempoTotalA1.First().varPeso;
                    nuevaPuntuacion.variable = vlURDifTiempoTotalA1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
                try
                {
                    PuntuacionFase vlPuntuacionTiempoLiberacioApD1 = ObtenPuntuacionComNums(ObtenDifTiempoLibTrinqueteA1(), vlURDifLibTrinqueteA1);
                    vlListadoPuntuaciones.Add(vlPuntuacionTiempoLiberacioApD1);
                    vlListaPuntMecOperacion.Add(vlPuntuacionTiempoLiberacioApD1);
                }
                catch
                {
                    PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                    nuevaPuntuacion.peso = (decimal)vlURDifLibTrinqueteA1.First().varPeso;
                    nuevaPuntuacion.variable = vlURDifLibTrinqueteA1.First().variable;
                    vlListadoPuntuaciones.Add(nuevaPuntuacion);
                }
            }

            //Apertura 2
            try
            {
                PuntuacionFase vlPuntuacionPicoApD2 = ObtenPuntuacionComNums(ObtenDifCorrientePicoBobinaA2(), vlURDifPicoBobinaA2);
                vlListadoPuntuaciones.Add(vlPuntuacionPicoApD2);
                vlListaPuntMecOperacion.Add(vlPuntuacionPicoApD2);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifPicoBobinaA2.First().varPeso;
                nuevaPuntuacion.variable = vlURDifPicoBobinaA2.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPuntuacionTiempoApD2 = ObtenPuntuacionComNums(ObtenDifTiempoBobinaA2(), vlURDifTiempoBobinaA2);
                vlListadoPuntuaciones.Add(vlPuntuacionTiempoApD2);
                vlListaPuntMecOperacion.Add(vlPuntuacionTiempoApD2);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifTiempoBobinaA2.First().varPeso;
                nuevaPuntuacion.variable = vlURDifTiempoBobinaA2.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPuntuacionTiempoTotalApD2 = ObtenPuntuacionComNums(ObtenDifTiempoTotalBobinaA2(), vlURDifTiempoTotalA2);
                vlListadoPuntuaciones.Add(vlPuntuacionTiempoTotalApD2);
                vlListaPuntMecOperacion.Add(vlPuntuacionTiempoTotalApD2);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifTiempoTotalA2.First().varPeso;
                nuevaPuntuacion.variable = vlURDifTiempoTotalA2.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPuntuacionTiempoLiberacioApD2 = ObtenPuntuacionComNums(ObtenDifTiempoLibTrinqueteA2(), vlURDifLibTrinqueteA2);
                vlListadoPuntuaciones.Add(vlPuntuacionTiempoLiberacioApD2);
                vlListaPuntMecOperacion.Add(vlPuntuacionTiempoLiberacioApD2);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifLibTrinqueteA2.First().varPeso;
                nuevaPuntuacion.variable = vlURDifLibTrinqueteA2.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            //Funcionamiento bobina al 70%
            String vlValorEvaluar = ObtenFBobinaApertura1();
            if (vlValorEvaluar != null)
            {
                PuntuacionFase vlPuntuacionTFuncAper1_170 = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlURFuncAper1_170);
                vlListadoPuntuaciones.Add(vlPuntuacionTFuncAper1_170);
                vlListaPuntMecOperacion.Add(vlPuntuacionTFuncAper1_170);
            }
            else
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURFuncAper1_170.First().varPeso;
                nuevaPuntuacion.variable = vlURFuncAper1_170.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            vlValorEvaluar = ObtenFBobinaApertura2();
            if (vlValorEvaluar != null)
            {
                PuntuacionFase vlPuntuacionTFuncAper2_270 = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlURFuncAper2_270);
                vlListadoPuntuaciones.Add(vlPuntuacionTFuncAper2_270);
                vlListaPuntMecOperacion.Add(vlPuntuacionTFuncAper2_270);
            }
            else
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURFuncAper2_270.First().varPeso;
                nuevaPuntuacion.variable = vlURFuncAper2_270.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            // Cierre
            try
            {
                PuntuacionFase vlPuntuacionPicoApBC = ObtenPuntuacionComNums(ObtenDifCorrientePicoBobinaBC(), vlURDifPicoBobinaC);
                vlListadoPuntuaciones.Add(vlPuntuacionPicoApBC);
                vlListaPuntMecOperacion.Add(vlPuntuacionPicoApBC);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifPicoBobinaC.First().varPeso;
                nuevaPuntuacion.variable = vlURDifPicoBobinaC.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPuntuacionTiempoApBC = ObtenPuntuacionComNums(ObtenDifTiempoBobinaBC(), vlURDifTiempoBobinaC);
                vlListadoPuntuaciones.Add(vlPuntuacionTiempoApBC);
                vlListaPuntMecOperacion.Add(vlPuntuacionTiempoApBC);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifTiempoBobinaC.First().varPeso;
                nuevaPuntuacion.variable = vlURDifTiempoBobinaC.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPuntuacionTiempoTotalApBC = ObtenPuntuacionComNums(ObtenDifTiempoTotalBobinaBC(), vlURDifTiempoTotalBC);
                vlListadoPuntuaciones.Add(vlPuntuacionTiempoTotalApBC);
                vlListaPuntMecOperacion.Add(vlPuntuacionTiempoTotalApBC);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifTiempoTotalBC.First().varPeso;
                nuevaPuntuacion.variable = vlURDifTiempoTotalBC.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }
            try
            {
                PuntuacionFase vlPuntuacionTiempoLiberacioApBC = ObtenPuntuacionComNums(ObtenDifTiempoLibTrinqueteBC(), vlURDifLibTrinqueteBC);
                vlListadoPuntuaciones.Add(vlPuntuacionTiempoLiberacioApBC);
                vlListaPuntMecOperacion.Add(vlPuntuacionTiempoLiberacioApBC);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifLibTrinqueteBC.First().varPeso;
                nuevaPuntuacion.variable = vlURDifLibTrinqueteBC.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            vlValorEvaluar = ObtenFuncBC_85();
            if (vlValorEvaluar != null)
            {
                PuntuacionFase vlPuntuacionTFuncBC_85 = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlURFuncBC_85);
                vlListadoPuntuaciones.Add(vlPuntuacionTFuncBC_85);
                vlListaPuntMecOperacion.Add(vlPuntuacionTFuncBC_85);
            }
            else
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURFuncBC_85.First().varPeso;
                nuevaPuntuacion.variable = vlURFuncBC_85.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            try
            {
                //PuntuacionFase vlPuntuacionPruebaArranqueBA1 = ObtenPuntuacionComNums(ObtenDifPruebaArranqueBA1(), vlURPruebaArranqueBA1);
                PuntuacionFase vlPuntuacionPruebaArranqueBA1 = ObtenDifPruebaArranqueBA1();
                vlPuntuacionPruebaArranqueBA1.variable = vlURPruebaArranqueBA1.First().variable;
                vlPuntuacionPruebaArranqueBA1.peso = (decimal)vlURPruebaArranqueBA1.First().varPeso;
                vlListadoPuntuaciones.Add(vlPuntuacionPruebaArranqueBA1);
                vlListaPuntMecOperacion.Add(vlPuntuacionPruebaArranqueBA1);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURPruebaArranqueBA1.First().varPeso;
                nuevaPuntuacion.variable = vlURPruebaArranqueBA1.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            try
            {
                PuntuacionFase vlPuntuacionPruebaArranqueBA2 = ObtenDifPruebaArranqueBA2();
                vlPuntuacionPruebaArranqueBA2.variable = vlURPruebaArranqueBA2.First().variable;
                vlPuntuacionPruebaArranqueBA2.peso = (decimal)vlURPruebaArranqueBA2.First().varPeso;
                vlListadoPuntuaciones.Add(vlPuntuacionPruebaArranqueBA2);
                vlListaPuntMecOperacion.Add(vlPuntuacionPruebaArranqueBA2);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURPruebaArranqueBA2.First().varPeso;
                nuevaPuntuacion.variable = vlURPruebaArranqueBA2.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            try
            {
                //PuntuacionFase vlPuntuacionPruebaArranqueBC = ObtenPuntuacionComNums(ObtenDifPruebaArranqueBC(), vlURPruebaArranqueBC);
                PuntuacionFase vlPuntuacionPruebaArranqueBC = ObtenDifPruebaArranqueBC();
                vlPuntuacionPruebaArranqueBC.variable = vlURPruebaArranqueBC.First().variable;
                vlPuntuacionPruebaArranqueBC.peso = (decimal)vlURPruebaArranqueBC.First().varPeso;
                vlListadoPuntuaciones.Add(vlPuntuacionPruebaArranqueBC);
                vlListaPuntMecOperacion.Add(vlPuntuacionPruebaArranqueBC);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURPruebaArranqueBC.First().varPeso;
                nuevaPuntuacion.variable = vlURPruebaArranqueBC.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            try
            {
                PuntuacionFase vlPuntuacionDifVelCierreCRef_1 = evaluaObtenTCargaResorteFabricante(ObtenDifFTiempoCargaResorte());
                vlListadoPuntuaciones.Add(vlPuntuacionDifVelCierreCRef_1);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifVelCierreCRef_1);
            } catch {  }

            try
            {
                PuntuacionFase vlPuntuacionCorrienteMotor = ObtenPuntuacionComNums(ObtenDifCorrienteMotor(), vlURDifCorrienteMotor);
                vlListadoPuntuaciones.Add(vlPuntuacionCorrienteMotor);
                vlListaPuntMecOperacion.Add(vlPuntuacionCorrienteMotor);
            }
            catch
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURDifCorrienteMotor.First().varPeso;
                nuevaPuntuacion.variable = vlURDifCorrienteMotor.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

            // Velocidad cierre
            try
            {
                PuntuacionFase vlPuntuacionDifVelocidadCierre = evaluaDifVelocidadCierre();
                vlListadoPuntuaciones.Add(vlPuntuacionDifVelocidadCierre);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifVelocidadCierre);
            }catch{ }

            // Diferencial de Velocidad Apertura
            try
            {
                PuntuacionFase vlPuntuacionDifVelocidadApertura = evaluaDifVelocidadApertura();
                vlListadoPuntuaciones.Add(vlPuntuacionDifVelocidadApertura);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifVelocidadApertura);
            }
            catch { }

            // Carrera Total
            try
            {
                PuntuacionFase vlPuntuacionDifCarrTotalApertura = evaluaDifCarrTotalApertura();
                vlListadoPuntuaciones.Add(vlPuntuacionDifCarrTotalApertura);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifCarrTotalApertura);
            }
            catch { }

            // Rebote Apertura
            try
            {
                PuntuacionFase vlPuntuacionDifRebContactFasesApertura = evaluaDifRebContactFasesApertura();
                vlListadoPuntuaciones.Add(vlPuntuacionDifRebContactFasesApertura);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifRebContactFasesApertura);
            }
            catch { }

            // Sobreviaje Apertura 
            try
            {
                PuntuacionFase vlPuntuacionDifSobreviajeApertura = evaluaDifSobreviajeApertura();
                vlListadoPuntuaciones.Add(vlPuntuacionDifSobreviajeApertura);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifSobreviajeApertura);
            }
            catch { }

            //Carrera Total  Cierre               
            try
            {
                PuntuacionFase vlPuntuacionDifCarrTotalCierre = evaluaDifCarrTotalCierre();
                vlListadoPuntuaciones.Add(vlPuntuacionDifCarrTotalCierre);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifCarrTotalCierre);
            }
            catch { }
            // Penetración
            try
            {
                PuntuacionFase vlPuntuacionPenetracionCierre = evaluaPenetracionCierre();
                vlListadoPuntuaciones.Add(vlPuntuacionPenetracionCierre);
                vlListaPuntMecOperacion.Add(vlPuntuacionPenetracionCierre);
            }
            catch { }
            //Rebote Cierre                
            try
            {
                PuntuacionFase vlPuntuacionDifRebContactFasesCierre = evaluaDifRebContactFasesCierre();
                vlListadoPuntuaciones.Add(vlPuntuacionDifRebContactFasesCierre);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifRebContactFasesCierre);
            }
            catch { }
            //Sobreviaje Cierre                  
            try
            {
                PuntuacionFase vlPuntuacionDifSobreviajeCierre = evaluaDifSobreviajeCierre();
                vlListadoPuntuaciones.Add(vlPuntuacionDifSobreviajeCierre);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifSobreviajeCierre);
            }
            catch { }
            //
            try
            {
                vlValorEvaluar = ObtenDifSecNominalOpeA_CA_CA();
            }
            catch { }
            if (vlValorEvaluar != null)
            {
                PuntuacionFase vlPuntuacionDifSecNominalOpeA_CA_CA = ObtenPuntuacionCumpleNoCumple(vlValorEvaluar, vlURSecNominalOpeA_CA_CA);
                vlListadoPuntuaciones.Add(vlPuntuacionDifSecNominalOpeA_CA_CA);
                vlListaPuntMecOperacion.Add(vlPuntuacionDifSecNominalOpeA_CA_CA);
                /*if (vlValorEvaluar == "CU")
                {

                }*/
            }
            else
            {
                PuntuacionFase nuevaPuntuacion = new PuntuacionFase();
                nuevaPuntuacion.peso = (decimal)vlURSecNominalOpeA_CA_CA.First().varPeso;
                nuevaPuntuacion.variable = vlURSecNominalOpeA_CA_CA.First().variable;
                vlListadoPuntuaciones.Add(nuevaPuntuacion);
            }

        }
        #endregion

        #region MetodosMecanismosOperacion

        //Diferenciales a la apertura 1
        public decimal ObtenDifCorrientePicoBobinaA1()
        {
            decimal vPorcentaje, vDiferencialPicoBobina, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifCorrPicoBobinaFA.ipd1, vTmp_DifCorrPicoBobinaFB.ipd1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifCorrPicoBobinaFC.ipd1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifCorrPicoBobinaFA.ipd1, vTmp_DifCorrPicoBobinaFB.ipd1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifCorrPicoBobinaFC.ipd1);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialPicoBobina = (vMaximoValor - vMinimoValor);
            vPorcentaje = vDiferencialPicoBobina / vMaximoValor*100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoBobinaA1()
        {
            decimal vPorcentaje, vDiferencialTiempoBobinaA1, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.t2d1, vTmp_DifFB.t2d1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.t2d1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.t2d1, vTmp_DifFB.t2d1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.t2d1);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoBobinaA1 = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialTiempoBobinaA1 / vMaximoValor*100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoTotalBobinaA1()
        {
            decimal vPorcentaje, vDiferencialTiempoTotalBobinaA1, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.ttd1, vTmp_DifFB.ttd1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.ttd1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.ttd1, vTmp_DifFB.ttd1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.ttd1);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoTotalBobinaA1 = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialTiempoTotalBobinaA1 / vMaximoValor*100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoLibTrinqueteA1()
        {
            decimal vPorcentaje, vDiferencialTiempoLibTrinqueteA1, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.t1d1, vTmp_DifFB.t1d1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.t1d1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.t1d1, vTmp_DifFB.t1d1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.t1d1);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoLibTrinqueteA1 = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialTiempoLibTrinqueteA1 / vMaximoValor*100;
            return vPorcentaje;
        }

        //Diferenciales a la apertura 2
        public decimal ObtenDifCorrientePicoBobinaA2()
        {
            decimal vPorcentaje, vDiferencialPicoBobina, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifCorrPicoBobinaFA.ipd2, vTmp_DifCorrPicoBobinaFB.ipd2);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifCorrPicoBobinaFC.ipd2);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifCorrPicoBobinaFA.ipd2, vTmp_DifCorrPicoBobinaFB.ipd2);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifCorrPicoBobinaFC.ipd2);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialPicoBobina = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialPicoBobina / vMaximoValor*100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoBobinaA2()
        {
            decimal vPorcentaje, vDiferencialTiempoBobinaA1, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.t2d2, vTmp_DifFB.t2d2);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.t2d2);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.t2d2, vTmp_DifFB.t2d2);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.t2d2);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoBobinaA1 = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialTiempoBobinaA1 / vMaximoValor*100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoTotalBobinaA2()
        {
            decimal vPorcentaje, vDiferencialTiempoTotalBobinaA1, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.ttd2, vTmp_DifFB.ttd2);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.ttd2);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.ttd2, vTmp_DifFB.ttd2);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.ttd2);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoTotalBobinaA1 = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialTiempoTotalBobinaA1 / vMaximoValor*100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoLibTrinqueteA2()
        {
            decimal vPorcentaje, vDiferencialTiempoLibTrinqueteA2, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.t1d2, vTmp_DifFB.t1d2);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.t1d2);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.t1d2, vTmp_DifFB.t1d2);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.t1d2);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoLibTrinqueteA2 = vMaximoValor - vMinimoValor;
            vPorcentaje = vDiferencialTiempoLibTrinqueteA2 / vMaximoValor*100;
            return vPorcentaje;
        }

        // Deferenciales al 70%
        public string ObtenFBobinaApertura1()
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            String vbobina_aper70d1 = vTmp_DifFA.func_bobina_aper70d1;

            return vbobina_aper70d1;
        }

        public string ObtenFBobinaApertura2()
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            String vbobina_aper70d2 = vTmp_DifFA.func_bobina_aper70d2;

            return vbobina_aper70d2;
        }

        // Diferenciales al cierre
        public decimal ObtenDifCorrientePicoBobinaBC()
        {
            decimal vPorcentaje, vDiferencialPicoBobina, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifCorrPicoBobinaFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifCorrPicoBobinaFA.ip_cierre, vTmp_DifCorrPicoBobinaFB.ip_cierre);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifCorrPicoBobinaFC.ip_cierre);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifCorrPicoBobinaFA.ip_cierre, vTmp_DifCorrPicoBobinaFB.ip_cierre);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifCorrPicoBobinaFC.ip_cierre);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialPicoBobina = vMaximoValor - vMinimoValor;
            vPorcentaje = (vDiferencialPicoBobina / vMaximoValor) * 100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoBobinaBC()
        {
            decimal vPorcentaje, vDiferencialTiempoBobinaBC, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.t2_cierre, vTmp_DifFB.t2_cierre);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.t2_cierre);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.t2_cierre, vTmp_DifFB.t2_cierre);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.t2_cierre);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoBobinaBC = vMaximoValor - vMinimoValor;
            vPorcentaje = (vDiferencialTiempoBobinaBC / vMaximoValor) * 100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoTotalBobinaBC()
        {
            decimal vPorcentaje, vDiferencialTiempoTotalBobinaC, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.tt_cierre, vTmp_DifFB.tt_cierre);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.tt_cierre);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.ttd1, vTmp_DifFB.tt_cierre);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.tt_cierre);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoTotalBobinaC = vMaximoValor - vMinimoValor;
            vPorcentaje = (vDiferencialTiempoTotalBobinaC / vMaximoValor) * 100;
            return vPorcentaje;
        }

        public decimal ObtenDifTiempoLibTrinqueteBC()
        {
            decimal vPorcentaje, vDiferencialTiempoLibTrinqueteBC, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.t1_cierre, vTmp_DifFB.t1_cierre);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.t1_cierre);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.t1_cierre, vTmp_DifFB.t1_cierre);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.t1_cierre);
            // Simultaneidad de operación de polos en apertura (entre polos) Disparo 1
            vDiferencialTiempoLibTrinqueteBC = vMaximoValor - vMinimoValor;
            vPorcentaje = (vDiferencialTiempoLibTrinqueteBC / vMaximoValor) * 100;
            return vPorcentaje;
        }

        public string ObtenFuncBC_85()
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            String vbobina_FuncBC_85 = vTmp_DifFA.func_bobina_cierre85;

            return vbobina_FuncBC_85;
        }

        // Prueba arranque mínimo
        public PuntuacionFase ObtenDifPruebaArranqueBA1()
        {
            decimal vVDC;
            PuntuacionFase vpf = new PuntuacionFase();
            
            vVDC = (decimal)vlEquipo.voltajeNominalBobina;
            var vVDC70 = vVDC * (decimal)0.70;
            vPeMecanismoOperacion vTmp_DifF = vlPEMecOper.First();
            vpf.valorNumero = (decimal)vTmp_DifF.arranque_minbobd1;

            if (vTmp_DifF.arranque_minbobd1 > vVDC70)
            {
                vpf.puntuacionLetra = "E";
                vpf.puntuacionNumero = 0;
            }
            else if (vTmp_DifF.arranque_minbobd1 <= vVDC70)
            {
                vpf.puntuacionLetra = "A";
                vpf.puntuacionNumero = 4;
            }

            return vpf;
        }

        public PuntuacionFase ObtenDifPruebaArranqueBA2()
        {
            decimal vVDC;
            PuntuacionFase vpf = new PuntuacionFase();

            vVDC = (decimal)vlEquipo.voltajeNominalBobina;
            var vVDC70 = vVDC * (decimal)0.70;
            vPeMecanismoOperacion vTmp_DifF = vlPEMecOper.First();
            vpf.valorNumero = (decimal)vTmp_DifF.arranque_minbobd2;

            if (vTmp_DifF.arranque_minbobd2 > vVDC70)
            {
                vpf.puntuacionLetra = "E";
                vpf.puntuacionNumero = 0;
            } 
            else if (vTmp_DifF.arranque_minbobd2 <= vVDC70)
            {
                vpf.puntuacionLetra = "A";
                vpf.puntuacionNumero = 4;
            }

            return vpf;
        }

        public PuntuacionFase ObtenDifPruebaArranqueBC()
        {
            decimal vVDC;
            PuntuacionFase vpf = new PuntuacionFase();
            vVDC = (decimal)vlEquipo.voltajeNominalBobina;
            var vVDC85 = vVDC * (decimal)0.85;
            vPeMecanismoOperacion vTmp_DifF = vlPEMecOper.First();
            vpf.valorNumero = (decimal)vTmp_DifF.arranque_minbob_cierre;

            if (vTmp_DifF.arranque_minbob_cierre > vVDC85)
            {
                vpf.puntuacionLetra = "E";
                vpf.puntuacionNumero = 0;
            }
            else if (vTmp_DifF.arranque_minbob_cierre <= vVDC85)
            {
                vpf.puntuacionLetra = "A";
                vpf.puntuacionNumero = 4;
            }

            return vpf;
        }

        public decimal ObtenDifFTiempoCargaResorte()
        {
            decimal vDiferencialTiempoCargaR, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.tcarga_resorte, vTmp_DifFB.tcarga_resorte);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.tcarga_resorte);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.tcarga_resorte, vTmp_DifFB.tcarga_resorte);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.tcarga_resorte);

            vDiferencialTiempoCargaR = (vMaximoValor - vMinimoValor) / vMaximoValor;

            return vDiferencialTiempoCargaR;
        }

        private PuntuacionFase evaluaObtenTCargaResorteFabricante(decimal pValor)
        {
            /*Mecanismo de operación>Tiempo de carga de resorte*/
            IQueryable<vCatParamVarRango> vlURTiempoCargaR = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Tiempo de carga de resorte")).OrderBy(r => r.rango);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.variable = vlURTiempoCargaR.First().variable;
            vlPuntuacion.peso = (decimal)vlURTiempoCargaR.First().varPeso;
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.Modelo == vlEquipo.Modelo);

            try
            {
                if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
                {
                    ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                    if (pValor < unParametroFabricante.mtrTCargaResorteLimInf || pValor > unParametroFabricante.mtrTCargaResorteLimSup)
                    {
                        vlPuntuacion.puntuacionLetra = "E";
                        vlPuntuacion.valorNumero = pValor;
                    }
                    else if (pValor >= unParametroFabricante.mtrTCargaResorteLimInf && pValor <= unParametroFabricante.mtrTCargaResorteLimSup)
                    {
                        vlPuntuacion.puntuacionLetra = "A";
                        vlPuntuacion.valorNumero = pValor;
                    }
                }
            }
            catch
            {
                vlPuntuacion.variable = vlPuntuacion.variable + " **" + SINDATOSFABRICANTE;
                return vlPuntuacion;
            }
            if (vlPuntuacion.puntuacionLetra == null)
                vlPuntuacion.variable = vlPuntuacion.variable + " **" + SINDATOSFABRICANTE;
            return vlPuntuacion;
        }

        public decimal ObtenDifCorrienteMotor()
        {
            decimal vPorcentaje, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.imotor, vTmp_DifFB.imotor);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.imotor);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.imotor, vTmp_DifFB.imotor);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.imotor);

            vPorcentaje = ((vMaximoValor - vMinimoValor) / vMaximoValor) * 100;
            return vPorcentaje;
        }

        //Diferenciales de Parametros de Desplazamiento

        #region ParametrosDesplazamientoCierre

        #region DiferencialVelocidad
        public decimal ObtenDifVelCierrePorcentaje()
        {
            decimal vPorcentaje, vDiferencialVelCierreSRef, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.pardescierre_velocidad, vTmp_DifFB.pardescierre_velocidad);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.pardescierre_velocidad);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.pardescierre_velocidad, vTmp_DifFB.pardescierre_velocidad);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.pardescierre_velocidad);

            vDiferencialVelCierreSRef = vMaximoValor - vMinimoValor;
            vPorcentaje = ((vMaximoValor - vMinimoValor) / vMaximoValor)*100;
            return vPorcentaje;
        }

        public PuntuacionFase evaluaVelocidadFabricanteCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento)
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFA.pardescierre_velocidad, "A");
            vlEvaluacionFA.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFA.puntuacionLetra);
            PuntuacionFase vlEvaluacionFB = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFB.pardescierre_velocidad, "B");
            vlEvaluacionFB.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFB.puntuacionLetra);
            PuntuacionFase vlEvaluacionFC = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFC.pardescierre_velocidad, "C");
            vlEvaluacionFC.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFC.puntuacionLetra);

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase evaluaRangoVelFabCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor >= (pLimiteInferior - pValorIncremento) && pValor <= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor < (pLimiteInferior - pValorIncremento) || pValor >= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifVelocidadCierre()
        {
            /*Mecanismo de operación>Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelCierreCRef_1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión=1 y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelCierreSRef_1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión=1 y sin referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelCierreCRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelCierreSRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y sin referencia.")).OrderBy(r => r.rango);
            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURDifVelCierreCRef_1.First().varPeso;

            decimal vlValor = ObtenDifVelCierrePorcentaje();

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.dcVelocidadLimInf != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión=1 y con referencia.
                        decimal vlValFabVelInfCierre = (decimal)unParametroFabricante.dcVelocidadLimInf;
                        decimal vlValFabVelSupCierre = (decimal)unParametroFabricante.dcVelocidadLimSup;
                        //
                        vlPuntuacion = evaluaVelocidadFabricanteCierre(vlValFabVelInfCierre, vlValFabVelSupCierre, (decimal)0.2);
                        vCatParamVarRango vRango = vlURDifVelCierreCRef_1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.dcVelocidadLimInf == null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifVelCierreSRef);
                    }
                    else if (unParametroFabricante.dcVelocidadLimInf != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y con referencia
                        decimal vlValFabVelInfCierre = (decimal)unParametroFabricante.dcVelocidadLimInf;
                        decimal vlValFabVelSupCierre = (decimal)unParametroFabricante.dcVelocidadLimSup;
                        //
                        vlPuntuacion = evaluaVelocidadFabricanteCierre(vlValFabVelInfCierre, vlValFabVelSupCierre, (decimal)0.5);
                        vCatParamVarRango vRango = vlURDifVelCierreCRef.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else
                    {
                        //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifVelCierreSRef_1);
                    }
                    vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                }
                catch { return vlPuntuacion; }
            }
            else
            {
                //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifVelCierreSRef_1);
            }
            return vlPuntuacion;
        }
        #endregion DiferencialVelocidad

        #region CarreraTotal
        public PuntuacionFase evaluaCarreraTotalCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento)
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = evaluaRangoCarreraTotalCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFA.pardescierre_carreratot, "A");
            vlEvaluacionFA.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFA.puntuacionLetra);
            PuntuacionFase vlEvaluacionFB = evaluaRangoCarreraTotalCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFB.pardescierre_carreratot, "B");
            vlEvaluacionFB.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFB.puntuacionLetra);
            PuntuacionFase vlEvaluacionFC = evaluaRangoCarreraTotalCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFC.pardescierre_carreratot, "C");
            vlEvaluacionFC.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFC.puntuacionLetra);

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase evaluaRangoCarreraTotalCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor >= (pLimiteInferior - pValorIncremento) && pValor <= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor < (pLimiteInferior - pValorIncremento) || pValor > (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifCarrTotalCierre()
        {
            /*Mecanismo de operación>Carrera total al cierre 230mm. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURCarreraCierre_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Carrera total al cierre. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Carrera total al cierre 230mm. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURCarreraCierre_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Carrera total al cierre. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURCarreraCierre_C1.First().varPeso;
            //decimal vlValor = ObtenDifCarrTotCierre230();

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.pdCarrTotalLimInf != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        decimal vlValFabVelInfCierre = (decimal)unParametroFabricante.pdCarrTotalLimInf;
                        decimal vlValFabVelSupCierre = (decimal)unParametroFabricante.pdCarrTotalLimSup;
                        //
                        vlPuntuacion = evaluaCarreraTotalCierre(vlValFabVelInfCierre, vlValFabVelSupCierre, (decimal)0.7);
                        vCatParamVarRango vRango = vlURCarreraCierre_C1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.pdCarrTotalLimInf != null && unParametroFabricante.pdFactoConversion != null)
                    {
                        decimal vlValFabVelInfCierre = (decimal)unParametroFabricante.pdCarrTotalLimInf; 
                        decimal vlValFabVelSupCierre = (decimal)unParametroFabricante.pdCarrTotalLimSup;
                        //
                        vlPuntuacion = evaluaCarreraTotalCierre(vlValFabVelInfCierre, vlValFabVelSupCierre, (decimal)2.0);
                        //
                        vCatParamVarRango vRango = vlURCarreraCierre_C.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                }
                catch { return vlPuntuacion; }
            }
            if (vlPuntuacion.puntuacionLetra == null)
                vlPuntuacion.variable = "Carrera total al cierre. ** " + SINDATOSFABRICANTE;
            return vlPuntuacion;
        }
        #endregion CarreraTotal

        #region PenetracionContactos
        public PuntuacionFase evaluaPenetracionFabricanteCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento)
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();
            decimal vlPuntuacionNumero = 0;
           
            PuntuacionFase vlEvaluacionFA = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFA.pardescierre_penetracion, "A");
            vlPuntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFA.puntuacionLetra);
            vlEvaluacionFA.puntuacionNumero = vlPuntuacionNumero;
            PuntuacionFase vlEvaluacionFB = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFB.pardescierre_penetracion, "B");
            vlPuntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFB.puntuacionLetra);
            vlEvaluacionFB.puntuacionNumero = vlPuntuacionNumero;
            PuntuacionFase vlEvaluacionFC = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFC.pardescierre_penetracion, "C");
            vlPuntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFC.puntuacionLetra);
            vlEvaluacionFC.puntuacionNumero = vlPuntuacionNumero;
            
            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            
            if (vlEquipo.conf_camaras != "C")
            {
                PuntuacionFase vlEvaluacionFAC2 = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFA.pardescierre_penetracion_c2, "A");
                vlPuntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFAC2.puntuacionLetra);
                vlEvaluacionFAC2.puntuacionNumero = vlPuntuacionNumero;
                PuntuacionFase vlEvaluacionFBC2 = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFB.pardescierre_penetracion_c2, "B");
                vlPuntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFBC2.puntuacionLetra);
                vlEvaluacionFBC2.puntuacionNumero = vlPuntuacionNumero;
                PuntuacionFase vlEvaluacionFCC2 = evaluaRangoVelFabCierre(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFC.pardescierre_penetracion_c2, "C");
                vlPuntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFCC2.puntuacionLetra);
                vlEvaluacionFCC2.puntuacionNumero = vlPuntuacionNumero;

                vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFAC2);
                vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFBC2);
                vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFCC2);

            }

            return vlEvaluacionMinima;
        }
        
        public PuntuacionFase evaluaRangoPenetracionCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor >= (pLimiteInferior - pValorIncremento) && pValor <= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor < (pLimiteInferior - pValorIncremento) || pValor > (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaPenetracionCierre()
        {
            /*Mecanismo de operación>Penetración de contactos mismos modelos al cierre. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURPenetracionCierreCRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Penetración de contactos mismos modelos al cierre. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Penetración de contactos mismos modelos al cierre. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURPenetracionCierreCRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Penetración de contactos mismos modelos al cierre. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURPenetracionCierreCRef_C1.First().varPeso;

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.dcPenetracionLimInf != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        // Con factor de conversión=1 y con referencia.
                        decimal vlValFabPenetracionInfCierre = (decimal)unParametroFabricante.dcPenetracionLimInf;
                        decimal vlValFabPenetracionSupCierre = (decimal)unParametroFabricante.dcPenetracionLimSup;
                        vlPuntuacion = evaluaPenetracionFabricanteCierre(vlValFabPenetracionInfCierre, vlValFabPenetracionSupCierre, (decimal)1.2);
                        //
                        vCatParamVarRango vRango = vlURPenetracionCierreCRef_C1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.dcPenetracionLimInf != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        //  Con factor de conversión y con referencia
                        decimal vlValFabPenetracionInfCierre = (decimal)unParametroFabricante.dcPenetracionLimInf;
                        decimal vlValFabPenetracionSupCierre = (decimal)unParametroFabricante.dcPenetracionLimSup;
                        vlPuntuacion = evaluaPenetracionFabricanteCierre(vlValFabPenetracionInfCierre, vlValFabPenetracionSupCierre, (decimal)3.0);
                        //
                        vCatParamVarRango vRango = vlURPenetracionCierreCRef_C.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                }
                catch { return vlPuntuacion; }
            }
            
            return vlPuntuacion;
        }
        #endregion PenetracionContactos

        #region ReboteContactos
        public decimal ObtenDifRebContactosEntreFasesCierre()
        {
            decimal vDifRebConEntreFasesApe, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.pardescierre_rebote, vTmp_DifFB.pardescierre_rebote);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.pardescierre_rebote);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.pardescierre_rebote, vTmp_DifFB.pardescierre_rebote);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.pardescierre_rebote);

            vDifRebConEntreFasesApe = (vMaximoValor - vMinimoValor);
            return vDifRebConEntreFasesApe;
        }

        public PuntuacionFase EvaluaRebContactosEntreFasesCierre(decimal pLimiteSuperior)
        {
            vPeMecanismoOperacion vTmp_ReboteFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_ReboteFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_ReboteFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = EvaluaRangoRebContFasesFabCierre(pLimiteSuperior,(decimal)vTmp_ReboteFA.pardescierre_rebote,"A");
            PuntuacionFase vlEvaluacionFB = EvaluaRangoRebContFasesFabCierre(pLimiteSuperior, (decimal)vTmp_ReboteFB.pardescierre_rebote, "B");
            PuntuacionFase vlEvaluacionFC = EvaluaRangoRebContFasesFabCierre(pLimiteSuperior, (decimal)vTmp_ReboteFC.pardescierre_rebote, "C");

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);

            return vlEvaluacionMinima;
        }

        public PuntuacionFase EvaluaRangoRebContFasesFabCierre(decimal pLimiteSuperior, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor <= pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor > pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            return vlPuntuacion;
        }

        public PuntuacionFase EvaluaRebContactosEntreFasesCierre(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento)
        {
            vPeMecanismoOperacion vTmp_ReboteFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_ReboteFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_ReboteFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = EvaluaRangoRebContFasesFabCierreFC1CR(pLimiteInferior,pLimiteSuperior,pValorIncremento, (decimal)vTmp_ReboteFA.pardescierre_rebote, "A");
            PuntuacionFase vlEvaluacionFB = EvaluaRangoRebContFasesFabCierreFC1CR(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_ReboteFB.pardescierre_rebote, "B");
            PuntuacionFase vlEvaluacionFC = EvaluaRangoRebContFasesFabCierreFC1CR(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_ReboteFC.pardescierre_rebote, "C");

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);

            return vlEvaluacionMinima;
        }

        public PuntuacionFase EvaluaRangoRebContFasesFabCierreFC1CR(decimal pLimiteInferior,decimal pLimiteSuperior, decimal pValorIncremento, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor >= (pLimiteInferior - pValorIncremento) && pValor <= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor < (pLimiteInferior - pValorIncremento) || pValor > (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifRebContactFasesCierre()
        {
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURModReboteCierreCRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión=1 y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURModReboteCierreSRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión=1 y sin referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURModReboteCierreCRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURModReboteCierreSRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases al cierre. Con factor de conversión y sin referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURModReboteCierreCRef_C1.First().varPeso;
            decimal vlValor = ObtenDifRebContactosEntreFasesCierre();

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.dcRebote != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        // Con factor de conversión=1 y con referencia.
                        vlPuntuacion = EvaluaRebContactosEntreFasesCierre((decimal)unParametroFabricante.dcRebote, (decimal)unParametroFabricante.dcRebote,(decimal)0.2);
                        vCatParamVarRango vRango = vlURModReboteCierreCRef_C1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.dcRebote == null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURModReboteCierreSRef_C);
                    }
                    else if (unParametroFabricante.dcRebote != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y con referencia
                        vlPuntuacion = EvaluaRebContactosEntreFasesCierre((decimal)unParametroFabricante.dcRebote);
                        vCatParamVarRango vRango = vlURModReboteCierreCRef_C.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else
                    {
                        //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURModReboteCierreSRef_C1);
                        vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                    }
                    vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                }
                catch { return vlPuntuacion; }
            }
            else
            {
                //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURModReboteCierreSRef_C1);
                vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            }
            return vlPuntuacion;
        }
        #endregion ReboteContactos

        #region Sobreviaje
        public decimal ObtenDifSobreviajeCierre()
        {
            decimal vDiferencialSobreviajeCierre, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.pardescierre_sobrerecorrido, vTmp_DifFB.pardescierre_sobrerecorrido);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.pardescierre_sobrerecorrido);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.pardescierre_sobrerecorrido, vTmp_DifFB.pardescierre_sobrerecorrido);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.pardescierre_sobrerecorrido);

            vDiferencialSobreviajeCierre = vMaximoValor - vMinimoValor;
            return vDiferencialSobreviajeCierre;
        }

        public PuntuacionFase EvaluaSobreviajeCierre(decimal pLimiteSuperior)
        {
            vPeMecanismoOperacion vTmp_ReboteFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_ReboteFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_ReboteFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = EvaluaRangoRebContFasesFabCierre(pLimiteSuperior, (decimal)vTmp_ReboteFA.pardescierre_sobrerecorrido, "A");
            PuntuacionFase vlEvaluacionFB = EvaluaRangoRebContFasesFabCierre(pLimiteSuperior, (decimal)vTmp_ReboteFB.pardescierre_sobrerecorrido, "B");
            PuntuacionFase vlEvaluacionFC = EvaluaRangoRebContFasesFabCierre(pLimiteSuperior, (decimal)vTmp_ReboteFC.pardescierre_sobrerecorrido, "C");

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);

            return vlEvaluacionMinima;
        }

        public PuntuacionFase EvaluaRangoSobreviajeCierre(decimal pLimiteSuperior, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor <= pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor > pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifSobreviajeCierre()
        {
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURSobreviajeCierrreCRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión=1 y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURSobreviajeCierreSRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión=1 y sin referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURSobreviajeCierreCRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURSobreviajeCierreSRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión y sin referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURSobreviajeCierrreCRef_C1.First().varPeso;
            decimal vlValor = ObtenDifSobreviajeCierre();

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.dcSobreviaje != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        // Con factor de conversión=1 y con referencia.
                        vlPuntuacion = EvaluaSobreviajeCierre((decimal)unParametroFabricante.dcSobreviaje);
                        vCatParamVarRango vRango = vlURSobreviajeCierrreCRef_C1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.dcSobreviaje == null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Con factor de conversión y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURSobreviajeCierreSRef_C);
                    }
                    else if (unParametroFabricante.dcSobreviaje != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Con factor de conversión y con referencia
                        vlPuntuacion = EvaluaSobreviajeCierre((decimal)unParametroFabricante.dcSobreviaje);
                        vCatParamVarRango vRango = vlURSobreviajeCierreCRef_C.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else
                    {
                        //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURSobreviajeCierreSRef_C1);
                        vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                    }
                    vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
                }
                catch { return vlPuntuacion; }
            }
            else
            {
                //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURSobreviajeCierreSRef_C1);
                vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            }
            return vlPuntuacion;
        }
        #endregion Sobreviaje

        #endregion ParametrosDesplazamientoCierre


        #region ParametrosDesplazamientoApertura

        #region DiferencialVelocidad
        public decimal ObtenDifVelocidadAperturaPorcentaje()
        {
            decimal vDiferencialVelApertura, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.pda_velocidad_ap1, vTmp_DifFB.pda_velocidad_ap1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.pda_velocidad_ap1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.pda_velocidad_ap1, vTmp_DifFB.pda_velocidad_ap1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.pda_velocidad_ap1);

            vDiferencialVelApertura = ((vMaximoValor - vMinimoValor) / vMaximoValor)*100;
            return vDiferencialVelApertura;
        }

        public PuntuacionFase evaluaVelocidadFabricanteApertura(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento)
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = evaluaRangoVelFabApertura(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFA.pda_velocidad_ap1, "A");
            vlEvaluacionFA.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFA.puntuacionLetra);
            PuntuacionFase vlEvaluacionFB = evaluaRangoVelFabApertura(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFB.pda_velocidad_ap1, "B");
            vlEvaluacionFB.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFB.puntuacionLetra);
            PuntuacionFase vlEvaluacionFC = evaluaRangoVelFabApertura(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFC.pda_velocidad_ap1, "C");
            vlEvaluacionFC.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFC.puntuacionLetra);

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA,vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima,vlEvaluacionFC);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase evaluaRangoVelFabApertura(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor >= (pLimiteInferior - pValorIncremento) && pValor <= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor < (pLimiteInferior - pValorIncremento) || pValor >= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifVelocidadApertura()
        {
            /*Mecanismo de operación>Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelAperturaCRef_1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión=1 y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelAperturaSRef_1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión=1 y sin referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelAperturaCRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifVelAperturaSRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de Velocidad de apertura mismo modelo. Con factor de conversión y sin referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURDifVelAperturaCRef_1.First().varPeso;

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.daVelocidadLimInf != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        decimal vlValFabVelInfApertura = (decimal)unParametroFabricante.daVelocidadLimInf;
                        decimal vlValFabVelSupApertura = (decimal)unParametroFabricante.daVelocidadLimSup;
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión=1 y con referencia.
                        vlPuntuacion = evaluaVelocidadFabricanteApertura(vlValFabVelInfApertura, vlValFabVelSupApertura,(decimal) 0.2);
                        vCatParamVarRango vRango = vlURDifVelAperturaCRef_1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.daVelocidadLimInf == null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y sin referencia.
                        decimal vlValorPorcentaje = ObtenDifVelocidadAperturaPorcentaje();
                        vlPuntuacion = ObtenPuntuacionComNums(vlValorPorcentaje, vlURDifVelAperturaSRef);
                    }
                    else if (unParametroFabricante.daVelocidadLimInf != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        decimal vlValFabVelInfApertura = (decimal)unParametroFabricante.daVelocidadLimInf;
                        decimal vlValFabVelSupApertura = (decimal)unParametroFabricante.daVelocidadLimSup;
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y con referencia
                        vlPuntuacion = evaluaVelocidadFabricanteApertura(vlValFabVelInfApertura, vlValFabVelSupApertura,(decimal)0.5);
                        vCatParamVarRango vRango = vlURDifVelAperturaCRef.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else
                    {
                        decimal vlValorPorcentaje = ObtenDifVelocidadAperturaPorcentaje();
                        //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValorPorcentaje, vlURDifVelAperturaSRef_1);
                    }
                }
                catch { return vlPuntuacion; }
            }
            else
            {
                decimal vlValorPorcentaje = ObtenDifVelocidadAperturaPorcentaje();
                //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                vlPuntuacion = ObtenPuntuacionComNums(vlValorPorcentaje, vlURDifVelAperturaSRef_1);
            }
            return vlPuntuacion;
        }
        #endregion DiferencialVelocidad

        #region CarreraTotal
        public PuntuacionFase ObtenDifCarrTotApe230(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento)
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = evaluaRangoCarreraTotApertura(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFA.pda_carreratot_ap1, "A");
            vlEvaluacionFA.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFA.puntuacionLetra);
            PuntuacionFase vlEvaluacionFB = evaluaRangoCarreraTotApertura(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFB.pda_carreratot_ap1, "B");
            vlEvaluacionFB.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFB.puntuacionLetra);
            PuntuacionFase vlEvaluacionFC = evaluaRangoCarreraTotApertura(pLimiteInferior, pLimiteSuperior, pValorIncremento, (decimal)vTmp_DifFC.pda_carreratot_ap1, "C");
            vlEvaluacionFC.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionFC.puntuacionLetra);

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase evaluaRangoCarreraTotApertura(decimal pLimiteInferior, decimal pLimiteSuperior, decimal pValorIncremento, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor >= (pLimiteInferior - pValorIncremento) && pValor <= (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor < (pLimiteInferior - pValorIncremento) || pValor > (pLimiteSuperior + pValorIncremento))
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifCarrTotalApertura()
        {
            /*Mecanismo de operación>Carrera total a la apertura 230mm. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURCarreraTotalConv1CRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Carrera total a la apertura. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Carrera total a la apertura 230mm. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURCarreraTotalConvCRef = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Carrera total a la apertura. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURCarreraTotalConv1CRef.First().varPeso;

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.pdCarrTotalLimInf != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        //decimal vlValFabVelInfCarrTotApertura = (decimal)unParametroFabricante.carreraTotal - (decimal)unParametroFabricante.carreraTotalIncremento;
                        //decimal vlValFabVelSupCarrTotApertura = (decimal)unParametroFabricante.carreraTotal + (decimal)unParametroFabricante.carreraTotalIncremento;
                        decimal vlValFabVelInfCarrTotApertura = (decimal)unParametroFabricante.pdCarrTotalLimInf;
                        decimal vlValFabVelSupCarrTotApertura = (decimal)unParametroFabricante.pdCarrTotalLimSup;
                        // Con factor de conversión=1 y con referencia.
                        vlPuntuacion = ObtenDifCarrTotApe230(vlValFabVelInfCarrTotApertura,vlValFabVelSupCarrTotApertura, (decimal)0.7);
                        vCatParamVarRango vRango = vlURCarreraTotalConv1CRef.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.pdCarrTotalLimInf != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        //decimal vlValFabVelInfCarrTotApertura = (decimal)unParametroFabricante.carreraTotal - (decimal)unParametroFabricante.carreraTotalIncremento;
                        //decimal vlValFabVelSupCarrTotApertura = (decimal)unParametroFabricante.carreraTotal + (decimal)unParametroFabricante.carreraTotalIncremento;
                        decimal vlValFabVelInfCarrTotApertura = (decimal)unParametroFabricante.pdCarrTotalLimInf;
                        decimal vlValFabVelSupCarrTotApertura = (decimal)unParametroFabricante.pdCarrTotalLimSup;
                        //  Con factor de conversión y con referencia
                        vlPuntuacion = ObtenDifCarrTotApe230(vlValFabVelInfCarrTotApertura, vlValFabVelSupCarrTotApertura, (decimal)2.0);
                        vCatParamVarRango vRango = vlURCarreraTotalConvCRef.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                }
                catch { return vlPuntuacion; }
            }
            if (vlPuntuacion.puntuacionLetra == null)
                vlPuntuacion.variable = "Carrera total a la apertura. ** " + SINDATOSFABRICANTE;
            return vlPuntuacion;
        }
        #endregion CarreraTotal

        #region ReboteContactosEntreFases
        public decimal ObtenDifRebContEntreFasesApertura()
        {
            decimal vDifRebConEntreFasesApe, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.pda_rebote_ap1, vTmp_DifFB.pda_rebote_ap1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.pda_rebote_ap1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.pda_rebote_ap1, vTmp_DifFB.pda_rebote_ap1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.pda_rebote_ap1);

            vDifRebConEntreFasesApe = (vMaximoValor - vMinimoValor);
            return vDifRebConEntreFasesApe;
        }

        public PuntuacionFase EvalRebContEntreFasesApertura(IQueryable<vCatParamVarRango> pRangoDifReboteApertura)
        {
            vPeMecanismoOperacion vTmp_ReboteFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_ReboteFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_ReboteFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = ObtenPuntuacionComNums((decimal)vTmp_ReboteFA.pda_rebote_ap1, pRangoDifReboteApertura);
            vlEvaluacionFA.fase = "A";
            PuntuacionFase vlEvaluacionFB = ObtenPuntuacionComNums((decimal)vTmp_ReboteFB.pda_rebote_ap1, pRangoDifReboteApertura);
            vlEvaluacionFB.fase = "B";
            PuntuacionFase vlEvaluacionFC = ObtenPuntuacionComNums((decimal)vTmp_ReboteFC.pda_rebote_ap1, pRangoDifReboteApertura);
            vlEvaluacionFC.fase = "C";

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            vlEvaluacionMinima.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionMinima.puntuacionLetra);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase EvaluaRebContactosEntreFasesApertura(decimal pLimiteSuperior)
        {
            vPeMecanismoOperacion vTmp_ReboteFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_ReboteFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_ReboteFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = EvaluaRangoRebContFasesFabApertura(pLimiteSuperior, (decimal)vTmp_ReboteFA.pda_rebote_ap1, "A");
            PuntuacionFase vlEvaluacionFB = EvaluaRangoRebContFasesFabApertura(pLimiteSuperior, (decimal)vTmp_ReboteFB.pda_rebote_ap1, "B");
            PuntuacionFase vlEvaluacionFC = EvaluaRangoRebContFasesFabApertura(pLimiteSuperior, (decimal)vTmp_ReboteFC.pda_rebote_ap1, "C");

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            vlEvaluacionMinima.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionMinima.puntuacionLetra);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase EvaluaRangoRebContFasesFabApertura(decimal pLimiteSuperior, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor <= pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor > pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifRebContactFasesApertura()
        {
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifReboteApCRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión=1 y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifReboteApSRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión=1 y sin referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifReboteApCRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifReboteApSRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de rebote de contactos entre fases a la apertura. Con factor de conversión y sin referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURDifReboteApCRef_C1.First().varPeso;
            decimal vlValor = ObtenDifRebContEntreFasesApertura();

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.daRebote != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        // Con factor de conversión=1 y con referencia.
                        vlPuntuacion = EvaluaRebContactosEntreFasesApertura((decimal)unParametroFabricante.daRebote);
                        vCatParamVarRango vRango = vlURDifReboteApCRef_C1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.daRebote == null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifReboteApSRef_C);
                        //vlPuntuacion = EvalRebContEntreFasesApertura(vlURDifReboteApSRef_C);
                    }
                    else if (unParametroFabricante.daRebote != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Diferencial de Velocidad de cierre mismo modelo. Con factor de conversión y con referencia
                        vlPuntuacion = EvaluaRebContactosEntreFasesApertura((decimal)unParametroFabricante.daRebote);
                        vCatParamVarRango vRango = vlURDifReboteApCRef_C.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else
                    {
                        //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifReboteApSRef_C1);
                        //vlPuntuacion = ObtenDifRebContEntreFasesApertura(vlURDifReboteApSRef_C1);
                    }
                }
                catch { return vlPuntuacion; }
            }
            else
            {
                //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifReboteApSRef_C1);
                //vlPuntuacion = ObtenDifRebContEntreFasesApertura(vlURDifReboteApSRef_C1);
            }
            vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            return vlPuntuacion;
        }
        #endregion ReboteContactosEntreFases

        #region Sobreviaje
        public decimal ObtenDifSobreviajeApertura()
        {
            decimal vDiferencialelSobreviajeApertura, vMaximoValor, vMinimoValor;

            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vTmp_DifFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vTmp_DifFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            vMaximoValor = MaximoDeDosNumeros((decimal)vTmp_DifFA.pda_sobrerecorrido_ap1, vTmp_DifFB.pda_sobrerecorrido_ap1);
            vMaximoValor = MaximoDeDosNumeros(vMaximoValor, vTmp_DifFC.pda_sobrerecorrido_ap1);

            vMinimoValor = MinimoDeDosNumeros((decimal)vTmp_DifFA.pda_sobrerecorrido_ap1, vTmp_DifFB.pda_sobrerecorrido_ap1);
            vMinimoValor = MinimoDeDosNumeros(vMinimoValor, vTmp_DifFC.pda_sobrerecorrido_ap1);

            vDiferencialelSobreviajeApertura = vMaximoValor - vMinimoValor;
            return vDiferencialelSobreviajeApertura;
        }

        /*public PuntuacionFase EvalSobreviajeApertura(IQueryable<vCatParamVarRango> pRangoSobreviajeApertura)
        {
            vPeMecanismoOperacion vDifSobreviajeFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vDifSobreviajeFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vDifSobreviajeFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = ObtenPuntuacionComNums((decimal)vDifSobreviajeFA.pda_sobrerecorrido_ap1, pRangoSobreviajeApertura);
            vlEvaluacionFA.fase = "A";
            PuntuacionFase vlEvaluacionFB = ObtenPuntuacionComNums((decimal)vDifSobreviajeFB.pda_sobrerecorrido_ap1, pRangoSobreviajeApertura);
            vlEvaluacionFB.fase = "B";
            PuntuacionFase vlEvaluacionFC = ObtenPuntuacionComNums((decimal)vDifSobreviajeFC.pda_sobrerecorrido_ap1, pRangoSobreviajeApertura);
            vlEvaluacionFC.fase = "C";

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            vlEvaluacionMinima.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionMinima.puntuacionLetra);
            return vlEvaluacionMinima;
        }*/

        public PuntuacionFase EvaluaSobreviajeApertura(decimal pLimiteSuperior)
        {
            vPeMecanismoOperacion vDifSobreviajeFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            vPeMecanismoOperacion vDifSobreviajeFB = vlPEMecOper.Where(pemoip => pemoip.fase == "B").First();
            vPeMecanismoOperacion vDifSobreviajeFC = vlPEMecOper.Where(pemoip => pemoip.fase == "C").First();

            PuntuacionFase vlEvaluacionFA = EvaluaRangoDifSobreviajeApertura(pLimiteSuperior, (decimal)vDifSobreviajeFA.pda_sobrerecorrido_ap1, "A");
            PuntuacionFase vlEvaluacionFB = EvaluaRangoDifSobreviajeApertura(pLimiteSuperior, (decimal)vDifSobreviajeFB.pda_sobrerecorrido_ap1, "B");
            PuntuacionFase vlEvaluacionFC = EvaluaRangoDifSobreviajeApertura(pLimiteSuperior, (decimal)vDifSobreviajeFC.pda_sobrerecorrido_ap1, "C");

            PuntuacionFase vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionFA, vlEvaluacionFB);
            vlEvaluacionMinima = MinimoDeDosPuntuaciones(vlEvaluacionMinima, vlEvaluacionFC);
            vlEvaluacionMinima.puntuacionNumero = UnaPuntuacionLetraNumero(vlEvaluacionMinima.puntuacionLetra);
            return vlEvaluacionMinima;
        }

        public PuntuacionFase EvaluaRangoDifSobreviajeApertura(decimal pLimiteSuperior, decimal pValor, string pFase)
        {
            PuntuacionFase vlPuntuacion = new PuntuacionFase();

            if (pValor <= pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "A";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            else if (pValor > pLimiteSuperior)
            {
                vlPuntuacion.puntuacionLetra = "E";
                vlPuntuacion.fase = pFase;
                vlPuntuacion.valorNumero = pValor;
            }
            vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            return vlPuntuacion;
        }

        private PuntuacionFase evaluaDifSobreviajeApertura()
        {
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión=1 y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifSobreviajeApCRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión=1 y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión=1 y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifSobreviajeApSRef_C1 = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión=1 y sin referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión y con referencia.*/
            IQueryable<vCatParamVarRango> vlURDifSobreviajeApCRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión y con referencia.")).OrderBy(r => r.rango);
            /*Mecanismo de operación>Diferencial de sobreviaje de contactos entre fases al cierre. Con factor de conversión y sin referencia.*/
            IQueryable<vCatParamVarRango> vlURDifSobreviajeApSRef_C = db.vCatParamVarRango.Where(r => r.catalogo.Equals("Pruebas Especiales") &&
            r.parametro.Equals("Mecanismo de operación") && r.variable.Equals("Diferencial de sobreviaje de contactos entre fases a la apertura. Con factor de conversión y sin referencia.")).OrderBy(r => r.rango);

            /* Parametros del fabricante */
            IQueryable<ParametrosFabricantePE> vlParametroFabricante = db.ParametrosFabricantePE.Where(pf => pf.MarcaId == vlEquipo.Marca_id && pf.ModeloId == vlEquipo.Modelo_id);

            PuntuacionFase vlPuntuacion = new PuntuacionFase();
            vlPuntuacion.peso = (decimal)vlURDifSobreviajeApCRef_C1.First().varPeso;
            decimal vlValor = ObtenDifSobreviajeApertura();

            if (vlParametroFabricante != null && vlParametroFabricante.Count() > 0)
            {
                ParametrosFabricantePE unParametroFabricante = vlParametroFabricante.First();
                try
                {
                    decimal vlFactorConversion = (decimal)unParametroFabricante.pdFactoConversion;
                    if (unParametroFabricante.daSobreviaje != null && unParametroFabricante.pdFactoConversion == 1)
                    {
                        // Con factor de conversión=1 y con referencia.
                        vlPuntuacion = EvaluaSobreviajeApertura((decimal)unParametroFabricante.daSobreviaje);
                        vCatParamVarRango vRango = vlURDifSobreviajeApCRef_C1.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else if (unParametroFabricante.daSobreviaje == null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Con factor de conversión y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifSobreviajeApSRef_C);
                        //vlPuntuacion = EvalSobreviajeApertura(vlURDifSobreviajeApSRef_C);
                    }
                    else if (unParametroFabricante.daSobreviaje != null && unParametroFabricante.pdFactoConversion != 1)
                    {
                        // Con factor de conversión y con referencia
                        vlPuntuacion = EvaluaSobreviajeApertura((decimal)unParametroFabricante.daSobreviaje);
                        vCatParamVarRango vRango = vlURDifSobreviajeApCRef_C.First();
                        vlPuntuacion.variable = vRango.variable;
                        vlPuntuacion.peso = (decimal)vRango.varPeso;
                    }
                    else
                    {
                        //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                        vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifSobreviajeApSRef_C1);
                        //vlPuntuacion = EvalSobreviajeApertura(vlURDifSobreviajeApSRef_C1);
                    }
                }
                catch { return vlPuntuacion; }
            }
            else
            {
                //Diferencial de Velocidad de cierre mismo modelo.Con factor de conversión = 1 y sin referencia.
                vlPuntuacion = ObtenPuntuacionComNums(vlValor, vlURDifSobreviajeApSRef_C1);
                //vlPuntuacion = EvalSobreviajeApertura(vlURDifSobreviajeApSRef_C1);
            }
            vlPuntuacion.puntuacionNumero = UnaPuntuacionLetraNumero(vlPuntuacion.puntuacionLetra);
            return vlPuntuacion;
        }
        #endregion Sobreviaje

        #endregion ParametrosDesplazamientoApertura

        public string ObtenDifSecNominalOpeA_CA_CA()
        {
            vPeMecanismoOperacion vTmp_DifFA = vlPEMecOper.Where(pemoip => pemoip.fase == "A").First();
            string vsec_nominal = "";
            bool vlSecNominal = Convert.ToBoolean(vTmp_DifFA.sec_nominal_operacion);

            if (vlSecNominal)
                vsec_nominal = "CU";
            else
                vsec_nominal = "NC";

            return vsec_nominal;
        }

        #endregion

        #region MetodosUtilerias

        #region IndiceSalud
        private decimal ObtenISPE()
        {
            bool vlUnidadRuptoraIncluirEnOperacion = true;
            bool vlMecOperacionIncluirEnOperacion = true;

            try
            {
                viCPCmUnidadRuptora = ObtenCPCms(vlListaPuntUnidadRuptora);
            }
            catch
            {
                vlUnidadRuptoraIncluirEnOperacion = false;
                viCPCmUnidadRuptora = 0;
            }

            try
            {
                viCPCmMecOperacion = ObtenCPCms(vlListaPuntMecOperacion);
            }
            catch
            {
                vlMecOperacionIncluirEnOperacion = false;
                viCPCmMecOperacion = 0;
            }

            vCatParamVarRango vlParamUnidadRuptora = vlParamVarRango.Where(vpr => vpr.parametro == "Condición del aislamiento/ Unidad ruptora").First();
            vCatParamVarRango vlParamMecOperacion = vlParamVarRango.Where(vpr => vpr.parametro == "Mecanismo de operación").First();
            decimal vlSumaMultiplicaciones = (viCPCmUnidadRuptora * vlParamUnidadRuptora.parPeso) + (viCPCmMecOperacion * vlParamMecOperacion.parPeso);

            //decimal vlSumaPPCms = vlParamUnidadRuptora.parPeso + vlParamMecOperacion.parPeso;
            decimal vlSumaPPCms = 0;
            if (vlUnidadRuptoraIncluirEnOperacion) vlSumaPPCms += vlParamUnidadRuptora.parPeso;
            if (vlMecOperacionIncluirEnOperacion) vlSumaPPCms += vlParamMecOperacion.parPeso;

            return (vlSumaMultiplicaciones / (vlSumaPPCms * 4)) * 100;
        }

        private decimal ObtenCPCms(List<PuntuacionFase> pPuntuaciones)
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
            decimal vlPesosUnidadRuptora = ObtenSumaPesosVarParametro(vlListaPuntUnidadRuptora);
            decimal vlPesosMecOperacion = ObtenSumaPesosVarParametro(vlListaPuntMecOperacion);

            return (vlPesosUnidadRuptora + vlPesosMecOperacion);
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

        private PuntuacionFase MinimoDeDosPuntuaciones(PuntuacionFase pValor1, PuntuacionFase pValor2)
        {
            PuntuacionFase minimoValor;
            if (pValor2.puntuacionLetra == null)
            {
                minimoValor = pValor1;
            }
            else
            {
                if (pValor1.puntuacionNumero < pValor2.puntuacionNumero)
                    minimoValor = pValor1;
                else
                    minimoValor = pValor2;
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
            foreach (vCatalogoPuntuaciones regCatPuntuacion in vlPuntuacionesNum)
            {
                if (pPuntuacionLetra.Equals(regCatPuntuacion.abreviatura))
                {
                    vlPuntuacionNumero = (decimal)regCatPuntuacion.valorFlotante;
                    break;
                }
            }
            return vlPuntuacionNumero;
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

        #endregion MetodosUtilerias

    }
}