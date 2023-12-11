using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreIdentity.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "PermissionsOrderRead")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
