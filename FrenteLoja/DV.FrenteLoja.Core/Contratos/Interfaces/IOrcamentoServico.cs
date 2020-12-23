using System.Collections.Generic;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IOrcamentoServico
	{
		//long IniciarOrcamento(OrcamentoDto novoOrcamentoDto);
		//OrcamentoDto ObterOrcamentoPorId(long idOrcamento);
		Task<OrcamentoDto> ObterOrcamentoRelatorioProtheus(string campoCodigo);
		//Task<bool> EnviarOrcamentoProtheus(long idOrcamento);
		List<OrcamentoDto> ObterOrcamentoPorCPF(string termoBusca);
		List<OrcamentoDto> ObterOrcamentoPorCNPJ(string termoBusca);
		List<OrcamentoDto> ObterOrcamentoPorCodigoCliente(string termoBusca);
        Task<string> AtualizarOrcamentoComProtheus(OrcamentoDto orcDto);
        void AtualizarReservaEstoque(long id, bool reservaEstoque);
        //ICollection<OrcamentoConsultaDto> ObterListaOrcamento(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento);
        //ICollection<OrcamentoConsultaDto> ObterListaOrcamentoVencidosHoje();
        //void AtualizarDadosVeiculo(OrcamentoDto orcamentoDto);
		//Task<ICollection<OrcamentoConsultaDto>> ObterListaOrcamentoProtheus(TipoFiltroOrcamento? tipoFiltro, string termoBusca, StatusOrcamento? statusOrcamento);
	}
}