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

        /* This method is called by the runtime to configure the model
         * Note: If you need to configure relationships, indexes, or other properties,
         * you can do so here using the Fluent API.
         * For example, to configure a one-to-many relationship:
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder) //Fluent API, one-to-many relationship
        {
            modelBuilder.Entity<Employee>()
            .HasOne(e => e.Superior)
               .WithMany()
                .HasForeignKey(e => e.SuperiorId);

            //base.OnModelCreating(modelBuilder);
            // Additional configuration can be done here if needed
        }
    }
}
