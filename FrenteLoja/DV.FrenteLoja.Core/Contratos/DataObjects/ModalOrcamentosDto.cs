using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ModalOrcamentosDto
    {
        public bool Varejo { get; set; }
        public List<OrcamentoDto> Orcamentos { get; set; }
    }
}
