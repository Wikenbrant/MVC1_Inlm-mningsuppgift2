using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inlmämningsuppgift2.Data;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Repository;

namespace Inlmämningsuppgift2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IOrderService _orderService;

        public OrdersController(IRepository<Order> orderRepository, IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            return View(await _orderRepository.GetAll(o => o.Customer));
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.FirstOrDefault(m => m.OrderId == id, o => o.Customer,o=>o.OrderFoodItems);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,bool delivered)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var order = await _orderRepository.FirstOrDefault(m => m.OrderId == id, o => o.Customer, o => o.OrderFoodItems);
            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateOrder(order, delivered);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.FirstOrDefault(m => m.OrderId == id, o => o.Customer);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.RemoveOrder(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderExists(int id)
        {
            return await _orderRepository.Any(e => e.OrderId == id);
        }
    }
}
