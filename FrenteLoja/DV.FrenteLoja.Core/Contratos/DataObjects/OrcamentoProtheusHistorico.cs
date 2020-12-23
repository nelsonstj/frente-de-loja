using System;
using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class OrcamentoProtheusHistorico
	{
		public StatusOrcamento StatusOrcamento { get; set; }
		public string DescricaoLoja { get; set; }
		public string CampoCodigoOrcamento { get; set; }
		public DateTime DataCriacao { get; set; }
		public DateTime DataVencimento { get; set; }
		public string DescricaoMarca { get; set; }
		public string DescricaoModelo { get; set; }
		public string Placa { get; set; }
		public string Km { get; set; }
		public string NomeCliente { get; set; }
		public string CpfCnpjCliente { get; set; }
		public string CodCliente { get; set; }
		public string EmailCliente { get; set; }
		public string CelularCliente { get; set; }
		public string TelefoneCliente { get; set; }
		public List<OrcamentoItemProtheusDto> Itens { get; set; }
		public List<OrcamentoFormaPagamentoProtheusDto> FormasPagamento { get; set; }
		public decimal ValorTotalFormaPagamento { get; set; }
	}
}