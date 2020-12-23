using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using EntityFramework.BulkInsert.Extensions;

namespace DV.FrenteLoja.Core.Infra.EntityFramework
{
	class Repositorio<TEntity> : IDisposable,
		IRepositorio<TEntity> where TEntity : class, IEntidade
	{
		public readonly DbContext contexto;

		public Repositorio(DbContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			this.contexto = context;
		}

		public void Dispose()
		{
			contexto.Dispose();
		}

		public TEntity Add(TEntity obj)
		{
			var local = contexto.Set<TEntity>();

			var newObject = local.Add(obj);
			contexto.SaveChanges();
			return newObject;
		}

		public void AddRange(List<TEntity> entities)
		{
			if (entities.Count <= 0) return;
			var local = contexto.Set<TEntity>();
			contexto.BulkInsert(entities);
			contexto.SaveChanges();
		}

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return contexto.Set<TEntity>().Any(predicate);
        }

		public long Count()
		{
			return contexto.Set<TEntity>().Count();
		}

        public long Count(Expression<Func<TEntity, bool>> predicate)
        {            
            return contexto.Set<TEntity>().Count(predicate);
        }

		public TEntity FindByKey(params object[] key)
		{
			return contexto.Set<TEntity>().Find(key);
		}

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }        

		public IQueryable<TEntity> GetAll()
		{
			return contexto.Set<TEntity>();
		}

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return contexto.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            contexto.Set<TEntity>()
                .Where(predicate).ToList()
                .ForEach(del => contexto.Set<TEntity>().Remove(del));
        }

		public void Remove(TEntity entity)
		{
			contexto.Set<TEntity>().Remove(entity);
		}

		public void Update(TEntity obj)
		{
			var local = contexto.Set<TEntity>().Local.FirstOrDefault(f => f.Id == obj.Id);

			if (local != null)
			{
				contexto.Entry(local).State = EntityState.Detached;
			}
			contexto.Set<TEntity>().Attach(obj);
			contexto.Entry(obj).State = EntityState.Modified;
		}
	}
}
