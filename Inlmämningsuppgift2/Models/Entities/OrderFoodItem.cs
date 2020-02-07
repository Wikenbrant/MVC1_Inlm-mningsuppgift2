namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class OrderFoodItem
    {
        public int FoodItemId { get; set; }
        public int OrderId { get; set; }
        public int Antal { get; set; }

        public virtual Order Order { get; set; }
        public virtual FoodItem FoodItem { get; set; }
    }
}
