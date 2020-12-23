using DV.FrenteLoja.Core.Contratos.Enums;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class ProfissionalMontagemDto
	{
		public string Id { get; set; }
        [Display(Name = "Profissional")]
        public string ProfissionalNome { get; set; }
        [Display(Name = "Função")]
        public ProfissionalMontagemFuncao Funcao { get; set; }
	}
}