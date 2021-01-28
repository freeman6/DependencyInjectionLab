using Lab09.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab09.Services
{
    public class ShoppingCartResolver: IShoppingCartRepository
    {
        private readonly Func<string, IShoppingCart> shoppingCart;
        private readonly IConfiguration config;
        public ShoppingCartResolver(Func<string, IShoppingCart> shoppingCart, IConfiguration config)
        {
            this.shoppingCart = shoppingCart;
            this.config = config;
        }

        public object GetCart() => shoppingCart(config["CartSource"]).GetCart();
    }
}
