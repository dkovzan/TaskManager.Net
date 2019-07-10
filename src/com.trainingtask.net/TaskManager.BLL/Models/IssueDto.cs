using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.BLL.Models
{
    public class IssueDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Work { get; set; }

        public int? ProjectId { get; set; }

        public int? EmployeeId { get; set; }

        public int? StatusId { get; set; }

        public int IsDeleted { get; set; }

        public ProjectDto ProjectDto { get; set; }

        public EmployeeDto EmployeeDto { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, Name {Name}, Work {Work}, BeginDate {BeginDate}, EndDate {EndDate}, ProjectId {ProjectId}, EmployeeId {EmployeeId}, StatusId {StatusId}";
        }
    }

    public static class StatusDict
    {
        private static readonly Dictionary<int, string> DictEn = new Dictionary<int, string>
        {
            {0, "New"},
            {1, "In progress"},
            {2, "Resolved"},
            {3, "Closed"},
            {4, "Reopened"}
        };

        private static readonly Dictionary<int, string> DictRu = new Dictionary<int, string>
        {
            {0, "Новая"},
            {1, "В работе"},
            {2, "Решена"},
            {3, "Закрыта"},
            {4, "Переоткрыта"}
        };

        public static Dictionary<int, string> GetStatusDictEn()
        {
            return DictEn;
        }

        public static Dictionary<int, string> GetStatusDictRu()
        {
            return DictRu;
        }

    }
}