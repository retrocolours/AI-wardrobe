using AI_Wardrobe.Data;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PayPalDemo.Controllers
{
    public class ShopController(ApplicationDbContext context, ShopAllRepo shopallRepo) : Controller
    {
        private readonly ShopAllRepo _shopallRepo = shopallRepo;

        public IActionResult Index()
        {
            IEnumerable<ShopAllVM> products = _shopallRepo.GetAllProducts();

            return View("Index", products);
        }
    }
}
