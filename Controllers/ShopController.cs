using AI_Wardrobe.Repositories;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Wardrobe.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductRepo _productRepo;

        public ShopController(ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductVM> products = _productRepo.GetAll();
            return View("Index", products);
        }

        public IActionResult About()
        {
            return View();
        }

        //public IActionResult Cart()
        //{
        //    return View();
        //}

        public IActionResult ProductDetail()
        {
            return View();
        }

        //public IActionResult ProductList()
        //{
        //    return View();
        //}
    }
}
