using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class FoodItem
    {
        public FoodItem()
        {
            OrderFoodItems = new HashSet<OrderFoodItem>();
            FoodItemProducts = new HashSet<FoodItemProduct>();
        }

        public int FoodItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public int FoodItemTypeId { get; set; }

        public virtual FoodItemType FoodItemType { get; set; }
        public virtual ICollection<OrderFoodItem> OrderFoodItems { get; set; }
        public virtual ICollection<FoodItemProduct> FoodItemProducts { get; set; }
    }
}
