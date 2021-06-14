using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace sievis.Models
{
    [MetadataTypeAttribute(typeof(AppUsersBO))]
    public partial class AppUsers
    {        
    }

    public partial class AppUsersBO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //[RequiredIf(@"Action === ""Agregar"" ")]        
        //public string Password { get; set; }        
        //[RequiredIf(@"Action === ""Agregar"" ")]        
        //public string RePassword { get; set; }
    }
}

