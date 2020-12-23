using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IClienteServico
	{
		//ClienteDto ObterClientePorId(long id);
		//ClienteDto ObterClientePorCnpj(string cnpj);
        //ClienteDto ObterClientePorCpf(string cpf);
        //ClienteDto ObterClientePorCodigo(string codigo);
		//int TamanhoClientePorTermo(string termoBusca);
		ClienteDto ObterClientePorNome(string nome);
		//List<ClienteDto> ObterClientePorNome(string termoBusca, int tamanhoPagina, int numeroPagina);
	}
}