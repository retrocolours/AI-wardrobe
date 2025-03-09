using AI_Wardrobe.Models;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIWardrobe.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepo _userRepo;

        public UserController(UserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult MyProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {

                var userVm = _userRepo.GetUserVM(email);
                return View(userVm);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public IActionResult UpdateProfile(UserVm userVm)
        {
            bool isSuccess = true;
            //extract existing registered user vial signed email to prevent form hacks
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                var registeredUser = _userRepo.GetUserFromEmail(email);
                if (registeredUser != null)
                {
                    registeredUser.Firstname = userVm.FirstName;
                    registeredUser.Lastname = userVm.LastName;
                    registeredUser.Phone = userVm.Phone;

                    isSuccess = isSuccess && _userRepo.UpdateUser(registeredUser);

                    var address = new Address
                    {
                        Address1 = userVm.Address1,
                        Address2 = userVm.Address2,
                        City = userVm.City,
                        Province = userVm.Province,
                        Postalcode = userVm.PostalCode,
                        Fkuserid = registeredUser.Userid
                    };

                    isSuccess = isSuccess && _userRepo.UpdateAddress(registeredUser.Userid, address);
                }
            }
            else
            {
                isSuccess = false;
            }

            if (isSuccess)
            {
                return Redirect("MyProfile");
            }
            else
            {
                return Redirect("MyProfile");
            }
        }
    }
}
