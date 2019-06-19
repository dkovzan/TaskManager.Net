using AutoMapper;
using System.Collections.Generic;
using System.Web;
using TaskManager.BLL.Models;
using TaskManager.DAL;
using TaskManager.DAL.Entities;

namespace TaskManager.BLL.Services
{
    public interface IIssueService
    {
        List<IssueDto> GetIssues();
        IssueDto FindIssueById(int id);
        void DeleteIssueById(int id);
        void AddOrUpdateIssue(IssueDto issue);

    }

    public class IssueService : IIssueService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public IssueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<IssueDto> GetIssues()
        {
            using (_unitOfWork)
            {
                return _mapper.Map<List<IssueDto>>(_unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee"));
            } 
        }

        public IssueDto FindIssueById(int id)
        {
            using (_unitOfWork)
                return _mapper.Map<IssueDto>(_unitOfWork.IssueRepository.GetById(id));
        }

        public void DeleteIssueById(int id)
        {
            using (_unitOfWork)
            {
                _unitOfWork.IssueRepository.Delete(id);
                _unitOfWork.Save();
            }
                
        }

        public void AddOrUpdateIssue(IssueDto issue)
        {
            using (_unitOfWork)
            {
                if (issue.Id == 0)
                {
                    issue.ProjectDto = null;
                    issue.EmployeeDto = null;
                    _unitOfWork.IssueRepository.Add(_mapper.Map<Issue>(issue));
                    _unitOfWork.Save();
                }
                else
                {
                    issue.ProjectDto = null;
                    issue.EmployeeDto = null;
                    _unitOfWork.IssueRepository.Update(_mapper.Map<Issue>(issue));
                    _unitOfWork.Save();
                }
            }
        }

        private static int newRuntimeTaskId = -1;


        public IssueDto EditRuntimeIssue(int id)
        {

            var projectId = (int)HttpContext.Current.Session["ProjectId"];

            var issue = new IssueDto() { ProjectId = projectId };

            if (id == 0)
            {
                issue.Id = newRuntimeTaskId;
            }
            else
            {
                var runtimeIssues = (List<IssueDto>) HttpContext.Current.Session["runtimeIssues"];

                foreach (var runtimeIssue in runtimeIssues)
                {
                    if (runtimeIssue.Id == id)
                    {
                        issue = runtimeIssue;
                    }
                }
            }

            return issue;
        }

        public void AddOrUpdateRuntimeIssue(IssueDto issue)
        {
            using (_unitOfWork)
            { 
                issue.EmployeeDto = _mapper.Map<EmployeeDto>(_unitOfWork.EmployeeRepository.GetById((int)issue.EmployeeId));

            }
            var runtimeIssues = (List<IssueDto>) HttpContext.Current.Session["runtimeIssues"] ?? new List<IssueDto>();

            for (int i = 0; i < runtimeIssues.Count; i++)
            {
                if (runtimeIssues[i].Id == issue.Id)
                {
                    runtimeIssues.RemoveAt(i);
                    runtimeIssues.Insert(i, issue);
                    break;
                }
            }

            if (issue.Id == newRuntimeTaskId)
            {
                runtimeIssues.Add(issue);
                newRuntimeTaskId--;
            }

            HttpContext.Current.Session["runtimeIssues"] = runtimeIssues;
        }

        public void DeleteRuntimeIssueById(int id)
        {
            var runtimeIssues = (List<IssueDto>) HttpContext.Current.Session["runtimeIssues"];

            for (int i = 0; i < runtimeIssues.Count; i++)
            {
                if (runtimeIssues[i].Id == id)
                {
                    runtimeIssues.RemoveAt(i);
                    break;
                }
            }

            HttpContext.Current.Session["runtimeIssues"] = runtimeIssues;
        }

    }
}