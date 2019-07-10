using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.BLL.Exceptions;
using TaskManager.BLL.Infrastructure;
using TaskManager.BLL.Models;
using TaskManager.DAL;
using TaskManager.DAL.Entities;
using TaskManager.Resources;

namespace TaskManager.BLL.Services
{
    public interface IIssueService
    {
        List<IssueDto> GetIssues(string searchTerm, string sortColumn, bool isAscending, string culture);
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

        public List<IssueDto> GetIssues(string searchTerm, string sortColumn, bool isAscending, string culture)
        {
            IEnumerable<Issue> issues;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchTerms = searchTerm.ToTermsArray();

                if (culture.Equals("ru"))
                {
                    issues = _unitOfWork.IssueRepository.Get(
                        includeProperties: "Project,Employee",
                        filter: _ => _.IsDeleted == 0).Where(_ => searchTerms.All(x => _.BeginDate.Date.ToShortDateString().Contains(x) ||
                                                                                       _.EndDate.Date.ToShortDateString().Contains(x) ||
                                                                                       _.Work.ToString().Contains(x) ||
                                                                                       _.Employee.FirstName.ToLower().Contains(x.ToLower()) ||
                                                                                       _.Employee.LastName.ToLower().Contains(x.ToLower()) ||
                                                                                       _.Project.ShortName.ToLower().Contains(x.ToLower()) ||
                                                                                       _.Name.ToLower().Contains(x.ToLower())));
                }
                else
                {
                    issues = _unitOfWork.IssueRepository.Get(
                        includeProperties: "Project,Employee",
                        filter: _ => _.IsDeleted == 0).Where(_ => searchTerms.All(x => _.BeginDate.Date.ToShortDateString().Contains(x) ||
                                                                                       _.EndDate.Date.ToShortDateString().Contains(x) ||
                                                                                       _.Work.ToString().Contains(x) ||
                                                                                       _.Employee.FirstName.ToLower().Contains(x.ToLower()) ||
                                                                                       _.Employee.LastName.ToLower().Contains(x.ToLower()) ||
                                                                                       _.Project.ShortName.ToLower().Contains(x.ToLower()) ||
                                                                                       _.Name.ToLower().Contains(x.ToLower())));
                }


            }
            else
            {
                issues = _unitOfWork.IssueRepository.Get(includeProperties: "Project,Employee", filter: _ => _.IsDeleted == 0);
            }

            switch (sortColumn)
            {
                case "ProjectShortName" when isAscending:
                    issues = issues.OrderBy(_ => _.Project.ShortName);
                    break;

                case "ProjectShortName" when !isAscending:
                    issues = issues.OrderByDescending(_ => _.Project.ShortName);
                    break;

                case "Name" when isAscending:
                    issues = issues.OrderBy(_ => _.Name);
                    break;

                case "Name" when !isAscending:
                    issues = issues.OrderByDescending(_ => _.Name);
                    break;

                case "BeginDate" when isAscending:
                    issues = issues.OrderBy(_ => _.BeginDate);
                    break;

                case "BeginDate" when !isAscending:
                    issues = issues.OrderByDescending(_ => _.BeginDate);
                    break;

                case "EndDate" when isAscending:
                    issues = issues.OrderBy(_ => _.EndDate);
                    break;

                case "EndDate" when !isAscending:
                    issues = issues.OrderByDescending(_ => _.EndDate);
                    break;

                case "EmployeeFullName" when isAscending:
                    issues = issues.OrderBy(_ => _.Employee.FirstName).ThenBy(_ => _.Employee.LastName);
                    break;

                case "EmployeeFullName" when !isAscending:
                    issues = issues.OrderByDescending(_ => _.Employee.FirstName).ThenByDescending(_ => _.Employee.LastName);
                    break;

                default:
                    issues = issues.OrderBy(_ => _.Id);
                    break;
            }

            return _mapper.Map<List<IssueDto>>(issues);
        }

        public List<IssueDto> GetIssues()
        {
            return _mapper.Map<List<IssueDto>>(_unitOfWork.IssueRepository.Get(_ => _.IsDeleted == 0,
                includeProperties: "Employee,Project"));
        }

        public IssueDto FindIssueById(int id)
        {
            var issue = _mapper.Map<IssueDto>(_unitOfWork.IssueRepository.GetById(id));

            if (issue == null || issue.IsDeleted == 1)
            {
                throw new EntityNotFoundException(IssueResource.IssueNotFoundById + id);
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
                        throw new EntityNotFoundException(IssueResource.IssueNotFoundById + issue.Id);
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
                throw new EntityNotFoundException(IssueResource.IssueNotFoundById + id);
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