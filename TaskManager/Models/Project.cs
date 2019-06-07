using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.Services;

namespace TaskManager.Models
{
    public class Project : Entity
    {

        [Required]
        [StringLength(255, ErrorMessage = "Name should contain maximum 255 characters.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Short Name")]
        [UniqueForProjects(ErrorMessage = "Short name should be unique.")]
        [StringLength(255, ErrorMessage = "Short name should contain maximum 255 characters.")]
        public string ShortName { get; set; }

        [StringLength(4000, ErrorMessage = "Description should contain maximum 4000 characters.")]
        public string Description { get; set; }

        public ICollection<Issue> Issues { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, ShortName {ShortName}, Description {Description}";
        }
    }
}