using System;
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
            CartLines = new List<CartLine>();
        }
        public List<CartLine> CartLines { get; set; }

        public virtual void AddItem(int quantity, FoodItem foodItem)
        {
            var line = CartLines.FirstOrDefault(l => l.FoodItem.FoodItemId == foodItem.FoodItemId);
            if (line == null)
                CartLines.Add(new CartLine {FoodItem = foodItem, Quantity = quantity});
            else
                line.Quantity += quantity;
        }

        public virtual void RemoveLine(FoodItem foodItem)
        {
            CartLines.RemoveAll(i => i.FoodItem.FoodItemId == foodItem.FoodItemId);
        }

        public virtual void Clear()
        {
            CartLines.Clear();
        }

        public int CalcTotal(ApplicationUser user)
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
            return sum;
        }

        private int Sum => CartLines.Sum(l => l.FoodItem.Price * l.Quantity);
    }
}
