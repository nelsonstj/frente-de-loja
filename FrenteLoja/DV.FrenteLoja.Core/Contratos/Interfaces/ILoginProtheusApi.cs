using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ILoginProtheusApi
	{
		Task<JObject> LoginUsuario(string nome, string password);
	}
}