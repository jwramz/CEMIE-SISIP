using System;
using Resources;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ExpressiveAnnotations.MvcUnobtrusive;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(PruebaEspecialDetalleBO))]
    public partial class PruebaEspecialDetalle
    {
    }

    public class PruebaEspecialDetalleBO
    {
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Longitud Contacto de Arqueo Camara 1")]
        public Nullable<int> long_contac_arqueo_c1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Longitud Contacto de Arqueo Camara 2")]
        public Nullable<int> long_contac_arqueo_c2 { get; set; }
        //[Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Temperatura Máxima")]
        ////public Nullable<decimal> temp_maxima { get; set; }
        /*public string ubicacion_tempmax { get; set; }*/
        
        //-- Pico de la bobina
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Corriente Pico de la Bobina Disparo 1")]
        public Nullable<decimal> ipd1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Corriente Pico de la Bobina Disparo 2")]
        public Nullable<decimal> ipd2 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Corriente Pico de la Bobina Cierre")]
        public Nullable<decimal> ip_cierre { get; set; }

        //-- Tiempo Total de la Bobina de Apertura
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo Total de la Bobina de Apertura Disparo 1")]
        public Nullable<decimal> ttd1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo Total de la Bobina de Apertura Disparo 2")]
        public Nullable<decimal> ttd2 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Tiempo Total de la Bobina de Apertura Cierre")]
        public Nullable<decimal> tt_cierre { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Parámetro de Desplazamiento a la Apertura Sobrerrecorrido")]
        public Nullable<int> pda_sobrerecorrido_ap1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Parámetro de Desplazamiento al Cierre Sobrerrecorrido")]
        public Nullable<int> pardescierre_sobrerecorrido { get; set; }

        /*public Nullable<decimal> t1d1 { get; set; }
        public Nullable<decimal> t2d1 { get; set; }
        
        public Nullable<decimal> t1d2 { get; set; }
        public Nullable<decimal> t2d2 { get; set; }
        
        public Nullable<decimal> t1_cierre { get; set; }
        public Nullable<decimal> t2_cierre { get; set; }        

        public Nullable<int> pda_velocidad_ap1 { get; set; }
        public Nullable<int> pda_carreratot_ap1 { get; set; }        
        public Nullable<int> pda_rebote_ap1 { get; set; }
        public Nullable<int> pda_velocidad_ap2 { get; set; }
        public Nullable<int> pda_carreratot_ap2 { get; set; }
        public Nullable<int> pda_sobrerecorrido_ap2 { get; set; }
        public Nullable<int> pda_rebote_ap2 { get; set; }
        public Nullable<int> pardescierre_velocidad { get; set; }        
        
        public Nullable<int> pardescierre_rebote { get; set; }
        public Nullable<int> pardescierre_penetracion { get; set; }
        public Nullable<int> subprodsf6_so2 { get; set; }
        public Nullable<int> subprodsf6_hf { get; set; }
        public Nullable<int> subprodsf6_cf4 { get; set; }

        public string sec_nominal_operacion { get; set; }
        public Nullable<int> tapertura_c1 { get; set; }
        public Nullable<int> tapertura_c2 { get; set; }
        public Nullable<int> tapertura_cierre_c1 { get; set; }
        public Nullable<int> tapertura_cierre_c2 { get; set; }
        public Nullable<int> tcierre_apertura_c1 { get; set; }*/
    }
}