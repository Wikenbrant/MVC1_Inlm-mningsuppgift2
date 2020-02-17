using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Areas.Admin.Services
{
    public interface IProductService
    {
        Task AddProduct(Product product);
        IAsyncEnumerable<string> DeleteFoodItem(int productId);
    }
}
