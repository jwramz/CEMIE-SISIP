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
    
    public partial class vdPrMecOperacion
    {
        public int id { get; set; }
        public int prueba_id { get; set; }
        public string fase { get; set; }
        public Nullable<decimal> tapertura_d1c1 { get; set; }
        public Nullable<decimal> tapertura_d1c2 { get; set; }
        public Nullable<decimal> tapertura_d2c1 { get; set; }
        public Nullable<decimal> tapertura_d2c2 { get; set; }
        public Nullable<decimal> tcierre_c1 { get; set; }
        public Nullable<decimal> tcierre_c2 { get; set; }
        public Nullable<decimal> tent_resprein_c1 { get; set; }
        public Nullable<decimal> tent_resprein_c2 { get; set; }
    }
}
