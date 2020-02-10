using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.Requests;
using Inlmämningsuppgift2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inlmämningsuppgift2.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()=> PartialView("_Cart", await _cartService.GetCart());

        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemRequest request) =>
            PartialView("_Cart", await _cartService.AddItemToCart(request.FoodItem, request.Quantity));

        [HttpPost]
        public async Task<IActionResult> DeleteItem(FoodItem item) =>
            PartialView("_Cart", await _cartService.DeleteItemFromCart(item));

        [HttpPost]
        public async Task<IActionResult> Clear() =>
            PartialView("_Cart", await _cartService.ClearCart());
    }
}