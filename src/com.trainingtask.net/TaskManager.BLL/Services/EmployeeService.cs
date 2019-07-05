using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TaskManager.BLL.Exceptions;
using TaskManager.BLL.Infrastructure;
using TaskManager.BLL.Models;
using TaskManager.DAL;
using TaskManager.DAL.Entities;
using TaskManager.Resources;

namespace TaskManager.BLL.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetEmployees(string searchTerm, string sortColumn, bool isAscending);
        List<EmployeeDto> GetEmployees();
        EmployeeDto FindEmployeeById(int id);
        void DeleteEmployeeById(int id);
        void AddOrUpdateEmployee(EmployeeDto employee);

    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public EmployeeService(IMapper mapper, IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        public List<EmployeeDto> GetEmployees(string searchTerm, string sortColumn, bool isAscending)
        {
            IEnumerable<Employee> employees;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchTerms = searchTerm.ToTermsArray();

                employees = _unitOfWork.EmployeeRepository.Get(
                    _ => searchTerms.All(x => _.FirstName.Contains(x) || _.LastName.Contains(x) || _.MiddleName.Contains(x) || _.Position.Contains(x)) 
                         && _.IsDeleted == 0);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0);
            }

            switch (sortColumn)
            {
                case "FirstName" when isAscending:
                    employees = employees.OrderBy(_ => _.FirstName);
                    break;

                case "FirstName" when !isAscending:
                    employees = employees.OrderByDescending(_ => _.FirstName);
                    break;

                case "LastName" when isAscending:
                    employees = employees.OrderBy(_ => _.LastName);
                    break;

                case "LastName" when !isAscending:
                    employees = employees.OrderByDescending(_ => _.LastName);
                    break;

                case "MiddleName" when isAscending:
                    employees = employees.OrderBy(_ => _.MiddleName);
                    break;

                case "MiddleName" when !isAscending:
                    employees = employees.OrderByDescending(_ => _.MiddleName);
                    break;

                case "Position" when isAscending:
                    employees = employees.OrderBy(_ => _.Position);
                    break;

                case "Position" when !isAscending:
                    employees = employees.OrderByDescending(_ => _.Position);
                    break;

                default:
                    employees = employees.OrderBy(_ => _.Id);
                    break;
            }
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public List<EmployeeDto> GetEmployees()
        {
            return _mapper.Map<List<EmployeeDto>>(_unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0));
        }

        public EmployeeDto FindEmployeeById(int id)
        {
            var employee = _mapper.Map<EmployeeDto>(_unitOfWork.EmployeeRepository.GetById(id));

            if (employee == null || employee.IsDeleted == 1)
            {
                throw new EntityNotFoundException( EmployeeResource.EmployeeNotFoundById + id);
            }

            return employee;
        }

        public void DeleteEmployeeById(int id)
        {
            if (CanEmployeeBeDeleted(id))
            {
                var employee = _unitOfWork.EmployeeRepository.GetById(id);

                employee.IsDeleted = 1;

                _unitOfWork.EmployeeRepository.Update(employee);

                _unitOfWork.Save();
            }
            else
            {
                throw new DaoException(EmployeeResource.EmployeeCannotBeDeleted);
            }
        }

        public void AddOrUpdateEmployee(EmployeeDto employee)
        {
            if (employee.Id == 0)
            {
                _unitOfWork.EmployeeRepository.Add(_mapper.Map<Employee>(employee));
            }
            else
            {
                if (employee.Id != null)
                {
                    var employeeFromDb = _unitOfWork.EmployeeRepository.GetById((int)employee.Id);

                    if (employeeFromDb == null || employeeFromDb.IsDeleted == 1)
                    {
                        throw new EntityNotFoundException(EmployeeResource.EmployeeNotFoundById + employee.Id);
                    }
                }

                _unitOfWork.EmployeeRepository.Update(_mapper.Map<Employee>(employee));
                _unitOfWork.Save();
            }
        }

        private bool CanEmployeeBeDeleted(int id)
        {
            return !_unitOfWork.IssueRepository.Get(_ => _.EmployeeId == id && _.IsDeleted == 0).Any();
        }
    }
}