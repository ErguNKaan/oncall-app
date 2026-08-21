using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnCallApp.Models;
using OnCallApp.ViewModels;
using System.Security.Claims;

namespace OnCallApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // If user is already authenticated, redirect to home
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user by email
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Unit)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi veya hesabınız pasif.");
                return View(model);
            }

            // Verify password using PasswordHasher
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                // Increment access failed count and check for lockout (omitted here for brevity, to be implemented later)
                ModelState.AddModelError(string.Empty, "Geçersiz parola.");
                return View(model);
            }

            // Create claims based on document section 4.1
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("unitId", user.UnitId.ToString()),
                new Claim("roleName", user.Role.Name),
                new Claim("fullName", user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);

            // Check if MustChangePassword is true (to be handled by middleware, but redirecting to Profile for now)
            if (user.MustChangePassword)
            {
                // return RedirectToAction("ChangePassword", "Profile");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
