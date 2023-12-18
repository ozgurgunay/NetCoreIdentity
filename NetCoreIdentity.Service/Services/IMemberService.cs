using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreIdentity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreIdentity.Service.Services
{
    public interface IMemberService
    {
        Task<UserViewModel> GetUserViewModelByUserName(string userName);
        Task Logout();

        Task<bool> CheckPasswordAsync(string userName, string password);

        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
        Task<UserEditViewModel> GetUserEditViewModelAsync(string userName);
        SelectList GetGenderSelectList();
        Task<(bool, IEnumerable<IdentityError>?)> EditUserAsync(UserEditViewModel model, string userName);
    }
}
