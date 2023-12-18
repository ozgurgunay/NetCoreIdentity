using Microsoft.AspNetCore.Identity;
using NetCoreIdentity.Core.Models;

namespace NetCoreIdentity.Repository.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public EGender? Gender { get; set; }
    }
}
