using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Core.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {

        }
        //If you write these values in the constructor, you will not receive the nullable warning, or you can right-click on the project and close Properties->Build->General->Nullable to turn off the nullable feature that comes with .net 6.
        public SignUpViewModel(string userName, string email, string password, string phoneNumber)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

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

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can not be left empty!")]
        [Display(Name = "Password :")]
        [MinLength(6, ErrorMessage = "Password have to be min. 6 character")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password is not same!")]
        [Required(ErrorMessage = "Password Confirm can not be left empty!")]
        [Display(Name = "Password Confirm :")]
        [MinLength(6, ErrorMessage = "Password have to be min. 6 character")]
        public string PasswordConfirm { get; set; } = null!;
        
    }
}
