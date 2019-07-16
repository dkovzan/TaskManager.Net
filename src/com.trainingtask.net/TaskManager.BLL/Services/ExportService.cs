using OfficeOpenXml;
using System.Globalization;

namespace TaskManager.BLL.Services
{
    public class ExportService
    {
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        private readonly IssueService _issueService;

        public ExportService(EmployeeService employeeService, ProjectService projectService, IssueService issueService)
        {
            _issueService = issueService;
            _projectService = projectService;
            _employeeService = employeeService;
        }

        public ExcelPackage GenerateReport(string code)
        {
            ExcelPackage report;

            switch (code)
            {
                case "Employee":
                    report = GenerateEmployeesReport();
                    break;

                case "Project":
                    report = GenerateProjectsReport();
                    break;

                case "Issue":
                    report = GenerateIssuesReport();
                    break;

                default:
                    report = GenerateEmployeesReport();
                    break;
            }

            return report;
        }

        private ExcelPackage GenerateEmployeesReport()
        {
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.Cells[1, 1].Value = Resources.EmployeeResource.FirstName;
            workSheet.Cells[1, 2].Value = Resources.EmployeeResource.LastName;
            workSheet.Cells[1, 3].Value = Resources.EmployeeResource.MiddleName;
            workSheet.Cells[1, 4].Value = Resources.EmployeeResource.Position;

            MakeBold(workSheet.Cells[1, 1, 1, 4]);

            var employees = _employeeService.GetEmployees();

            for (var i = 0; i < employees.Count; i++)
            {
                workSheet.Cells[2 + i, 1].Value = employees[i].FirstName;
                workSheet.Cells[2 + i, 2].Value = employees[i].LastName;
                workSheet.Cells[2 + i, 3].Value = employees[i].MiddleName;
                workSheet.Cells[2 + i, 4].Value = employees[i].Position;
            }

            workSheet.Cells.AutoFitColumns();

            return excel;
        }

        private ExcelPackage GenerateProjectsReport()
        {
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.Cells[1, 1].Value = Resources.ProjectResource.Name;
            workSheet.Cells[1, 2].Value = Resources.ProjectResource.ShortName;
            workSheet.Cells[1, 3].Value = Resources.ProjectResource.Description;

            MakeBold(workSheet.Cells[1, 1, 1, 3]);

            var projects = _projectService.GetProjects();

            for (var i = 0; i < projects.Count; i++)
            {
                workSheet.Cells[2 + i, 1].Value = projects[i].Name;
                workSheet.Cells[2 + i, 2].Value = projects[i].ShortName;
                workSheet.Cells[2 + i, 3].Value = projects[i].Description;
            }

            workSheet.Cells.AutoFitColumns();

            return excel;
        }

        private ExcelPackage GenerateIssuesReport()
        {
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.Cells[1, 2].Value = Resources.IssueResource.IssueProject;
            workSheet.Cells[1, 1].Value = Resources.IssueResource.Name;
            workSheet.Cells[1, 3].Value = Resources.IssueResource.BeginDate;
            workSheet.Cells[1, 4].Value = Resources.IssueResource.EndDate;
            workSheet.Cells[1, 5].Value = Resources.IssueResource.Work;
            workSheet.Cells[1, 6].Value = Resources.IssueResource.IssueEmployee;

            MakeBold(workSheet.Cells[1, 1, 1, 6]);

            var issues = _issueService.GetIssues();

            for (var i = 0; i < issues.Count; i++)
            {
                workSheet.Cells[2 + i, 2].Value = issues[i].ProjectDto.ShortName;
                workSheet.Cells[2 + i, 1].Value = issues[i].Name;
                workSheet.Cells[2 + i, 3].Value = issues[i].BeginDate;
                workSheet.Cells[2 + i, 4].Value = issues[i].EndDate;
                workSheet.Cells[2 + i, 5].Value = issues[i].Work;
                workSheet.Cells[2 + i, 6].Value = issues[i].EmployeeDto.FullName;
            }

            workSheet.Cells[2, 3, 2 + issues.Count, 4].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

            workSheet.Cells.AutoFitColumns();

            return excel;
        }

        private static void MakeBold(ExcelRange range)
        {
            range.Style.Font.Bold = true;
        }
    }
}
