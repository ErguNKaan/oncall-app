using OnCallApp.Models;
using OnCallApp.Repositories.Interfaces;
using OnCallApp.Services.Interfaces;

namespace OnCallApp.Services.Implementations
{
    // Service class for managing on-call shift assignments and validation rules.
    public class OnCallAssignmentService : IOnCallAssignmentService
    {
        private readonly IRepository<OnCallAssignment> _assignmentRepository;

        public OnCallAssignmentService(IRepository<OnCallAssignment> assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        // Retrieves all on-call assignments including related user details.
        public async Task<IEnumerable<OnCallAssignment>> GetAllAssignmentsAsync()
        {
            return await _assignmentRepository.GetAllAsync(null, "PrimaryUser", "ResponsibleUser");
        }

        // Retrieves all on-call assignments for a specific responsible user.
        public async Task<IEnumerable<OnCallAssignment>> GetAssignmentsByUserAsync(int userId)
        {
            return await _assignmentRepository.GetAllAsync(
                a => a.ResponsibleUserId == userId, 
                "PrimaryUser", 
                "ResponsibleUser"
            );
        }

        // Retrieves a specific on-call assignment by its unique identifier.
        public async Task<OnCallAssignment?> GetAssignmentByIdAsync(int id)
        {
            return await _assignmentRepository.GetAsync(a => a.Id == id, "PrimaryUser", "ResponsibleUser");
        }

        public async Task CreateAssignmentAsync(OnCallAssignment assignment)
        {
            // Validate: No overlapping assignments for the responsible user on the same date
            var existingAssignments = await _assignmentRepository.GetAllAsync(
                a => a.ResponsibleUserId == assignment.ResponsibleUserId &&
                     a.StartsAt.Date == assignment.StartsAt.Date
            );

            if (existingAssignments.Any())
            {
                throw new InvalidOperationException("Bu kullanıcıya aynı gün için zaten bir icap atanmış.");
            }

            await _assignmentRepository.CreateAsync(assignment);
            await _assignmentRepository.SaveAsync();
        }

        // Updates an existing on-call assignment record.
        public async Task UpdateAssignmentAsync(OnCallAssignment assignment)
        {
            _assignmentRepository.Update(assignment);
            await _assignmentRepository.SaveAsync();
        }

        // Deletes an on-call assignment by its unique identifier.
        public async Task DeleteAssignmentAsync(int id)
        {
            var assignment = await _assignmentRepository.GetAsync(a => a.Id == id);
            if (assignment != null)
            {
                _assignmentRepository.Delete(assignment);
                await _assignmentRepository.SaveAsync();
            }
        }
    }
}
