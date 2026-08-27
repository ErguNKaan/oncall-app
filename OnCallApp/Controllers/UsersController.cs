using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnCallApp.Models;
using OnCallApp.Services.Interfaces;
using OnCallApp.ViewModels;

namespace OnCallApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitService _unitService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService, IUnitService unitService, IRoleService roleService)
        {
            _userService = userService;
            _unitService = unitService;
            _roleService = roleService;
        }

        // List all users
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            var model = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                UnitName = u.Unit?.Name,
                RoleName = u.Role?.Name,
                IncludeInRotation = u.IncludeInRotation,
                IsActive = u.IsActive
            }).ToList();

            return View(model);
        }

        // Show create form
        public async Task<IActionResult> Create()
        {
            var model = new UserViewModel();
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // Process create form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Parola zorunludur.");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UnitId = model.UnitId,
                    RoleId = model.RoleId,
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    IncludeInRotation = model.IncludeInRotation,
                    IsActive = model.IsActive,
                    PasswordHash = "" // Will be set in service
                };
                
                await _userService.CreateUserAsync(user, model.Password!);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // Show edit form
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                UnitId = user.UnitId,
                RoleId = user.RoleId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IncludeInRotation = user.IncludeInRotation,
                IsActive = user.IsActive
            };
            
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // Process edit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();

                user.UnitId = model.UnitId;
                user.RoleId = model.RoleId;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.IncludeInRotation = model.IncludeInRotation;
                user.IsActive = model.IsActive;

                await _userService.UpdateUserAsync(user, model.Password);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // Process delete (Soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Helper to populate dropdowns
        private async Task PopulateDropdownsAsync(UserViewModel model)
        {
            var units = await _unitService.GetAllUnitsAsync();
            var roles = await _roleService.GetAllRolesAsync();

            model.Units = units.Where(u => u.IsActive).Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name });
            model.Roles = roles.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });
        }
    }
}
