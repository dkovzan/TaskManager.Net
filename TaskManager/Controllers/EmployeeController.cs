using log4net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManager.Exceptions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILog _logger;

        private readonly EmployeeService _employeeService;

        public EmployeeController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _employeeService = new EmployeeService();
        }

        // async methods
        public async Task<ActionResult> List()
        {
            _logger.Info("GET Employee/List");

            var employees = await _employeeService.GetEmployeesAsync();

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(employees);
        }

        [Route("Employee/Edit/{id?}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            } 

            _logger.InfoFormat("GET Employee/Edit/{0}", id);

            try
            {
                var employee = await _employeeService.FindEmployeeByIdAsync((int) id);

                _logger.InfoFormat("Employee sent into view: {0}", employee.ToString());

                return View(employee);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.WarnFormat("Employee is not found by id {0}", id);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }
        }

        // sync methods
        public ActionResult Delete(int id)
        {
            _logger.InfoFormat("GET Employee/Delete/{0}", id);

            try
            {
                _employeeService.DeleteEmployeeById(id);

                _logger.InfoFormat("Employee with id {0} successfully deleted", id);
            }
            catch (DaoException ex)
            {
                _logger.WarnFormat("Logical error while deleting employee: {0}", ex.Message);

                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(actionName: "List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, FirstName, LastName, MiddleName, Position")]Employee employee)
        {
            _logger.InfoFormat("POST Employee/AddOrUpdate {0}", employee.ToString());

            if (!ModelState.IsValid)
            {
                return View("Edit", employee);
            }

            _employeeService.AddOrUpdateEmployee(employee);

            _logger.InfoFormat("Employee: {0} successfully added/updated", employee.ToString());

            return RedirectToAction(actionName: "List");
        }

    }
}