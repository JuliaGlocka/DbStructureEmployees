using Microsoft.EntityFrameworkCore;
using DbStructureEmployees.Models;


namespace DbStructureEmployees.Data
{
    //Bridge between the database and the application
    public class AppDbContext : DbContext
    {
        // Constructor to pass options to the base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // DbSet properties for each entity type
        public DbSet<Employee> Employees { get; set; }
        // Configuring the model using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration can be done here if needed
        }
    }
}
