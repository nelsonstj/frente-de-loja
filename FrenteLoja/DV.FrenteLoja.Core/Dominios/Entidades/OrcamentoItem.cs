using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class OrcamentoItem : Entidade
	{
		public long OrcamentoId { get; set; }
		public virtual Orcamento Orcamento { get; set; }
		public long? IdDescontoModeloVenda { get; set; }
		public virtual DescontoModeloVenda DescontoModeloVenda { get; set; }
		public int NrItem { get; set; }
		public decimal Quantidade { get; set; }
		public decimal PrecoUnitario { get; set; }
		public decimal TotalItem { get; set; }
		public string ProdutoId { get; set; }
		public virtual Produto Produto { get; set; }
		public int? NrItemProdutoPaiId { get; set; }
		public decimal ValorDesconto { get; set; }
		public decimal PercDescon { get; set; }
		public string TipoOperacao { get; set; }
		public bool ReservaEstoque { get; set; }
		public virtual ICollection<OrcamentoItemEquipeMontagem> EquipeMontagemList { get; set; }
        public DescontoModeloVendaUtilizado? DescontoModeloVendaUtilizado { get; set; }
        public virtual ICollection<SolicitacaoDescontoVendaAlcada> SolicitacaoDescontoVendaAlcadaList { get; set; }
    }
}