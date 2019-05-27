using System;
using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.DAL
{
    public class TaskManagerInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<EntitiesContext>
    {
        protected override void Seed(EntitiesContext context)
        {
            var employees = new List<Employee>
            {
                new Employee {FirstName = "Ivan", LastName = "Ivanov", Position = "QA Engineer"}
            };

            employees.ForEach(s => context.Employees.Add(s));
            context.SaveChanges();

            var projects = new List<Project>
            {
                new Project {Name = "Test project", ShortName = "TP", Description = "Some description of test project"}
            };

            projects.ForEach(s => context.Projects.Add(s));
            context.SaveChanges();

            var tasks = new List<Issue>
            {
                new Issue {Name = "Test task", BeginDate = DateTime.Parse("2019-01-01"), EndDate = DateTime.Parse("2019-01-01"), ProjectId = 1, EmployeeId = 1, StatusId = 0 }
            };

            tasks.ForEach(s => context.Issues.Add(s));
            context.SaveChanges();

        }
    }
}