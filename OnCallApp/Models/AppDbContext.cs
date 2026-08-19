using Microsoft.EntityFrameworkCore;

namespace OnCallApp.Models
{
    public class AppDbContext : DbContext
    {
        // Program.cs'den bağlantı ayarlarını almak için
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanı tabloları
        public DbSet<Unit> Units { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OnCallAssignment> OnCallAssignments { get; set; }

        // Model kuralları (ilişkiler, zorunlu alanlar) buraya
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
