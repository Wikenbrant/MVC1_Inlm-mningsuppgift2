using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Repository;

namespace Inlmämningsuppgift2.Services
{
    public class CartService : ICartService
    {
        private readonly Cart _cart;
        private readonly IRepository<FoodItem> _repository;

        public CartService(Cart cart,IRepository<FoodItem> repository)
        {
            _cart = cart;
            _repository = repository;
        }

        public async Task SetItemInCart(int foodItemId, int quantity)
        {
            var foodItem = await _repository.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (foodItem != null) _cart.SetLine(foodItem, quantity);
        }

        public Task ClearCart()
        {
             _cart.Clear();
            return Task.CompletedTask;
        }
    }
}