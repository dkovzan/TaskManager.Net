using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_manager.Models
{
    public enum Status
    {
        New, InProgress, Resolved, Closed
    }
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Work { get; set; }

        public int ProjectId { get; set; }

        public int EmployeeId { get; set; }

        public Status? Status { get; set; }

        public virtual Project Project { get; set; }

        public virtual Employee Employee { get; set; }

    }
}