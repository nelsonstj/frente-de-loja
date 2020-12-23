using System.Data.Entity;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Infra.EntityFramework;

namespace DV.FrenteLoja.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static IRepositorio<T> GetRepository<T>(this DbContext dataContext) where T : class, IEntidade
        {
            return new Repositorio<T>(dataContext);
        }
    }
}
