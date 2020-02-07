namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class FoodItemProduct
    {
        public int FoodItemId { get; set; }
        public int ProductId { get; set; }

        public virtual FoodItem FoodItem { get; set; }
        public virtual Product Product { get; set; }
    }
}
