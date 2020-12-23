using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class ConvenioCliente : Entidade
    {
        public string IdConvenio { get; set; }
        public virtual Convenio Convenio { get; set; }
        public string IdCliente { get; set; }

        public string IdLoja { get; set; }
    }
}
