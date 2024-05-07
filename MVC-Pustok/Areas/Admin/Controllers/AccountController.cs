using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Pustok.Areas.Admin.ViewModels;
using MVC_Pustok.Models;

namespace MVC_Pustok.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "mimy",
            };

            var result = await _userManager.CreateAsync(admin, "mimy11mimy");

            return Json(result);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminViewModel logvm,string returnUrl)
        {
            AppUser admin = await _userManager.FindByNameAsync(logvm.UserName);

            if (admin == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect !");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(admin,logvm.Password,false,false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect !");
                return View();
            }
			return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("index", "dashboard");
		}

        public IActionResult GetName()
        {
            var username = User.Identity.Name;
            return Json(User.Identity);
        }
    }
}
