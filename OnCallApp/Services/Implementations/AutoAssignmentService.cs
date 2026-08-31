using OnCallApp.Models;
using OnCallApp.Repositories.Interfaces;
using OnCallApp.Services.Interfaces;

namespace OnCallApp.Services.Implementations
{
    // Service for handling automatic shift distribution algorithms.
    
    /*
 * AUTO-ASSIGNMENT ALGORITHM LOGIC:
 * 1- Fetch all active and rotation-eligible users for the selected unit.
 * 2- Calculate current shift counts to track workloads and ensure fairness.
 * 3- Iterate through each day within the specified date range.
 * 4- Skip the day if an assignment already exists to avoid overwriting.
 * 5- Select the user with the lowest shift count for the empty day.
 * 6- Create the assignment record and increment that user's shift count.
 * 7- Bulk save all newly generated assignments to the database.
 */
    public class AutoAssignmentService : IAutoAssignmentService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<OnCallAssignment> _assignmentRepository;

        public AutoAssignmentService(IRepository<User> userRepository, IRepository<OnCallAssignment> assignmentRepository)
        {
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }

        // Generates automatic on-call assignments for a given date range and unit.
        public async Task GenerateAutoAssignmentsAsync(DateTime startDate, DateTime endDate, int unitId)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Başlangıç tarihi bitiş tarihinden büyük olamaz.");
            }

            var eligibleUsers = (await _userRepository.GetAllAsync(
                u => u.IsActive && u.IncludeInRotation && u.UnitId == unitId
            )).ToList();

            if (!eligibleUsers.Any())
            {
                throw new InvalidOperationException("Bu birimde rotasyona dahil aktif kullanıcı bulunamadı.");
            }

            // Keep track of assignment counts to ensure fair distribution
            var userAssignmentCounts = eligibleUsers.ToDictionary(u => u.Id, u => 0);

            // Fetch existing assignments in the date range to avoid overwriting and to calculate counts
            var existingAssignments = await _assignmentRepository.GetAllAsync(
                a => a.StartsAt.Date >= startDate.Date && a.StartsAt.Date <= endDate.Date
            );

            // Update counts for users who already have assignments
            foreach (var existing in existingAssignments)
            {
                if (userAssignmentCounts.ContainsKey(existing.ResponsibleUserId))
                {
                    userAssignmentCounts[existing.ResponsibleUserId]++;
                }
            }

            var newAssignments = new List<OnCallAssignment>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                // Skip if there's already an assignment for this date in the unit
                // To keep it simple, we assume one person per unit per day
                bool dateHasAssignment = existingAssignments.Any(a => 
                    a.StartsAt.Date == date && 
                    eligibleUsers.Any(u => u.Id == a.ResponsibleUserId)
                );

                if (dateHasAssignment)
                {
                    continue; // Skip this day, already covered
                }

                // Find the user with the lowest assignment count
                var candidateUserId = userAssignmentCounts
                    .OrderBy(kvp => kvp.Value)
                    .First().Key;

                var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

                var newAssignment = new OnCallAssignment
                {
                    StartsAt = date.AddHours(18), // Default start time 18:00
                    EndsAt = date.AddDays(1).AddHours(9), // Default end time next day 09:00
                    DayType = isWeekend ? DayType.Weekend : DayType.WorkDay,
                    PrimaryUserId = candidateUserId,
                    ResponsibleUserId = candidateUserId,
                    Source = AssignmentSource.Auto,
                    Note = "Sistem tarafından otomatik atandı."
                };

                newAssignments.Add(newAssignment);

                // Increment the chosen user's count
                userAssignmentCounts[candidateUserId]++;
            }

            foreach (var assignment in newAssignments)
            {
                await _assignmentRepository.CreateAsync(assignment);
            }
            
            await _assignmentRepository.SaveAsync();
        }
    }
}
