using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnCallApp.Models;
using OnCallApp.Services.Interfaces;
using OnCallApp.ViewModels;

namespace OnCallApp.Controllers
{
    // Restricting manual assignment to Admins and UnitManagers
    [Authorize(Roles = "Admin,UnitManager")]
    public class OnCallAssignmentsController : Controller
    {
        private readonly IOnCallAssignmentService _assignmentService;
        private readonly IUserService _userService;

        public OnCallAssignmentsController(IOnCallAssignmentService assignmentService, IUserService userService)
        {
            _assignmentService = assignmentService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            var model = assignments.Select(a => new OnCallAssignmentViewModel
            {
                Id = a.Id,
                StartsAt = a.StartsAt,
                EndsAt = a.EndsAt,
                DayType = a.DayType,
                PrimaryUserName = a.PrimaryUser?.FullName,
                ResponsibleUserName = a.ResponsibleUser?.FullName,
                Source = a.Source,
                Note = a.Note
            }).OrderBy(a => a.StartsAt).ToList();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new OnCallAssignmentViewModel
            {
                StartsAt = DateTime.Today.AddHours(18),
                EndsAt = DateTime.Today.AddDays(1).AddHours(9),
                Source = AssignmentSource.ManualAdmin
            };
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OnCallAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var assignment = new OnCallAssignment
                {
                    StartsAt = model.StartsAt,
                    EndsAt = model.EndsAt,
                    DayType = model.DayType,
                    PrimaryUserId = model.PrimaryUserId,
                    ResponsibleUserId = model.ResponsibleUserId,
                    Source = model.Source,
                    Note = model.Note
                };

                try
                {
                    await _assignmentService.CreateAssignmentAsync(assignment);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _assignmentService.DeleteAssignmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(OnCallAssignmentViewModel model)
        {
            var users = await _userService.GetAllUsersAsync();
            // Only active users who are included in rotation
            model.Users = users
                .Where(u => u.IsActive && u.IncludeInRotation)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FullName })
                .ToList();
        }
    }
}
