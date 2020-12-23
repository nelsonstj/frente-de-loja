using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface IOrcamentoItemServico
	{
		/// <summary>
		/// Responsável por Inserir itens no orçamento.
		/// </summary>
		/// <param name="modalDetalhesProdutoDto">Data trans</param>
		/// <returns>OrcamentoProdutoBuscaDto obj que contem as informações necessarias para popular a partial view da sacola</returns>
		//OrcamentoProdutoBuscaDto InserirItensOrcamento(ModalDetalhesProdutoDto modalDetalhesProdutoDto);

		//OrcamentoProdutoBuscaDto AtualizarItensOrcamento(ModalDetalhesProdutoDto modalDetalhesProdutoDto);

		OrcamentoProdutoBuscaDto ObterOrcamentoProdutoBuscaDtoPorOrcamentoId(long idOrcamento);

		//OrcamentoProdutoBuscaDto RemoverItensOrcamento(long OrcamentoId, long OrcamentoItemId);

		//ModalDetalhesProdutoDto ObterDadosModalDetalhesProduto(long idOrcamentoItemPai);

		//void AplicarDescontoItensOrcamento(AplicarDescontoDto aplicarDescontoDto);
		//AplicarDescontoDto ObterItemOrcamentoDesconto(long orcamentoId, long orcamentoItemId);

		int TamanhoProfissionalMontagemPorTermo(string termoBusca, long idFilial);

		List<ProfissionalMontagemDto> ObterProfissionalMontagemPorNome(string termoBusca, int tamanhoPagina, int numeroPagina,
			long idFilial);

		//OrcamentoPagamentoDto InserirFormaPagamento(FormaPagamentoDto formaPagamentoDto);

        //OrcamentoPagamentoDto RemoverFormaPagamento(long idOrcamentoFormaPagamento);

		//OrcamentoPagamentoDto ObterOrcamentoPagamentoDto(long idOrcamento);

		//int TamanhoFormaPagamentoPorTermo(string termoBusca, long? tipoVenda);

		//List<FormaPagamentoDto> ObterFormaPagamentoPorNome(string termoBusca, long? tipoVenda, int tamanhoPagina, int numeroPagina);

		int TamanhoBancoPorTermo(string termoBusca);

		List<BancoDto> ObterBancoPorNome(string termoBusca, int tamanhoPagina, int numeroPagina);

		//int TamanhoAdmFinanceiraPorTermo(string termoBusca);

		//List<AdministradoraFinanceiraDto> ObterAdmFinanceiraPorNome(string termoBusca, int tamanhoPagina, int numeroPagina);

		//void InserirEquipeMontagem(EquipeMontagemDto equipeMontagemDto, long idOrcamentoItem);
		//EquipeMontagemDto ObterEquipeMontagemDto(long idOrcamentoItem);

		ParcelamentoDto ObterParcelamento(long idOrcamento);
	
	}
}