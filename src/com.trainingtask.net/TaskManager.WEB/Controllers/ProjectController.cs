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

        public ProjectController(ProjectService projectService, IMapper mapper) : base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _projectService = projectService;
        }

        public ActionResult List(string searchTerm, string currentFilter, string sortColumn, bool? isAscending, int? page, int? pageSize)
        {
            _logger.Info($"GET Project/List?page={page}&pageSize={pageSize}");

            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewBag.CurrentFilter = searchTerm;

            var projectsFullList = Mapper.Map<List<ProjectDetailsView>>(_projectService.GetProjects(searchTerm, sortColumn, isAscending ?? true));

            var entitiesListViewPerPage = GetListViewPerPageWithPageInfo(projectsFullList, page, pageSize);
            
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            ViewBag.SortColumn = sortColumn;
            ViewBag.IsAscending = isAscending ?? true;

            return View(new ProjectListView { Projects = entitiesListViewPerPage.EntitiesPerPageList, PageInfo = entitiesListViewPerPage.PageInfo });
        }

        [Route("Project/Edit/{id?}")]
        public ActionResult Edit(int? id, bool isCleanSessionNeeded = false)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.Info($"GET Project/Edit/{id}?isCleanSessionNeeded={isCleanSessionNeeded}");

            if (isCleanSessionNeeded)
            {
                Session.Clear();
            }

            ProjectDetailsView project;

            try
            {
                project = id == 0 ? new ProjectDetailsView { Id = id } : Mapper.Map<ProjectDetailsView>(_projectService.FindProjectById((int)id));

                project.IssuesOfProject = Mapper.Map<List<IssueInListView>> (Session["runtimeIssues"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.Info(ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }

            _logger.Info($"Project sent into view: {project}");

            Session["ProjectId"] = project.Id;

            return View(project);
        }

        public ActionResult Delete(int id)
        {
            _logger.Info($"GET Project/Delete/{id}");

            _projectService.DeleteProjectById(id);

            _logger.Info($"Project with id {id} successfully deleted");

            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, ShortName, Description, IssuesOfProject")]ProjectDetailsView project)
        {
            _logger.Info($"POST Project/AddOrUpdate {project}");

            project.IssuesOfProject = Mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

            if (!ModelState.IsValid)
            {
                return View("Edit", project);
            }

            try
            {
                _projectService.AddOrUpdateProject(Mapper.Map<ProjectDto>(project));
            }
            catch (ValidationException ex)
            {
                foreach (var invalidField in ex.InvalidFieldsWithMessages)
                {
                    ModelState.AddModelError(invalidField.Key, invalidField.Value);
                }

                return View("Edit", project);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.Warn(ex.Message);

                ViewBag.Error = ex.Message;

                return View("Edit", project);
            }

            _logger.Info($"Project {project} successfully added/updated");

            return RedirectToAction(actionName: "List");
        }
    }
}