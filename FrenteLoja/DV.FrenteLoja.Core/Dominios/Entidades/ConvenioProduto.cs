using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class ConvenioProduto : Entidade
    {
        public string IdConvenio { get; set; }
        public virtual Convenio Convenio { get; set; }
        public string IdProduto{ get; set; }
        public string IdGrupoProduto { get; set; }
        public TipoPreco TipoPreco { get; set; }
	    
    }
}
