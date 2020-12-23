using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IVeiculoServico
    {
        IQueryable<Marca> GetMarcas();

        int QuantidadeMarcasPorTermo(string termoBusca);

        List<MarcaDto> ObterMarcaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina);

        int TamanhoModelosPorTermo(string termoBusca, long idMarca);

	    int TamanhoVersaoPorTermo(string termoBusca, long idModelo);

        int TamanhoVersaoMotorPorTermo(string termoBusca, long idVersao);

        //Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlacaWS(string placa);

		List<MarcaModeloDto> ObterModelosPeloIdMarca(string termoBusca, int tamanhoPagina, int numeroPagina, long idMarca);

		List<ClienteMarcaModeloVersaoDto> ObterVersoesPeloIdModelo(string termoBusca, int tamanhoPagina, int numeroPagina, long idMarca);

        List<VersaoMotorDTO> ObterVersoesMotorPeloIdVersaoVeiculo(string termoBusca, long idVersao);

        //ClienteMarcaModeloVersaoDto ObterVeiculoPorPlacaDBLocal(string placa);

	    //Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlaca(string placa);

		Cliente ObterClientePorPlaca(string placa);

        ClienteMarcaModeloVersaoDto ObterVeiculoRetira();
    }
}
