using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.Validation;

namespace TaskManager.Models
{
    public class Project
    {
        public int? Id { get; set; }

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

        public virtual ICollection<Issue> Tasks { get; set; }
    }
}