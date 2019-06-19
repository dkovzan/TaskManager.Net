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

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BeginDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
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

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Begin Date")]
        [MinDate("01-01-1900", ErrorMessage = "Date cannot be earlier then 01-01-1900")]
        [MaxDate("31-12-9999", ErrorMessage = "Date cannot be later then 31-12-9999")]
        [EarlierDate("EndDate", ErrorMessage = "Begin date cannot be earlier then end date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BeginDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [MinDate("01-01-1900", ErrorMessage = "Date cannot be earlier then 01-01-1900")]
        [MaxDate("31-12-9999", ErrorMessage = "Date cannot be later then 31-12-9999")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Work hours should be between 1 and 1000000")]
        public int? Work { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        [Required]
        [Display(Name = "Assignee")]
        public int? EmployeeId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int? StatusId { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, Work {Work}, BeginDate {BeginDate}, EndDate {EndDate}, ProjectId {ProjectId}, EmployeeId {EmployeeId}, StatusId {StatusId}";
        }

    }

}