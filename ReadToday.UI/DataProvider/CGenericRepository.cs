using System;
using System.Data.Entity;

namespace ReadToday.UI.DataProvider
{
    public class CGenericRepository<TEntity,TContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext Context;

        protected CGenericRepository(TContext contex)
        {
            Context = contex;
        }
        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public void Delete(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public virtual TEntity GetById(Guid id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public Boolean HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
