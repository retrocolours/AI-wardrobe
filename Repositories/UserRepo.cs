using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using AIWardrobe.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
    }
}
