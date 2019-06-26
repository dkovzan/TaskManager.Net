using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.BLL.Exceptions;
using TaskManager.BLL.Models;
using TaskManager.DAL;
using TaskManager.DAL.Entities;

namespace TaskManager.BLL.Services
{
    public interface IProjectService
    {
        List<ProjectDto> GetProjects();
        ProjectDto FindProjectById(int id);
        void DeleteProjectById(int id);
        void AddOrUpdateProject(ProjectDto project);

    }

    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<ProjectDto> GetProjects()
        {
            using (_unitOfWork)
                return _mapper.Map<List<ProjectDto>>(_unitOfWork.ProjectRepository.Get());
        }

        public ProjectDto FindProjectById(int id)
        {
            var project = new ProjectDto() { Id = id };

            using (_unitOfWork)
            { 

                if (id != 0)
                {
                    project = _mapper.Map<ProjectDto>(_unitOfWork.ProjectRepository.GetById(id));

                    if (project == null)
                    {
                        throw new EntityNotFoundException("Entity not found by id " + id);
                    }

                    var runtimeIssues = (List<IssueDto>)HttpContext.Current.Session["runtimeIssues"];

                    if (runtimeIssues == null)
                    {
                        HttpContext.Current.Session["runtimeIssues"] = _mapper.Map<List<IssueDto>>(_unitOfWork.IssueRepository.Get(x => x.ProjectId == id, includeProperties: "Employee"));
                    }

                }
            }

            return project;
        }

        public void DeleteProjectById(int id)
        {
            using (_unitOfWork)
            {
                _unitOfWork.ProjectRepository.Delete(id);
                _unitOfWork.Save();
            }
                
            
        }

        public void AddOrUpdateProject(ProjectDto project)
        {
            Dictionary<string, string> invalidFieldsWithMessages = new Dictionary<string, string>();

            if (!IsProjectShortNameUnique((int) project.Id, project.ShortName))
            {
                invalidFieldsWithMessages.Add("ShortName", "Short name should be unique.");
            }

            if (invalidFieldsWithMessages.Any())
            {
                throw new ValidationException("", invalidFieldsWithMessages);
            }

            
            if (project.Id == 0)
            {

                AddProjectWithIssues(project);
            }
            else
            {
                UpdateProjectWithIssues(project);
            }

            HttpContext.Current.Session.Clear();
        }

        private void AddProjectWithIssues(ProjectDto project)
        {
            var issues = (List<IssueDto>)HttpContext.Current.Session["runtimeIssues"] ?? new List<IssueDto>();

            using (_unitOfWork)
            { 
                int generatedProjectId = _unitOfWork.ProjectRepository.Add(_mapper.Map<Project>(project));

                foreach (var issue in issues)
                {
                    issue.EmployeeDto = null;
                    issue.ProjectDto = null;
                    issue.ProjectId = generatedProjectId;

                    _unitOfWork.IssueRepository.Add(_mapper.Map<Issue>(issue));
                }

                _unitOfWork.Save();
            }
        }

        private void UpdateProjectWithIssues(ProjectDto project)
        {
            var runtimeIssues = (List<IssueDto>)HttpContext.Current.Session["runtimeIssues"];

            using (_unitOfWork)
            {
                _unitOfWork.ProjectRepository.Update(_mapper.Map<Project>(project));

                var issuesFromDb = _unitOfWork.IssueRepository.Get(_ => _.ProjectId == project.Id).ToList();

                var issuesToDelete = new List<Issue>(issuesFromDb);

                foreach (var runtimeIssue in runtimeIssues)
                {
                    if (runtimeIssue.Id < 0)
                    {
                        runtimeIssue.EmployeeDto = null;
                        runtimeIssue.ProjectDto = null;
                        runtimeIssue.ProjectId = project.Id;

                        _unitOfWork.IssueRepository.Add(_mapper.Map<Issue>(runtimeIssue));
                    }
                    else
                    {
                        foreach (var issueFromDb in issuesFromDb)
                        {
                            if (issueFromDb.Id == runtimeIssue.Id)
                            {
                                runtimeIssue.EmployeeDto = null;
                                runtimeIssue.ProjectDto = null;
                                runtimeIssue.ProjectId = (int)project.Id;

                                _unitOfWork.IssueRepository.Update(_mapper.Map<Issue>(runtimeIssue));

                                issuesToDelete.Remove(issueFromDb);
                            }
                        }
                    }
                }

                foreach (var issueToDelete in issuesToDelete)
                {
                    _unitOfWork.IssueRepository.Delete(issueToDelete.Id);
                }

                _unitOfWork.Save();
            }
        }

        private bool IsProjectShortNameUnique(int projectId, string shortName)
        {
            return !(_unitOfWork.ProjectRepository.Get(_ => _.Id != projectId && _.ShortName == shortName).Count() > 0);
        }

    }
}