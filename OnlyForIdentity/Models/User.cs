using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlyForIdentity.Models
{
    public class User : IdentityUser
    {
        [Required] public string? FullName { get; set; }
        [Required] public long AadharNo { get; set; }

        //public string Role { get; set; } = "User";
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
