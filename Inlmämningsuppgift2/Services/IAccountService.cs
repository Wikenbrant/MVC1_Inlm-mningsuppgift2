using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inlmämningsuppgift2.Services
{
    public interface IAccountService
    {
        Task<SignInResult> Login(string email, string password);
        Task LogOut();
        Task Register(Customer customer, string password, ModelStateDictionary modelState);
        Task UpdateUserPassword(ApplicationUser user, ModelStateDictionary modelState, string oldPassword, string newPassword);
        Task UpdateUserEmail(ApplicationUser user, string newEmail);
        Task UpdateUserName(ApplicationUser user, string newUserName);
        Task UpdateCustomer(Customer customer);
    }
}
