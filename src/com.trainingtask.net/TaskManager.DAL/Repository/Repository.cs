using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskManager.DAL.Entities;

namespace TaskManager.DAL.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        T GetById(int id);
        int Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }

    public class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected readonly EntitiesContext _entitiesContext;
        internal DbSet<T> _dbSet;
        public Repository(EntitiesContext entitiesContext)
        {
            _entitiesContext = entitiesContext;
            _dbSet = entitiesContext.Set<T>();
        }

        public IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var includeProperty in properties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public int Add(T entity)
        {
            _dbSet.Add(entity);
            Save();

            int generatedId = entity.Id;

            return generatedId;
        }

        public void Update(T entity)
        {
            
            _dbSet.Attach(entity);
            _entitiesContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public void Delete(T entity)
        {
            if (_entitiesContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void Save()
        {
            _entitiesContext.SaveChanges();
        }

    }
}
