using System;

namespace TaskManager.DAL.Entities
{
    public class Issue : Entity
    {
        public string Name { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Work { get; set; }

        public int ProjectId { get; set; }

        public int EmployeeId { get; set; }

        public int StatusId { get; set; }

        public Project Project { get; set; }

        public Employee Employee { get; set; }

    }

}