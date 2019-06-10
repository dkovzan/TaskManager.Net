using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TaskManager.DAL;
using TaskManager.Exceptions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class ProjectService
    {
        private readonly EntitiesContext _entitiesContext;

        public ProjectService()
        {
            _entitiesContext = new EntitiesContext();
        }

        // async methods
        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _entitiesContext.Projects.ToListAsync();
        }

        public async Task<Project> FindProjectByIdAsync(int id)
        {
            var project = new Project() { Id = id };

            if (id != 0)
            {
                project = await _entitiesContext.Projects.FindAsync(id);

                if (project == null)
                {
                    throw new EntityNotFoundException("Entity not found by id " + id);
                }

                var runtimeIssues = (List<Issue>) HttpContext.Current.Session["runtimeIssues"];

                if (runtimeIssues != null)
                {
                    project.Issues = runtimeIssues;
                }
                else
                {
                    project.Issues = await _entitiesContext.Issues.Where(_ => _.ProjectId == id).ToListAsync();

                    HttpContext.Current.Session["runtimeIssues"] = project.Issues;
                }

            }
            else
            {
                project.Issues = (List<Issue>)HttpContext.Current.Session["runtimeIssues"];
            }
            return project;
        }

        //sync methods
        public void DeleteProjectById(int id)
        {
            var project = new Project() { Id = id };

            _entitiesContext.Projects.Attach(project);
            _entitiesContext.Entry(project).State = EntityState.Deleted;
            _entitiesContext.SaveChanges();
            
        }

        public void AddOrUpdateProject(Project project)
        {
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

        private void AddProjectWithIssues(Project project)
        {
            var issues = (List<Issue>) HttpContext.Current.Session["runtimeIssues"];

            foreach (var issue in issues)
            {
                issue.Employee = null;
                issue.ProjectId = project.Id;
                _entitiesContext.Issues.Add(issue);
            }

            _entitiesContext.Projects.Add(project);
            _entitiesContext.SaveChanges();
        }

        private void UpdateProjectWithIssues(Project project)
        {
            _entitiesContext.Projects.AddOrUpdate(project);

            var issuesFromDb = _entitiesContext.Issues.Where(_ => _.ProjectId == project.Id).ToList();

            var runtimeIssues = (List<Issue>) HttpContext.Current.Session["runtimeIssues"];

            var issuesToDelete = new List<Issue>(issuesFromDb);

            foreach (var runtimeIssue in runtimeIssues)
            {
                if (runtimeIssue.Id > 0)
                {
                    foreach (var issueFromDb in issuesFromDb)
                    {
                        if (issueFromDb.Id == runtimeIssue.Id)
                        {
                            runtimeIssue.ProjectId = project.Id;

                            _entitiesContext.Issues.AddOrUpdate(runtimeIssue);

                            issuesToDelete.Remove(issueFromDb);
                        }
                    }
                }
                else
                {
                    runtimeIssue.Employee = null;
                    runtimeIssue.ProjectId = project.Id;
                    _entitiesContext.Issues.Add(runtimeIssue);
                }
            }

            foreach (var issueToDelete in issuesToDelete)
            {
                _entitiesContext.Issues.Attach(issueToDelete);
                _entitiesContext.Entry(issueToDelete).State = EntityState.Deleted;
            }
            _entitiesContext.SaveChanges();
        }

        public bool IsProjectShortNameUnique(int projectId, string shortName)
        {
            return !_entitiesContext.Projects.Where(x => x.ShortName == shortName && x.Id != projectId).Any();
        }

        ~ProjectService()
        {
            if (_entitiesContext != null)
            {
                _entitiesContext.Dispose();
            }
        }
    }
}