using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using NetCoreIdentity.Areas.Admin.Models;
using NetCoreIdentity.Extensions;
using NetCoreIdentity.Models;
using NetCoreIdentity.ViewModels;

namespace NetCoreIdentity.Controllers
{
    //[Authorize] this means only members can access these pages. You can use for only actionmethod or all controller it depends on your where you put this tag [Authorize]
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new ViewModels.UserViewModel
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };

            return View(userViewModel);
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

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, model.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Incorrect old password!");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, model.PasswordOld, model.PasswordNew);
            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, model.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Password has been change successfly.";

            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(EGender)));

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            currentUser.UserName = model.UserName;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.BirthDate = model.BirthDate;
            currentUser.City = model.City;
            currentUser.Gender = model.Gender;


            if(model.Picture != null && model.Picture.Length > 0)
            {
                var wwwroot = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwroot.First(x => x.Name == "userPictures").PhysicalPath, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await model.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }
            var updateToUserResult = await _userManager.UpdateAsync(currentUser);
            
            if(!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender
            };

            TempData["SuccessMessage"] = "User information has been changed successfly.";

            return View(userEditViewModel);

        }

    }
}
