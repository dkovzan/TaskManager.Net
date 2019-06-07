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
            return await _entitiesContext.Issues.ToListAsync();
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

        ~IssueService()
        {
            if (_entitiesContext != null)
            {
                _entitiesContext.Dispose();
            }
        }
    }
}