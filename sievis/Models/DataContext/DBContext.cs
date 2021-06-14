using System.Linq;

namespace sievis.Models
{
    public class DBContext
    {
        public Prueba Get(ModeloSievis context, int? equipoId, int? pruebaId)
        {
            Prueba result = new Prueba();

            var prueba_db = (from t in context.Prueba where t.id == pruebaId select t).FirstOrDefault();

            //-- nuevo
            if (prueba_db == null || prueba_db.id == 0)
            {
                result.Initialize();
            }
            //-- existente
            else
            {
                result = prueba_db;
            }

            return result;
        }

        public Prueba Save(ModeloSievis context, int equipoId, int pruebaId, Prueba prueba)
        {
            var prueba_db = (from t in context.Prueba where t.id == pruebaId select t).FirstOrDefault();

            //-- agregar
            if (prueba_db == null || prueba_db.id == 0)
            {
                prueba.Equipo_id = equipoId;
                context.Prueba.Add(prueba);
                context.SaveChanges();

            }
            //-- modificar
            else
            {
                prueba.Equipo_id = equipoId;
                context.Entry(prueba_db).CurrentValues.SetValues(prueba);
                context.SaveChanges();

                context_Update_GabineteCentralizador(context, equipoId, pruebaId, prueba.CondicionGabineteCentralizador.ElementAt(0));

                context_Update_Inspeccion_visual(context, equipoId, pruebaId, prueba.Inspeccion_visual.ElementAt(0), "A");
                context_Update_Inspeccion_visual(context, equipoId, pruebaId, prueba.Inspeccion_visual.ElementAt(1), "B");
                context_Update_Inspeccion_visual(context, equipoId, pruebaId, prueba.Inspeccion_visual.ElementAt(2), "C");

                context_Update_PruebaRutina(context, equipoId, pruebaId, prueba.PruebaRutina.ElementAt(0));
                context_Update_PruebaEspecial(context, equipoId, pruebaId, prueba.PruebaEspecial.ElementAt(0));
            }
            return prueba;
        }

        private CondicionGabineteCentralizador context_Update_GabineteCentralizador(ModeloSievis context, int equipoId, int pruebaId, CondicionGabineteCentralizador prueba) {

            var prueba_db = (from t in context.CondicionGabineteCentralizador where t.Prueba_id == pruebaId select t).FirstOrDefault();

            prueba.Prueba_id = pruebaId;
            prueba.id = prueba_db.id;

            if (prueba.id > 0)
                context.Entry(prueba_db).CurrentValues.SetValues(prueba);
            else
                context.CondicionGabineteCentralizador.Add(prueba);

            context.SaveChanges();

            return prueba;
        }

        private Inspeccion_visual context_Update_Inspeccion_visual(ModeloSievis context, int equipoId, int pruebaId, Inspeccion_visual prueba, string fase)
        {
            var prueba_db = (from t in context.Inspeccion_visual where t.Prueba_id == pruebaId && t.fase.Equals(fase) select t).FirstOrDefault();

            //-- guarda datos del encabezado [Inspeccion_visual]
            prueba.Prueba_id = prueba_db.Prueba_id;
            prueba.id = prueba_db.id;
            context.Entry(prueba_db).CurrentValues.SetValues(prueba);
            context.SaveChanges();

            //-- guarda datos de cada tabla de detalle
            var ca = prueba_db.CondicionAislador.FirstOrDefault();
            prueba.ca.Inspeccion_visual_id = prueba_db.id;
            prueba.ca.id = ca.id;
            if (ca.id > 0)
                context.Entry(ca).CurrentValues.SetValues(prueba.ca);
            else
                context.CondicionAislador.Add(prueba.ca);

            var gc = prueba_db.CondicionGabineteControl.FirstOrDefault();
            prueba.gc.Inspeccion_visual_id = prueba_db.id;
            prueba.gc.id = gc.id;
            if (gc.id > 0)
                context.Entry(gc).CurrentValues.SetValues(prueba.gc);
            else
                context.CondicionGabineteControl.Add(prueba.gc);

         
            var d = prueba_db.Densimetro.FirstOrDefault();
            prueba.d.Inspeccion_visual_id = prueba_db.id;
            prueba.d.id = d.id;
            if (d.id > 0)
                context.Entry(d).CurrentValues.SetValues(prueba.d);
            else
                context.Densimetro.Add(prueba.d);

            var mh = prueba_db.MecanismoHidraulico.FirstOrDefault();
            prueba.mh.Inspeccion_visual_id = prueba_db.id;
            prueba.mh.id = mh.id;
            if (mh.id > 0)
                context.Entry(mh).CurrentValues.SetValues(prueba.mh);
            else
                context.MecanismoHidraulico.Add(prueba.mh);

            var mn = prueba_db.MecanismoNeumatico.FirstOrDefault();
            prueba.mn.Inspeccion_visual_id = prueba_db.id;
            prueba.mn.id = mn.id;
            if (mn.id > 0)
                context.Entry(mn).CurrentValues.SetValues(prueba.mn);
            else
                context.MecanismoNeumatico.Add(prueba.mn);

            var mr = prueba_db.MecanismoResortes.FirstOrDefault();
            prueba.mr.Inspeccion_visual_id = prueba_db.id;
            prueba.mr.id = mr.id;
            if (mr.id > 0)
                context.Entry(mr).CurrentValues.SetValues(prueba.mr);
            else
                context.MecanismoResortes.Add(prueba.mr);

            var p = prueba_db.Presostato.FirstOrDefault();
            prueba.p.Inspeccion_visual_id = prueba_db.id;
            prueba.p.id = p.id;
            if (p.id > 0)
                context.Entry(p).CurrentValues.SetValues(prueba.p);
            else
                context.Presostato.Add(prueba.p);

            var coc = prueba_db.CondicionOtrosComponentes.FirstOrDefault();
            prueba.coc.Inspeccion_visual_id = prueba_db.id;
            prueba.coc.id = coc.id;
            if (coc.id > 0)
                context.Entry(coc).CurrentValues.SetValues(prueba.coc);
            else
                context.CondicionOtrosComponentes.Add(prueba.coc);

            var cv = prueba_db.CondicionVarillaje.FirstOrDefault();
            prueba.cv.Inspeccion_visual_id = prueba_db.id;
            prueba.cv.id = cv.id;
            if (cv.id > 0)
                context.Entry(cv).CurrentValues.SetValues(prueba.cv);
            else
                context.CondicionVarillaje.Add(prueba.cv);

            context.SaveChanges();

            return prueba;
        }

