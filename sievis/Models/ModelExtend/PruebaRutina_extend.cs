using System.Linq;

namespace sievis.Models
{
    public partial class PruebaRutina
    {
        public PruebaRutinaDetalle DetFA
        {
            get
            {
                if (this.PruebaRutinaDetalle.Count == 0) Initialize();
                return this.PruebaRutinaDetalle.ElementAt(0);
            }
            set
            {
                var item = this.PruebaRutinaDetalle.ElementAt(0);
                item = value;
            }
        }

        public PruebaRutinaDetalle DetFB
        {
            get
            {
                if (this.PruebaRutinaDetalle.Count == 0) Initialize();
                return this.PruebaRutinaDetalle.ElementAt(1);
            }
            set
            {
                var item = this.PruebaRutinaDetalle.ElementAt(1);
                item = value;
            }
        }

        public PruebaRutinaDetalle DetFC
        {
            get
            {
                if (this.PruebaRutinaDetalle.Count == 0) Initialize();
                return this.PruebaRutinaDetalle.ElementAt(2);
            }
            set
            {
                var item = this.PruebaRutinaDetalle.ElementAt(2);
                item = value;
            }
        }

        public void Initialize()
        {
            this.PruebaRutinaDetalle.Clear();
            if (this.PruebaRutinaDetalle.Count == 0)
            {
                this.PruebaRutinaDetalle.Add(new Models.PruebaRutinaDetalle() { fase = "A" });
                this.PruebaRutinaDetalle.Add(new Models.PruebaRutinaDetalle() { fase = "B" });
                this.PruebaRutinaDetalle.Add(new Models.PruebaRutinaDetalle() { fase = "C" });
            }
        }
    }
}