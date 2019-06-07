using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManager.Exceptions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class IssueController : Controller
    {
        private readonly ILog _logger;

        private readonly IssueService _issueService;

        private readonly ProjectService _projectService;

        private readonly EmployeeService _employeeService;

        public IssueController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _issueService = new IssueService();
            _projectService = new ProjectService();
            _employeeService = new EmployeeService();
        }
        public async Task<ActionResult> List()
        {
            _logger.Info("GET Issue/List");

            var issues = await _issueService.GetIssuesAsync();

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(issues);
        }

        [Route("Issue/Edit/{id?}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.InfoFormat("GET Issue/Edit/{0}", id);

            try
            {
                var issue = await _issueService.FindIssueByIdAsync((int) id);

                ViewBag.Projects = await _projectService.GetProjectsAsync();
                ViewBag.Employees = await _employeeService.GetEmployeesAsync();
                ViewBag.Statuses = StatusDict.GetStatusDict();

                _logger.InfoFormat("Issue sent into view: {0}", issue.ToString());

                return View(issue);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.WarnFormat("Issue is not found by id {0}", id);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }
        }

        public ActionResult Delete(int id)
        {
            _logger.InfoFormat("GET Issue/Delete/{0}", id);

            _issueService.DeleteIssueById(id);

            _logger.InfoFormat("Issue with id {0} successfully deleted", id);

            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdate([Bind(Include = "Id, Name, Work, BeginDate, EndDate, ProjectId, EmployeeId, StatusId")]Issue issue)
        {
            _logger.InfoFormat("POST Issue/AddOrUpdate {0}", issue.ToString());

            if (!ModelState.IsValid)
            {
                ViewBag.Projects = await _projectService.GetProjectsAsync();
                ViewBag.Employees = await _employeeService.GetEmployeesAsync();
                ViewBag.Statuses = StatusDict.GetStatusDict();

                return View("Edit", issue);
            }

            _issueService.AddOrUpdateIssue(issue);

            _logger.InfoFormat("Issue: {0} successfully added/updated", issue.ToString());

            return RedirectToAction(actionName: "List");
        }

        private static int newRuntimeTaskId = -1;

        public async Task<ActionResult> EditRuntime(int id)
        {
            _logger.InfoFormat("GET Issue/EditRuntime/{0}", id);

            var projectId = (int) Session["ProjectId"];

            var issue = new Issue() { ProjectId = projectId };

            if (id == 0)
            {
                issue.Id = newRuntimeTaskId;
            }
            else
            {
                var runtimeIssues = (List<Issue>) Session["runtimeIssues"];

                foreach (var runtimeIssue in runtimeIssues)
                {
                    if (runtimeIssue.Id == id)
                    {
                        issue = runtimeIssue;
                    }
                }

            }

            //ViewBag.Projects = await _projectService.GetProjectsAsync();
            ViewBag.Employees = await _employeeService.GetEmployeesAsync();
            ViewBag.Statuses = StatusDict.GetStatusDict();

            _logger.InfoFormat("Issue sent into view: {0}", issue.ToString());

            return View("EditRuntime", issue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdateRuntime([Bind(Include = "Id, Name, Work, BeginDate, EndDate, ProjectId, EmployeeId, StatusId")]Issue issue)
        {
            _logger.InfoFormat("POST Issue/AddOrUpdate {0}", issue.ToString());

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = await _employeeService.GetEmployeesAsync();
                ViewBag.Statuses = StatusDict.GetStatusDict();

                return View("EditRuntime", issue);
            }

            var runtimeIssues = (List<Issue>) Session["runtimeIssues"] ?? new List<Issue>();

            for (int i = 0; i < runtimeIssues.Count; i++)
            {
                if (runtimeIssues[i].Id == issue.Id)
                {
                    runtimeIssues.RemoveAt(i);
                    runtimeIssues.Insert(i, issue);

                    _logger.InfoFormat("Runtime issue updated {0}", issue.ToString());

                    break;
                }
            }

            if (issue.Id == newRuntimeTaskId)
            {
                runtimeIssues.Add(issue);

                _logger.InfoFormat("Runtime issue added {0}", issue.ToString());

                newRuntimeTaskId--;
            }

            Session["runtimeIssues"] = runtimeIssues;

            return RedirectToAction(actionName: "Edit", routeValues: new { id = issue.ProjectId }, controllerName: "Project");
        }

        public ActionResult DeleteRuntime(int id)
        {
            _logger.InfoFormat("GET Issue/DeleteRuntime/{0}", id);

            var runtimeIssues = (List<Issue>) Session["runtimeIssues"];

            for (int i = 0; i < runtimeIssues.Count; i++)
            {
                if (runtimeIssues[i].Id == id)
                {
                    runtimeIssues.RemoveAt(i);

                    _logger.InfoFormat("Runtime issue removed {0}", runtimeIssues[i].ToString());

                    break;
                }
            }

            Session["runtimeIssues"] = runtimeIssues;

            var projectId = (int) Session["ProjectId"];

            return RedirectToAction(actionName: "Edit", routeValues: new { id = projectId }, controllerName: "Project");
        }
    }
}