        private PruebaRutina context_Update_PruebaRutina(ModeloSievis context, int equipoId, int pruebaId, PruebaRutina prueba)
        {
            var prueba_db = (from t in context.PruebaRutina where t.Prueba_id == pruebaId select t).FirstOrDefault();

            //-- guarda datos del encabezado [PruebaRutina]
            prueba.Prueba_id = prueba_db.Prueba_id;
            prueba.id = prueba_db.id;
            context.Entry(prueba_db).CurrentValues.SetValues(prueba);
            context.SaveChanges();

            //-- guarda datos de detalle por fase
            var detA = prueba_db.PruebaRutinaDetalle.ElementAt(0);
            prueba.DetFA.PruebaRutina_id = prueba_db.id;
            prueba.DetFA.id = detA.id;
            if (detA.id > 0)
                context.Entry(detA).CurrentValues.SetValues(prueba.DetFA);
            else
                context.PruebaRutinaDetalle.Add(prueba.DetFA);

            var detB = prueba_db.PruebaRutinaDetalle.ElementAt(1);
            prueba.DetFB.PruebaRutina_id = prueba_db.id;
            prueba.DetFB.id = detB.id;
            if (detA.id > 0)
                context.Entry(detB).CurrentValues.SetValues(prueba.DetFB);
            else
                context.PruebaRutinaDetalle.Add(prueba.DetFB);

            var detC = prueba_db.PruebaRutinaDetalle.ElementAt(2);
            prueba.DetFC.PruebaRutina_id = prueba_db.id;
            prueba.DetFC.id = detC.id;
            if (detA.id > 0)
                context.Entry(detC).CurrentValues.SetValues(prueba.DetFC);
            else
                context.PruebaRutinaDetalle.Add(prueba.DetFC);

            context.SaveChanges();

            return prueba;
        }

        private PruebaEspecial context_Update_PruebaEspecial(ModeloSievis context, int equipoId, int pruebaId, PruebaEspecial prueba)
        {
            var prueba_db = (from t in context.PruebaEspecial where t.Prueba_id == pruebaId select t).FirstOrDefault();

            //-- guarda datos del encabezado [PruebaEspecial]
            prueba.Prueba_id = prueba_db.Prueba_id;
            prueba.Id = prueba_db.Id;
            context.Entry(prueba_db).CurrentValues.SetValues(prueba);
            context.SaveChanges();

            //-- guarda datos de detalle por fase
            var detA = prueba_db.PruebaEspecialDetalle.ElementAt(0);
            prueba.DetFA.PruebaEspecial_id = prueba_db.Id;
            prueba.DetFA.id = detA.id;
            if (detA.id > 0)
                context.Entry(detA).CurrentValues.SetValues(prueba.DetFA);
            else
                context.PruebaEspecialDetalle.Add(prueba.DetFA);

            var detB = prueba_db.PruebaEspecialDetalle.ElementAt(1);
            prueba.DetFB.PruebaEspecial_id = prueba_db.Id;
            prueba.DetFB.id = detB.id;
            if (detA.id > 0)
                context.Entry(detB).CurrentValues.SetValues(prueba.DetFB);
            else
                context.PruebaEspecialDetalle.Add(prueba.DetFB);

            var detC = prueba_db.PruebaEspecialDetalle.ElementAt(2);
            prueba.DetFC.PruebaEspecial_id = prueba_db.Id;
            prueba.DetFC.id = detC.id;
            if (detA.id > 0)
                context.Entry(detC).CurrentValues.SetValues(prueba.DetFC);
            else
                context.PruebaEspecialDetalle.Add(prueba.DetFC);

            context.SaveChanges();

            return prueba;
        }

    }
}