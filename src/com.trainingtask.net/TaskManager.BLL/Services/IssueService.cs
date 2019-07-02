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
    public interface IIssueService
    {
        List<IssueDto> GetIssues(string sortColumn, bool isAscending);
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

        public List<IssueDto> GetIssues(string sortColumn, bool isAscending)
        {
            IEnumerable<Issue> issues;

            switch (sortColumn)
            {
                case "ProjectShortName" when isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.Project.ShortName));
                    break;

                case "ProjectShortName" when !isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.Project.ShortName));
                    break;

                case "Name" when isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.Name));
                    break;

                case "Name" when !isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.Name));
                    break;

                case "BeginDate" when isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.BeginDate));
                    break;

                case "BeginDate" when !isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.BeginDate));
                    break;

                case "EndDate" when isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.EndDate));
                    break;

                case "EndDate" when !isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.EndDate));
                    break;

                case "EmployeeFullName" when isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.Employee.FirstName).ThenBy(x => x.Employee.LastName));
                    break;

                case "EmployeeFullName" when !isAscending:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderByDescending(x => x.Employee.FirstName).ThenByDescending(x => x.Employee.LastName));
                    break;

                default:
                    issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0, orderBy: _ => _.OrderBy(x => x.Id));
                    break;
            }

            return _mapper.Map<List<IssueDto>>(issues);
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
            }
            else
            {
                issue.ProjectDto = null;
                issue.EmployeeDto = null;

                if (issue.Id != null)
                {
                    var issueFromDb = _unitOfWork.IssueRepository.GetById((int)issue.Id);

                    if (issueFromDb == null || issueFromDb.IsDeleted == 1)
                    {
                        throw new EntityNotFoundException("Issue not found by id " + issue.Id);
                    }
                }

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