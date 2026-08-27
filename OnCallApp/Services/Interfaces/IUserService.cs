using OnCallApp.Models;

namespace OnCallApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user, string plainPassword);
        Task UpdateUserAsync(User user, string? newPlainPassword = null);
        Task DeleteUserAsync(int id);
    }
}
