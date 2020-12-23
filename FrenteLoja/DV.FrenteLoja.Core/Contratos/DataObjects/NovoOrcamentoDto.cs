using System.ComponentModel.DataAnnotations;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class NovoOrcamentoDto
    {
        [Display(Name = "Km")]
        public string Km { get; set; }
        [Display(Name = "Placa")]
        public string Placa { get; set; }
        [Display(Name = "Marca")]
        public string MarcaVeiculo { get; set; }
        [Display(Name = "Modelo")]
        public string ModeloVeiculo { get; set; }
        [Display(Name = "Versão")]
        public string Versao { get; set; }
        [Display(Name = "Ano")]
        public string Ano { get; set; }
        [Display(Name = "CPF/CNPJ Cliente")]
        public string CpfCnpj { get; set; }
        [Display(Name = "Código Cliente")]
        public string CodigoCliente { get; set; }
        [Display(Name = "Nome Cliente")]
        public string NomeCliente { get; set; }
		[Display(Name = "Status do Cliente")]
	    public StatusCliente StatusCliente { get; set; }
		[Display(Name = "Telefone")]
        public string Telefone { get; set; }
        [Display(Name = "Complemento")]
        public string Complemento { get; set; }
        [Display(Name = "Tipo de venda")]
        public string TipoVenda { get; set; }
        [Display(Name = "Convênio")]
        public string Convenio { get; set; }
        [Display(Name = "Informação do convênio")]
        public string InformacaoConvenio { get; set; }
        [Display(Name = "Tabela de preços")]
        public string TabelaPreco { get; set; }
		[Display(Name = "Tipo Orçamento")]
	    public TipoOrcamento TipoOrcamento { get; set; }
	    [Display(Name = "Loja Destino")]
		public string LojaDestino { get; set; }

	}
}