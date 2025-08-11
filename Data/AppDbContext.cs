using Microsoft.EntityFrameworkCore;
using DbStructureEmployees.Models;

namespace DbStructureEmployees.Data
{
    // Bridge between the database and the application
    public class AppDbContext : DbContext
    {
        // Constructor to pass options to the base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for each entity type
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<VacationPackage> VacationPackages { get; set; }

        // Configure the model using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Superior)    // Employee has one Superior
                .WithMany()                 // Superior can have many subordinates
                .HasForeignKey(e => e.SuperiorId)
                .IsRequired(false);         // SuperiorId can be null (top-level employees)

            base.OnModelCreating(modelBuilder);
        }
    }
}
