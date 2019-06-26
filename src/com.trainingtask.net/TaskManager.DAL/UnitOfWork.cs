using System;
using TaskManager.DAL.Entities;
using TaskManager.DAL.Repository;

namespace TaskManager.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<Employee> EmployeeRepository { get; }
        Repository<Issue> IssueRepository { get;  }
        Repository<Project> ProjectRepository { get; }
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly EntitiesContext _entitiesContext;

        public UnitOfWork(string connectionString)
        {
            _entitiesContext = new EntitiesContext(connectionString);
        }

        private Repository<Employee> _employeeRepository;

        public Repository<Employee> EmployeeRepository => _employeeRepository ?? (_employeeRepository = new Repository<Employee>(_entitiesContext));

        private Repository<Issue> _issuesRepository;

        public Repository<Issue> IssueRepository => _issuesRepository ?? (_issuesRepository = new Repository<Issue>(_entitiesContext));

        private Repository<Project> _projectsRepository;

        public Repository<Project> ProjectRepository => _projectsRepository ?? (_projectsRepository = new Repository<Project>(_entitiesContext));

        public void Save()
        {
            _entitiesContext.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _entitiesContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
