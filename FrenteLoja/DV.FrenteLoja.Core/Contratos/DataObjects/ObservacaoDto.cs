using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ObservacaoDto
    {
        public ObservacaoDto()
        {
            Informacoes = new List<string>();
        }
        public string Titulo { get; set; }

        public List<string> Informacoes { get; set; }
    }
}
