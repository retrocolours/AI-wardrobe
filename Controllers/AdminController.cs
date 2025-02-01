using Microsoft.AspNetCore.Mvc;

namespace AI_Wardrobe.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
