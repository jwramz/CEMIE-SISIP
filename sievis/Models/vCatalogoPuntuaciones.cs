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
    
    public partial class vCatalogoPuntuaciones
    {
        public int catId { get; set; }
        public int detId { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string abreviatura { get; set; }
        public Nullable<int> valorEntero { get; set; }
        public Nullable<decimal> valorFlotante { get; set; }
    }
}