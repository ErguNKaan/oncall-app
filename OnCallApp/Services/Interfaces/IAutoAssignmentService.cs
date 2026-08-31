using OnCallApp.Models;

namespace OnCallApp.Services.Interfaces
{
    public interface IAutoAssignmentService
    {
        Task GenerateAutoAssignmentsAsync(DateTime startDate, DateTime endDate, int unitId);
    }
}
