using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AI_Wardrobe.Repositories
{
    public class UserRepo
    {
        private readonly AiwardrobeContext _aiWardrobeContext;

        // Correct constructor
        public UserRepo(AiwardrobeContext aiWardrobeContext)
        {
            _aiWardrobeContext = aiWardrobeContext ?? throw new ArgumentNullException(nameof(aiWardrobeContext));
        }

        // Add user with error handling
        public void AddUser(RegisteredUser user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User cannot be null.");
                }

                _aiWardrobeContext.RegisteredUsers.Add(user);
                _aiWardrobeContext.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database error: {dbEx.Message}");
                throw new Exception("An error occurred while saving the user to the database.", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                throw new Exception("An unexpected error occurred while adding the user.", ex);
            }
        }

        // Get user by email with error handling
        public RegisteredUser? GetUserFromEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be empty.", nameof(email));
                }
                return _aiWardrobeContext.RegisteredUsers.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by email: {ex.Message}");
                return null;
            }
        }

        public UserVm? GetUserVM(string email)
        {
            var registeredUser = GetUserFromEmail(email);

            if (registeredUser != null)
            {
                var userVM = new UserVm
                {
                    Id = registeredUser.Userid,
                    FirstName = registeredUser.Firstname,
                    LastName = registeredUser.Lastname,
                    Phone = registeredUser.Phone,
                    Email = registeredUser.Email,
                };

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

            //return null if no user data found
            return null;
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
