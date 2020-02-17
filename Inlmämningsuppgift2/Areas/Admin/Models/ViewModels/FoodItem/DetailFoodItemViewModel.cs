using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inlmämningsuppgift2.Areas.Admin.Models.ViewModels.FoodItem
{
    public class DetailFoodItemViewModel
    {
        public Inlmämningsuppgift2.Models.Entities.FoodItem FoodItem { get; set; }
        public SelectList Products { get; set; }
    }
}
