using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Repository;

namespace Inlmämningsuppgift2.Areas.Admin.Services
{
    class FoodItemService : IFoodItemService
    {
        private readonly IRepository<FoodItem> _foodItemRepository;
        private readonly IRepository<FoodItemProduct> _foodItemProductRepository;

        public FoodItemService(
            IRepository<FoodItem> foodItemRepository,
            IRepository<FoodItemProduct> foodItemProductRepository)
        {
            _foodItemRepository = foodItemRepository;
            _foodItemProductRepository = foodItemProductRepository;
        }
        public async Task AddFoodItem(string name, string description, int price, int foodItemTypeId, IEnumerable<int> productsIds)
        {
            var foodItem = new FoodItem
            {
                Name = name,
                Description = description,
                Price = price,
                FoodItemTypeId = foodItemTypeId
            };
            await _foodItemRepository.Add(foodItem);

            foreach (var productsId in productsIds)
            {
                foodItem.FoodItemProducts.Add(new FoodItemProduct{
                    FoodItemId = foodItem.FoodItemId,
                    ProductId = productsId
                });
            }

            await _foodItemRepository.Update(foodItem);
        }

        public async Task UpdateFoodItem(FoodItem foodItem, int foodItemTypeId, List<int> productsIds)
        {
            var oldFoodItem =
                await _foodItemRepository.FirstOrDefault(i => i.FoodItemId == foodItem.FoodItemId, i => i.FoodItemProducts);

            var newProductsIds = productsIds.Where(p => !oldFoodItem.FoodItemProducts.Select(i => i.ProductId).Contains(p)).ToList();
            var foodItemProductToDelete = oldFoodItem.FoodItemProducts.Where(i => !productsIds.Contains(i.ProductId)).ToList();

            foreach (var foodItemProduct in foodItemProductToDelete)
            {
                await _foodItemProductRepository.Remove(foodItemProduct);
                oldFoodItem.FoodItemProducts.Remove(foodItemProduct);
            }

            foreach (var productsId in newProductsIds)
            {
                var newFoodItemProduct = new FoodItemProduct
                {
                    FoodItemId = foodItem.FoodItemId,
                    ProductId = productsId
                };
                await _foodItemProductRepository.Add(newFoodItemProduct);
                oldFoodItem.FoodItemProducts.Add(newFoodItemProduct);
            }

            oldFoodItem.Name = foodItem.Name;
            oldFoodItem.Description = foodItem.Description;
            oldFoodItem.Price = foodItem.Price;
            oldFoodItem.FoodItemTypeId = foodItemTypeId;

            await _foodItemRepository.Update(oldFoodItem);
        }

        public async Task DeleteFoodItem(int foodItemId)
        {
            var foodItem = await _foodItemRepository.FirstOrDefault(i=>i.FoodItemId== foodItemId,i=>i.FoodItemProducts);
            await _foodItemRepository.Remove(foodItem, foodItem.FoodItemProducts);
        }
    }
}