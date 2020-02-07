using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Models.Cart
{
    public class Cart
    {
        public List<CartLine> CartLines { get; set; }

        public int Total => CartLines.Sum(l => l.FoodItem.Price * l.Quantity);

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
    }
}
