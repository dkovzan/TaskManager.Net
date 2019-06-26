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
            _logger.InfoFormat("GET Project/List?page={0}&pageSize={1}", page, pageSize);

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

            _logger.InfoFormat("GET Project/Edit/{0}?isCleanSessionNeeded={1}", id, isCleanSessionNeeded);

            try
            {
                if (isCleanSessionNeeded)
                {
                    Session.Clear();
                }
                
                var project = Mapper.Map<ProjectDetailsView>(_projectService.FindProjectById((int)id)) ?? new ProjectDetailsView { Id = id };

                project.IssuesOfProject = Mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

                _logger.InfoFormat("Project sent into view: {0}", project);

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
                _logger.InfoFormat("POST Project/AddOrUpdate {0}", project);

            if (!ModelState.IsValid)
            {
                project.IssuesOfProject = Mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

                return View("Edit", project);
            }
            else
            {
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

                    project.IssuesOfProject = Mapper.Map<List<IssueInListView>>(Session["runtimeIssues"]);

                    return View("Edit", project);
                }
            }

            _logger.InfoFormat("Project {0} successfully added/updated", project);

            return RedirectToAction(actionName: "List");
        }
    }
}