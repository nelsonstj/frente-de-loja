// ReSharper disable InconsistentNaming

using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum TipoTabelaProtheus
    {
        [Description("Tipo de Vendas")]
        PAG,
        [Description("Marcas")]
        PA0,
        [Description("Modelos")]
        PA1,
        [Description("Vendedores")]
        SA3,
        [Description("Transportadoras")]
        SA4,
        [Description("Bancos")]
        SA6,
        [Description("Filiais")]
        SLJ,
        [Description("Convênios")]
        PA6,
        [Description("Convênios Cliente")]
        PBO,
        [Description("Convênio Produto")]
        PBP,
        [Description("Convênio Condição de Pagamento")]
        PBQ,
        [Description("Clientes")]
        SA1,
        [Description("Cliente Veículos")]
        PA7,
        [Description("Tabela de Preços")]
        DA0,
        [Description("Tabela de Preços Item")]
        DA1,
        [Description("Orçamento")]
        SL1,
        [Description("Orçamento Item")]
        SL2,
        [Description("Produto")]
        SB1,
        [Description("Grupo de Produto")]
        SBM,
        [Description("Condição de Pagamento")]
        SE4,
        [Description("Grupo de Serviços Agregados")]
        SZH,
        [Description("Grupo de Serviços Agregados Produtos")]
        PA3,
        [Description("Produto Complemento")]
        SB5,
        [Description("Operador")]
        SU7,
        [Description("Parametros Gerais")]
        SX6,
        [Description("Forma de pagamento")]
        SX5,
	    [Description("Administração Financeira")]
	    SAE,
		[Description("Desconto Venda Alçada")]
		PAN,
		[Description("Desconto Venda Alçada Grupo Produto")]
		PAO,
		[Description("Desconto Modelo de Venda")]
		ZAL,
	    [Description("Solicitacao Analise Crédito")]
		MAH,
        [Description("Outros")]
        XXX,
    }
}