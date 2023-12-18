using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Core.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Old Password can not be left empty!")]
        [Display(Name = " Old Password :")]
        [MinLength(6, ErrorMessage = "Password have to be min. 6 character")]
        public string PasswordOld { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New Password can not be left empty!")]
        [Display(Name = " New Password :")]
        [MinLength(6, ErrorMessage = "Password have to be min. 6 character")]
        public string? PasswordNew { get; set;} = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm New Password can not be left empty!")]
        [Display(Name = "Confirm New Password :")]
        [MinLength(6, ErrorMessage = "Password have to be min. 6 character")]
        public string? PasswordNewConfirm { get; set;} = null!;
    }
}
