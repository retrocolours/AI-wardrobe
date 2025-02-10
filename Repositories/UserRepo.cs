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

        public MyProfileVM? getMyProfile(string email)
        {
            return _aiWardrobeContext.RegisteredUsers
          .Join(_aiWardrobeContext.Addresses,
                i => i.Userid,
                s => s.Fkuserid,
                (i, s) => new { i, s })
          .Where(x => x.i.Email == email)
          .Select(x => new MyProfileVM
          {
              Id = x.i.Userid,
              FirstName = x.i.Firstname,
              LastName = x.i.Lastname,
              StreetNumber = x.s.Streetnum,
              StreetName = x.s.Streetname,
              Apartment = x.s.Apartmentnum,
              City = x.s.City,
              Province = x.s.Province,
              PostalCode = x.s.Postalcode,
              Phone = x.i.Phone,
              Email = x.i.Email
          })
          .FirstOrDefault();
        }

        //public IEnumerable<User> GetAllUsers()
        //{
        //    var users = _context.Users;

        //    return users;
        //}
    }
}
