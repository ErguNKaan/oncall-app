using System.ComponentModel.DataAnnotations;

namespace OnCallApp.ViewModels
{
    public class UnitViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Birim adı zorunludur.")]
        [Display(Name = "Birim Adı")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Mesai başlangıç saati zorunludur.")]
        [Display(Name = "Mesai Başlangıç")]
        public TimeSpan WorkStartTime { get; set; }

        [Required(ErrorMessage = "Mesai bitiş saati zorunludur.")]
        [Display(Name = "Mesai Bitiş")]
        public TimeSpan WorkEndTime { get; set; }

        [Required(ErrorMessage = "Yarım gün mesai bitiş saati zorunludur.")]
        [Display(Name = "Yarım Gün Bitiş")]
        public TimeSpan HalfDayWorkEndTime { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;
    }
}
