using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Models.Requests
{
    public class DeleteItemFromCartRequest
    {
        public FoodItem FoodItem { get; set; }
    }
}
