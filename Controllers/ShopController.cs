using AI_Wardrobe.Repositories;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Wardrobe.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopAllRepo _shopallRepo;

        public ShopController(ShopAllRepo shopallRepo)
        {
            _shopallRepo = shopallRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<ShopAllVM> products = _shopallRepo.GetAllProducts();
            return View("Index", products);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View();
        }
    }
}