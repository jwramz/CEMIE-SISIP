using System;
using System.ComponentModel.DataAnnotations;


namespace sievis.Models.BusinessObject
{

    [MetadataTypeAttribute(typeof(Inpeccion_visualBO))]
    public partial class Inspeccion_visual
    {
    }

    public class Inpeccion_visualBO
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fecha_inspeccion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fecha_puestaservicio { get; set; }

        //Selector de Opciones sea obligatoria

        [Required(ErrorMessage = "Resistencia Calefactora: Debe seleccionar un elemento de la lista")]
        public Nullable<int> num_operaciones { get; set; }

        [Required(ErrorMessage = "Resistencia Calefactora: Debe seleccionar un elemento de la lista")]
        public string resistencia_calefactora { get; set; }

        [Required(ErrorMessage = "Condision Pernos: Debe seleccionar un elemento de la lista")]
        public string condision_pernos { get; set; }

   
    }
}