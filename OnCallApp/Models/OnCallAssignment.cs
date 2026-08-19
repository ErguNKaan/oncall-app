namespace OnCallApp.Models;

public class OnCallAssignment
{
    public int Id { get; set; }
    
    // Zaman sınırları
    public DateTime StartsAt { get; set; } // Slot başlangıcı
    public DateTime EndsAt { get; set; }   // Slot bitişi
    
    // Gün tipi (WorkDay, Weekend, PublicHoliday vb.) enum olarak tutulacak
    public DayType DayType { get; set; } 

    // Kişiler (Asıl kişi ve Fiili sorumlu ayrı tutuluyor)
    public int PrimaryUserId { get; set; } // Rotasyon gereği asıl olması gereken kişi
    public int ResponsibleUserId { get; set; } // O an gerçekten icapçı olan kişi
    
    // Değişim nedeni izlenebilir olmalı
    public AssignmentSource Source { get; set; } // Auto, LeaveShift, Transfer, HolidayDistribution, ManualAdmin
    
    public string? Note { get; set; } // "Uygun yedek bulunamadı" vb. notlar için

    // İlişkiler
    public User PrimaryUser { get; set; }
    public User ResponsibleUser { get; set; }
}

// Bu enum'ları kodda magic number dolaşmaması için kullanıyoruz
public enum DayType
{
    WorkDay = 1,
    Weekend = 2,
    PublicHoliday = 3
}

public enum AssignmentSource
{
    Auto = 1,                 // Sistem otomatik atadı
    LeaveShift = 2,           // İzin nedeniyle kaydı
    Transfer = 3,             // Devir/Takas yapıldı
    HolidayDistribution = 4,  // Tatil dağıtımı
    ManualAdmin = 5           // Admin elle değiştirdi
}