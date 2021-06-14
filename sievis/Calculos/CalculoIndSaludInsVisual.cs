using sievis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static sievis.Models.AppEnum;

namespace sievis.Calculos
{
   
    public class CalculoIndSaludInsVisual
    {
        #region Private
        private ModeloSievis db = new ModeloSievis();
        private DBContext dbContext = new DBContext();
        private Equipo vlEquipo;
        private Prueba vlPrueba;
        private Mecanismo vlMecanismo;
        #endregion
        public List<PuntuacionFase> pDatosCPCms { get; set; }

        public CalculoIndSaludInsVisual(int equipoId, int pruebaId)
        {
            vlEquipo = db.Equipo.SingleOrDefault(de => de.id == equipoId);
            vlPrueba = dbContext.Get(db, equipoId, pruebaId);
            vlMecanismo = db.Mecanismo.SingleOrDefault(me => me.id == vlEquipo.Mecanismo_id);
            pDatosCPCms = new List<PuntuacionFase>();
        }

        public double IndSaludInsVisual()
        {
            String sentenciaSQL = "SELECT pruebaId, CONVERT(FLOAT,((SUM(dividendo)/SUM(divisor))*100.0)) AS ISx FROM ( " +
              " SELECT vcpcm.pruebaId, vcpcm.CPCm,vpvp.paramPeso, (vcpcm.CPCm * vpvp.paramPeso) dividendo, (4.0 * vpvp.paramPeso) divisor FROM(";

            String VCPs = "SELECT * FROM vIVCPCms WHERE pruebaId = " + vlPrueba.id +
              " AND ( nomVariable = 'Varillaje' OR nomVariable = 'Boquillas y aisladores soporte' OR nomVariable = 'Otros componentes' ";

            /* Para el tipo de gabinete que tiene el equipo (De control por fase, Centralizador)*/
            if (vlPrueba.existe_gabinetectrl_xfase == true)
                VCPs += " OR nomVariable = 'Gabinete de control' OR nomVariable = 'Gabinete de control (monitoreo remoto)' ";

            if (vlPrueba.existe_gabinete_centralizador == true)
                VCPs += " OR nomVariable = 'Gabinete centralizador' ";

            /* Para el tipo de Mecanismo (Resorte, Hidráulico o Neumático)*/
            if (vlMecanismo.descripcion == "Resorte")
                VCPs += " OR nomVariable = 'Mec. operación Resortes' ";

            if (vlMecanismo.descripcion == "Hidráulico")
                VCPs += " OR nomVariable = 'Mec. operación Hidráulico' ";

            if (vlMecanismo.descripcion == "Neumático")
                VCPs += " OR nomVariable = 'Mec. operación Neumático' ";

            /* Para el tipo de instrumento de medición Densímetro o Presostato*/
            if (vlPrueba.instrumento_medicionSF6 == "D")
                VCPs += " OR nomVariable = 'Densímetro' ";

            if (vlPrueba.instrumento_medicionSF6 == "P")
                VCPs += " OR nomVariable = 'Presostato' ";

            VCPs += " )";

            sentenciaSQL += VCPs;
            sentenciaSQL += ") AS vcpcm, vParametrosVariablesPeso vpvp WHERE vcpcm.parametro = vpvp.parametro AND vcpcm.nomVariable = vpvp.nomVariable) AS ecuacion3 GROUP BY pruebaId";
            var indice = db.vIVIndiceSalud.SqlQuery(sentenciaSQL);

            var vlTempDatosCPC = db.vIVCPCms.SqlQuery(VCPs);

            foreach (vIVCPCms tmp in vlTempDatosCPC)
            {
                PuntuacionFase unaPuntuacion = new PuntuacionFase();
                unaPuntuacion.variable = tmp.nomVariable;
                unaPuntuacion.peso = (decimal)tmp.varPeso;
                unaPuntuacion.puntuacionNumero = (decimal)tmp.CPCm;
                pDatosCPCms.Add(unaPuntuacion);
            }

            return (double)indice.First().ISx;
        }
       
    }

}