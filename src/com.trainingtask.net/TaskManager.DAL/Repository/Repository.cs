using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskManager.DAL.Entities;

namespace TaskManager.DAL.Repository
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
    }

    public class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected readonly EntitiesContext EntitiesContext;
        internal DbSet<T> DbSet;
        public Repository(EntitiesContext entitiesContext)
        {
            EntitiesContext = entitiesContext;
            DbSet = entitiesContext.Set<T>();
        }

        public IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = DbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var properties = includeProperties.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            query = properties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }
        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public int Add(T entity)
        {
            DbSet.Add(entity);
            EntitiesContext.SaveChanges();

            var generatedId = entity.Id;

            return generatedId;
        }

        public void Update(T entity)
        {
            var localEntity = DbSet
                .Local
                .FirstOrDefault(f => f.Id == entity.Id);

            if (localEntity != null)
            {
                EntitiesContext.Entry(localEntity).State = EntityState.Detached;
            }

            EntitiesContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = DbSet.Find(id);
            Delete(entity);
        }

        public void Delete(T entity)
        {
            if (EntitiesContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

    }
}
