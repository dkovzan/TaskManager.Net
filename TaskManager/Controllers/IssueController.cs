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
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
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

        [Route("Issue/EditRuntime/{int?}")]
        public async Task<ActionResult> EditRuntime(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.InfoFormat("GET Issue/EditRuntime/{0}", id);

            var issue = _issueService.EditRuntimeIssue((int)id);

            ViewBag.Employees = await _employeeService.GetEmployeesAsync();
            ViewBag.Statuses = StatusDict.GetStatusDict();

            _logger.InfoFormat("Issue sent into view: {0}", issue.ToString());

            return View("EditRuntime", issue);
        }

        [HttpPost]
        [ValidateInput(false)]
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

            _issueService.AddOrUpdateRuntimeIssue(issue);

            return RedirectToAction(actionName: "Edit", routeValues: new { id = issue.ProjectId }, controllerName: "Project");
        }

        public ActionResult DeleteRuntime(int id)
        {
            _logger.InfoFormat("GET Issue/DeleteRuntime/{0}", id);

            _issueService.DeleteRuntimeIssueById(id);

            var projectId = (int) Session["ProjectId"];

            return RedirectToAction(actionName: "Edit", routeValues: new { id = projectId }, controllerName: "Project");
        }
    }
}