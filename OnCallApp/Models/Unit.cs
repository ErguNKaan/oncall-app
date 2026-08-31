public class Unit
{
    public int Id { get; set; }
    public required string Name { get; set; } // "yazılım" or "destek"
    public TimeSpan WorkStartTime { get; set; } // workshift start
    public TimeSpan WorkEndTime { get; set; } // workshift end
    public TimeSpan HalfDayWorkEndTime { get; set; } // half day workshift end
    public bool IsActive { get; set; } // Passive units won't be included in planning
    
    // Relations
    public ICollection<User> Users { get; set; } = new List<User>();
}