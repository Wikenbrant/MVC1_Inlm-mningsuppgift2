using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Repository;
using Inlmämningsuppgift2.Services;
using Inlmämningsuppgift2.Services.Cart;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inlmämningsuppgift2.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService,UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddItem(int foodItemId, int quantity)
        {
            await _cartService.SetItemInCart(foodItemId, quantity);
            return ViewComponent("CartDetail");
        }
        [HttpPost]
        public async Task<IActionResult> AddOneItem(int foodItemId)
        {
            await _cartService.AddOneItemInCart(foodItemId);
            return ViewComponent("CartDetail");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int foodItemId)
        {
            await _cartService.SetItemInCart(foodItemId, 0);
            return ViewComponent("CartDetail");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOneItem(int foodItemId)
        {
            await _cartService.DeleteOneItemInCart(foodItemId);
            return ViewComponent("CartDetail");
        }
        [HttpPost]
        public async Task<IActionResult> CheckOut()
        {
            return View(await _cartService.CheckOut(await _userManager.GetUserAsync(User)));
        }
    }
}