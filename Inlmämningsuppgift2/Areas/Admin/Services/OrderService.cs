using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Repository;

namespace Inlmämningsuppgift2.Areas.Admin.Services
{
    class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task UpdateOrder(Order order, bool delivered)
        {
            order.Delivered = delivered;
            await _orderRepository.Update(order);
        }

        public async Task RemoveOrder(int id)
        {
            var order = await _orderRepository.FirstOrDefault(m => m.OrderId == id,o=>o.OrderFoodItems);
            await _orderRepository.Remove(order,order.OrderFoodItems);
        }
    }
}