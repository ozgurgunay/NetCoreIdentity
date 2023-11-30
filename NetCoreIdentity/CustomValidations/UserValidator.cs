using Microsoft.AspNetCore.Identity;
using NetCoreIdentity.Models;

namespace NetCoreIdentity.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isNumeric = int.TryParse(user.UserName[0]!.ToString(), out _);

            if (isNumeric)
            {
                errors.Add(new() { Code = "UserNameContainFirstLetterDigit", Description = "The first character of the user name contains a numeric character."});
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}
