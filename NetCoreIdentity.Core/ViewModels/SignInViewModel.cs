using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Core.ViewModels
{
    public class SignInViewModel
    {
        //write ctor and press enter for create empty constructor
        public SignInViewModel()
        {
            
        }
        //alt + enter shortcut for create Constructor.
        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required(ErrorMessage = "Email can not be left empty!")]
        [EmailAddress(ErrorMessage = "Password")]
        [Display(Name ="Email")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can not be left empty!")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Display(Name = "Check me out")]
        public bool RememberMe { get; set; }
    }
}
