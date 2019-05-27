using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
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
            var project = new Project();
            if (id != 0)
            {
                project = await _entitiesContext.Projects.FindAsync(id);
                if (project == null)
                {
                    throw new EntityNotFoundException("Entity not found by id " + id);
                }
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
            _entitiesContext.Projects.AddOrUpdate(project);
            _entitiesContext.SaveChanges();
        }

        public bool isProjectShortNameUnique(int projectId, string shortName)
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