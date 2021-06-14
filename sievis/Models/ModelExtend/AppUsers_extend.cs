using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace sievis.Models
{
    public partial class AppUsers
    {
        public AppUsers() {
            this.Action = string.Empty;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Action { get; set; }
    }
}
