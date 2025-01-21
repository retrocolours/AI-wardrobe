﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI_Wardrobe.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

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