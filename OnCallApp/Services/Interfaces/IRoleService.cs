using OnCallApp.Models;

namespace OnCallApp.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
