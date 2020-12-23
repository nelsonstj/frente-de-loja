using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IEstoqueProtheusApi
    {
        Task<JArray> BuscaEstoqueProdutoFiliais(string codProduto, string[] codLojasDellaVia);
	    Task<JArray> BuscaEstoqueProdutoFiliais(string[] codProdutos, string codLojaDellaVia);

    }
}