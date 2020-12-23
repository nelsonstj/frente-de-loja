using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum StatusOrcamento
    {
		[Description("Em Aberto")]
		EmAberto = 0,
		[Description("Venda efetuada")]
		VendaEfetuada = 1,
		[Description("Reserva")]
		Reserva = 2,
        [Description("Em aberto vencido")]
        EmAbertoVencido = 3,
        [Description("Cancelado")]
        Cancelado = 4,
        [Description("Encerrado")]
        Encerrado = 5,
        [Description("Devolução pendente")]
        DevolucaoPendente = 6,
        [Description("Transação TEF desfeita")]
        TransacaoTefDesfeita = 7,
        [Description("Orçamento com pedido de venda")]
        OrcamentoComPedidoDeVenda = 8,
        [Description("Orçamento pago no FrontLoja")]
        OrcamentoPagaNoFrontLoja = 9,
        [Description("Venda estornada")]
        VendaEstornada = 10,
		/// <summary>
		/// Utilizado na tela de consulta, não é um status valido do orçamento
		/// </summary>
	    [Description("Todos")]
	    Todos = 999
    }
}