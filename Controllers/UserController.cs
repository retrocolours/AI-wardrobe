using AIWardrobe.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIWardrobe.Controllers
{
    public class UserController : Controller
    {
        public IActionResult MyProfile()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "555 Seymour St",
                Apartment = "",
                City = "Vancouver",
                Province = "BC",
                PostalCode = "V6B 3H6",
                Phone = "",
                Email = "email@janesfakedomain.net"
            };

            return Json(user);
        }
    }
}
