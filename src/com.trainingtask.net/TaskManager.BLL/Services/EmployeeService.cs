using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TaskManager.BLL.Exceptions;
using TaskManager.BLL.Models;
using TaskManager.DAL;
using TaskManager.DAL.Entities;

namespace TaskManager.BLL.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetEmployees(string sortColumn, bool isAscending);
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

        public List<EmployeeDto> GetEmployees(string sortColumn, bool isAscending)
        {
            IEnumerable<Employee> employees;

            switch (sortColumn)
            {
                case "FirstName" when isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.FirstName));
                    break;

                case "FirstName" when !isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.FirstName));
                    break;

                case "LastName" when isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.LastName));
                    break;

                case "LastName" when !isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.LastName));
                    break;

                case "MiddleName" when isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.MiddleName));
                    break;

                case "MiddleName" when !isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.MiddleName));
                    break;

                case "Position" when isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.Position));
                    break;

                case "Position" when !isAscending:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.Position));
                    break;

                default:
                    employees = _unitOfWork.EmployeeRepository.Get(_ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.Id));
                    break;
            }
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public EmployeeDto FindEmployeeById(int id)
        {
            var employee = _mapper.Map<EmployeeDto>(_unitOfWork.EmployeeRepository.GetById(id));

            if (employee == null || employee.IsDeleted == 1)
            {
                throw new EntityNotFoundException("Employee not found by id " + id);
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
                throw new DaoException("Employee cannot be deleted while is being assigned to task");
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
                        throw new EntityNotFoundException("Employee not found by id " + employee.Id);
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