using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(MecanismoNeumaticoBO))]
    public partial class MecanismoNeumatico
    {

    }

    public class MecanismoNeumaticoBO
    {
       
        [Required(ErrorMessage = "Presión de Aire: Debe seleccionar un elemento de la lista")]
        public Nullable<int> presion_aire { get; set; }

        [Required(ErrorMessage = "Fuga de Aire: Debe seleccionar un elemento de la lista")]
        public Nullable<int> fuga_aire { get; set; }

        [Required(ErrorMessage = "Valvulas: Debe seleccionar un elemento de la lista")]
        public Nullable<int> valvulas { get; set; }

        [Required(ErrorMessage = "Corrosión de Mecanismo: Debe seleccionar un elemento de la lista")]
        public Nullable<int> corrosion_mecanismo { get; set; }
        

    }
}