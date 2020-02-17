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
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IProductService _productService;

        public ProductsController(IRepository<Product> productRepository, IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductNamn")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var product = await _productRepository.FirstOrDefault(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Product product)
        {

            await foreach (var error in _productService.DeleteFoodItem(product.ProductId))
            {
                ModelState.AddModelError("", error);
            }
            if (!ModelState.IsValid) return View(product);

            await _productRepository.Remove(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
