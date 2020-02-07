using System;
using System.Collections.Generic;

namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderFoodItems = new HashSet<OrderFoodItem>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public bool Delivered { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderFoodItem> OrderFoodItems { get; set; }
    }
}
