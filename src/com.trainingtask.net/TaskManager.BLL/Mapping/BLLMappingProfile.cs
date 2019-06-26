using AutoMapper;
using TaskManager.BLL.Models;
using TaskManager.DAL.Entities;

namespace TaskManager.BLL.Mapping
{
    public class BllMappingProfile : Profile
    {
        public BllMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<EmployeeDto, Employee>()
                .ForSourceMember(src => src.FullName, opt => opt.DoNotValidate());

            CreateMap<Issue, IssueDto>()
                .ForMember(dest => dest.ProjectDto, opt => opt.MapFrom(src => src.Project))
                .ForMember(dest => dest.EmployeeDto, opt => opt.MapFrom(src => src.Employee));

            CreateMap<IssueDto, Issue>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.ProjectDto))
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.EmployeeDto));

            CreateMap<int, ProjectDto>().ConvertUsing<ProjectIdToProjectDtoConverter>();

            CreateMap<int, EmployeeDto>().ConvertUsing<EmployeeIdToEmployeeDtoConverter>();

            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.IssuesDto, opt => opt.MapFrom(src => src.Issues));

            CreateMap<ProjectDto, Project>()
                .ForMember(dest => dest.Issues, opt => opt.MapFrom(src => src.IssuesDto));
        }

    }
}
