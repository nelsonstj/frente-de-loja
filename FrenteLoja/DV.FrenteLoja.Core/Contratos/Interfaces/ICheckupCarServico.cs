using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICheckupCarServico<T> : IServico<T>
    {
        int TamanhoModelosPorTermo(string termoBusca);
        List<string> ObterEspecificacao(string termoBusca, int tamanhoPagina, int numeroPagina);
        
    }
}
