using System.Linq;

namespace sievis.Models
{
    public partial class PruebaEspecial
    {
        public PruebaEspecialDetalle DetFA
        {
            get
            {
                if (this.PruebaEspecialDetalle.Count == 0) Initialize();
                return this.PruebaEspecialDetalle.ElementAt(0);
            }
            set
            {
                var item = this.PruebaEspecialDetalle.ElementAt(0);
                item = value;
            }
        }

        public PruebaEspecialDetalle DetFB
        {
            get
            {
                if (this.PruebaEspecialDetalle.Count == 0) Initialize();
                return this.PruebaEspecialDetalle.ElementAt(1);
            }
            set
            {
                var item = this.PruebaEspecialDetalle.ElementAt(1);
                item = value;
            }
        }

        public PruebaEspecialDetalle DetFC
        {
            get
            {
                if (this.PruebaEspecialDetalle.Count == 0) Initialize();
                return this.PruebaEspecialDetalle.ElementAt(2);
            }
            set
            {
                var item = this.PruebaEspecialDetalle.ElementAt(2);
                item = value;
            }
        }

        public void Initialize() {
            this.PruebaEspecialDetalle.Clear();
            if (this.PruebaEspecialDetalle.Count == 0)
            {
                this.PruebaEspecialDetalle.Add(new Models.PruebaEspecialDetalle() { fase = "A" });
                this.PruebaEspecialDetalle.Add(new Models.PruebaEspecialDetalle() { fase = "B" });
                this.PruebaEspecialDetalle.Add(new Models.PruebaEspecialDetalle() { fase = "C" });
            }
        }

    }
}
