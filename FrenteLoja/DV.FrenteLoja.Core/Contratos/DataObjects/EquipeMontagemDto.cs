using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class EquipeMontagemDto
    {
        public EquipeMontagemDto()
        {
            Equipe = new List<ProfissionalMontagemDto>();
        }
        public string DescricaoProduto { get; set; }
        public string IdLojaOrcamento { get; set; }
        public long IdOrcamentoItem { get; set; }
        public int IndexRemover { get; set; }
        public List<ProfissionalMontagemDto> Equipe { get; set; }
    }
}
