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
    
    public partial class CondicionAislador
    {
        public int id { get; set; }
        public int Inspeccion_visual_id { get; set; }
        public Nullable<bool> corrosion { get; set; }
        public string observaciones_corrosion { get; set; }
        public Nullable<bool> roturas { get; set; }
        public string observaciones_roturas { get; set; }
        public Nullable<bool> grietas { get; set; }
        public string observaciones_grietas { get; set; }
        public Nullable<bool> flameo { get; set; }
        public string observaciones_flameo { get; set; }
        public Nullable<bool> danio_cementado { get; set; }
        public string observaciones_cementado { get; set; }
        public Nullable<bool> danio_piezasfijacion { get; set; }
        public string observaciones_piezasfijacion { get; set; }
    
        public virtual Inspeccion_visual Inspeccion_visual { get; set; }
    }
}
