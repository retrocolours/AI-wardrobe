using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.Models;
using System.Text.RegularExpressions;

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

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            // Ensure a file is uploaded
            if (image != null && image.Length > 0)
            {
                try
                {
                    // Get file name and path
                    var fileName = ConvertToHtmlSafeString(Path.GetFileName(image.FileName));
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/products", fileName);

                    // Ensure the directory exists
                    var directoryPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Save the file asynchronously to the server
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var src = Path.Combine("/images/products", fileName);
                    // Return a success message as JSON
                    return Json(new { success = true, message = "Image uploaded successfully!", filePath = src });
                }
                catch (Exception ex)
                {
                    // Return error message as JSON
                    return Json(new { success = false, message = "Error uploading image: " + ex.Message });
                }
            }

            // Return an error message if no file was uploaded
            return Json(new { success = false, message = "No image selected!" });
        }

        public ActionResult GetFiles()
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/products");

            // Get all files in the directory
            var files = Directory.GetFiles(directoryPath);

            // Return just the filenames (or you can adjust as needed)
            var fileNames = files.Select(f => Path.GetFileName(f)).ToList();

            return PartialView("_ImageList", fileNames);  // Return the partial view with the files
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