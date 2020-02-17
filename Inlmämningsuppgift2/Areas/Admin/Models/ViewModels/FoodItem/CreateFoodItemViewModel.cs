using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inlmämningsuppgift2.Areas.Admin.Models.ViewModels.FoodItem
{
    public class CreateFoodItemViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Products")]
        public int[] SelectedProductsIds { get; set; }

        public MultiSelectList AvailableProducts { get; set; }

        [Required]
        [Display(Name = "FoodType")]
        public int SelectedFoodItemTypeId { get; set; }

        public SelectList AvailableFoodItemType { get; set; }
    }
}
