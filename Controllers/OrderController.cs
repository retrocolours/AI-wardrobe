using Microsoft.AspNetCore.Mvc;

namespace AI_Wardrobe.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}
