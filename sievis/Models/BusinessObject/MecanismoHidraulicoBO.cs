using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(MecanismoHidraulicoBO))]
    public partial class MecanismoHidraulico
    {

    }

    public class MecanismoHidraulicoBO
    {
       

        [Required(ErrorMessage = "Fuga de aceite: Debe seleccionar un elemento de la lista")]
        public Nullable<int> fuga_aceite { get; set; }

        [Required(ErrorMessage = "Acumuladore: Debe seleccionar un elemento de la lista")]
        public Nullable<int> acumulador { get; set; }

        [Required(ErrorMessage = "Presion de Aceite: Debe seleccionar un elemento de la lista")]
        public Nullable<int> presion_aceite { get; set; }

        [Required(ErrorMessage = "Unidad Control: Debe seleccionar un elemento de la lista")]
        public Nullable<int> unidad_control { get; set; }
        
        [Required(ErrorMessage = "Valvulas: Debe seleccionar un elemento de la lista")]
        public Nullable<int> valvulas { get; set; }

        [Required(ErrorMessage = "Burbujas de Reservorio: Debe seleccionar un elemento de la lista")]
        public Nullable<int> burbujas_reservorio { get; set; }

        [Required(ErrorMessage = "Compresor : Debe seleccionar un elemento de la lista")]
        public Nullable<int> compresor { get; set; }

        


    }
}