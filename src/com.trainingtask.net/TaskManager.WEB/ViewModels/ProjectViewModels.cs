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

        [Display(Name = "Name")]
        [Required]
        [StringLength(255, ErrorMessage = "Name should contain maximum 255 characters.")]
        public string Name { get; set; }

        [Display(Name = "Short Name")]
        [Required]
        [StringLength(255, ErrorMessage = "Name should contain maximum 255 characters.")]
        public string ShortName { get; set; }

        [Display(Name = "Description")]
        [StringLength(4000, ErrorMessage = "Description should contain maximum 4000 characters.")]
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