using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class OrcamentoObservacao : Entidade
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public TipoObservacao TipoObservacao { get; set; }
    }
}
