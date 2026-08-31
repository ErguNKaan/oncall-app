public class User
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public int RoleId { get; set; }
    
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    
    public required string PasswordHash { get; set; } // Hashed password
    public bool MustChangePassword { get; set; } // Must change password on first login
    public int AccessFailedCount { get; set; } // failed login counter
    public DateTime? LockoutEndAt { get; set; } // Tiime for Account Lockout
    public DateTime? LastLoginAt { get; set; } // Last Login as mentioned
    
    public bool IncludeInRotation { get; set; } // Included in rotation???
    public bool IsActive { get; set; } // Passive user cannot login
    
    // Relations
    public Unit Unit { get; set; } = null!;
    public Role Role { get; set; } = null!;
}