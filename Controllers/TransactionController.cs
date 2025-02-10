using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.Data; 
using Microsoft.EntityFrameworkCore; 
using AI_Wardrobe.Models;
namespace AI_Wardrobe.Controllers;
using AI_Wardrobe.Repositories;


    [Authorize(Roles = "Manager")]
    public class TransactionController : Controller
    {
        private TransactionRepo _context;

        public TransactionController(TransactionRepo context)
{
    _context = context;
}

public IActionResult Index()
{


    var transactions = _context.GetAllTransactions();
    return View(transactions);
}
        }


