using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ITipoVendaServico
    {
        int QuantidadeTipoVendasPorTermo(string termoBusca);

        List<AreaNegocioDto> ObterTipoVendaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina);

	    AreaNegocioDto ObterTipoVendaUsuarioLogado();
    }
}
