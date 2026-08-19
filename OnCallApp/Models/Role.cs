public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; } // Sadece "Employee", "UnitManager", "Admin" olacak
    
    // İlişkiler
    public ICollection<User> Users { get; set; } = new List<User>();
}