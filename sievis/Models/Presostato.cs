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
    
    public partial class Presostato
    {
        public int id { get; set; }
        public int Inspeccion_visual_id { get; set; }
        public string carcasa { get; set; }
        public string observaciones_carcasa { get; set; }
        public string caratula { get; set; }
        public string observaciones_caratula { get; set; }
        public string condicion_aguja { get; set; }
        public string observaciones_cond_aguja { get; set; }
        public string nivel_glicerina { get; set; }
        public string observaciones_nglicerina { get; set; }
        public string escala { get; set; }
        public string observaciones_escala { get; set; }
        public Nullable<decimal> presionSF6_va { get; set; }
        public Nullable<decimal> presionSF6_vn { get; set; }
        public string observaciones_presionSF6 { get; set; }
        public Nullable<decimal> temperatura_va { get; set; }
        public Nullable<decimal> temperatura_vn { get; set; }
        public string observaciones_temperatura { get; set; }
    
        public virtual Inspeccion_visual Inspeccion_visual { get; set; }
    }
}