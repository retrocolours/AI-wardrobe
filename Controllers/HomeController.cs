using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.Models;

namespace AI_Wardrobe.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

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
    ViewBag.TransactionId = TransactionId;
    ViewBag.Amount = Amount;
    ViewBag.PayerName = PayerName;
    ViewBag.CreateTime = CreateTime;
    ViewBag.Email = Email;
    return View();
}


}
