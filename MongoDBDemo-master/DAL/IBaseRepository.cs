using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        IEnumerable<TEntity> GetAll(string collection);
        long Count(string collection);
        TEntity GetSingle(string id, string collection);
        TEntity GetSingleItemPredicate(Expression<Func<TEntity, bool>> predicate, string collection);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string collection);
        void Add(TEntity entity, string collection);
        void Update(TEntity entity, string collection);
        void Delete(TEntity entity, string collection);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate, string collection);
    }
}
