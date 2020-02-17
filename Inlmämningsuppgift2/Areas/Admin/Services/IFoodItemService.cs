using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Areas.Admin.Services
{
    public interface IFoodItemService
    {
        Task AddFoodItem(string name, string description, int price, int foodItemTypeId, IEnumerable<int> productsIds);
        Task UpdateFoodItem(FoodItem foodItem, int foodItemTypeId, List<int> productsIds);
        Task DeleteFoodItem(int foodItemId);
    }
}
