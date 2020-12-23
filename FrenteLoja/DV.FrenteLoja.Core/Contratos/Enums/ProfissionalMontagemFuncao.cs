using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum ProfissionalMontagemFuncao
	{
		[Description(nameof(Montador))]
	  	Montador = 1,
		[Description(nameof(Alinhador))]
		Alinhador = 2,
        [Display(Name = "Mont/Alinh")]
        [Description("Mont/Alinh")]
		MontAlinhador = 4
	}
}