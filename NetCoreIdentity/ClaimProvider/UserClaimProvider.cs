﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using NetCoreIdentity.Repository.Models;
using System.Security.Claims;

namespace NetCoreIdentity.ClaimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityUser = principal.Identity as ClaimsIdentity;    //User.Claims same object
            var currentUser = await _userManager.FindByNameAsync(identityUser!.Name);

            if (String.IsNullOrEmpty(currentUser!.City))
            {
                return principal;
            }

            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", currentUser.City);

                identityUser.AddClaim(cityClaim);
            }

            return principal;
        }
    }
}
