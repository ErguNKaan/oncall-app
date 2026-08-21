using Microsoft.EntityFrameworkCore;

namespace OnCallApp.Models
{
    public class AppDbContext : DbContext
    {
        // Constructor for DbContextOptions injection
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Database tables
        public DbSet<Unit> Units { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OnCallAssignment> OnCallAssignments { get; set; }

        // Model configurations (relations, constraints, seed data)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent multiple cascade paths error in SQL Server
            modelBuilder.Entity<OnCallAssignment>()
                .HasOne(a => a.PrimaryUser)
                .WithMany()
                .HasForeignKey(a => a.PrimaryUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OnCallAssignment>()
                .HasOne(a => a.ResponsibleUser)
                .WithMany()
                .HasForeignKey(a => a.ResponsibleUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            
            // 1. Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Employee" },
                new Role { Id = 2, Name = "UnitManager" },
                new Role { Id = 3, Name = "Admin" }
            );

            // 2. Units
            modelBuilder.Entity<Unit>().HasData(
                new Unit 
                { 
                    Id = 1, 
                    Name = "Yazılım", 
                    WorkStartTime = new TimeSpan(9, 0, 0), // 09:00
                    WorkEndTime = new TimeSpan(18, 0, 0),  // 18:00
                    HalfDayWorkEndTime = new TimeSpan(13, 0, 0), // 13:00
                    IsActive = true 
                },
                new Unit 
                { 
                    Id = 2, 
                    Name = "Destek", 
                    WorkStartTime = new TimeSpan(9, 0, 0), 
                    WorkEndTime = new TimeSpan(18, 0, 0), 
                    HalfDayWorkEndTime = new TimeSpan(13, 0, 0), 
                    IsActive = true 
                }
            );
        }
    }
}
