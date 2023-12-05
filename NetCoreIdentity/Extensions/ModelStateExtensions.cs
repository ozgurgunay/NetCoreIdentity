using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetCoreIdentity.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
        {
            errors.ForEach(error =>
            {
                modelState.AddModelError(string.Empty, error);
            });
        }

        public static void AddModelErrorList(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(error =>
            {
                modelState.AddModelError(string.Empty, error.Description);
            });
        }

    }
}
