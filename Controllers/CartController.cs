


using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AI_Wardrobe.Controllers
{
    public class CartController : Controller
    {
        private static List<ShopAllVM> _cart = new List<ShopAllVM>();

        public IActionResult Index()
        {
            return View(_cart);
        }

        public IActionResult AddToCart(int id, string type, string description, decimal price, string image, string currency)
        {
            if (!_cart.Any(p => p.ID == id))
            {
                var product = new ShopAllVM
                {
                    ID = id,
                    Type = type,
                    Description = description,
                    Price = price,
                    Image = image,
                    Currency = currency
                };
                
                _cart.Add(product);
                TempData["CartMessage"] = "Product added to cart successfully!";
            }
            else
            {
                TempData["CartMessage"] = "Product is already in the cart.";
            }
            
            return RedirectToAction("Index", "Shop");
        }
    }
}
