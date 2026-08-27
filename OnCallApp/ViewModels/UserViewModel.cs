using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnCallApp.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Birim seçimi zorunludur.")]
        [Display(Name = "Birim")]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur.")]
        [Display(Name = "Rol")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Rotasyona Dahil Mi?")]
        public bool IncludeInRotation { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        // View support properties
        public IEnumerable<SelectListItem>? Units { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
        public string? UnitName { get; set; }
        public string? RoleName { get; set; }
    }
}
