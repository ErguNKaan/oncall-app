public class Unit
{
    public int Id { get; set; }
    public required string Name { get; set; } // yazılım veya destek
    public TimeSpan WorkStartTime { get; set; } // mesai başlangıcı
    public TimeSpan WorkEndTime { get; set; } // mesai bitişi
    public TimeSpan HalfDayWorkEndTime { get; set; } // yarım gün mesai bitişi
    public bool IsActive { get; set; } // Pasif birimler planlamaya girmez
    
    // İlişkiler
    public ICollection<User> Users { get; set; } = new List<User>();
}