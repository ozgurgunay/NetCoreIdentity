using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Please check e-mail format!")]
        [Required(ErrorMessage = "E-Mail can not be left empty!")]
        [Display(Name = "E-Mail :")]
        public string? Email { get; set; }
    }
}
