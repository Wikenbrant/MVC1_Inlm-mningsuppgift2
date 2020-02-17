using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Areas.Admin.Services
{
    public interface IOrderService
    {
        Task UpdateOrder(Order order, bool delivered);
        Task RemoveOrder(int id);
    }
}
