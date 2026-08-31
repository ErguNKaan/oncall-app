using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnCallApp.ViewModels
{
    public class AutoAssignViewModel
    {
        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [Display(Name = "Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

        [Required(ErrorMessage = "Birim seçimi zorunludur.")]
        [Display(Name = "Birim")]
        public int UnitId { get; set; }

        // Dropdown list for units
        public IEnumerable<SelectListItem>? Units { get; set; }
    }
}
