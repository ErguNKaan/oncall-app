using System.ComponentModel.DataAnnotations;

namespace OnCallApp.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mevcut parola zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Parola")]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessage = "Yeni parola zorunludur.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Parola")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Parola (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Yeni parola ve onay parolası eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
