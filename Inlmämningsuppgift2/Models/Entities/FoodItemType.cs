using System.Collections.Generic;

namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class FoodItemType
    {
        public FoodItemType()
        {
            FoodItems = new HashSet<FoodItem>();
        }

        public int FoodItemTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<FoodItem> FoodItems { get; set; }
    }
}
