using OnCallApp.Models;

namespace OnCallApp.Services.Interfaces
{
    public interface IOnCallAssignmentService
    {
        Task<IEnumerable<OnCallAssignment>> GetAllAssignmentsAsync();
        Task<IEnumerable<OnCallAssignment>> GetAssignmentsByUserAsync(int userId);
        Task<OnCallAssignment?> GetAssignmentByIdAsync(int id);
        Task CreateAssignmentAsync(OnCallAssignment assignment);
        Task UpdateAssignmentAsync(OnCallAssignment assignment);
        Task DeleteAssignmentAsync(int id);
    }
}
