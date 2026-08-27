using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnCallApp.Models;
using OnCallApp.Services.Interfaces;
using OnCallApp.ViewModels;

namespace OnCallApp.Controllers
{
    // Admin only access as per document requirements
    [Authorize(Roles = "Admin")]
    public class UnitsController : Controller
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        // List all units
        public async Task<IActionResult> Index()
        {
            var units = await _unitService.GetAllUnitsAsync();
            var model = units.Select(u => new UnitViewModel
            {
                Id = u.Id,
                Name = u.Name,
                WorkStartTime = u.WorkStartTime,
                WorkEndTime = u.WorkEndTime,
                HalfDayWorkEndTime = u.HalfDayWorkEndTime,
                IsActive = u.IsActive
            }).ToList();

            return View(model);
        }

        // Show create form
        public IActionResult Create()
        {
            return View(new UnitViewModel());
        }

        // Process create form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var unit = new Unit
                {
                    Name = model.Name,
                    WorkStartTime = model.WorkStartTime,
                    WorkEndTime = model.WorkEndTime,
                    HalfDayWorkEndTime = model.HalfDayWorkEndTime,
                    IsActive = model.IsActive
                };
                
                await _unitService.CreateUnitAsync(unit);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Show edit form
        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _unitService.GetUnitByIdAsync(id);
            if (unit == null) return NotFound();

            var model = new UnitViewModel
            {
                Id = unit.Id,
                Name = unit.Name,
                WorkStartTime = unit.WorkStartTime,
                WorkEndTime = unit.WorkEndTime,
                HalfDayWorkEndTime = unit.HalfDayWorkEndTime,
                IsActive = unit.IsActive
            };
            return View(model);
        }

        // Process edit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UnitViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var unit = await _unitService.GetUnitByIdAsync(id);
                if (unit == null) return NotFound();

                unit.Name = model.Name;
                unit.WorkStartTime = model.WorkStartTime;
                unit.WorkEndTime = model.WorkEndTime;
                unit.HalfDayWorkEndTime = model.HalfDayWorkEndTime;
                unit.IsActive = model.IsActive;

                await _unitService.UpdateUnitAsync(unit);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Process delete (Soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitService.DeleteUnitAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
