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
            var  project = srcId != 0 ? _projectService.FindProjectById(System.Convert.ToInt32(srcId)) : new ProjectDto {Id = srcId};

            return project;
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
            var employee = srcId != 0
                ? _employeeService.FindEmployeeById(System.Convert.ToInt32(srcId))
                : new EmployeeDto {Id = srcId};

            return employee;
        }
    }
}