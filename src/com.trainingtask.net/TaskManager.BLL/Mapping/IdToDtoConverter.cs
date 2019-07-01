using AutoMapper;
using TaskManager.BLL.Models;
using TaskManager.DAL;

namespace TaskManager.BLL.Mapping
{
    public class ProjectIdToProjectDtoConverter : ITypeConverter<int, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ProjectIdToProjectDtoConverter (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ProjectDto Convert(int srcId, ProjectDto dest, ResolutionContext context)
        {
            var  project = srcId != 0 
                ? _mapper.Map<ProjectDto> (_unitOfWork.ProjectRepository.GetById(srcId)) 
                : new ProjectDto {Id = srcId};

            return project;
        }
    }

    public class EmployeeIdToEmployeeDtoConverter : ITypeConverter<int, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;


        public EmployeeIdToEmployeeDtoConverter(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public EmployeeDto Convert(int srcId, EmployeeDto dest, ResolutionContext context)
        {
            var employee = srcId != 0
                ? _mapper.Map<EmployeeDto> (_unitOfWork.EmployeeRepository.GetById(srcId))
                : new EmployeeDto {Id = srcId};

            return employee;
        }
    }
}