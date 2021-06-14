using System.Linq;

namespace sievis.Models
{
    public partial class Prueba
    {
        public Inspeccion_visual IVA
        {
            get
            {
                if (this.Inspeccion_visual.Count == 0) Initialize();
                return this.Inspeccion_visual.ElementAt(0);
            }
            set
            {
                var item = this.Inspeccion_visual.ElementAt(0);
                item = value;
            }
        }
        public Inspeccion_visual IVB
        {
            get
            {
                if (this.Inspeccion_visual.Count == 0) Initialize();
                return this.Inspeccion_visual.ElementAt(1);
            }
            set
            {
                var item = this.Inspeccion_visual.ElementAt(1);
                item = value;
            }
        }
        public Inspeccion_visual IVC
        {
            get
            {
                if (this.Inspeccion_visual.Count == 0) Initialize();
                return this.Inspeccion_visual.ElementAt(2);
            }
            set
            {
                var item = this.Inspeccion_visual.ElementAt(2);
                item = value;
            }
        }

        public PruebaRutina PR
        {
            get
            {
                if (this.PruebaRutina.Count == 0) Initialize();
                return this.PruebaRutina.ElementAt(0);
            }
            set
            {
                var item = this.PruebaRutina.ElementAt(0);
                item = value;
            }
        }

        public CondicionGabineteCentralizador GCE
        {
            get
            {
                if (this.CondicionGabineteCentralizador.Count == 0) Initialize();
                return this.CondicionGabineteCentralizador.ElementAt(0);
            }
            set
            {
                var item = this.CondicionGabineteCentralizador.ElementAt(0);
                item = value;
            }
        }

        public PruebaEspecial PE
        {
            get
            {
                if (this.PruebaEspecial.Count == 0) Initialize();
                return this.PruebaEspecial.ElementAt(0);
            }
            set
            {
                var item = this.PruebaEspecial.ElementAt(0);
                item = value;
            }
        }

        public void Initialize() {
            this.fecha_prueba = System.DateTime.Now;
            this.fecha_inspeccion = null;
            this.existe_gabinetectrl_xfase = false;
            this.existe_gabinete_centralizador = false;

            this.Inspeccion_visual.Clear();
            this.Inspeccion_visual.Add(new Inspeccion_visual() { fase = "A" });
            this.Inspeccion_visual.Add(new Inspeccion_visual() { fase = "B" });
            this.Inspeccion_visual.Add(new Inspeccion_visual() { fase = "C" });

            this.Inspeccion_visual.ElementAt(0).Initialize();
            this.Inspeccion_visual.ElementAt(1).Initialize();
            this.Inspeccion_visual.ElementAt(2).Initialize();

            this.PruebaRutina.Clear();
            this.PruebaRutina.Add(new PruebaRutina());
            this.PruebaRutina.ElementAt(0).Initialize();

            this.PruebaEspecial.Clear();
            this.PruebaEspecial.Add(new PruebaEspecial());
            this.PruebaEspecial.ElementAt(0).Initialize();

            this.CondicionGabineteCentralizador.Clear();
            if (this.CondicionGabineteCentralizador.Count == 0)
                this.CondicionGabineteCentralizador.Add(new Models.CondicionGabineteCentralizador());
        }
    }
}
