using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inlmämningsuppgift2.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View(nameof(Users));
        }

        public async Task<IActionResult> Users()
        {
            return View(await _userManager.Users.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(roleName))
            {
                await _userManager.AddToRoleAsync(user,roleName);
            }
            return Index();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(roleName) && await _userManager.IsInRoleAsync(user,roleName))
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
            return Index();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCustomerType(string userId, CustomerType customerType)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.CustomerType = customerType;
                await _userManager.UpdateAsync(user);
            }
            return Index();
        }
    }
}