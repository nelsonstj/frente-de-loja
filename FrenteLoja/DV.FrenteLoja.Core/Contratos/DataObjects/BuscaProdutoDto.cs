using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class BuscaProdutoDto
    {
        [Display(Name = "Marca:")]
        public string MarcaVeiculo { get; set; }
        [Display(Name = "Modelo:")]
        public string ModeloVeiculo { get; set; }
        [Display(Name = "Versão:")]
        public string VersaoVeiculo { get; set; }
        [Display(Name = "Ano:")]
        public string Ano { get; set; }
        [Display(Name = "Grupo Produto:")]
        public string GrupoProduto { get; set; }
        [Display(Name = "Fabricante Peça:")]
        public string FabricantePeca { get; set; }
        [Display(Name = "Produto:")]
        public string Descricao { get; set; }
    }
}
