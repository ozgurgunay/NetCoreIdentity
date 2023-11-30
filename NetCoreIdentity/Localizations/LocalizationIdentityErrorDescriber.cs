using Microsoft.AspNetCore.Identity;

namespace NetCoreIdentity.Localizations
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        //In this class override error descriptions if you want you change error description language or contents. And you have to call this class in StartupExtensions class.
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"{userName} not available." };
            //return base.DuplicateUserName(userName);
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return base.DuplicateEmail(email);
        }
        
        public override IdentityError PasswordTooShort(int length)
        {
            return base.PasswordTooShort(length);
        }


    }
}
