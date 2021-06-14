using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace sievis.Models
{
    #region TextValueList
    public class TextValueItem
    {
        public TextValueItem(string descripcion, string valor)
        {
            this.Descripcion = descripcion;
            this.Valor = valor;
        }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }

    public class TextValueList
    {
        public List<TextValueItem> _list { get; set; }
        public TextValueList()
        {
            _list = new List<TextValueItem>();
        }
        public void agregar(TextValueItem nuevoElemento)
        {
            _list.Add(nuevoElemento);
        }
    }
    #endregion

    #region Estructuras
    public struct PuntuacionFase
    {
        public string variable;
        public decimal peso;
        public string fase;
        public decimal valorNumero;
        public string valorLetra;
        public string puntuacionLetra;
        public decimal puntuacionNumero;
        public string recomendacion;
    }

    public class PuntuacionVariable
    {
        private string variable;
        private decimal peso;
        private List<char> fases;
        private decimal valorNumero;
        private string valorLetra;
        private string puntuacionLetra;
        private decimal puntuacionNumero;
        private string recomendacion;

        public string Variable { get => variable; set => variable = value; }
        public decimal Peso { get => peso; set => peso = value; }
        public List<char> Fases { get => fases; set => fases = value; }
        public decimal ValorNumero { get => valorNumero; set => valorNumero = value; }
        public string ValorLetra { get => valorLetra; set => valorLetra = value; }
        public string PuntuacionLetra { get => puntuacionLetra; set => puntuacionLetra = value; }
        public decimal PuntuacionNumero { get => puntuacionNumero; set => puntuacionNumero = value; }
        public string Recomendacion { get => recomendacion; set => recomendacion = value; }
    }

    public class Parametro
    {
        private string parametro;
        private decimal peso;
        private List<PuntuacionVariable> listaVariables;

        public int numVariables() => listaVariables.Count;

        public decimal obtenCPCm()
        {
            decimal vlCPC = 0, vlSumaMultiplicaciones = 0, vlSumaPuntuaciones = 0;

            foreach (PuntuacionVariable unaPuntuacion in listaVariables)
            {
                if (unaPuntuacion.PuntuacionLetra != null && unaPuntuacion.PuntuacionLetra != "")
                {
                    vlSumaMultiplicaciones += unaPuntuacion.PuntuacionNumero * unaPuntuacion.Peso;
                    vlSumaPuntuaciones += unaPuntuacion.Peso;
                }
            }
            vlCPC = (vlSumaMultiplicaciones / (4 * vlSumaPuntuaciones)) * 4;
            return vlCPC;
        }
    }
    #endregion

    public static class AppEnum {
        #region Listas
        public static TextValueList GetConfiguracionCamaras()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Columna", "C"));
            lista.agregar(new TextValueItem("Tipo T", "T"));
            lista.agregar(new TextValueItem("Tipo Y", "Y"));
            return lista;
        }

        public static TextValueList GetDisenioEstructural()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Tanque Vivo", "TV"));
            lista.agregar(new TextValueItem("Tanque Muerto", "TM"));
            return lista;
        }

        public static TextValueList GetInterruptorContiene()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Resistencia de preinserción", "R"));
            lista.agregar(new TextValueItem("Capacitor de gradiente", "C"));
            lista.agregar(new TextValueItem("Ambos", "A"));
            lista.agregar(new TextValueItem("Ninguno", "N"));
            return lista;
        }

        public static TextValueList GetDisponibilidad()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Alta", "A"));
            lista.agregar(new TextValueItem("Media", "M"));
            lista.agregar(new TextValueItem("Baja", "B"));
            return lista;
        }

        public static TextValueList GetEstado() {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Buen Estado", "BE"));
            lista.agregar(new TextValueItem("Mal Estado", "ME"));
            return lista;
        }

        public static TextValueList GetFuncionaNoFunciona()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Funciona", "SF"));
            lista.agregar(new TextValueItem("No funciona", "NF"));
            return lista;
        }

        public static TextValueList GetEstadoBMNA()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Buen Estado", "BE"));
            lista.agregar(new TextValueItem("Mal Estado", "ME"));
            lista.agregar(new TextValueItem("No aplica", "NA"));
            return lista;
        }

        public static TextValueList GetFases()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Fase A", "A"));
            lista.agregar(new TextValueItem("Fase B", "B"));
            lista.agregar(new TextValueItem("Fase C", "C"));
            return lista;
        }

        public static TextValueList GetEstadoNA()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Estado Normal", "N"));
            lista.agregar(new TextValueItem("Estado Anormal", "A"));
            return lista;
        }

        public static TextValueList GetSiNo()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Si", "1"));
            lista.agregar(new TextValueItem("No", "0"));
            return lista;
        }

        public static TextValueList GetSiNoSD()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Si", "1"));
            lista.agregar(new TextValueItem("No", "0"));
            lista.agregar(new TextValueItem("Sin Datos", "2"));
            return lista;
        }
        public static TextValueList GetSiNoBool()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Si", "true"));
            lista.agregar(new TextValueItem("No", "false"));
            return lista;
        }
        public static TextValueList GetBoolean()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("", ""));
            lista.agregar(new TextValueItem("Si", "true"));
            lista.agregar(new TextValueItem("No", "False"));
            return lista;
        }

        public static TextValueList GetUbicaTempMax()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Terminal Lado Cuchilla", "TLCU"));
            lista.agregar(new TextValueItem("Terminal Lado TC", "TLTC"));
            return lista;
        }

        public static TextValueList GetTipoDisparo()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Monopolar", "M"));
            lista.agregar(new TextValueItem("Tripolar", "T"));
            return lista;
        }

        public static TextValueList GetComandoCierre()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Monopolar", "M"));
            lista.agregar(new TextValueItem("Tripolar", "T"));
            return lista;
        }

        public static TextValueList GetTipoUnidadPresion()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Unidad", ""));
            lista.agregar(new TextValueItem("bar", "B"));
            lista.agregar(new TextValueItem("MPa", "M"));
            return lista;
        }

        public static TextValueList GetUnidadMedicionHumedad()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Unidad", ""));
            lista.agregar(new TextValueItem("°C", "C"));
            lista.agregar(new TextValueItem("ppmv", "P"));
            return lista;
        }

        public static TextValueList GetNivelContaminacion()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Alta", "A"));
            lista.agregar(new TextValueItem("Media", "M"));
            lista.agregar(new TextValueItem("Baja", "B"));
            return lista;
        }

        public static TextValueList GetInstrumentoMedicion()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Presostato", "P"));
            lista.agregar(new TextValueItem("Densimetro", "D"));
            return lista;
        }

        public static TextValueList GetCumpleNoCumple()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Cumple", "CU"));
            lista.agregar(new TextValueItem("No Cumple", "NC"));
            return lista;
        }
        public static TextValueList GetCumpleNoCumplePR()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Si", "CU"));
            lista.agregar(new TextValueItem("No", "NC"));
            lista.agregar(new TextValueItem("Sin datos", "SD"));
            return lista;
        }
        public static TextValueList GetExisteNoExiste()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Existe", "1"));
            lista.agregar(new TextValueItem("No Existe", "0"));
            return lista;
        }
        public static TextValueList GetClaseInterruptor()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("M1-2000", "M1"));
            lista.agregar(new TextValueItem("M2-10000", "M2"));
            return lista;
        }
        public static TextValueList GetInstrumentoMedicionSF6()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("Densímetro", "D"));
            lista.agregar(new TextValueItem("Presóstato", "P"));
            return lista;
        }
        public static TextValueList GetFrecuenciaLlenado()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("nunca", "0"));
            lista.agregar(new TextValueItem("una vez al año", "1"));
            lista.agregar(new TextValueItem("hasta 3 veces al año", "2"));
            lista.agregar(new TextValueItem("más de 3 veces al año", "3"));
            return lista;
        }
        public static TextValueList GetRoles()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Administrador", "AD"));
            lista.agregar(new TextValueItem("Administrador Zona", "AZ"));
            lista.agregar(new TextValueItem("Capturista de Zona", "CZ"));
            lista.agregar(new TextValueItem("Visualizador de Zona", "VZ"));
            lista.agregar(new TextValueItem("Visualizador Gerencia", "VG"));
            return lista;
        }
        public static TextValueList GetEstatusUsuario()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Solicitud", "S"));
            lista.agregar(new TextValueItem("Activado", "A"));
            lista.agregar(new TextValueItem("Inactivado", "I"));
            return lista;
        }
        public static TextValueList GetNumeroMotores()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("1", "1"));
            lista.agregar(new TextValueItem("3", "3"));
            return lista;
        }

        public static TextValueList GetVoltajeNominalBobina()
        {
            TextValueList lista = new TextValueList();
            lista.agregar(new TextValueItem("Elija una opción", ""));
            lista.agregar(new TextValueItem("125", "125"));
            lista.agregar(new TextValueItem("250", "250"));
            return lista;
        }
        #endregion

        #region Helper
        public static List<SelectListItem> ToSelectList(this TextValueList lista)
        {
            var listaResultante = new List<SelectListItem>();
            foreach (TextValueItem elemento in lista._list)
            {
                var descripcion = elemento.Descripcion;
                var valor = elemento.Valor;
                listaResultante.Add(new SelectListItem()
                {
                    Text = descripcion,
                    Value = valor.ToString()
                });
            }
            return listaResultante;
        }

        public static List<SelectListItem> ToSelectList(this TextValueList lista, string seleccion)
        {
            var listaResultante = new List<SelectListItem>();
            foreach (TextValueItem elemento in lista._list)
            {
                var descripcion = elemento.Descripcion;
                var valor = elemento.Valor;
                if (valor.Equals(seleccion))
                {
                    listaResultante.Add(new SelectListItem()
                    {
                        Text = descripcion,
                        Value = valor.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    listaResultante.Add(new SelectListItem()
                    {
                        Text = descripcion,
                        Value = valor.ToString()
                    });
                }
            }
            return listaResultante;
        }

        public static String ToItemSelected(this TextValueList lista, string seleccion)
        {
            String elementoSeleccionado = "";
            foreach (TextValueItem elemento in lista._list)
            {
                var descripcion = elemento.Descripcion;
                var valor = elemento.Valor;
                if (valor.Equals(seleccion))
                {
                    elementoSeleccionado = descripcion;
                }
            }
            return elementoSeleccionado;
        }
        #endregion

    }
}