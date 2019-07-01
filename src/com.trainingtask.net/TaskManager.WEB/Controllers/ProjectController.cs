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

        public override ActionResult List(int page = 1, int pageSize = 5)
        {
            _logger.InfoFormat($"GET Project/List?page={page}&pageSize={pageSize}");

            var projectsFullList = Mapper.Map<List<ProjectDetailsView>>(_projectService.GetProjects());

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

            _logger.InfoFormat($"GET Project/Edit/{id}?isCleanSessionNeeded={isCleanSessionNeeded}");

            if (isCleanSessionNeeded)
            {
                Session.Clear();
            }

            ProjectDetailsView project;

            try
            {
                project = id == 0 ? new ProjectDetailsView { Id = id } : Mapper.Map<ProjectDetailsView>(_projectService.FindProjectById((int)id));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.InfoFormat(ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }

            project.IssuesOfProject = Mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

            _logger.InfoFormat($"Project sent into view: {project}");

            Session["ProjectId"] = project.Id;

            return View(project);
        }

        public override ActionResult Delete(int id)
        {
            _logger.InfoFormat($"GET Project/Delete/{id}");

            _projectService.DeleteProjectById(id);

            _logger.InfoFormat($"Project with id {id} successfully deleted");

            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, ShortName, Description, IssuesOfProject")]ProjectDetailsView project)
        {
            _logger.InfoFormat($"POST Project/AddOrUpdate {project}");

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

            _logger.InfoFormat($"Project {project} successfully added/updated");

            return RedirectToAction(actionName: "List");
        }
    }
}