using AutoMapper;
using System.Collections.Generic;
using System.Web;
using TaskManager.BLL.Exceptions;
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
            return _mapper.Map<List<IssueDto>>(_unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0));
        }

        public IssueDto FindIssueById(int id)
        {
            var issue = _mapper.Map<IssueDto>(_unitOfWork.IssueRepository.GetById(id));

            if (issue == null || issue.IsDeleted == 1)
            {
                throw new EntityNotFoundException("Issue not found by id " + id);
            }

            return issue;
        }

        public void DeleteIssueById(int id)
        {
            var issue = _unitOfWork.IssueRepository.GetById(id);

            issue.IsDeleted = 1;

            _unitOfWork.IssueRepository.Update(issue);

            _unitOfWork.Save();
        }

        public void AddOrUpdateIssue(IssueDto issue)
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

        private static int _newRuntimeTaskId = -1;

        public IssueDto EditRuntimeIssue(int id)
        {
            IssueDto issue = null;

            if (id == 0)
            {
                issue = new IssueDto {Id = _newRuntimeTaskId};
            }
            else
            {
                var runtimeIssues = (List<IssueDto>) HttpContext.Current.Session["runtimeIssues"];

                foreach (var runtimeIssue in runtimeIssues)
                {
                    if (runtimeIssue.Id != id) continue;
                    issue = runtimeIssue;
                    break;
                }
            }

            if (issue == null)
            {
                throw new EntityNotFoundException($"Runtime issue is not found by id: {id}");
            }

            return issue;
        }

        public void AddOrUpdateRuntimeIssue(IssueDto issue)
        {
            if (issue.EmployeeId != null)
            {
                issue.EmployeeDto =
                    _mapper.Map<EmployeeDto>(_unitOfWork.EmployeeRepository.GetById((int)issue.EmployeeId));
            }

            var runtimeIssues = (List<IssueDto>)HttpContext.Current.Session["runtimeIssues"] ?? new List<IssueDto>();

            for (var i = 0; i < runtimeIssues.Count; i++)
            {
                if (runtimeIssues[i].Id != issue.Id) continue;
                runtimeIssues.RemoveAt(i);
                runtimeIssues.Insert(i, issue);
                break;
            }

            if (issue.Id == _newRuntimeTaskId)
            {
                runtimeIssues.Add(issue);
                _newRuntimeTaskId--;
            }

            HttpContext.Current.Session["runtimeIssues"] = runtimeIssues;
        }

        public void DeleteRuntimeIssueById(int id)
        {
            var runtimeIssues = (List<IssueDto>)HttpContext.Current.Session["runtimeIssues"];

            for (var i = 0; i < runtimeIssues.Count; i++)
            {
                if (runtimeIssues[i].Id != id) continue;
                runtimeIssues.RemoveAt(i);
                break;
            }

            HttpContext.Current.Session["runtimeIssues"] = runtimeIssues;
        }

    }
}