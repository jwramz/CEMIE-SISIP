using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(DensimetroBO))]
    public partial class Densimetro
    {
    }
    public class DensimetroBO
    {

        [Required(ErrorMessage = "Estado de la carcasa: Debe seleccionar un elemento de la lista")]
        public Nullable<int> carcasa { get; set; }

        [Required(ErrorMessage = "Estado de la carátula: Debe seleccionar un elemento de la lista")]
        public Nullable<int> caratula { get; set; }

        [Required(ErrorMessage = "Condición de Aguja: Debe seleccionar un elemento de la lista")]
        public Nullable<int> condicion_aguja { get; set; }

        [Required(ErrorMessage = "Estado de la escala: Debe seleccionar un elemento de la lista")]
        public Nullable<int> escala { get; set; }

        [Required(ErrorMessage = "Nivel de Glicerina: Debe seleccionar un elemento de la lista")]
        public Nullable<int> nivel_glicerina { get; set; }
    }
}