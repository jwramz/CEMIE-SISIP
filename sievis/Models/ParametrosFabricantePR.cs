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
    
    public partial class ParametrosFabricantePR
    {
        public int id { get; set; }
        public Nullable<int> numeroCamarasPolo { get; set; }
        public Nullable<int> tiempoAperturaD1 { get; set; }
        public string criterioTAD1 { get; set; }
        public Nullable<int> tiempoAperturaD2 { get; set; }
        public string criterioTAD2 { get; set; }
        public Nullable<int> tiempoCierre { get; set; }
        public int ModeloId { get; set; }
        public int Marca_id { get; set; }
        public decimal ParametrosFabricantePR_ID { get; set; }
    
        public virtual Modelo Modelo { get; set; }
    }
}