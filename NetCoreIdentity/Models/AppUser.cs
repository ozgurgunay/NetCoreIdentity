using Microsoft.AspNetCore.Identity;

namespace NetCoreIdentity.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public EGender? Gender { get; set; }
    }
}
