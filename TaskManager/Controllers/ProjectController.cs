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
    public class ProjectController : Controller
    {
        private readonly ILog _logger;

        private readonly ProjectService _projectService;

        private readonly EmployeeService _employeeService;
        public ProjectController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _projectService = new ProjectService();
            _employeeService = new EmployeeService();
        }

        //async methods
        public async Task<ActionResult> List()
        {
            _logger.Info("GET Project/List");

            var projects = await _projectService.GetProjectsAsync();

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(projects);
        }

        [Route("Project/Edit/{id?}")]
        public async Task<ActionResult> Edit(int? id, bool isCleanSessionNeeded = false)
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

                var project = await _projectService.FindProjectByIdAsync((int) id);

                ViewBag.Employees = await _employeeService.GetEmployeesAsync();

                _logger.InfoFormat("Project sent into view: {0}", project.ToString());

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

        //sync methods
        public ActionResult Delete(int id)
        {
            _logger.InfoFormat("GET Project/Delete/{0}", id);

            _projectService.DeleteProjectById(id);

            _logger.InfoFormat("Project with id {0} successfully deleted", id);

            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, ShortName, Description, Issues")]Project project)
        {
            _logger.InfoFormat("POST Project/AddOrUpdate {0}", project.ToString());

            if (!ModelState.IsValid)
            {
                project.Issues = (List<Issue>) Session["runtimeIssues"];

                return View("Edit", project);
            }

            _projectService.AddOrUpdateProject(project);

            _logger.InfoFormat("Project {0} successfully added/updated", project.ToString());

            return RedirectToAction(actionName: "List");
        }
    }
}