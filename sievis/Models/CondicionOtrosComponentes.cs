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
    
    public partial class CondicionOtrosComponentes
    {
        public int id { get; set; }
        public int Inspeccion_visual_id { get; set; }
        public Nullable<bool> ruido_audible { get; set; }
        public string observaciones_ruidoaudible { get; set; }
        public Nullable<bool> vibracion_audible { get; set; }
        public string observaciones_vibracionaudible { get; set; }
        public Nullable<bool> corrosion_tubing { get; set; }
        public string observaciones_ctubing { get; set; }
        public Nullable<bool> corrosion_ctierra { get; set; }
        public string observaciones_ctierra { get; set; }
    
        public virtual Inspeccion_visual Inspeccion_visual { get; set; }
    }
}
