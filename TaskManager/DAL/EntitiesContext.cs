using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TaskManager.Models;

namespace TaskManager.DAL
{
    public class EntitiesContext : DbContext
    {
        public EntitiesContext() : base("EntitiesContext")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}