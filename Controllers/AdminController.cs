using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace AI_Wardrobe.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductRepo _productRepo;
        private readonly OrderRepo _orderRepo;
        private readonly UserRepo _userRepo;
        private readonly TransactionRepo _transactionRepo;
        private readonly UserRoleRepo _userRoleRepo;

        public AdminController(ProductRepo productRepo, OrderRepo orderRepo,
            UserRepo userRepo, TransactionRepo transactionRepo, UserRoleRepo userRoleRepo)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _transactionRepo = transactionRepo;
            _userRoleRepo = userRoleRepo;
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
                var item = new Item
                {
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

        public IActionResult ViewUpdateOrder(int orderId)
        {
            var orderVm = _orderRepo.GetOrderVM(orderId);
            if (orderVm != null)
            {
                orderVm.TransactionVM = _transactionRepo.GetTransactionVm(orderId);
                orderVm.StatusOptions = _orderRepo.GetStatusOptions();

                return View(orderVm);
            }
            else
            {
                return RedirectToAction("Index", new { message = $"Unable to find order id: {orderId}" });
            }
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(OrderVM orderVM)
        {
            if (_orderRepo.UpdateOrderStatus(orderVM.Id, orderVM.Status))
            {

            }
            return RedirectToAction("ViewUpdateOrder", new { orderId = orderVM.Id });
        }


        public async Task<IActionResult> ViewAdmins()
        {
            var adminList = await _userRoleRepo.GetAdminUsers();
            return View(adminList);
        }

        public async Task<IActionResult> AddAdmin(String email)
        {
            await _userRoleRepo.AddAsAdmin(email);
            return RedirectToAction("ViewAdmins");
        }

        public async Task<IActionResult> RemoveAdmin(String email)
        {
            var adminList = await _userRoleRepo.GetAdminUsers();
            //prevent removal of the last admin fo fail safety
            if(adminList.Count() > 1)
            {
                await _userRoleRepo.RemoveFromAdmin(email);
            }
            return RedirectToAction("ViewAdmins");
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                try
                {
                    var fileName = ConvertToHtmlSafeString(Path.GetFileName(image.FileName));
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/products", fileName);

                    var directoryPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var src = Path.Combine("/images/products", fileName);

                    return Json(new { success = true, message = "Image uploaded successfully!", filePath = src });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error uploading image: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "No image selected!" });
        }

        public ActionResult GetFiles()
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/products");

            var files = Directory.GetFiles(directoryPath);

            var fileNames = files.Select(f => Path.GetFileName(f)).ToList();

            return PartialView("_ImageList", fileNames);
        }

        private void FillProductVMOptions(ProductVM vm)
        {
            vm.GenderOptions = _productRepo.GetGenderOptions();
            vm.SizeOptions = _productRepo.GetSizeOptions();
            vm.TypeOptions = _productRepo.GetTypeOptions();
        }

        private string ConvertToHtmlSafeString(string input)
        {
            string safeString = input.Replace(" ", "_");
            safeString = Regex.Replace(safeString, @"[^a-zA-Z0-9\-_\.]", string.Empty);


            return safeString;
        }

    }
}