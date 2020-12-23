using System.Collections.Generic;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IOrcamentoApi
	{
		Task<OrcamentoRetornoPostProtheusDto> EnviarOrcamento(Orcamento orc, string codUsuario);

		Task<List<OrcamentoProtheusDto>> ObterOrcamentos();
		Task<List<OrcamentoConsultaDto>> ObterOrcamentos(OrcamentoFiltroProtheusDto orcamentoFiltroProtheusDto);
		Task<OrcamentoProtheusDto> ObterOrcamentoPorCodProtheus(OrcamentoDto orcDto);
		Task<OrcamentoDto> ObterOrcamentoRelatorio(string codigoOrcamento);
		string AtualizarOrcamento(OrcamentoProtheusDto orcProtheus,long idOrc = 0);
	}
}