using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Areas.Admin.Models;
using NetCoreIdentity.Models;
using System.Security.AccessControl;
using NetCoreIdentity.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreIdentity.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();

            return View(roles);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = model.Name });
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            TempData["SuccessMessage"] = "Role created.";
            return RedirectToAction(nameof(RolesController.Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(string Id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(Id);
            if (roleToUpdate == null)
            {
                throw new Exception("We can not find this role!");
            }

            return View(new UpdateRoleViewModel() { Id = roleToUpdate.Id, Name = roleToUpdate.Name });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(model.Id);
            if (roleToUpdate == null)
            {
                throw new Exception("We can not find this role!");
            }

            roleToUpdate.Name = model.Name;

            await _roleManager.UpdateAsync(roleToUpdate);

            ViewData["SuccessMessage"] = "Updated role.";

            return View();
        }       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(Id);
            if (roleToDelete == null)
            {
                throw new Exception("We can not find this role!");
            }
            var result = await _roleManager.DeleteAsync(roleToDelete);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
            }

            TempData["SuccessMessage"] = "Role deleted.";

            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var roleViewModelList = new List<AssignRoleToUserViewModel>();
            ViewBag.userId = id;

            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel() { Id = role.Id, Name = role.Name };
                if (userRoles.Contains(role.Name!))
                {
                    assignRoleToUserViewModel.Exist = true;
                }
                roleViewModelList.Add(assignRoleToUserViewModel);
            }

            return View(roleViewModelList);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId,List<AssignRoleToUserViewModel> modelList)
        {
            var assignRoleToUser = await _userManager.FindByIdAsync(userId);
            
            foreach (var role in modelList)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(assignRoleToUser, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(assignRoleToUser, role.Name);
                }
            }

            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }

    }
}
