using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NetCoreIdentity.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int AgeLimit { get; set; }
    }
    public class ViolenceRequirementHandler: AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "Birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim birthDateClaim = context.User.FindFirst("Birthdate")!;

            var today = DateTime.Now;
            var birthDate = Convert.ToDateTime(birthDateClaim.Value);
            var age = today.Year - birthDate.Year;

            //this line for Lipe Year(artık yıl 28-29 February)
            if (birthDate > today.AddYears(-age)) age--;

            if(requirement.AgeLimit > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;

        }
    }
}
