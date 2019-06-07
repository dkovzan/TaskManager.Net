using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Employee : Entity
    {

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"\b[\p{L}\s-']+\b", ErrorMessage = "First name should include only letters, space, hyphen and single quote.")]
        [StringLength(255, ErrorMessage = "First name should contain maximum 255 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"\b[\p{L}\s-']+\b", ErrorMessage = "Last name should include only letters, space, hyphen and single quote.")]
        [StringLength(255, ErrorMessage = "Last name should contain maximum 255 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        [RegularExpression(@"\b[\p{L}\s-']+\b", ErrorMessage = "Middle name should include only letters, space, hyphen and single quote.")]
        [StringLength(255, ErrorMessage = "Middle name should contain maximum 255 characters.")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Position should contain maximum 255 characters.")]
        public string Position { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, FirstName {FirstName}, LastName {LastName}, MiddleName {MiddleName}, Position {Position}";
        }
    }
}