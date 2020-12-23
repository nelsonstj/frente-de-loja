using System.Collections.Generic;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ISolicitacaoAnaliseCreditoApi
	{
		Task<List<SolicitacaoAnaliseCreditoRetornoProtheus>> PostConsultaAnaliseCredito(List<SolicitacaoAnaliseCredito> analiseCredito);
	}
}