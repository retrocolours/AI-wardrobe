using AI_Wardrobe.Repositories;

namespace AI_Wardrobe.Controllers
{
    public class UserController
    {

        private readonly UserRepo _userRepo;

        public UserController(UserRepo userRepo)
        {
            _userRepo = userRepo;
        }



    }
}
