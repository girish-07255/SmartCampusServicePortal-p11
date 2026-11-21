using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Data
{
    // EF Core Database Context for Smart Campus Service Portal
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables for Students and Service Requests
        public DbSet<Student> Students { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.StudentIdNumber).IsUnique();
            });

            // Configure ServiceRequest entity
            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
