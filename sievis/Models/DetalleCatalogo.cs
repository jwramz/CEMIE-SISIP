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
    
    public partial class DetalleCatalogo
    {
        public int id { get; set; }
        public int Catalogo_id { get; set; }
        public string descripcion { get; set; }
        public string abreviatura { get; set; }
        public Nullable<int> valorEntero { get; set; }
        public Nullable<decimal> valorFlotante { get; set; }
    
        public virtual Catalogo Catalogo { get; set; }
    }
}
