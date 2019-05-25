using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task_manager.Models;

namespace Task_manager.DAL
{
    public class TaskManagerInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TaskManagerContext>
    {
        protected override void Seed(TaskManagerContext context)
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

            var tasks = new List<Task>
            {
                new Task {Name = "Test task", BeginDate = DateTime.Parse("2019-01-01"), EndDate = DateTime.Parse("2019-01-01"), ProjectId = 1, EmployeeId = 1, Status = Status.New }
            };

            tasks.ForEach(s => context.Tasks.Add(s));
            context.SaveChanges();

        }
    }
}