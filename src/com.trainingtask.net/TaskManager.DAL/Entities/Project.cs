using System.Collections.Generic;

namespace TaskManager.DAL.Entities
{
    public class Project : Entity
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public ICollection<Issue> Issues { get; set; }

    }
}