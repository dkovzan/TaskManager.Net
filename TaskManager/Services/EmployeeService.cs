using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL;
using TaskManager.Exceptions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class EmployeeService
    {
        private readonly EntitiesContext _entitiesContext;

        public EmployeeService()
        {
            _entitiesContext = new EntitiesContext();
        }

        // async methods
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _entitiesContext.Employees.ToListAsync();
        }

        public async Task<Employee> FindEmployeeByIdAsync(int id)
        {
            var employee = new Employee();
            if (id != 0)
            {
                employee = await _entitiesContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    throw new EntityNotFoundException("Entity not found by id " + id);
                }
            }
            return employee;
        }

        //sync methods
        public void DeleteEmployeeById(int id)
        {
            if (CanEmployeeBeDeleted(id))
            {
                var employee = new Employee() { Id = id };

                _entitiesContext.Employees.Attach(employee);
                _entitiesContext.Entry(employee).State = EntityState.Deleted;
                _entitiesContext.SaveChanges();
            }
            else
            {
                throw new DaoException("Employee cannot be deleted while is being assigned to task");
            }
        }

        public void AddOrUpdateEmployee(Employee employee)
        {
            _entitiesContext.Employees.AddOrUpdate(employee);
            _entitiesContext.SaveChanges();
        }

        private bool CanEmployeeBeDeleted(int id)
        {
            return !_entitiesContext.Issues.Where(s => s.EmployeeId == id).Any();
        }

        

        ~EmployeeService()
        {
            if (_entitiesContext != null)
            {
                _entitiesContext.Dispose();
            }
        }
    }
}