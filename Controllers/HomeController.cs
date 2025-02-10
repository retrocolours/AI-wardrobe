

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.Models;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.ViewModels;

namespace AI_Wardrobe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TransactionRepo _transactionRepo;


        public HomeController(ILogger<HomeController> logger, TransactionRepo transactionRepo)
        {
            _logger = logger;
            _transactionRepo = transactionRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewList()
        {
            return View();
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

      
   public IActionResult PayPalConfirmation(string TransactionId, string Amount, string PayerName, string CreateTime, string Email)
        {
            if (string.IsNullOrEmpty(TransactionId))
            {
                return BadRequest("Invalid transaction.");
            }

            var transaction = new TransactionVM
            {
                TransactionId = TransactionId,
                PayerName = PayerName,
                PayerEmail = Email,
                Amount = decimal.TryParse(Amount, out var amt) ? amt : null,
                CreateTime = DateTime.TryParse(CreateTime, out var date) ? date : DateTime.UtcNow,
                PaymentMethod = "PayPal",
            };

            bool success = _transactionRepo.AddTransaction(transaction);

            if (!success)
            {
                return StatusCode(500, "Failed to save transaction.");
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