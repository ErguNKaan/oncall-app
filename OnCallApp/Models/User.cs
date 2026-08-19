public class User
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public int RoleId { get; set; }
    
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    
    public required string PasswordHash { get; set; } // hashlenmiş şifre
    public bool MustChangePassword { get; set; } // ilk girişte şifre değiştirme zorunluluğu
    public int AccessFailedCount { get; set; } // başarısız giriş sayacı
    public DateTime? LockoutEndAt { get; set; } // hesap kilitlenme süresi
    public DateTime? LastLoginAt { get; set; } // son giriş zamanı
    
    public bool IncludeInRotation { get; set; } // icap rotasyonuna dahil mi?
    public bool IsActive { get; set; } // pasif kullanıcı sisteme giremez
    
    // İlişkiler
    public Unit Unit { get; set; } = null!;
    public Role Role { get; set; } = null!;
}