using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.Models;

namespace AI_Wardrobe.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductRepo _productRepo;

        public AdminController(ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View(_productRepo.GetAll());
        }

        public IActionResult CreateProduct()
        {
            ProductVM productVM = new ProductVM();

            productVM.GenderOptions = _productRepo.GetGenderOptions();
            productVM.SizeOptions = _productRepo.GetSizeOptions();
            productVM.TypeOptions = _productRepo.GetTypeOptions();

            return View(productVM);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductVM productVM)
        {

            if (ModelState.IsValid)
            {
                var item = new Item {
                                    Itemprice = productVM.Price,
                                    Itemdescription = productVM.Description,
                                    Imageurl = productVM.ImageUrl,
                                    Fkitemgenderid = productVM.GenderId,
                                    Fksizeid = productVM.SizeId,
                                    Fktypeid = productVM.TypeId
                                };

                var returnMessage = _productRepo.AddItem(item);
                return RedirectToAction("Index", new { message = returnMessage });
            }

            productVM.GenderOptions = _productRepo.GetGenderOptions();
            productVM.SizeOptions = _productRepo.GetSizeOptions();
            productVM.TypeOptions = _productRepo.GetTypeOptions();

            return View(productVM);
        }

        public IActionResult ManageProduct(int id)
        {
            var item = _productRepo.GetItem(id);

            if (item != null)
            {
                var vm = new ProductVM
                {
                    Id = item.Itemid,
                    Description = item.Itemdescription,
                    Price = item.Itemprice,
                    ImageUrl = item.Imageurl,
                    GenderId = item.Fkitemgenderid,
                    SizeId = item.Fksizeid,
                    TypeId = item.Fktypeid
                };

                FillProductVMOptions(vm);

                return View(vm);
            }
            else
            {
                return RedirectToAction("Index", new { message = $"Unable to manage product id: {id}" });
            }
        
        }

        [HttpPost]
        public IActionResult ManageProduct(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var item = new Item
                {
                    Itemid = productVM.Id,
                    Itemprice = productVM.Price,
                    Itemdescription = productVM.Description,
                    Imageurl = productVM.ImageUrl,
                    Fkitemgenderid = productVM.GenderId,
                    Fksizeid = productVM.SizeId,
                    Fktypeid = productVM.TypeId
                };

                var returnMessage = _productRepo.UpdateItem(item);
                return RedirectToAction("ProductList", new { message = returnMessage });
            }

            FillProductVMOptions(productVM);

            return View(productVM);

        }

        public IActionResult ViewUpdateOrder()
        {
            return View();
        }

        private void FillProductVMOptions(ProductVM vm)
        {
            vm.GenderOptions = _productRepo.GetGenderOptions();
            vm.SizeOptions = _productRepo.GetSizeOptions();
            vm.TypeOptions = _productRepo.GetTypeOptions();
        }
    }
}