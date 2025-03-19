using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AI_Wardrobe.Models;

namespace AI_Wardrobe.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Identity Roles
        SeedIdentityRoles(modelBuilder);
    }

    private void SeedIdentityRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "Ad", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "Ma", Name = "Manager", NormalizedName = "MANAGER" },
            new IdentityRole { Id = "Cu", Name = "Customer", NormalizedName = "CUSTOMER" }
        );
    }
}