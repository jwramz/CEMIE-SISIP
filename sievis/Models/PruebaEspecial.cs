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
    
    public partial class PruebaEspecial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PruebaEspecial()
        {
            this.PruebaEspecialDetalle = new HashSet<PruebaEspecialDetalle>();
        }
    
        public int Id { get; set; }
        public int Prueba_id { get; set; }
        public Nullable<decimal> temperatura_ambiente { get; set; }
        public string func_bobina_aper70d1 { get; set; }
        public string func_bobina_aper70d2 { get; set; }
        public string func_bobina_cierre85 { get; set; }
        public Nullable<decimal> arranque_minbobd1 { get; set; }
        public Nullable<decimal> arranque_minbobd2 { get; set; }
        public Nullable<decimal> arranque_minbob_cierre { get; set; }
        public string sec_nominal_operacion { get; set; }
        public Nullable<short> numero_motores { get; set; }
    
        public virtual Prueba Prueba { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PruebaEspecialDetalle> PruebaEspecialDetalle { get; set; }
    }
}
