using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Task_manager.Models;

namespace Task_manager.DAL
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext() : base("TaskManagerContext")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}