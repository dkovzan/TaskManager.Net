using AutoMapper;
using TaskManager.BLL.Models;
using TaskManager.WEB.ViewModels;

namespace TaskManager.WEB.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {

            CreateMap<EmployeeDto, EmployeeDetailsView>()
                .ForSourceMember(src => src.FullName, opt => opt.DoNotValidate());

            CreateMap<EmployeeDetailsView, EmployeeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<EmployeeDto, EmployeeInDropdownView>()
                .ForSourceMember(src => src.FirstName, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.LastName, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.MiddleName, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Position, opt => opt.DoNotValidate());

            CreateMap<IssueDto, IssueEditView>()
                .ForSourceMember(src => src.ProjectDto, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EmployeeDto, opt => opt.DoNotValidate());

            CreateMap<IssueEditView, IssueDto>()
                .ForMember(dest => dest.EmployeeDto, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.ProjectDto, opt => opt.MapFrom(src => src.ProjectId));

            CreateMap<IssueDto, IssueInListView>()
                .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src =>
                    $"{src.EmployeeDto.FirstName} {src.EmployeeDto.LastName}"))
                .ForMember(dest => dest.ProjectShortName, opt => opt.MapFrom(src => src.ProjectDto.ShortName))
                .ForSourceMember(src => src.ProjectId, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.StatusId, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Work, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EmployeeId, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EmployeeDto, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.ProjectDto, opt => opt.DoNotValidate());

            CreateMap<IssueInListView, IssueDto>()
                .ForMember(dest => dest.EmployeeDto, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.ProjectDto, opt => opt.MapFrom(src => src.ProjectId))
                .ForSourceMember(src => src.ProjectShortName, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EmployeeFullName, opt => opt.DoNotValidate());

            CreateMap<ProjectDto, ProjectDetailsView>()
                .ForMember(dest => dest.IssuesOfProject, opt => opt.MapFrom(src => src.IssuesDto));

            CreateMap<ProjectDetailsView, ProjectDto>()
                .ForMember(dest => dest.IssuesDto, opt => opt.MapFrom(src => src.IssuesOfProject));

            CreateMap<ProjectDto, ProjectInDropdownView>()
                .ForSourceMember(src => src.Name, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Description, opt => opt.DoNotValidate());

        }
    }
}