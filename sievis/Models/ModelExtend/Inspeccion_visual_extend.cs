using System.Linq;

namespace sievis.Models
{

    public partial class Inspeccion_visual
    {
        /// <summary>
        /// ca:REV:OK
        /// </summary>
        public CondicionAislador ca
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.CondicionAislador.First();
            }
            set
            {
                var item = this.CondicionAislador.ElementAt(0);
                item = value;
            }
        }
        /// <summary>
        /// gc:REV:OK
        /// </summary>
        public CondicionGabineteControl gc
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.CondicionGabineteControl.First();
            }
            set
            {
                var item = this.CondicionGabineteControl.First();
                item = value;
            }
        }
        /// <summary>
        /// coc:REV:OK
        /// </summary>
        public CondicionOtrosComponentes coc
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.CondicionOtrosComponentes.First();
            }
            set
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                var item = this.CondicionOtrosComponentes.First();
                item = value;
            }
        }
        /// <summary>
        /// cv:REV:OK
        /// </summary>
        public CondicionVarillaje cv
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.CondicionVarillaje.First();
            }
            set
            {
                var item = this.CondicionVarillaje.First();
                item = value;
            }
        }
        /// <summary>
        /// d:REV:OK
        /// </summary>
        public Densimetro d
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.Densimetro.First();
            }
            set
            {
                var item = this.Densimetro.First();
                item = value;
            }
        }
        /// <summary>
        /// mh:REV:OK
        /// </summary>
        public MecanismoHidraulico mh
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.MecanismoHidraulico.First();
            }
            set
            {
                var item = this.MecanismoHidraulico.First();
                item = value;
            }
        }
        /// <summary>
        /// mn:REV:OK
        /// </summary>
        public MecanismoNeumatico mn
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.MecanismoNeumatico.First();
            }
            set
            {
                var item = this.MecanismoNeumatico.First();
                item = value;
            }
        }
        /// <summary>
        /// mr:REV:OK
        /// </summary>
        public MecanismoResortes mr
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.MecanismoResortes.First();
            }
            set
            {
                var item = this.MecanismoResortes.First();
                item = value;
            }
        }
        /// <summary>
        /// p:REV:OK
        /// </summary>
        public Presostato p
        {
            get
            {
                if (this.CondicionAislador.Count == 0) Initialize();
                return this.Presostato.First();
            }
            set
            {
                var item = this.Presostato.First();
                item = value;
            }
        }

        public void Initialize() {

            this.CondicionAislador.Clear();
            this.CondicionGabineteControl.Clear();
           
            this.CondicionOtrosComponentes.Clear();
            this.CondicionVarillaje.Clear();
            this.Densimetro.Clear();
            this.MecanismoHidraulico.Clear();
            this.MecanismoNeumatico.Clear();
            this.MecanismoResortes.Clear();
            this.Presostato.Clear();

            if (this.CondicionAislador.Count == 0)
                this.CondicionAislador.Add(new Models.CondicionAislador());
            if (this.CondicionGabineteControl.Count == 0)
                this.CondicionGabineteControl.Add(new Models.CondicionGabineteControl());           
            if (this.CondicionOtrosComponentes.Count == 0)
                this.CondicionOtrosComponentes.Add(new Models.CondicionOtrosComponentes());
            if (this.CondicionVarillaje.Count == 0)
                this.CondicionVarillaje.Add(new Models.CondicionVarillaje());
            if (this.Densimetro.Count == 0)
                this.Densimetro.Add(new Models.Densimetro());
            if (this.MecanismoHidraulico.Count == 0)
                this.MecanismoHidraulico.Add(new Models.MecanismoHidraulico());
            if (this.MecanismoNeumatico.Count == 0)
                this.MecanismoNeumatico.Add(new Models.MecanismoNeumatico());
            if (this.MecanismoResortes.Count == 0)
                this.MecanismoResortes.Add(new Models.MecanismoResortes());
            if (this.Presostato.Count == 0)
                this.Presostato.Add(new Models.Presostato());
        }
    }
}
