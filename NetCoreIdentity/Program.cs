using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NetCoreIdentity.ClaimProvider;
using NetCoreIdentity.Extensions;
using NetCoreIdentity.Repository.Models;
using NetCoreIdentity.Core.OptionsModel;
using NetCoreIdentity.PermissionsRoot;
using NetCoreIdentity.Requirements;
using NetCoreIdentity.Seeds;
using NetCoreIdentity.Service.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("ContentCreatorConnection"), options => { options.MigrationsAssembly("NetCoreIdentity.Repository"); });
        });

        builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));


        //for sending email
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IMemberService, MemberService>();
        //builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();     //for claims
        builder.Services.AddScoped<IAuthorizationHandler, CityRequirementHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
        builder.Services.AddAuthorization(options =>
        {
            //you can add more rule in this policy. Depends role, claim or more! And you should call  in controller before action method "[Authorize(Policy = "AnkaraPolicy")]"
            //if you add business with claims-based authorization this means => policy based authorization.
            //options.AddPolicy("AnkaraPolicy", policy =>
            //{
            //    policy.RequireClaim("city", "ankara");
            //    //policy.RequireRole("admin");
            //});
            options.AddPolicy("CityPolicy", policy =>
            {
                policy.AddRequirements(new CityRequirement() { CityName = "Ankara" });
            });
            //policy based authorization example:
            options.AddPolicy("ExchangePolicy", policy =>
            {
                policy.AddRequirements(new ExchangeExpireRequirement());
            });

            options.AddPolicy("ViolencePolicy", policy =>
            {
                policy.AddRequirements(new ViolenceRequirement() { AgeLimit = 18 });
            });

            options.AddPolicy("OrderPermissionReadAndDelete", policy =>
            {
                policy.RequireClaim("permission", Permissions.Order.Read);
                policy.RequireClaim("permission", Permissions.Order.Delete);
                policy.RequireClaim("permission", Permissions.Stock.Delete);
            });

            options.AddPolicy("PermissionsOrderRead", policy =>
            {
                policy.RequireClaim("permission", Permissions.Order.Read);
            });
            options.AddPolicy("PermissionsOrderDelete", policy =>
            {
                policy.RequireClaim("permission", Permissions.Order.Delete);
            });
            options.AddPolicy("PermissionsStockDelete", policy =>
            {
                policy.RequireClaim("permission", Permissions.Stock.Delete);
            });

        });

        //calling StartupExtensions methodin Extensions File.
        builder.Services.AddIdentityExtension();

        builder.Services.ConfigureApplicationCookie(opt =>
        {
            var cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "IdentityAppCookie";
            opt.LoginPath = new PathString("/Home/SignIn");
            opt.LogoutPath = new PathString("/Member/Logout");  //this line adding for alternative logout method in MemberController
            opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
            opt.Cookie = cookieBuilder;
            opt.ExpireTimeSpan = TimeSpan.FromDays(15);
            opt.SlidingExpiration = true;
        });

        var app = builder.Build();

        //added for permissions seed data - uygulama bir kere ayaða kalkarken çalýþýr ve sonra memory den düþer
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            await PermissionSeed.Seed(roleManager);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        //adding for new Areas!
        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}