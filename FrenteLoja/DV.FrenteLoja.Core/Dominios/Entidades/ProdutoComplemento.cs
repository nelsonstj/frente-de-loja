using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class ProdutoComplemento : Entidade
    {
        public string IdProduto { get; set; }
        public virtual Produto Produto { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Espessura { get; set; }
        public decimal Largura { get; set; }
        public decimal VolumeM3 { get; set; }
	    public string CampoHTML { get; set; }
        public decimal Perfil { get; set; }
        public decimal Aro { get; set; }
        public string Carga { get; set; } 
        public string Indice { get; set; } 
    }
}