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
        private readonly IAutoAssignmentService _autoAssignmentService;
        private readonly IUnitService _unitService;

        public OnCallAssignmentsController(
            IOnCallAssignmentService assignmentService, 
            IUserService userService,
            IAutoAssignmentService autoAssignmentService,
            IUnitService unitService)
        {
            _assignmentService = assignmentService;
            _userService = userService;
            _autoAssignmentService = autoAssignmentService;
            _unitService = unitService;
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

        public async Task<IActionResult> AutoAssign()
        {
            var model = new AutoAssignViewModel();
            await PopulateAutoAssignDropdownsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoAssign(AutoAssignViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _autoAssignmentService.GenerateAutoAssignmentsAsync(model.StartDate, model.EndDate, model.UnitId);
                    TempData["SuccessMessage"] = "Otomatik atama başarıyla tamamlandı.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await PopulateAutoAssignDropdownsAsync(model);
            return View(model);
        }

        private async Task PopulateAutoAssignDropdownsAsync(AutoAssignViewModel model)
        {
            var units = await _unitService.GetAllUnitsAsync();
            model.Units = units.Where(u => u.IsActive).Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name });
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

        [HttpGet]
        public async Task<IActionResult> ExportToCsv()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("Baslangic,Bitis,Gun Tipi,Asil Sorumlu,Fiili Sorumlu,Atama Kaynagi,Not");

            foreach (var a in assignments.OrderBy(a => a.StartsAt))
            {
                var startsAt = a.StartsAt.ToString("yyyy-MM-dd HH:mm");
                var endsAt = a.EndsAt.ToString("yyyy-MM-dd HH:mm");
                var primaryUser = a.PrimaryUser?.FullName ?? "";
                var responsibleUser = a.ResponsibleUser?.FullName ?? "";
                var note = a.Note?.Replace(",", " ") ?? ""; // Avoid CSV breaking

                builder.AppendLine($"{startsAt},{endsAt},{a.DayType},{primaryUser},{responsibleUser},{a.Source},{note}");
            }

            var fileBytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            return File(fileBytes, "text/csv", $"NobetListesi_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
