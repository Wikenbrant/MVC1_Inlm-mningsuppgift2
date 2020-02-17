using System;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Models.ViewModels;
using Inlmämningsuppgift2.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inlmämningsuppgift2.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IRepository<Customer> _customerRepository;

        public AccountService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IPasswordValidator<ApplicationUser> passwordValidator,
            IRepository<Customer> customerRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _customerRepository = customerRepository;
        }

        public async Task<SignInResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _signInManager.SignOutAsync();
            return await LoginUser(user, password);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task Register(Customer customer, string password, ModelStateDictionary modelState)
        {
            var user = await _userManager.FindByEmailAsync(customer.Email);

            if (user != null)
            {
                modelState.AddModelError("", "User already exists");
                return;
            }
            user = new ApplicationUser
            {
                UserName = customer.Name,
                Email = customer.Email,
                CustomerType = CustomerType.Regular
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError("", error.Description);
                }
                return;
            }

            await _customerRepository.Add(customer);
            await _signInManager.SignOutAsync();

            await _userManager.AddToRoleAsync(user, "Customer");
            await LoginUser(user, password);

            user.CustomerId = customer.CustomerId;
            await _userManager.UpdateAsync(user);
        }

        public Task Manage()
        {
            throw new NotImplementedException();
        }

        private async Task<SignInResult> LoginUser(ApplicationUser user, string password)
        {
            return await _signInManager.PasswordSignInAsync(user, password, false, false);
        }

        public async Task UpdateUserPassword(ApplicationUser user, ModelStateDictionary modelState, string oldPassword, string newPassword)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, oldPassword, false);
            if (!result.Succeeded)
                modelState.AddModelError("", "Invalid Password");
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
                        modelState.AddModelError(nameof(ManageViewModel.NewPassword), error.Description);
                    }
                }
            }
            oldPassword = String.Empty;
            newPassword = String.Empty;
        }

        public async Task UpdateUserEmail(ApplicationUser user, string newEmail)
        {
            user.Email = newEmail;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateUserName(ApplicationUser user, string newUserName)
        {
            user.UserName = newUserName;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            await _customerRepository.Update(customer);
        }
    }
}
