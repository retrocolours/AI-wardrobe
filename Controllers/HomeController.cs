using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.Models;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.ViewModels;
using System.Security.Claims;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Identity;

namespace AI_Wardrobe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TransactionRepo _transactionRepo;
        private readonly OrderRepo _orderRepo;
        private readonly UserRepo _userRepo;
        private readonly ProductRepo _productRepo;
        private readonly CookieRepository _cookieRepo;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            TransactionRepo transactionRepo,
            OrderRepo orderRepo,
            UserRepo userRepo,
            ProductRepo productRepo,
            CookieRepository cookieRepo,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _transactionRepo = transactionRepo;
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _productRepo = productRepo;
            _cookieRepo = cookieRepo;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var featuredProducts = _productRepo.GetFeaturedProduct();
            return View(featuredProducts);
        }

        public IActionResult Setup()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult PayPalConfirmation(
            string TransactionId,
            string Amount,
            string Currency,
            string PayerName,
            string CreateTime,
            string Email)
        {
            if (string.IsNullOrEmpty(TransactionId))
            {
                return BadRequest("Invalid transaction.");
            }

            var transactionVm = new TransactionVM
            {
                TransactionId = TransactionId,
                Currency = Currency,
                PayerName = PayerName,
                PayerEmail = Email,
                Amount = decimal.TryParse(Amount, out var amt) ? amt : (decimal?)null, // Fixed nullability issue
                CreateTime = DateTime.UtcNow, // Ensure CreateTime matches TransactionVM type
                PaymentMethod = "PayPal",
            };

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("User email not found.");
            }

            var userId = _userRepo.GetUserId(email);
            if (userId.HasValue)
            {
                bool success = _transactionRepo.AddTransaction(transactionVm, userId.Value, email);
                if (!success)
                {
                    return StatusCode(500, "Failed to save transaction.");
                }

            }
            else
            {
                return StatusCode(500, "User ID not found.");
            }

            ViewBag.TransactionId = TransactionId;
            ViewBag.Amount = Amount;
            ViewBag.PayerName = PayerName;
            ViewBag.CreateTime = CreateTime;
            ViewBag.Email = Email;

            return View();
        }
    }
}
