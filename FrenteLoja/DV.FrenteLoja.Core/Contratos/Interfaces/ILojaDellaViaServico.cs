using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ILojaDellaViaServico
	{
		int TamanhoLojasPorTermo(string termoBusca);
		List<LojaDellaViaDto> ObterLojasPorNome(string termoBusca, int tamanhoPagina, int numeroPagina);
		LojaDellaViaDto ObterLojaUsuarioLogado();
		List<LojaDellaViaDto> ObterLojasProximas(long iDLojaDellavia);

        List<LojaDellaViaDto> ObterLojasPorId(long id);


    }
}