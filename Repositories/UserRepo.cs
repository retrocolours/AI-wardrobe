using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AI_Wardrobe.Repositories
{
    public class UserRepo(AiwardrobeContext aiwardrobeContext, ApplicationDbContext appContext)
    {

        private readonly AiwardrobeContext _aiWardrobeContext = aiwardrobeContext;
        private readonly ApplicationDbContext _appContext = appContext;

        public string AddUser(RegisteredUser user)
        {
            try
            {
                _aiWardrobeContext.RegisteredUsers.Add(user);
                _aiWardrobeContext.SaveChanges();

                return "User add successfull.";
            }
            catch (Exception ex)
            {
                return "Error adding user.";
            }
        }

        public bool UpdateUser(RegisteredUser user)
        {
            try
            {
                _aiWardrobeContext.RegisteredUsers.Update(user);
                _aiWardrobeContext.SaveChanges();

                return true;
            }
            catch (Exception ex)

            {
                return false;
            }
        }

        public IEnumerable<RegisteredUser> GetUserByID(int id)
        {
            return _aiWardrobeContext.RegisteredUsers.Where(usr => usr.Userid == id);
        }

        public IEnumerable<RegisteredUser> GetUserByEmail(string email)
        {
            return _aiWardrobeContext.RegisteredUsers.Where(usr => usr.Email == email);
        }

        public int? GetUserId(String email)
        {
            var registerdUser = _aiWardrobeContext.RegisteredUsers.Where((user) => user.Email == email).FirstOrDefault();
            Console.Write($"registerdUser: {email}, id: {registerdUser?.Userid}");
            return registerdUser?.Userid;
        }

        public UserVm? GetUserVM(string email)
        {
            var userVM = GetUserByEmail(email)
                    .Select(usr => new UserVm
                    {
                        Id = usr.Userid,
                        FirstName = usr.Firstname,
                        LastName = usr.Lastname,
                        Phone = usr.Phone,
                        Email = usr.Email,
                    }).FirstOrDefault();

            if (userVM != null)
            {
                var adr = GetAddress(userVM.Id).FirstOrDefault();

                userVM.Address1 = adr?.Address1;
                userVM.Address2 = adr?.Address2;
                userVM.City = adr?.City;
                userVM.Province = adr?.Province;
                userVM.PostalCode = adr?.Postalcode;
            }

            return userVM;
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
                .Select(adr => $"{adr.Address1} {adr.Address2} {adr.City} " +
                                $"{adr.Province} {adr.Postalcode}")
                .FirstOrDefault();
        }

        public IEnumerable<Address> GetAddress(int userId)
        {
            return _aiWardrobeContext.Addresses.Where(adr => adr.Fkuserid == userId);
        }

        public bool UpdateAddress(int userId, Address address)
        {
            try
            {
                var adr = GetAddress(userId).FirstOrDefault();

                //if address already exist update, if not then insert
                if (adr != null)
                {
                    adr.Address1 = address.Address1;
                    adr.Address2 = address.Address2;
                    adr.City = address.City;
                    adr.Province = address.Province;
                    adr.Postalcode = address.Postalcode;

                    _aiWardrobeContext.Addresses.Update(adr);
                }
                else
                {
                    _aiWardrobeContext.Add(address);
                }

                _aiWardrobeContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
