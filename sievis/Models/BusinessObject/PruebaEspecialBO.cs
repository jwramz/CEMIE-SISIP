using System;
using Resources;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ExpressiveAnnotations.MvcUnobtrusive;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(PruebaEspecialBO))]
    public partial class PruebaEspecial
    {
    }

    public class PruebaEspecialBO
    {
        [Range(-20, 60, ErrorMessage = "Debe ser un número rango -20 a 60 °C: Temperatura Ambiente")]
        public Nullable<decimal> temperatura_ambiente { get; set; }

        //-- Arranque minimo de Bobinas
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Arranque minimo de Bobinas Disparo 1")]
        public Nullable<decimal> arranque_minbobd1 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Arranque minimo de Bobinas Disparo 1")]
        public Nullable<decimal> arranque_minbobd2 { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe ser un número mayor que cero: Arranque minimo de Bobinas Cierre")]
        public Nullable<decimal> arranque_minbob_cierre { get; set; }
      

    }
}