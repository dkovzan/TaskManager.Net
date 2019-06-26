using AutoMapper;
using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using TaskManager.BLL.Exceptions;
using TaskManager.BLL.Models;
using TaskManager.BLL.Services;
using TaskManager.WEB.ViewModels;

namespace TaskManager.WEB.Controllers
{
    public class IssueController : BaseController
    {
        private readonly ILog _logger;

        private readonly IssueService _issueService;

        private readonly ProjectService _projectService;

        private readonly EmployeeService _employeeService;

        public IssueController(IssueService issueService, ProjectService projectService, EmployeeService employeeService, IMapper mapper): base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _issueService = issueService;
            _projectService = projectService;
            _employeeService = employeeService;
        }
        public override ActionResult List(int page = 1, int pageSize = 5)
        {

            _logger.InfoFormat("GET Issue/List?page={0}&pageSize={1}", page, pageSize);
            
            var issuesFullList = Mapper.Map<List<IssueInListView>> (_issueService.GetIssues());

            var entitiesListViewPerPage = GetListViewPerPageWithPageInfo(issuesFullList, page, pageSize);

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(new IssuesListView { Issues = entitiesListViewPerPage.EntitiesPerPageList, PageInfo = entitiesListViewPerPage.PageInfo });
        }

        [Route("Issue/Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.InfoFormat("GET Issue/Edit/{0}", id);

            try
            {
                var issue = Mapper.Map<IssueEditView>(_issueService.FindIssueById((int)id)) ?? new IssueEditView { Id = id};
                
                ViewBag.Projects = Mapper.Map<List<ProjectInDropdownView>>(_projectService.GetProjects());
                ViewBag.Employees = Mapper.Map<List<EmployeeInDropdownView>>(_employeeService.GetEmployees());
                ViewBag.Statuses = StatusDict.GetStatusDict();

                _logger.InfoFormat("Issue sent into view: {0}", issue);

                return View(issue);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.WarnFormat("Issue is not found by id {0}", id);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }
        }

        public override ActionResult Delete(int id)
        {
            _logger.InfoFormat("GET Issue/Delete/{0}", id);

            _issueService.DeleteIssueById(id);

            _logger.InfoFormat("Issue with id {0} successfully deleted", id);

            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, Work, BeginDate, EndDate, ProjectId, EmployeeId, StatusId")]IssueEditView issue)
        {
            _logger.InfoFormat("POST Issue/AddOrUpdate {0}", issue);

            if (!ModelState.IsValid)
            {
                ViewBag.Projects = Mapper.Map<List<ProjectInDropdownView>>(_projectService.GetProjects());
                ViewBag.Employees = Mapper.Map<List<EmployeeInDropdownView>>(_employeeService.GetEmployees());
                ViewBag.Statuses = StatusDict.GetStatusDict();

                return View("Edit", issue);
            }
            
            _issueService.AddOrUpdateIssue(Mapper.Map<IssueEditView, IssueDto>(issue));

            _logger.InfoFormat("Issue: {0} successfully added/updated", issue);

            return RedirectToAction(actionName: "List");
        }

        [Route("Issue/EditRuntime/{int?}")]
        public ActionResult EditRuntime(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.InfoFormat("GET Issue/EditRuntime/{0}", id);

            var issue = Mapper.Map<IssueEditView>(_issueService.EditRuntimeIssue((int)id)) ?? new IssueEditView { Id = id };

            ViewBag.Employees = Mapper.Map<List<EmployeeInDropdownView>>(_employeeService.GetEmployees());
            ViewBag.Statuses = StatusDict.GetStatusDict();

            _logger.InfoFormat("Issue sent into view: {0}", issue);

            return View("EditRuntime", issue);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdateRuntime([Bind(Include = "Id, Name, Work, BeginDate, EndDate, ProjectId, EmployeeId, StatusId")]IssueEditView issue)
        {
            _logger.InfoFormat("POST Issue/AddOrUpdate {0}", issue);

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = Mapper.Map<List<EmployeeInDropdownView>>(_employeeService.GetEmployees());
                ViewBag.Statuses = StatusDict.GetStatusDict();

                return View("EditRuntime", issue);
            }

            _issueService.AddOrUpdateRuntimeIssue(Mapper.Map<IssueDto>(issue));

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