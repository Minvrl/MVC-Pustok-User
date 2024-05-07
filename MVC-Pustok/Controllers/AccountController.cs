using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Pustok.Areas.Admin.ViewModels;
using MVC_Pustok.Data;
using MVC_Pustok.Models;
using MVC_Pustok.ViewModels;

namespace MVC_Pustok.Controllers
{
	public class AccountController : Controller
	{
		public UserManager<AppUser> _userManager { get; set; }
        public AppDbContext _context { get; set; }
		private readonly SignInManager<AppUser> _signInManager;
		public AccountController(UserManager<AppUser> userManager, AppDbContext context, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegViewModel regvm)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser 
            { 
                FullName = regvm.FullName,
                UserName = regvm.UserName,
                Email = regvm.Email
            };
            var r = await _userManager.CreateAsync(user, regvm.Password);

            if (!r.Succeeded)
            {
                foreach (var eror in r.Errors)
                {
                    ModelState.AddModelError("", eror.Description);
                }
                return View();
            }
            return RedirectToAction("index", "home");

        }

        public IActionResult Profile()
        {
            AppUser user = _context.AppUsers.ToList().FirstOrDefault(x=> x.UserName != "mimy");

			if (user == null) return RedirectToAction("notfound", "error");
			return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel logvm)
        {
            AppUser user = await _userManager.FindByEmailAsync(logvm.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Email or password incorrect !");
                return View();
            }

            var r = await _signInManager.PasswordSignInAsync(user, logvm.Password, false, false);
            if (!r.Succeeded)
            {
				ModelState.AddModelError("", "Email or password incorrect !");
				return View();
			}
			return RedirectToAction("index", "home");
		}

	}
}
