using System.Collections.Generic;

namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class Product
    {
        public Product()
        {
            FoodItemProduct = new HashSet<FoodItemProduct>();
        }

        public int ProductId { get; set; }
        public string ProductNamn { get; set; }

        public virtual ICollection<FoodItemProduct> FoodItemProduct { get; set; }
    }
}
