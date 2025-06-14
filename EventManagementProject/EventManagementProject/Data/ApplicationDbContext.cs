using EventManagementProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Attendee> Attendees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendees)
                .HasForeignKey(a => a.UserID)
                .IsRequired(false);  // Cho phép UserID null

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(a => a.EventID)
                .IsRequired(false);  // Cho phép EventID null

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventCategory)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryID)
                .IsRequired(false);  // Cho phép CategoryID null
        }
    }
}
