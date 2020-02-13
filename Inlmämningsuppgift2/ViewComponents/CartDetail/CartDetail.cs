using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace Inlmämningsuppgift2.ViewComponents.CartDetail
{
    public class CartDetail : ViewComponent
    {
        private readonly Cart _cart;

        public CartDetail(Cart cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
