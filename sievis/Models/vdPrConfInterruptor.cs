//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sievis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vdPrConfInterruptor
    {
        public int equipoId { get; set; }
        public int pruebaId { get; set; }
        public Nullable<System.DateTime> fecha_puestaservicio { get; set; }
        public Nullable<int> aPuestaServicio { get; set; }
        public Nullable<System.DateTime> fultimo_mantenimiento { get; set; }
        public Nullable<int> aUltimoManto { get; set; }
        public Nullable<int> numOperaciones { get; set; }
        public string disponibilidad_refaccion_st { get; set; }
        public string nivel_contaminacion { get; set; }
    }
}
