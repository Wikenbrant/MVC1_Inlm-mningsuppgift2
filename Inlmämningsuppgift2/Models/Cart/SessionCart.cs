using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Infrastructure;
using Inlmämningsuppgift2.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Inlmämningsuppgift2.Models.Cart
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var cart = session?.GetJson<SessionCart>("Cart")
                               ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override Task<Cart> AddItem(int quantity, FoodItem foodItem)
        {
            base.AddItem(quantity, foodItem);
            Session.SetJson("Cart", this);
            return Task.FromResult((Cart)this);
        }

        public override Task<Cart> RemoveLine(FoodItem foodItem)
        {
            base.RemoveLine(foodItem);
            Session.SetJson("Cart", this);
            return Task.FromResult((Cart)this);
        }

        public override Task<Cart> Clear()
        {
            base.Clear();
            Session.Remove("Cart");
            return Task.FromResult((Cart)this);
        }
    }
}
