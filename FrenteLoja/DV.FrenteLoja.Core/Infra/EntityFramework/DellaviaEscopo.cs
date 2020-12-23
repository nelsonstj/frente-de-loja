using System;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Extensions;

namespace DV.FrenteLoja.Core.Infra.EntityFramework
{
    public sealed class DellaviaEscopo : IRepositorioEscopo, IDisposable
    {
        private readonly DellaviaContexto _contexto;

        public DellaviaEscopo(DellaviaContexto contexto)
        {
            _contexto = contexto;
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }

        public bool Finalizar()
        {
            try
            {
                return _contexto.SaveChanges() > 0;
            }
            catch (Exception excecao)
            {
                throw new InvalidOperationException("Erro ao finalizar repositorio escopo", excecao);
            }
        }

        IRepositorio<TEntidade> IRepositorioEscopo.GetRepositorio<TEntidade>()
        {
            return _contexto.GetRepository<TEntidade>();
        }
    }
}
