using System.Linq;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IServico<TDto>
    {
        long Cadastrar(TDto entidadeDto);

        IQueryable<TDto> Obter();

        TDto ObterPorId(long? id);

        void Atualizar(TDto entidadeDto);

        void Excluir(long id);
    }
}
