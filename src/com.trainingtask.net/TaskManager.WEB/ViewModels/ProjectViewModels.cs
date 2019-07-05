using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.WEB.ViewModels
{
    public class ProjectListView
    {
        public IEnumerable<ProjectDetailsView> Projects { get; set; }

        public PageInfo PageInfo { get; set; }
    }

    public class ProjectDetailsView
    {
        public int? Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.ProjectResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ProjectResource), ErrorMessageResourceName = "NameRequired")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.ProjectResource), 
            ErrorMessageResourceName = "NameLong")]
        public string Name { get; set; }

        [Display(Name = "ShortName", ResourceType = typeof(Resources.ProjectResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ProjectResource), ErrorMessageResourceName = "ShortNameRequired")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.ProjectResource),
            ErrorMessageResourceName = "ShortNameLong")]
        public string ShortName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.ProjectResource))]
        [StringLength(4000, ErrorMessageResourceType = typeof(Resources.ProjectResource),
            ErrorMessageResourceName = "DescriptionLong")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public ICollection<IssueInListView> IssuesOfProject { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, ShortName {ShortName}, Description {Description}";
        }

    }

    public class ProjectInDropdownView
    {
        public int Id { get; set; }

        public string ShortName { get; set; }
    }


}