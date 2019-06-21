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
    public class ProjectController : BaseController
    {
        private readonly ILog _logger;

        private readonly ProjectService _projectService;

        private readonly EmployeeService _employeeService;
        public ProjectController(ProjectService projectService, EmployeeService employeeService, IMapper mapper) : base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _projectService = projectService;
            _employeeService = employeeService;
        }

        public override ActionResult List(int page = 1, int pageSize = 5)
        {

            _logger.InfoFormat("GET Project/List?page={0}&pageSize={1}", page, pageSize);

            var projectsFullList = _mapper.Map<List<ProjectDetailsView>>(_projectService.GetProjects());

            var entitiesListViewPerPage = GetListViewPerPageWithPageInfo(projectsFullList, page, pageSize);
            
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(new ProjectListView { Projects = entitiesListViewPerPage.EntitiesPerPageList, PageInfo = entitiesListViewPerPage.PageInfo });
        }

        [Route("Project/Edit/{id?}")]
        public ActionResult Edit(int? id, bool isCleanSessionNeeded = false)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.InfoFormat("GET Project/Edit/{0}?isCleanSessionNeeded={1}", id, isCleanSessionNeeded);

            try
            {
                if (isCleanSessionNeeded)
                {
                    Session.Clear();
                }
                
                var project = _mapper.Map<ProjectDetailsView>(_projectService.FindProjectById((int)id)) ?? new ProjectDetailsView { Id = id };

                project.IssuesOfProject = _mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

                _logger.InfoFormat("Project sent into view: {0}", project.ToString() ?? string.Empty);

                Session["ProjectId"] = project.Id;

                return View(project);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.InfoFormat("Project is not found by id {0}", id);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }
        }

        public override ActionResult Delete(int id)
        {
            _logger.InfoFormat("GET Project/Delete/{0}", id);

            _projectService.DeleteProjectById(id);

            _logger.InfoFormat("Project with id {0} successfully deleted", id);

            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, ShortName, Description, IssuesOfProject")]ProjectDetailsView project)
        {
            if (project != null)
                _logger.InfoFormat("POST Project/AddOrUpdate {0}", project.ToString());

            if (!ModelState.IsValid)
            {
                project.IssuesOfProject = _mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

                return View("Edit", project);
            }
            else
            {
                try
                {
                    _projectService.AddOrUpdateProject(_mapper.Map<ProjectDto>(project));
                }
                catch (ValidationException ex)
                {
                    foreach (KeyValuePair<string, string> invalidField in ex._invalidFieldsWithMessages)
                    {
                        ModelState.AddModelError(invalidField.Key, invalidField.Value);
                    }

                    project.IssuesOfProject = _mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

                    return View("Edit", project);
                }
            }

            _logger.InfoFormat("Project {0} successfully added/updated", project.ToString());

            return RedirectToAction(actionName: "List");
        }
    }
}