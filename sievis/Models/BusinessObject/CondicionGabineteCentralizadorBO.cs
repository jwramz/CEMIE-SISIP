using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(CondicionGabineteCentralizadorBO))]
    public partial class CondicionGabineteCentralizador
    {
    }

    public class CondicionGabineteCentralizadorBO
    {
        [Required(ErrorMessage = "Tablillas: Debe seleccionar un elemento de la lista")]
        public Nullable<int> tablillas { get; set; }


        [Required(ErrorMessage = "Contactores: Debe seleccionar un elemento de la lista")]
        public Nullable<int> contactores { get; set; }

        [Required(ErrorMessage = "Relevadores: Debe seleccionar un elemento de la lista")]
        public Nullable<int> relevadores { get; set; }

        [Required(ErrorMessage = "Elementos de control: Debe seleccionar un elemento de la lista")]
        public Nullable<int> elementos_ctrl { get; set; }

        [Required(ErrorMessage = "Resistencia Calefactora: Debe seleccionar un elemento de la lista")]
        public Nullable<int> funciona_resis_calefactora { get; set; }





    }
}