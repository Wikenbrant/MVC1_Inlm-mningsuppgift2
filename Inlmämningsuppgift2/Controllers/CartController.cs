using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.Requests;
using Inlmämningsuppgift2.Repository;
using Inlmämningsuppgift2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inlmämningsuppgift2.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IRepository<FoodItem> _repository;

        public CartController(ICartService cartService,IRepository<FoodItem> repository)
        {
            _cartService = cartService;
            _repository = repository;
        }
        [HttpPost]
        public async Task<IActionResult> AddItem(int foodItemId, int quantity)
        {
            await _cartService.SetItemInCart(foodItemId, quantity);
            return ViewComponent("CartDetail");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int foodItemId)
        {
            await _cartService.SetItemInCart(foodItemId, 0);
            return ViewComponent("CartDetail");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCart();
            return ViewComponent("CartDetail");
        }

    }
}