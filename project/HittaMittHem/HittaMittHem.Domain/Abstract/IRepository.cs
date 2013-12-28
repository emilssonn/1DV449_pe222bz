using System;
using System.Collections.Generic;
using System.Linq;

namespace HittaMittHem.Domain.Abstract
{
    internal interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null,
            Func<System.Linq.IQueryable<TEntity>,
                System.Linq.IOrderedQueryable<TEntity>> orderby = null,
            string includeProps = "");

        TEntity GetById(object id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entity);
    }
}
