using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class ConvenioCondicaoPagamento : Entidade
    {
        public string IdConvenio { get; set; }
        public virtual Convenio Convenio { get; set; }
        public string IdCondicaoPagamento { get; set; }
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }
    }
}
