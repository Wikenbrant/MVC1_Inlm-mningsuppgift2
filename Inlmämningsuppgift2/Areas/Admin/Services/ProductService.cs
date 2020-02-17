using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Repository;

namespace Inlmämningsuppgift2.Areas.Admin.Services
{
    class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<FoodItem> _foodItemRepository;

        public ProductService(IRepository<Product> productRepository, IRepository<FoodItem> foodItemRepository)
        {
            _productRepository = productRepository;
            _foodItemRepository = foodItemRepository;
        }

        public async Task AddProduct(Product product)
        {
            await _productRepository.Add(product);
        }

        public async IAsyncEnumerable<string> DeleteFoodItem(int productId)
        {
            var errorCount = 0;
            var product = await _productRepository.FirstOrDefault(m => m.ProductId == productId, p => p.FoodItemProduct);
            if (product.FoodItemProduct.Count > 0)
            {
                foreach (var foodItemProduct in product.FoodItemProduct)
                {
                    var foodItem = await _foodItemRepository.GetById(foodItemProduct.FoodItemId);
                    errorCount++;
                    yield return $"{foodItem.Name} is using this product";
                }
            }
            if (errorCount==0)
            {
                await _productRepository.Remove(product);
            }
        }
    }
}