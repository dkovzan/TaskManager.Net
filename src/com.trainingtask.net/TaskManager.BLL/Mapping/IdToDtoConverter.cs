using AutoMapper;
using TaskManager.BLL.Models;
using TaskManager.BLL.Services;

namespace TaskManager.BLL.Mapping
{
    public class ProjectIdToProjectDtoConverter : ITypeConverter<int, ProjectDto>
    {
        private readonly ProjectService _projectService;

        public ProjectIdToProjectDtoConverter (ProjectService projectService)
        {
            _projectService = projectService;
        }
        public ProjectDto Convert(int srcId, ProjectDto dest, ResolutionContext context)
        {
            return _projectService.FindProjectById(System.Convert.ToInt32(srcId));
        }
    }

    public class EmployeeIdToEmployeeDtoConverter : ITypeConverter<int, EmployeeDto>
    {
        private readonly EmployeeService _employeeService;

        public EmployeeIdToEmployeeDtoConverter(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public EmployeeDto Convert(int srcId, EmployeeDto dest, ResolutionContext context)
        {
            return _employeeService.FindEmployeeById(System.Convert.ToInt32(srcId));
        }
    }
}