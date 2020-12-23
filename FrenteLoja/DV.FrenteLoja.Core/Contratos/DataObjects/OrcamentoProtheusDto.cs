using System;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoProtheusDto
	{
		public OrcamentoProtheusDto()
		{
			Itens = new List<OrcamentoItemProtheusDto>();
		}
		public string CodVendedor { get; set; }
		public string CodLojaCliente { get; set; }
		public string CodCliente { get; set; }
		public string CodCondicaoPagamento { get; set; }
		public string CodBanco { get; set; }
		public string CodOperador { get; set; }
		public string CodTabelaPreco { get; set; }
		public string CodConvenio { get; set; }
		public string CodTipoVenda { get; set; }
		/// <summary>
		/// Define o código da Loja destino do orçamento.
		/// </summary>
		public string CodDellaVia { get; set; }
		public decimal Valor { get; set; }
		public string CodMarca { get; set; }
		public string CodModelo { get; set; }
		public string CodTransportadora { get; set; }
		public string MensagemNF { get; set; }
		public string Xped { get; set; }
		public string Ano { get; set; }
		public string Km { get; set; }
		public string Placa { get; set; }
		public string ObsVeiculo { get; set; }
		public List<OrcamentoItemProtheusDto> Itens { get; set; }
		public string Complemento { get; set; }
		public DateTime DataValidade { get; set; }
		public string CampoCodigoOrcamento { get; set; }
        public string Numero { get; set; }
        public List<OrcamentoFormaPagamentoProtheusDto> FormasPagamento { get; set; }
		public bool PossuiReservaEstoque { get; set; }
		public string Telefone { get; set; }
		public string Observacoes { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
        public DateTime DataCriacao { get; set; }
		public string CodUsuario { get; set; }
		public int StatusOrcamento { get; set; }
		public string DescricaoMarca { get; set; }
		public string DescricaoModelo { get; set; }
		public string VeiculoIdFraga { get; set; }
	}
}