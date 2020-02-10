using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Services
{
    public class CartService : ICartService
    {
        private readonly Cart _cart;

        public CartService(Cart cart)
        {
            _cart = cart;
        }

        public Task<Cart> GetCart()
        {
           return Task.FromResult(_cart);
        }

        public async Task<Cart> AddItemToCart(FoodItem item, int quantity)
        {
           return await _cart.AddItem(quantity, item);
        }

        public async Task<Cart> DeleteItemFromCart(FoodItem foodItem)
        {
            return await _cart.RemoveLine(foodItem);
        }

        public async Task<Cart> ClearCart()
        {
            return await _cart.Clear();
        }
    }
}