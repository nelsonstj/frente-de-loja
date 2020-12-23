using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICargaCadastrosProtheusSyncApi
    {
		/// <summary>
		/// Método genérico que consulta o endpoint do Protheus que atualiza os cadastros satelites do sistema. (Cab. de orçamento).
		/// </summary>
		/// <param name="endpoint">Tipo do endpoint usado para a consulta</param>
		/// <param name="tabelaProtheus">O tipo da tabela consultado.</param>
		/// <returns></returns>
		Task<JArray> GetDataFromKeyTable(EndpointProtheus endpoint,TipoTabelaProtheus tabelaProtheus);
    }
}