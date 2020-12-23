using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web.Configuration;

namespace DV.FrenteLoja.Repository
{
    public abstract class AbstractRepository<TEntity, TKey> where TEntity : class
    {
        protected string strConn { get; } = WebConfigurationManager.ConnectionStrings["DellaviaContexto"].ConnectionString;
        protected string strConnFraga { get; } = WebConfigurationManager.ConnectionStrings["Fraga"].ConnectionString;
        protected string strConnPowerData { get; } = WebConfigurationManager.ConnectionStrings["PowerData"].ConnectionString;

        //public abstract List<TEntity> GetAll();
        //public abstract TEntity GetById(TKey id);
        public abstract void Save(TEntity entity);
        public abstract void Update(TEntity entity);
        public abstract void Delete(TEntity entity);
        //public abstract void DeleteById(TKey id);
    }
}