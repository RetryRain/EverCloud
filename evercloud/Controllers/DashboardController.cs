using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using evercloud.Domain.Models;

namespace evercloud.Controllers

{
    public class DashboardController(UserManager<Users> userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.FullName = user.FullName;
            }
            return View();
        }
    }
}
