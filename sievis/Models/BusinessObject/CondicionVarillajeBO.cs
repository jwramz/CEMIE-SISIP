using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(CondicionVarillajeBO))]
    public partial class CondicionVarillaje
    {
    }

    public class CondicionVarillajeBO
    {
        [Required(ErrorMessage = "Condición de Pernos: Debe seleccionar un elemento de la lista")]
        public Nullable<int> condision_pernos { get; set; }
    }
}