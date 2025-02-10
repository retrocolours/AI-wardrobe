using AI_Wardrobe.Repositories;
using AIWardrobe.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIWardrobe.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepo _userRepo;

        public UserController (UserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult MyProfile()
        {
            var user = _userRepo.getMyProfile;

            return View(user);
        }
    }
}
