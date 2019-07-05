using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.WEB.Validation;

namespace TaskManager.WEB.ViewModels
{
    public class IssuesListView
    {
        public IEnumerable<IssueInListView> Issues { get; set; }

        public PageInfo PageInfo { get; set; }
    }

    public class IssueInListView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int Work { get; set; }

        public int ProjectId { get; set; }

        public int EmployeeId { get; set; }

        public int StatusId { get; set; }

        public string ProjectShortName { get; set; }

        public string EmployeeFullName { get; set; }

    }

    public class IssueEditView
    {
        public int? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.IssueResource))]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.IssueResource),
            ErrorMessageResourceName = "NameLong")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "BeginDateRequired")]
        [Display(Name = "BeginDate", ResourceType = typeof(Resources.IssueResource))]
        [MinDate(1900, 1, 1, ErrorMessageResourceType = typeof(Resources.IssueResource), 
            ErrorMessageResourceName = "MinDate")]
        [MaxDate(9999, 12, 31, ErrorMessageResourceType = typeof(Resources.IssueResource), 
            ErrorMessageResourceName = "MaxDate")]
        [EarlierDate("EndDate", ErrorMessageResourceType = typeof(Resources.IssueResource),
            ErrorMessageResourceName = "EarlierDate")]
        [DataType(DataType.Date)]
        public DateTime? BeginDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "EndDateRequired")]
        [Display(Name = "EndDate", ResourceType = typeof(Resources.IssueResource))]
        [MinDate(1900, 1, 1, ErrorMessageResourceType = typeof(Resources.IssueResource),
            ErrorMessageResourceName = "MinDate")]
        [MaxDate(9999, 12, 31, ErrorMessageResourceType = typeof(Resources.IssueResource),
            ErrorMessageResourceName = "MaxDate")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "WorkRequired")]
        [Display(Name = "Work", ResourceType = typeof(Resources.IssueResource))]
        [Range(1, 1000000, ErrorMessageResourceType = typeof(Resources.IssueResource), 
            ErrorMessageResourceName = "WorkInvalid")]
        public int? Work { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "IssueProjectRequired")]
        [Display(Name = "IssueProject", ResourceType = typeof(Resources.IssueResource))]
        public int? ProjectId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "IssueEmployeeRequired")]
        [Display(Name = "IssueEmployee", ResourceType = typeof(Resources.IssueResource))]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.IssueResource), ErrorMessageResourceName = "StatusRequired")]
        [Display(Name = "Status", ResourceType = typeof(Resources.IssueResource))]
        public int? StatusId { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, Work {Work}, BeginDate {BeginDate}, EndDate {EndDate}, ProjectId {ProjectId}, EmployeeId {EmployeeId}, StatusId {StatusId}";
        }

    }

}