using OnCallApp.Models;
using OnCallApp.Repositories.Interfaces;
using OnCallApp.Services.Interfaces;

namespace OnCallApp.Services.Implementations
{
    // Service class for managing roles.
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        // Initializes a new instance of the role service.
        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // Retrieves a list of all available roles.
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }
    }
}
