using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Models.User;

namespace Inlmämningsuppgift2.Services.Cart
{
    public interface ICartService
    {
        Task SetItemInCart(int foodItemId, int quantity);
        Task AddOneItemInCart(int foodItemId);
        Task DeleteOneItemInCart(int foodItemId);
        Task<Order> CheckOut(ApplicationUser user);
    }
}
