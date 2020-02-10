using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;

namespace Inlmämningsuppgift2.Models.Cart
{
    public class Cart
    {

        public Cart()
        {
            CartLines = new ConcurrentBag<CartLine>();
        }
        public ConcurrentBag<CartLine> CartLines { get; set; }

        public virtual Task<Cart> AddItem(int quantity, FoodItem foodItem)
        {
            var line = CartLines.FirstOrDefault(l => l.FoodItem.FoodItemId == foodItem.FoodItemId);
            if (line == null)
                CartLines.Add(new CartLine {FoodItem = foodItem, Quantity = quantity});
            else
                line.Quantity += quantity;
            return Task.FromResult(this);
        }

        public virtual Task<Cart> RemoveLine(FoodItem foodItem)
        {
            if (CartLines.Any(l=>l.FoodItem.FoodItemId==foodItem.FoodItemId))
            {
                CartLines.FirstOrDefault(i => i.FoodItem.FoodItemId == foodItem.FoodItemId).Quantity = 0;
            }
            return Task.FromResult(this);
        }

        public virtual Task<Cart> Clear()
        {
            CartLines.Clear();
            return Task.FromResult(this);
        }

        public Task<int> CalcTotal(ApplicationUser user)
        {
            var numberOfFreePizzas = 0;
            if (user.CustomerType != CustomerType.Premium) return Sum;
            if (user.BonusPoints >= 100) numberOfFreePizzas = Convert.ToInt32(Math.Floor(user.BonusPoints / 100m));
            user.BonusPoints += (CartLines.Sum(i => i.Quantity) * 10)-(numberOfFreePizzas*100);
            if (CartLines.Sum(i => i.Quantity) < 3 && numberOfFreePizzas == 0) return Sum;
            var sum = 0;
            foreach (var line in CartLines)
            {
                if (line.Quantity >= numberOfFreePizzas)
                {
                    sum += line.FoodItem.Price * (line.Quantity - numberOfFreePizzas);
                    numberOfFreePizzas = 0;
                }
                else
                {
                    sum += line.FoodItem.Price * (line.Quantity - numberOfFreePizzas);
                    numberOfFreePizzas -= line.Quantity;
                    if (numberOfFreePizzas < 0) numberOfFreePizzas = 0;
                }
            }
            return Task.FromResult(sum);
        }

        private Task<int> Sum => Task.FromResult(CartLines.Sum(l => l.FoodItem.Price * l.Quantity));
    }
}
