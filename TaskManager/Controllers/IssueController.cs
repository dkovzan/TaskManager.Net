using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManager.Exceptions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueService _issueService;
        private readonly ProjectService _projectService;
        private readonly EmployeeService _employeeService;

        public IssueController()
        {
            _issueService = new IssueService();
            _projectService = new ProjectService();
            _employeeService = new EmployeeService();
        }
        public async Task<ActionResult> List()
        {
            var issues = await _issueService.GetIssuesAsync();
            
            return View(issues);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var issue = await _issueService.FindIssueByIdAsync(id);

                ViewBag.Projects = await _projectService.GetProjectsAsync();
                ViewBag.Employees = await _employeeService.GetEmployeesAsync();
                ViewBag.Statuses = StatusDict.GetStatusDict();

                return View(issue);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        public ActionResult Delete(int id)
        {
            _issueService.DeleteIssueById(id);
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, Work, BeginDate, EndDate, ProjectId, EmployeeId, StatusId")]Issue issue)
        {
           
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = _projectService.GetProjectsAsync();
                ViewBag.Employees = _employeeService.GetEmployeesAsync();
                ViewBag.Statuses = StatusDict.GetStatusDict();
                return View("Edit", issue);
            }

            _issueService.AddOrUpdateIssue(issue);
            return RedirectToAction("List");
        }
    }
}