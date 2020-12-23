using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICheckupServico<T> : IServico<T>
    {       
        List<T> ObterCheckupsCar();

        List<T> ObterCheckupsTruck();

        List<T> ObterCheckupsNomeCliente(string termoBusca);

        List<T> ObterCheckupsCPF(string termoBusca);
        List<T> ObterCheckupsCNPJ(string termoBusca);

        List<T> ObterCheckupsPlaca(string termoBusca);

        List<T> ObterCheckupsVeiculo(string termoBusca);

        List<T> ObterCheckupsIdOrcamento(string termoBusca);

        List<T> ObterCheckupsUsuario();
    }
}
