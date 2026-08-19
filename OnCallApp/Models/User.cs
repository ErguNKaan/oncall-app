public class User
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public int RoleId { get; set; }
    
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public string PasswordHash { get; set; } // PBKDF2 ile hashlenmiş şifre
    public bool MustChangePassword { get; set; } // İlk girişte şifre değiştirme zorunluluğu
    public int AccessFailedCount { get; set; } // Başarısız giriş sayacı
    public DateTime? LockoutEndAt { get; set; } // Hesap kilitlenme süresi
    public DateTime? LastLoginAt { get; set; } // Son giriş zamanı
    
    public bool IncludeInRotation { get; set; } // İcap rotasyonuna dahil mi?
    public bool IsActive { get; set; } // Pasif kullanıcı sisteme giremez
    
    // İlişkiler
    public Unit Unit { get; set; }
    public Role Role { get; set; }
}