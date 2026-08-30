using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnCallApp.Models;
using OnCallApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace OnCallApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOnCallAssignmentService _assignmentService;

        public HomeController(ILogger<HomeController> logger, IOnCallAssignmentService assignmentService)
        {
            _logger = logger;
            _assignmentService = assignmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarData()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            var events = assignments.Select(a => new
            {
                id = a.Id,
                title = $"{a.ResponsibleUser?.FullName} - {a.DayType}",
                start = a.StartsAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = a.EndsAt.ToString("yyyy-MM-ddTHH:mm:ss")
            });
            return Json(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
