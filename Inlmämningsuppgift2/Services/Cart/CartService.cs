using System;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Repository;
using Microsoft.AspNetCore.Identity;

namespace Inlmämningsuppgift2.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly Models.Cart.Cart _cart;
        private readonly IRepository<FoodItem> _repository;
        private readonly IRepository<Order> _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartService(Models.Cart.Cart cart,IRepository<FoodItem> repository,IRepository<Order> orderRepository, UserManager<ApplicationUser> userManager)
        {
            _cart = cart;
            _repository = repository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task SetItemInCart(int foodItemId, int quantity)
        {
            var foodItem = await _repository.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (foodItem != null) _cart.SetLine(foodItem, quantity);
        }

        public async Task AddOneItemInCart(int foodItemId)
        {
            var foodItem = await _repository.FirstOrDefault(i => i.FoodItemId == foodItemId);
            var quantity = _cart.CartLines.FirstOrDefault(l => l.FoodItem.FoodItemId == foodItemId)?.Quantity;
            if (foodItem != null) _cart.SetLine(foodItem, quantity+1 ?? 1);
        }

        public async Task DeleteOneItemInCart(int foodItemId)
        {
            var foodItem = await _repository.FirstOrDefault(i => i.FoodItemId == foodItemId);
            var quantity = _cart.CartLines.FirstOrDefault(l => l.FoodItem.FoodItemId == foodItemId)?.Quantity;
            if (foodItem != null) _cart.SetLine(foodItem, quantity + 1 ?? 0);
        }

        public async Task<Order> CheckOut(ApplicationUser user)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = _cart.CheckOut(user),
                CustomerId = user.CustomerId,
            };
            await _orderRepository.Add(order);
            foreach (var line in _cart.CartLines)
            {
                order.OrderFoodItems.Add(new OrderFoodItem
                {
                    OrderId = order.OrderId,
                    FoodItemId = line.FoodItem.FoodItemId,
                    Antal = line.Quantity
                });
            }
            await _orderRepository.Update(order);
            await _userManager.UpdateAsync(user);
            _cart.Clear();
            return order;
        }
    }
}