using System;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(PruebaBO))]
    public partial class Prueba
    {
     

    }
    
    public class PruebaBO
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_prueba { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fecha_inspeccion { get; set; }

        [Required]
        [StringLength(1, ErrorMessage = "Instrumento de Medicion SF6: Debe seleccionar un elemento de la lista")]
        public string instrumento_medicionSF6 { get; set; }

        //[RequiredTrueAorB("existe_gabinete_centralizador", "Gabinete de Control x Fase o Gabinete Centralizador es requerido")]
        //public bool existe_gabinete_centralizador { get; set; }
        //[RequiredTrueAorB("existe_gabinetectrl_xfase", "Gabinete Centralizador o Gabinete de Control x Fase es requerido")] 
        //public bool existe_gabinetectrl_xfase { get; set; }


        //[RequiredTrueAorB("evalBasica", "Evaluación Básica es requerido")]
        //public string evalBasica { get; set; }

        //[RequiredTrueAorB("evalExtendida", "Evaluación Extendida es requerido")]
        //public string evalExtendida { get; set; }




        //[Required]
        //[StringLength(1, ErrorMessage = "Instrumento de Medicion SF6: Debe seleccionar un elemento de la lista")]
        //public string instrumento_medicionSF6 { get; set; }
    }
}
