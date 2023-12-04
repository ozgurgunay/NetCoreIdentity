using Microsoft.Build.Framework;

namespace NetCoreIdentity.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
    }
}
