using System;
using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class Orcamento : Entidade
	{
		public bool IsOrigemProtheus { get; set; }
		public string IdConvenio { get; set; }
		public virtual Convenio Convenio { get; set; }
		public string IdCliente { get; set; }
		public virtual Cliente Cliente { get; set; }
		public DateTime DataValidade { get; set; }
		public string IdTabelaPreco { get; set; }
		public virtual TabelaPreco TabelaPreco { get; set; }
		public string Complemento { get; set; }
		public string Telefone { get; set; }
		public string IdVendedor { get; set; }
		public string InformacoesCliente { get; set; }
		public string Placa { get; set; }
		public int? Ano { get; set; }
		//public long IdMarcaModeloVersao { get; set; }
		//public virtual MarcaModeloVersao MarcaModeloVersao { get; set; }
        public string VeiculoIdFraga { get; set; }
        //public virtual Veiculo Veiculo { get; set; }
        public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string IdLojaDellaVia { get; set; }
		public virtual LojaDellaVia LojaDellaVia { get; set; }
		public string IdOperador { get; set; }
		public long? IdTransportadora { get; set; }
		public virtual Transportadora Transportadora { get; set; }
		public bool PossuiReservaEstoque { get; set; }
		public StatusOrcamento StatusOrcamento { get; set; }
		public bool ExisteAlcadaPendente { get; set; }
		public int? KM { get; set; }
		public string IdAreaNegocio { get; set; }
		public virtual PDAreaNegocio AreaNegocio { get; set; }
		public TipoOrcamento TipoOrcamento { get; set; }
		public string Xped { get; set; }
		public string MensagemNF { get; set; }
		public virtual ICollection<OrcamentoItem> OrcamentoItens { get; set; }
		public virtual ICollection<OrcamentoFormaPagamento> FormaPagamentos { get; set; }
		public long? IdBanco { get; set; }
		public Banco Banco { get; set; }
        public virtual ICollection<SolicitacaoAnaliseCredito> SolicitacaoAnaliseCreditoList { get; set; }
        public virtual ICollection<Checkup> CheckupList { get; set; }
		public decimal ValorImpostos { get; set; }
		public DateTime DataCriacao { get; set; }
		public string UsuarioCriacao { get; set; }
	}
}
