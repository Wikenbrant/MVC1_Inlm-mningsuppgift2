using System;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Models.ViewModels;
using Inlmämningsuppgift2.Repository;
using Inlmämningsuppgift2.Services;
using Inlmämningsuppgift2.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Inlmämningsuppgift2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Customer> _customerRepository;

        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            IRepository<Customer> customerRepository)
        {
            _accountService = accountService;
            _userManager = userManager;
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
            var result = await _accountService.Login(model.Email, model.Password);

            if (result.Succeeded) return Redirect(model.ReturnUrl);

            ModelState.AddModelError("","Invalid user or password");
            return View(model);
        }
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _accountService.LogOut();
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

            await _accountService.Register(model.Customer, model.Password, ModelState);

            if (!ModelState.IsValid) return View(model);
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
                await _accountService.UpdateUserPassword(user, ModelState, model.Password, model.NewPassword);
            }

            if (user.Email != model.Customer.Email)
            {
                await _accountService.UpdateUserEmail(user, model.Customer.Email);
            }
            if (user.UserName != model.Customer.Name)
            {
                await _accountService.UpdateUserName(user, model.Customer.Name);
            }

            await _accountService.UpdateCustomer(model.Customer);

            return View(model);
        }
    }
}