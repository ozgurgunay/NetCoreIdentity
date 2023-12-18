using Microsoft.AspNetCore.Identity;
using NetCoreIdentity.Repository.Models;

namespace NetCoreIdentity.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        //we added it to StartupExtensions class
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new() { Code = "PasswordContainUserName", Description = "Password can not contain user name!" });
            }

            if (password!.ToLower().StartsWith("1234"))
            {
                errors.Add(new() { Code = "PasswordStart1234", Description = "Password can not start 1234!" });
            }

            if(errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
