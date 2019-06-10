using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Web;
using TaskManager.DAL;
using TaskManager.Exceptions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class IssueService
    {
        private readonly EntitiesContext _entitiesContext;

        public IssueService()
        {
            _entitiesContext = new EntitiesContext();
        }

        // async methods
        public async Task<List<Issue>> GetIssuesAsync()
        {
            var issues = await _entitiesContext.Issues.Include("Project").Include("Employee").ToListAsync();
            return issues;
        }

        public async Task<Issue> FindIssueByIdAsync(int id)
        {
            var issue = new Issue();

            if (id != 0)
            {
                issue = await _entitiesContext.Issues.FindAsync(id);

                if (issue == null)
                {
                    throw new EntityNotFoundException("Entity not found by id " + id);
                }
            }
            return issue;
        }

        //sync methods
        public void DeleteIssueById(int id)
        {
            var issue = new Issue() { Id = id };

            _entitiesContext.Issues.Attach(issue);
            _entitiesContext.Entry(issue).State = EntityState.Deleted;
            _entitiesContext.SaveChanges();

        }

        public void AddOrUpdateIssue(Issue issue)
        {
            _entitiesContext.Issues.AddOrUpdate(issue);
            _entitiesContext.SaveChanges();
        }

        private static int newRuntimeTaskId = -1;

        public Issue EditRuntimeIssue(int id)
        {
            var projectId = (int)HttpContext.Current.Session["ProjectId"];

            var issue = new Issue() { ProjectId = projectId };

            if (id == 0)
            {
                issue.Id = newRuntimeTaskId;
            }
            else
            {
                var runtimeIssues = (List<Issue>)HttpContext.Current.Session["runtimeIssues"];

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

        public void AddOrUpdateRuntimeIssue(Issue issue)
        {
            issue.Employee = _entitiesContext.Employees.Find(issue.EmployeeId);

            var runtimeIssues = (List<Issue>)HttpContext.Current.Session["runtimeIssues"] ?? new List<Issue>();

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
            var runtimeIssues = (List<Issue>)HttpContext.Current.Session["runtimeIssues"];

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

        ~IssueService()
        {
            if (_entitiesContext != null)
            {
                _entitiesContext.Dispose();
            }
        }
    }
}