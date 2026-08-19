public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: "Yazılım" veya "Destek"
    public TimeSpan WorkStartTime { get; set; } // Mesai başlangıcı, Örn: 09:00
    public TimeSpan WorkEndTime { get; set; } // Mesai bitişi, Örn: 18:00
    public TimeSpan HalfDayWorkEndTime { get; set; } // Yarım gün mesai bitişi, Örn: 13:00
    public bool IsActive { get; set; } // Pasif birimler planlamaya girmez
    
    // İlişkiler
    public ICollection<User> Users { get; set; }
}