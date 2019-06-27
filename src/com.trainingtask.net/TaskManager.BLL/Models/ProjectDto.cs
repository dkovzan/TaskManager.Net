using System.Collections.Generic;

namespace TaskManager.BLL.Models
{
    public class ProjectDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public int IsDeleted { get; set; }

        public ICollection<IssueDto> IssuesDto { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, ShortName {ShortName}, Description {Description}";
        }
    }
}