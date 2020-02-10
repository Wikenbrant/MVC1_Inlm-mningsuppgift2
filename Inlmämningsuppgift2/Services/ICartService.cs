using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Services
{
    public interface ICartService
    {
        Task<Cart> GetCart();
        Task<Cart> AddItemToCart(FoodItem item, int quantity);
        Task<Cart> DeleteItemFromCart(FoodItem foodItem);
        Task<Cart> ClearCart();
    }
}
