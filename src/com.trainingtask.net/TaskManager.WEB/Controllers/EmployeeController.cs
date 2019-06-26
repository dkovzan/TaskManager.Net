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

        public override ActionResult List(int page = 1, int pageSize = 5)
        {

            _logger.InfoFormat("GET Employee/List?page={0}&pageSize={1}", page, pageSize);

            var employeesFullList = Mapper.Map<List<EmployeeDetailsView>>(_employeeService.GetEmployees());

            var entitiesListViewPerPage = GetListViewPerPageWithPageInfo (employeesFullList, page, pageSize);

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(new EmployeesListView { Employees = entitiesListViewPerPage.EntitiesPerPageList, PageInfo = entitiesListViewPerPage.PageInfo });
        }

        [Route("Employee/Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: "List");
            } 

            _logger.InfoFormat("GET Employee/Edit/{0}", id);

            try
            {
                var employee = Mapper.Map<EmployeeDetailsView>(_employeeService.FindEmployeeById((int)id)) ?? new EmployeeDetailsView { Id = id };

                _logger.InfoFormat("Employee sent into view: {0}", employee);

                return View(employee);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.WarnFormat("Employee is not found by id {0}", id);

                TempData["Error"] = ex.Message;

                return RedirectToAction(actionName: "List");
            }
        }

        public override ActionResult Delete(int id)
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
        [ValidateInput(false)] // disable request validation e.g. preventing script attacks >> dangerous values are encoded by Razor automatically
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, FirstName, LastName, MiddleName, Position")]EmployeeDetailsView employee)
        {

            if (!ModelState.IsValid)
            {
                return View("Edit", employee);
            }

            _logger.InfoFormat("POST Employee/AddOrUpdate {0}", employee);

            _employeeService.AddOrUpdateEmployee(Mapper.Map<EmployeeDto>(employee));

            return RedirectToAction(actionName: "List");
        }

    }
}