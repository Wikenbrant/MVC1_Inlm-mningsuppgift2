using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Areas.Admin.Models.ViewModels;
using Inlmämningsuppgift2.Areas.Admin.Models.ViewModels.FoodItem;
using Inlmämningsuppgift2.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inlmämningsuppgift2.Data;
using Inlmämningsuppgift2.Models.Entities;
using Inlmämningsuppgift2.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Inlmämningsuppgift2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FoodItemsController : Controller
    {
        private readonly IRepository<FoodItem> _foodItemRepository;
        private readonly IRepository<FoodItemType> _foodItemTypeRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IFoodItemService _foodItemService;

        public FoodItemsController(
            IRepository<FoodItem> foodItemRepository,
            IRepository<FoodItemType> foodItemTypeRepository, 
            IRepository<Product> productRepository,
            IFoodItemService foodItemService)
        {
            _foodItemRepository = foodItemRepository;
            _foodItemTypeRepository = foodItemTypeRepository;
            _productRepository = productRepository;
            _foodItemService = foodItemService;
        }

        // GET: Admin/Pizza
        public async Task<IActionResult> Index()
        {
            return View(await _foodItemRepository.GetAll(f => f.FoodItemType));
        }

        // GET: Admin/Pizza/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var foodItem = await _foodItemRepository.FirstOrDefault(m => m.FoodItemId == id, f => f.FoodItemType,p=>p.FoodItemProducts);
            if (foodItem == null)
            {
                return NotFound();
            }

            var x = await _productRepository.GetWhere(p =>
                foodItem.FoodItemProducts.Select(i => i.ProductId).Contains(p.ProductId));

            return View(new DetailFoodItemViewModel
            {
                FoodItem = foodItem,
                Products = new SelectList(
                    await _productRepository.GetWhere(p => foodItem.FoodItemProducts.Select(i => i.ProductId).Contains(p.ProductId)),
                    nameof(Product.ProductId),
                    nameof(Product.ProductNamn))
            });
        }

        // GET: Admin/Pizza/Create
        public async Task<IActionResult> Create()
        {
            return View(new CreateFoodItemViewModel
            {
                AvailableFoodItemType = new SelectList(
                    await _foodItemTypeRepository.GetAll(),
                    nameof(FoodItemType.FoodItemTypeId),
                    nameof(FoodItemType.Description)),

                AvailableProducts = new MultiSelectList(
                await _productRepository.GetAll(),
                nameof(Product.ProductId),
                nameof(Product.ProductNamn))
        });
        }

        // POST: Admin/Pizza/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFoodItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _foodItemService.AddFoodItem(model.Name,model.Description,model.Price,model.SelectedFoodItemTypeId,model.SelectedProductsIds);
                return RedirectToAction(nameof(Index));
            }
            model.AvailableFoodItemType = new SelectList(
                await _foodItemTypeRepository.GetAll(), 
                nameof(FoodItemType.FoodItemTypeId), 
                nameof(FoodItemType.Description), 
                model.SelectedFoodItemTypeId);
            model.AvailableProducts = new MultiSelectList(
                await _productRepository.GetAll(),
                nameof(Product.ProductId),
                nameof(Product.ProductNamn),
                model.SelectedProductsIds);

            return View(model);
        }

        // GET: Admin/Pizza/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var foodItem = await _foodItemRepository.FirstOrDefault(i=>i.FoodItemId==id,i=>i.FoodItemType,i=>i.FoodItemProducts);
            if (foodItem == null)
            {
                return NotFound();
            }

            var x = new SelectList(
                await _foodItemTypeRepository.GetAll(),
                nameof(FoodItemType.FoodItemTypeId),
                nameof(FoodItemType.Description),
                foodItem.FoodItemTypeId);

            return View(new EditFoodItemViewModel
            {
                FoodItem = foodItem,

                AvailableFoodItemType = new SelectList(
                    await _foodItemTypeRepository.GetAll(), 
                    nameof(FoodItemType.FoodItemTypeId), 
                    nameof(FoodItemType.Description), 
                    foodItem.FoodItemTypeId),

                AvailableProducts = new MultiSelectList(
                    await _productRepository.GetAll(), 
                    nameof(Product.ProductId), 
                    nameof(Product.ProductNamn), 
                    foodItem.FoodItemProducts.Select(p => p.ProductId))
            });
        }

        // POST: Admin/Pizza/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditFoodItemViewModel model)
        {
            if (id != model.FoodItem.FoodItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _foodItemService.UpdateFoodItem(model.FoodItem,model.SelectedFoodItemTypeId,model.SelectedProductsIds.ToList());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FoodItemExists(id))
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
            model.AvailableFoodItemType = new SelectList(
                await _foodItemTypeRepository.GetAll(),
                nameof(FoodItemType.FoodItemTypeId),
                nameof(FoodItemType.Description), 
                model.FoodItem.FoodItemTypeId);

            model.AvailableProducts = new MultiSelectList(
                await _productRepository.GetAll(),
                nameof(Product.ProductId), nameof(Product.ProductNamn),
                model.FoodItem.FoodItemProducts.Select(p => p.ProductId));
            return View(model);
        }

        // GET: Admin/Pizza/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await _foodItemRepository.FirstOrDefault(m => m.FoodItemId == id, f => f.FoodItemType);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        // POST: Admin/Pizza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodItemService.DeleteFoodItem(id);
            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> FoodItemExists(int id)
        {
            return await _foodItemRepository.Any(e => e.FoodItemId == id);
        }
    }
}
