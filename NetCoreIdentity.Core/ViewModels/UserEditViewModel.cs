using Microsoft.AspNetCore.Http;
using NetCoreIdentity.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "User Name can not be left empty!")]
        [Display(Name = "User Name :")]
        public string UserName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Please check e-mail format!")]
        [Required(ErrorMessage = "E-Mail can not be left empty!")]
        [Display(Name = "E-Mail :")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number can not be left empty!")]
        [Display(Name = "Phone Number :")]
        public string PhoneNumber { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date :")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "City :")]
        public string? City { get; set; }

        [Display(Name = "Profile Picture :")]
        public IFormFile? Picture { get; set; }
        
        [Display(Name = "Gender :")]
        public EGender? Gender{ get; set; }

    }
}
