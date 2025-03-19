using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AI_Wardrobe.Repositories
{
    public class UserRoleRepo(UserManager<IdentityUser> userManager)
    {

        public async Task<bool> AddAsCustomer(string email)
        {
            return await AddUserRoleAsync(email, "Customer");
        }

        public async Task<bool> AddAsAdmin(string email)
        {
            return await AddUserRoleAsync(email, "Admin");
        }

        public async Task<bool> RemoveFromAdmin(string email)
        {
            return await RemoveUserRoleAsync(email, "Admin");
        }

        // Assign a role to a user.
        public async Task<bool> AddUserRoleAsync(string email
                                                , string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.AddToRoleAsync(user
                                                              , roleName);
                return result.Succeeded;
            }

            return false;
        }

        // Remove role from a user.
        public async Task<bool> RemoveUserRoleAsync(string email
                                                   , string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.RemoveFromRoleAsync(user
                                                                   , roleName);
                return result.Succeeded;
            }

            return false;
        }

        // Get all roles of a specific user.
        public async Task<IEnumerable<RoleVM>> GetUserRolesAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                return roles.Select(roleName => new RoleVM { RoleName = roleName });
            }

            return Enumerable.Empty<RoleVM>();
        }

        public async Task<IEnumerable<UserVm>> GetAdminUsers()
        {
            var users = await userManager.GetUsersInRoleAsync("Admin");
            return users.Select(user => new UserVm { Email = user.Email });
        }
    }
}
