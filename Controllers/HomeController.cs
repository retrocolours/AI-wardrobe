using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace AI_Wardrobe.Controllers;

public class HomeController : Controller
{


    private readonly ILogger<HomeController> _logger;
    private readonly AiwardrobeContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, AiwardrobeContext context, SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        // Get featured products to display on the home page
        var featuredProducts = _context.Items
            .OrderBy(i => Guid.NewGuid()) // Random ordering
            .Take(4)
            .ToList();

        return View(featuredProducts);
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
        ViewBag.TransactionId = TransactionId;
        ViewBag.Amount = Amount;
        ViewBag.PayerName = PayerName;
        ViewBag.CreateTime = CreateTime;
        ViewBag.Email = Email;
        return View();
    }
}
