using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class LogIntegracao : Entidade
    {
        public TipoTabelaProtheus TipoTabelaProtheus { get; set; }
        public string Log { get; set; }
        public string RequestIntegracaoJson { get; set; }
        public string ResponseIntegracaoJson { get; set; }
        public StatusIntegracao StatusIntegracao { get; set; }
    }
}
