using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManager.Exceptions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        public ProjectController()
        {
            _projectService = new ProjectService();
        }

        //async methods
        public async Task<ActionResult> List()
        {
            var projects = await _projectService.GetProjectsAsync();
            return View(projects);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var employee = await _projectService.FindProjectByIdAsync(id);
                return View(employee);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        public ActionResult Delete(int id)
        {
            _projectService.DeleteProjectById(id);
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, Name, ShortName, Description")]Project project)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", project);
            }
            _projectService.AddOrUpdateProject(project);
            return RedirectToAction("List");
        }
    }
}