using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;

namespace AI_Wardrobe.Repositories
{
    public class UserRepo(AiwardrobeContext aiwardrobeContext, ApplicationDbContext appContext)
    {

        private readonly AiwardrobeContext _aiWardrobeContext = aiwardrobeContext;
        private readonly ApplicationDbContext _appContext = appContext;

        public void AddUser(RegisteredUser user)
        {
            _aiWardrobeContext.RegisteredUsers.Add(user);
            _aiWardrobeContext.SaveChanges();

        }

      public int? GetUserId(String email)
{
    var registerdUser = _aiWardrobeContext.RegisteredUsers.Where((user) => user.Email == email).FirstOrDefault();
    Console.Write($"registerdUser: {email}, id: {registerdUser?.Userid}");
    return registerdUser?.Userid;
}

    }
}
