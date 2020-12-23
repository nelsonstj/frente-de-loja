using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum StatusCreditoCliente
	{
		[Description("Liberado")]
		Liberado = 0,
		[Description("Acompanhamento de crédito")]
		BloqCredito = 1,
		[Description("Restrições Serasa")]
		BloqSerasa = 2,
		[Description("Acompanhamento de Pagamentos")]
		AcompPagamentos = 3,
		[Description("CNPJ Cancelado pela Receita Federal")]
		CNPJCancelado = 4,
		[Description("Bloqueado por solicitação do cliente")]
		BloqCliente = 5,
		[Description("Inscrição Estadual não homologada")]
		IENaoHomologada = 6,
		[Description("Necessário Apresentação de pedido de compra")]
		ApresentarPedidoCompra = 7,
		[Description("Empresa recente no mercado")]
		EmpresaRecente = 8,
		[Description("Empresa com alerta no Serasa/ACSP")]
		AlertaSerasa = 9,
        /// <summary>
        /// A1_BLQVEN=A
        /// </summary>
		[Description("Cadastro com informações inconsistentes")]
		CadastroInconsistente = 10,
        /// <summary>
        /// A1_BLQVEN=B
        /// </summary>
		[Description("Negociação de Saldo Devedor")]
		SaldoDevedor = 11,
        /// <summary>
        /// A1_BLQVEN=C
        /// </summary>
		[Description("Cadastro para venda E-Commerce")]
		VendaEcommerce = 12,
        /// <summary>
        /// A1_BLQVEN=D
        /// </summary>
		[Description("Cadastro total Óleo")]
		TotalOleo = 13,
        /// <summary>
        /// A1_BLQVEN=E
        /// </summary>
		[Description("Titulos Baixados em Perdas")]
		BaixadoPerdas = 14
	}
}