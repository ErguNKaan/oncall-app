namespace OnCallApp.Models;

public class OnCallAssignment
{
    public int Id { get; set; }
    
    // Time limits
    public DateTime StartsAt { get; set; } // Slot start
    public DateTime EndsAt { get; set; }   // Slot end
    
    // Day type (WorkDay, Weekend, PublicHoliday vb.) kept as enum 
    public DayType DayType { get; set; } 

    // Persons (Asıl kişi ve fiili sorumlu ayrı tutulcak)
    public int PrimaryUserId { get; set; } // Rotasyon gereği asıl olması gereken kişi
    public int ResponsibleUserId { get; set; } // O an gerçekten icapçı olan kişi
    
    // Reason of change must be traceable
    public AssignmentSource Source { get; set; } // Auto, LeaveShift, Transfer, HolidayDistribution, ManualAdmin
    
    public string? Note { get; set; } // for such notes as "uygun yedek bulunamadı" etc.

    // Relations
    public User PrimaryUser { get; set; } = null!;
    public User ResponsibleUser { get; set; } = null!;
}

// Using these enums to prevent magic numbers wandering around in the code
public enum DayType
{
    WorkDay = 1,
    Weekend = 2,
    PublicHoliday = 3
}

public enum AssignmentSource
{
    Auto = 1,                 // Auto assigned by system
    LeaveShift = 2,           // due to permission 
    Transfer = 3,             // transfer made
    HolidayDistribution = 4,  // holiday distribution as mentioned
    ManualAdmin = 5           // changed manually by admin
}