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
    public class EmployeeController : BaseController
    {
        private readonly ILog _logger;

        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService, IMapper mapper) : base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _employeeService = employeeService;
        }

        public ActionResult List(string searchTerm, string currentFilter, string sortColumn, bool? isAscending, int? page, int? pageSize)
        {
            _logger.Info($"GET Employee/List?page={page}&pageSize={pageSize}");

            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewBag.CurrentFilter = searchTerm;

            var employeesFullList = Mapper.Map<List<EmployeeDetailsView>>(_employeeService.GetEmployees(searchTerm, sortColumn, isAscending ?? true));

            var entitiesListViewPerPage = GetListViewPerPageWithPageInfo(employeesFullList, page, pageSize);

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            ViewBag.SortColumn = sortColumn;
            ViewBag.IsAscending = isAscending ?? true;

            return View(new EmployeesListView { Employees = entitiesListViewPerPage.EntitiesPerPageList, PageInfo = entitiesListViewPerPage.PageInfo });
        }

        [Route("Employee/Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            }

            _logger.Info($"GET Employee/Edit/{id}");

            EmployeeDetailsView employee;

            try
            {
                employee = id == 0
                    ? new EmployeeDetailsView { Id = id }
                    : Mapper.Map<EmployeeDetailsView>(_employeeService.FindEmployeeById((int)id));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.Warn(ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }

            _logger.Info($"Employee sent into view: {employee}");

            return View(employee);
        }

        public ActionResult Delete(int id)
        {
            _logger.InfoFormat($"GET Employee/Delete/{id}");

            try
            {
                _employeeService.DeleteEmployeeById(id);
            }
            catch (DaoException ex)
            {
                _logger.Warn($"Logical error while deleting employee: {ex.Message}");

                TempData["Error"] = ex.Message;
            }

            _logger.InfoFormat($"Employee with id {id} successfully deleted");

            return RedirectToAction(actionName: "List");
        }


        [HttpPost]
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, FirstName, LastName, MiddleName, Position")] EmployeeDetailsView employee)
        {

            if (!ModelState.IsValid)
            {
                return View("Edit", employee);
            }

            _logger.Info($"POST Employee/AddOrUpdate {employee}");

            try
            {
                _employeeService.AddOrUpdateEmployee(Mapper.Map<EmployeeDto>(employee));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.Warn(ex.Message);

                ViewBag.Error = ex.Message;

                return View("Edit", employee);
            }
            

            return RedirectToAction(actionName: "List");
        }

    }
}