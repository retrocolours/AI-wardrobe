using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AI_Wardrobe.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            ProductVM bankAccountVM = new ProductVM();

            return View(bankAccountVM);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductVM productVM)
        {

            if (ModelState.IsValid)
            {
                //var returnMessage = _bankAccountRepo.Create(bankAccountVM, email);
                //return RedirectToAction("Index", new { message = returnMessage });
            }

            //bankAccountVM.AccountTypeOptions = _bankAccountRepo.GetAccountTypeOptions();

            return View(productVM);
        }
    }
}
