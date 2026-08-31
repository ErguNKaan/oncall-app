public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; } //  "Employee", "UnitManager", "Admin" only
    
    // Relations
    public ICollection<User> Users { get; set; } = new List<User>();
}