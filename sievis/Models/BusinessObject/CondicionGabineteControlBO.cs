using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(CondicionGabineteControlBO))]
    public partial class CondicionGabineteControl
    {
    }

    public class CondicionGabineteControlBO
    {
        [Required(ErrorMessage = "Resistencia Calefactora: Debe seleccionar un elemento de la lista")]
        public Nullable<int> resistencia_calefactora { get; set; }
    }
}