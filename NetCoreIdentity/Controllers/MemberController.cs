using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Models;

namespace NetCoreIdentity.Controllers
{
    //[Authorize] this means only members can access these pages. You can use for only actionmethod or all controller it depends on your where you put this tag [Authorize]
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public MemberController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();

        //    return RedirectToAction("Index", "Home");
        //}
        //alternative Logout method
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
