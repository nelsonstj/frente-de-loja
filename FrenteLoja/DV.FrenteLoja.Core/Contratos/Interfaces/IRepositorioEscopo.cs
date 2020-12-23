namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IRepositorioEscopo
    {
        IRepositorio<T> GetRepositorio<T>() where T : class, IEntidade;
        bool Finalizar();
    }
}
