using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(EquipoBO))]
    public partial class Equipo
    {
    }

    public class EquipoBO
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fecha_puestaservicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fultimo_mantenimiento { get; set; }
        [Required]
        public int Gerencia_id { get; set; }
        [Required]
        public int Zona_id { get; set; }
        [Required]
        public int Subestacion_id { get; set; }
        [Required]
        public int AplicacionInterruptor_id { get; set; }
        [Required]
        public int Marca_id { get; set; }
        [Required]
        public string bahia { get; set; }
        [Required]
        public int Modelo_id { get; set; }
        [Required]
        public string ns { get; set; }
        [Required]
        public int anio_fabricacion { get; set; }
        [Required]
        public Nullable<int> altitud_instalacion { get; set; }
        //[Required]
        public string nivel_contaminacion { get; set; }
        //[Required]
        public Nullable<int> distancia_fuga { get; set; }
        [Required]
        public string tipo_disparo { get; set; }
        [Required]
        public string comando_cierre { get; set; }
        //[Required]
        public Nullable<int> altitud_operacion { get; set; }
        [Required]
        public string dis_estructural { get; set; }
        [Required]
        public int Mecanismo_id { get; set; }
        [Required]
        public decimal presionSF6 { get; set; }
        [Required]
        public string tipo_unidades_presion { get; set; }
        [Required]
        public Nullable<decimal> presion_alarma { get; set; }
        [Required]
        public string interruptor_contiene { get; set; }
        
        public Nullable<decimal> interruptor_resistencia { get; set; }
        
        public Nullable<decimal> interruptor_capacitor { get; set; }
        
        public string conf_camaras { get; set; }

        //-- [Not Required]
        /*
        public Nullable<decimal> tesion_nominal { get; set; }
        public Nullable<decimal> corriente_nominal { get; set; }
        public Nullable<decimal> corriente_cc { get; set; }
        public Nullable<decimal> bil { get; set; }
        public string disponibilidad_refaccion_st { get; set; }
        public Nullable<decimal> res_estatica_contactos { get; set; }
        public string clase_interruptor { get; set; }
        public virtual AplicacionInterruptor AplicacionInterruptor { get; set; }        
        public virtual ICollection<Prueba> Prueba { get; set; }
        public virtual Marca Marca { get; set; }
        public virtual Mecanismo Mecanismo { get; set; }
        public virtual Modelo Modelo { get; set; }
        public virtual Subestacion Subestacion { get; set; }
        */
    }
}