using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Models;
using NetCoreIdentity.ViewModels;
using System.Diagnostics;
using NetCoreIdentity.Extensions;
using System.Security.Policy;
using NetCoreIdentity.Services;

namespace NetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager; //for cookie
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            },
                request.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration is successful.";

                return RedirectToAction(nameof(HomeController.SignUp));
            }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //In this method, the returnUrl is for the user to give this link to return to the page they left off after logging in, if they want to become a member of a page they are trying to access, and be directed directly to the last page they viewed. It doesn't have to be.
            returnUrl ??= Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "E-Mail or Password is wrong!");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, request.Password, request.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl!);
            }
            ModelState.AddModelErrorList(new List<string>() { "E-Mail or Password is wrong!" });
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            //link
            //https://localhost:7013?userId=132&token=hvsydhba
            
            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "We can not found for this mail user.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email);

            TempData["SuccessMessage"] = "Password reset link has been sent to your e-mail address.";

            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if(userId == null || token ==null)
            {
                throw new Exception("error about password!");
            }
            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "We can not find user.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);
            if(result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your password has been reset successfly.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
            }

            return View();
        }
    }
}