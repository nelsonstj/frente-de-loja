using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IRepositorio<TEntity> where TEntity : class, IEntidade
    {
        long Count();
        long Count(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
        TEntity FindByKey(params object[] key);
        void Update(TEntity obj);
        TEntity Add(TEntity obj);
        void AddRange(List<TEntity> objs);
        void Remove(Expression<Func<TEntity, bool>> predicate);
        void Remove(TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> predicate);
    }
}
