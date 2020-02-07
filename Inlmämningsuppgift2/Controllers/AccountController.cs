using System;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Models.ViewModels;
using Inlmämningsuppgift2.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Inlmämningsuppgift2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IRepository<Customer> _customerRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordValidator<ApplicationUser> passwordValidator,
            IRepository<Customer> customerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordValidator = passwordValidator;
            _customerRepository = customerRepository;
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
                if ((await LoginUser(user,model.Password)).Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            ModelState.AddModelError("","Invalid user or password");
            return View(model);
        }
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel{ReturnUrl = returnUrl});
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Customer.Email);

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return View(model);
            }
            
            user = new ApplicationUser
            {
                UserName = model.Customer.Name,
                Email = model.Customer.Email,
                CustomerType = CustomerType.Regular
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(model);
            }

            await _customerRepository.Add(model.Customer);
            await _signInManager.SignOutAsync();

            await _userManager.AddToRoleAsync(user, "Customer");
            await LoginUser(user, model.Password);

            user.CustomerId = model.Customer.CustomerId;
            await _userManager.UpdateAsync(user);
            
            return Redirect(model.ReturnUrl);
        }
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(new ManageViewModel
            {
                Customer = await _customerRepository.GetById(user.CustomerId),
                IsAdmin = User.IsInRole("Admin")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(ManageViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.Customer.CustomerId = user.CustomerId;
            if (!String.IsNullOrEmpty(model.NewPassword))
            {
                await UpdatePassword(user, model.Password, model.NewPassword);
            }

            if (user.Email != model.Customer.Email)
            {
                user.Email = model.Customer.Email;
                await _userManager.UpdateAsync(user);
            }
            if (user.UserName != model.Customer.Name)
            {
                user.UserName = model.Customer.Name;
                await _userManager.UpdateAsync(user);
            }

            await _customerRepository.Update(model.Customer);

            return View(model);
        }

        private async Task<SignInResult> LoginUser(ApplicationUser user,string password)
        {
            return await _signInManager.PasswordSignInAsync(user, password, false,false);
        }

        private async Task UpdatePassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, oldPassword, false);
            if (!result.Succeeded)
                ModelState.AddModelError("", "Invalid Password");
            else
            {
                var passwordResult = await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
                if (passwordResult.Succeeded)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
                }
                else
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(nameof(ManageViewModel.NewPassword), error.Description);
                    }
                }
            }
            oldPassword = String.Empty;
            newPassword = String.Empty;
        }
    }
}