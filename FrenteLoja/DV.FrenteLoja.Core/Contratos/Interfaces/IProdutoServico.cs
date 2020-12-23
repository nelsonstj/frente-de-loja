using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IProdutoServico
	{
		/// <summary>
		/// Metodo usado para obter o saldo do produto em outras lojas Dellavia.
		/// </summary>
		/// <param name="idProduto">Código do produto</param>
		/// <param name="idsLojaDellavia"></param>
		/// <returns></returns>
		//Task<ModalEstoqueDto> ObterSaldoProdutoLojasDellaVia(string idProduto, long[] idsLojaDellavia);

		//ModalDetalhesProdutoDto ObterDadosModalDetalhesProduto(long idOrcamento, string campoCodigoProduto);

		//string ObterHtmlMaisDetalhesProduto(long idProduto);

		//Dictionary<string, decimal> ObterPrecoProdutoPorOrcamento(long idOrcamento, string[] campoCodigoList);
		Task<List<ProdutoProtheusDto>> ObterSaldosProdutoLojaDellaVia(string[] idProdutos, string idLoja);

        int TamanhoTermoFabricantePeca(string termoBusca);
        IQueryable<string> ObterFabricantePecaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina);
    }
}