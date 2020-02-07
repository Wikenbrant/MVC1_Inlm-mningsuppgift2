using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Models.Cart
{
    public class CartLine
    {
        public int Quantity { get; set; }

        public FoodItem FoodItem { get; set; }
    }
}
