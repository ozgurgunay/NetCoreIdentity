using Microsoft.AspNetCore.Identity;
using NetCoreIdentity.CustomValidations;
using NetCoreIdentity.Localizations;
using NetCoreIdentity.Repository.Models;

namespace NetCoreIdentity.Extensions
{
    public static class StartupExtensions
    {
        //You can write this in program.cs but it is better to write it here to keep it organized and make program.cs readable.
        public static void AddIdentityExtension(this IServiceCollection services)
        {
            //adding for token period
            services.Configure<DataProtectionTokenProviderOptions>(options => { options.TokenLifespan = TimeSpan.FromHours(1); });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                //for unique email address and user name characters
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                //settings required for password
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            }).AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();
            //.AddPasswordValidator<PasswordValidator>() this part adding with CustomValidations file.
        }
    }
}
