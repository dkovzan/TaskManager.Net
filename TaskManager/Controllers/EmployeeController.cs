using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManager.Exceptions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController()
        {
            _employeeService = new EmployeeService();
        }

        // async methods
        public async Task<ActionResult> List()
        {
            var employees = await _employeeService.GetEmployeesAsync();

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            return View(employees);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var employee = await _employeeService.FindEmployeeByIdAsync(id);

                return View(employee);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("List");
            }
        }

        // sync methods
        public ActionResult Delete(int id)
        {
            try
            {
                _employeeService.DeleteEmployeeById(id);
            }
            catch (DaoException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, FirstName, LastName, MiddleName, Position")]Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", employee);
            }

            _employeeService.AddOrUpdateEmployee(employee);

            return RedirectToAction("List");
        }

    }
}