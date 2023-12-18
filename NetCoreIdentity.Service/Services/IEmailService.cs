
namespace NetCoreIdentity.Service.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
    }
}
