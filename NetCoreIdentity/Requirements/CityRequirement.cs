using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace NetCoreIdentity.Requirements
{
    public class CityRequirement : IAuthorizationRequirement
    {
        public string CityName { get; set; } = null!;
    }
    public class CityRequirementHandler : AuthorizationHandler<CityRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CityRequirement requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "City"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim cityClaim = context.User.FindFirst("City")!;
            //business
            if(requirement.CityName != cityClaim.Value)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
            
        }
    }
}
