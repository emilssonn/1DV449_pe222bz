using HittaMittHem.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HittaMittHem.Domain.DAL
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

        public IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null, 
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

            return orderby == null ? query.ToList() : orderby(query).ToList();
        }

        public TEntity GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
