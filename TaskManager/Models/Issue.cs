using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.Services;

namespace TaskManager.Models
{
    public class Issue : Entity
    {

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

        public virtual Project Project { get; set; }

        public virtual Employee Employee { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, Work {Work}, BeginDate {BeginDate}, EndDate {EndDate}, ProjectId {ProjectId}, EmployeeId {EmployeeId}, StatusId {StatusId}";
        }
    }

    public static class StatusDict
    {
        static readonly Dictionary<int, string> _dict = new Dictionary<int, string>
        {
            {0, "New"},
            {1, "In progress"},
            {2, "Resolved"},
            {3, "Closed"},
            {4, "Reopened"}
        };

        public static Dictionary<int, string> GetStatusDict()
        {
            return _dict;
        }

    }
}