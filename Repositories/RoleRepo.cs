using AI_Wardrobe.Data;
using Microsoft.AspNetCore.Identity;
using AI_Wardrobe.ViewModels;

namespace AI_Wardrobe.Repositories
{
    public class RoleRepo(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public List<RoleVM> GetAllRoles()
        {
            var roles = _context.Roles.Select(r => new RoleVM
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();

            return roles;
        }

        public RoleVM GetRole(string roleName)
        {
            var role =
                _context.Roles.Where(r => r.Name == roleName)
                              .FirstOrDefault();

            if (role != null)
            {
                return new RoleVM()
                {
                    RoleName = role.Name,
                    Id = role.Id
                };
            }
            return null;
        }

        public bool CreateRole(string roleName)
        {
            bool isSuccess = true;

            try
            {
                _context.Roles.Add(new IdentityRole
                {
                    Name = roleName,
                    Id = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating role:" +
                                  $" {ex.Message}");

                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
