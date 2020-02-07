using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inlmämningsuppgift2.Areas.Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel{ReturnUrl = returnUrl});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            ModelState.AddModelError("","Invalid user or password");
            return View(model);
        }
        public IActionResult Logout(string returnUrl)
        {
            return Redirect(returnUrl);
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Manage()
        {
            return View();
        }
    }
}