using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreIdentity.Models;
using System.Text;

namespace NetCoreIdentity.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserId { get; set; } = null!;

        private readonly UserManager<AppUser> _userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var stringBuilder = new StringBuilder();

            userRoles.ToList().ForEach(x =>
            {
                //@ means -> If you put an @ sign in front of it, you can arrange the spans as you wish in terms of page layout and font style.
                stringBuilder.Append(@$"<span class='badge bg-dark mx-1'>{x.ToLower()}</span>");
            });
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
