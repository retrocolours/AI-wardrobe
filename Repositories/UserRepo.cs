using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;

namespace AI_Wardrobe.Repositories
{
    public class UserRepo
    {

        private readonly AiwardrobeContext _aiWardrobeContext;
        private readonly ApplicationDbContext _appContext;

        public UserRepo(AiwardrobeContext aiwardrobeContext, ApplicationDbContext appContext)
        {
            _aiWardrobeContext = aiwardrobeContext;
            _appContext = appContext;

            var appUser = appContext.Users;
            var aiUser = aiwardrobeContext.Users;
        }

        //public IEnumerable<User> GetAllUsers()
        //{
        //    var users = _context.Users;

        //    return users;
        //}
    }
}
