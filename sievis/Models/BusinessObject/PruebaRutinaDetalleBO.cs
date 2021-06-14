using System;
using Resources;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ExpressiveAnnotations.MvcUnobtrusive;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(PruebaRutinaDetalleBO))]
    public partial class PruebaRutinaDetalle
    {
    }

    public class PruebaRutinaDetalleBO
    {
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Apertura Disparo 1 Columna 1")]
        public Nullable<decimal> tapertura_d1c1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Apertura Disparo 1 Columna 1")]
        public Nullable<decimal> tapertura_d2c1 { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Cierre Columna 1")]
        public Nullable<decimal> tcierre_c1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Cierre Columna 2")]
        public Nullable<decimal> tcierre_c2 { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Apertura Disparo 1 Columna 2")]
        public Nullable<decimal> tapertura_d1c2 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Apertura Disparo 1 Columna 2")]

        public Nullable<decimal> tapertura_d2c2 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Respuesta Reincerción Columna 1")]
        public Nullable<decimal> tent_resprein_c1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Respuesta Reincerción Columna 2")]
        public Nullable<decimal> tent_resprein_c2 { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Cierre Apertura Columna 1")]
        public Nullable<decimal> tcierreapertura_c1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo de Cierre Apertura Columna 2")]
        public Nullable<decimal> tcierreapertura_c2 { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Capacitancia de Condensadores")]
        public Nullable<decimal> capacitancia_condesadores_c1 { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Capacitancia de Condensadores")]
        public Nullable<decimal> capacitancia_condesadores_c2 { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número entre 0 y 100: Pureza SF6")]
        public Nullable<decimal> purezasf6 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número entre 0 y 100: Aire")]
        public Nullable<decimal> aire { get; set; }

        /*public Nullable<decimal> humedad { get; set; }*/
        //[RequiredIf("humedad != null", AllowEmptyStrings = false)]
        //public string unidad_medicion_humedad { get; set; }


        //-- Not required
        /*
        public Nullable<decimal> resitencia_ohmica_rpi { get; set; }        
        */

        //-- Removed
        /*
        public Nullable<int> res_estat_contactos_d1c1 { get; set; }
        public Nullable<int> res_estat_contactos_d1c2 { get; set; }        
        */

        //-- Calculated
        /*
        public Nullable<decimal> presion { get; set; }
        public string unidad_medicion_presion { get; set; }
        */
    }
}