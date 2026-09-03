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
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("MustChangePassword", user.MustChangePassword.ToString())
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

            if (user.MustChangePassword)
            {
                return RedirectToAction("ChangePassword", "Account");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userIdClaim));
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.OldPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Mevcut parolanız yanlış.");
                return View(model);
            }

            if (model.OldPassword == model.NewPassword)
            {
                ModelState.AddModelError(string.Empty, "Yeni parolanız eski parolanızla aynı olamaz.");
                return View(model);
            }

            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
            user.MustChangePassword = false;

            _context.Update(user);
            await _context.SaveChangesAsync();

            // Sign out to force re-login with new password and clear the MustChangePassword claim
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", new { message = "Parolanız başarıyla değiştirildi. Lütfen yeni parolanızla tekrar giriş yapın." });
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
