namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class HomeDto
	{
		/// <summary>
		/// Indica o período atual do usuário (bom dia, boa tarde ou boa noite).
		/// </summary>
		public string PeriodoAtual { get; set; }

		/// <summary>
		/// Nome do usuario logado no sistema.
		/// </summary>
		public string NomeUsuario { get; set; }

		/// <summary>
		/// Apresenta a quantidade de checkups feitos pela filial do usuário logado no dia atual.
		/// </summary>
		public long QuantidadeCheckups { get; set; }

		/// <summary>
		/// Apresenta a quantidade de orçamentos feitos pelo usuário logado no dia atual.
		/// </summary>
		public long QuantidadeOrcamentosPorUsuario { get; set; }

		/// <summary>
		/// Apresenta a quantidade de orçamentos que estão para vencer pelo usuário logado no dia atual.
		/// </summary>
		public long QuantidadeOrcamentosVencendo { get; set; }

	}
}