using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using AIWardrobe.Models;

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

        public RegisteredUser? GetUserByID(int id)
        {
            return _aiWardrobeContext.RegisteredUsers.Where(usr => usr.Userid == id).FirstOrDefault();
        }

        public int? GetUserId(String email)
        {
            var registerdUser = _aiWardrobeContext.RegisteredUsers.Where((user) => user.Email == email).FirstOrDefault();
            Console.Write($"registerdUser: {email}, id: {registerdUser?.Userid}");
            return registerdUser?.Userid;
        }

        public string? GetFullName(int userId)
        {
            return _aiWardrobeContext.RegisteredUsers
                .Where(usr => usr.Userid == userId)
                .Select(usr => $"{usr.Firstname} {usr.Lastname}")
                .FirstOrDefault();
        }

        public string? GetFullAddress(int userId)
        {
            return _aiWardrobeContext.Addresses
                .Where(adr => adr.Fkuserid == userId)
                .Select(adr => $"{adr.Apartmentnum} {adr.Streetnum} " +
                                $"{adr.Streetname} {adr.City} " +
                                $"{adr.Province} {adr.Country} {adr.Postalcode}")
                .FirstOrDefault();
        }

    }
}
