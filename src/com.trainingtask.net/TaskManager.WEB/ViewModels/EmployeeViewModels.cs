using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.WEB.ViewModels
{
    public class EmployeesListView
    {
        public IEnumerable<EmployeeDetailsView> Employees { get; set; }

        public PageInfo PageInfo { get; set; }
    }

    public class EmployeeDetailsView
    {
        public int? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.EmployeeResource))]
        [RegularExpression(@"\b[\p{L}\s-']+\b", ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "FirstNameInvalid")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.EmployeeResource), 
            ErrorMessageResourceName = "FirstNameLong")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "LastNameRequired")]
        [Display(Name = "LastName", ResourceType = typeof(Resources.EmployeeResource))]
        [RegularExpression(@"\b[\p{L}\s-']+\b", ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "LastNameInvalid")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "LastNameLong")]
        public string LastName { get; set; }

        [Display(Name = "MiddleName", ResourceType = typeof(Resources.EmployeeResource))]
        [RegularExpression(@"\b[\p{L}\s-']+\b", ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "MiddleNameInvalid")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "MiddleNameLong")]
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "PositionRequired")]
        [Display(Name = "Position", ResourceType = typeof(Resources.EmployeeResource))]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.EmployeeResource),
            ErrorMessageResourceName = "PositionLong")]
        public string Position { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, FirstName {FirstName}, LastName {LastName}, MiddleName {MiddleName}, Position {Position}";
        }
    }

    public class EmployeeInDropdownView
    {
        public int Id { get; set; }

        public string FullName { get; set; }
    }
}