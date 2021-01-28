using Lab09.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab09.Services
{
    public class ShoppingCartAPI : IShoppingCart
    {
        public object GetCart()
        {
            return "WebAPI";
        }
    }
}
