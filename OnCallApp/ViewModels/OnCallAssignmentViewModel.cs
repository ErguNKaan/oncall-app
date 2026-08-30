using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnCallApp.Models;

namespace OnCallApp.ViewModels
{
    public class OnCallAssignmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlangıç zamanı zorunludur.")]
        [Display(Name = "Başlangıç")]
        public DateTime StartsAt { get; set; }

        [Required(ErrorMessage = "Bitiş zamanı zorunludur.")]
        [Display(Name = "Bitiş")]
        public DateTime EndsAt { get; set; }

        [Required(ErrorMessage = "Gün tipi zorunludur.")]
        [Display(Name = "Gün Tipi")]
        public DayType DayType { get; set; }

        [Required(ErrorMessage = "Asıl sorumlu seçimi zorunludur.")]
        [Display(Name = "Asıl Sorumlu")]
        public int PrimaryUserId { get; set; }

        [Required(ErrorMessage = "Fiili sorumlu seçimi zorunludur.")]
        [Display(Name = "Fiili Sorumlu")]
        public int ResponsibleUserId { get; set; }

        [Display(Name = "Atama Kaynağı")]
        public AssignmentSource Source { get; set; } = AssignmentSource.ManualAdmin;

        [Display(Name = "Not")]
        public string? Note { get; set; }

        // Display fields
        public string? PrimaryUserName { get; set; }
        public string? ResponsibleUserName { get; set; }

        // Dropdown lists
        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}
