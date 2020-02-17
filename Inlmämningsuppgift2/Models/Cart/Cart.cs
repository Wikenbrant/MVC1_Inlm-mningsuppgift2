using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;
using Inlmämningsuppgift2.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;

namespace Inlmämningsuppgift2.Models.Cart
{
    public class Cart
    {
        public Cart()
        {
            CartLines = new List<CartLine>();
        }
        public List< CartLine> CartLines { get; set; }

        public virtual void SetLine(FoodItem item,int quantity=1)
        {
            if (quantity < 0) quantity = 0;
            var line = CartLines.FirstOrDefault(l => l.FoodItem.FoodItemId == item.FoodItemId);
            if (line == null)
                CartLines.Add(new CartLine
                {
                    FoodItem = item, 
                    Quantity = quantity
                });
            else
                line.Quantity = quantity;
        }

        public virtual void Clear()

        {
            CartLines.Clear();
        }

        public int CheckOut(ApplicationUser user)
        {
            if (user.CustomerType == CustomerType.Premium) 
                user.BonusPoints += (CartLines.Sum(i => i.Quantity) * 10) - (NumberOfFreePizzas(user) * 100);

            return Total(user);
        }

        public int Total(ApplicationUser user)
        {
            return Sum-Discount(user);
        }
        public int Discount(ApplicationUser user)
        {
            if (user.CustomerType != CustomerType.Premium) return 0;
            if (CartLines.Sum(i => i.Quantity) < 3) return 0;
            var numberOfFreePizzas = NumberOfFreePizzas(user);
            var discount = 0;
            foreach (var line in CartLines)
            {
                if (line.Quantity >= numberOfFreePizzas)
                {
                    if (numberOfFreePizzas==0)
                    {
                        discount += Convert.ToInt32(line.FoodItem.Price * line.Quantity * 0.2m);
                    }
                    else
                    {
                        discount += line.FoodItem.Price * numberOfFreePizzas;
                        numberOfFreePizzas = 0;
                    }
                }
                else
                {
                    discount += line.FoodItem.Price * line.Quantity;
                    numberOfFreePizzas -= line.Quantity;
                    if (numberOfFreePizzas <= 0) numberOfFreePizzas = 0;
                }
            }
            return discount;
        }

        public int Sum => CartLines.Sum(l => l.FoodItem.Price * l.Quantity);

        public int NumberOfFreePizzas(ApplicationUser user)
        {
           return user.BonusPoints >= 100 ? Convert.ToInt32(Math.Floor(user.BonusPoints / 100m)) : 0;
        }
    }
}
