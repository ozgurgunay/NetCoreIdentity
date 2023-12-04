using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.ViewModels
{
    public class ResetPasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can not be left empty!")]
        [Display(Name = "New Password :")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password is not same!")]
        [Required(ErrorMessage = "Password Confirm can not be left empty!")]
        [Display(Name = "New Password Confirm :")]
        public string? PasswordConfirm { get; set; }
    }
}
