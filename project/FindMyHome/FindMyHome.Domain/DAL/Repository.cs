using FindMyHome.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace FindMyHome.Domain.DAL
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _dbContext;

        private DbSet<TEntity> _dbSet;

        public Repository(DbContext dbcontext)
        {
            this._dbContext = dbcontext;
            this._dbSet = this._dbContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProps = "")
        {
            IQueryable<TEntity> query = this._dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var prop in includeProps.Split(
                new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }

            return orderBy == null ? query.ToList() : orderBy(query).ToList();
        }

        public virtual TEntity GetById(object id)
        {
            return this._dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            this._dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this._dbSet.Attach(entity);
            this._dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = this._dbSet.Find(id);
            this.Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (this._dbContext.Entry(entity).State == EntityState.Detached)
            {
                this._dbSet.Attach(entity);
            }
            this._dbSet.Remove(entity);
        }
    }
}
