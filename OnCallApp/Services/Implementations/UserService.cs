using Microsoft.AspNetCore.Identity;
using OnCallApp.Models;
using OnCallApp.Repositories.Interfaces;
using OnCallApp.Services.Interfaces;

namespace OnCallApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Include Unit and Role for display
            return await _userRepository.GetAllAsync(null, "Unit", "Role");
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetAsync(u => u.Id == id, "Unit", "Role");
        }

        public async Task CreateUserAsync(User user, string plainPassword)
        {
            // Hash the password
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, plainPassword);
            
            // Set defaults based on document rules
            user.MustChangePassword = true; 
            user.AccessFailedCount = 0;
            user.IsActive = true;

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
        }

        public async Task UpdateUserAsync(User user, string? newPlainPassword = null)
        {
            // Only update password if a new one is provided
            if (!string.IsNullOrEmpty(newPlainPassword))
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, newPlainPassword);
                user.MustChangePassword = true; // Require password change on reset
            }

            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            if (user != null)
            {
                // Soft delete based on document requirement
                user.IsActive = false;
                _userRepository.Update(user);
                await _userRepository.SaveAsync();
            }
        }
    }
}
