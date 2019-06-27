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

        public List<EmployeeDto> GetEmployees()
        {
            return _mapper.Map<List<EmployeeDto>>(_unitOfWork.EmployeeRepository.Get());
        }

        public EmployeeDto FindEmployeeById(int id)
        {
            var employee = _mapper.Map<EmployeeDto>(_unitOfWork.EmployeeRepository.GetById(id));

            if (employee == null)
            {
                throw new EntityNotFoundException("Employee not found by id " + id);
            }

            return employee;
        }

        public void DeleteEmployeeById(int id)
        {
            if (CanEmployeeBeDeleted(id))
            {
                _unitOfWork.EmployeeRepository.Delete(id);
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
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.EmployeeRepository.Update(_mapper.Map<Employee>(employee));
                _unitOfWork.Save();
            }
        }

        private bool CanEmployeeBeDeleted(int id)
        {
            return !_unitOfWork.IssueRepository.Get(_ => _.EmployeeId == id).Any();
        }
    }
}