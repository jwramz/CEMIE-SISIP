using System;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(PruebaRutinaBO))]
    public partial class PruebaRutina
    {
    }

    public class PruebaRutinaBO
    {

        [Required(ErrorMessage = "Disparo Libre: Debe seleccionar un elemento de la lista")]
        public string seccuencia_displibre { get; set; }

        [Required(ErrorMessage = "Antibombeo: Debe seleccionar un elemento de la lista")]
        public string pba_antibombeo { get; set; }

        [Required(ErrorMessage = "Discrepancia de Polos: Debe seleccionar un elemento de la lista")]
        public string discrepancia_polos { get; set; }

        [Required(ErrorMessage = "Frecuencia de Llenado: Debe seleccionar un elemento de la lista")]
        public string frecuencia_llenado_SF6 { get; set; }

     



    }
}