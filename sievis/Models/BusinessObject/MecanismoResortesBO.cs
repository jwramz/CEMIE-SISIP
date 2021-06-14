using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(MecanismoResortesBO))]
    public partial class MecanismoResortes
    {

    }

    public class MecanismoResortesBO
    {
        [Required(ErrorMessage = "Alineación de resortes: Debe seleccionar un elemento de la lista")]
        public Nullable<int> alineacion_resortes { get; set; }

        [Required(ErrorMessage = "Amortiguadores: Debe seleccionar un elemento de la lista")]
        public Nullable<int> amortiguadores { get; set; }

        [Required(ErrorMessage = "Estado del motor: Debe seleccionar un elemento de la lista")]
        public Nullable<int> motor { get; set; }


    }
}