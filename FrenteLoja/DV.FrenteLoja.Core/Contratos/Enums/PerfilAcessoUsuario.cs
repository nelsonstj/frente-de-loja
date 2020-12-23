using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
	public enum PerfilAcessoUsuario
	{
		[Description("Telemarketing")]
		TMK = 1,
		[Description("Frente de Loja")]
		FrenteLoja,
        [Description("Administrativo")]
        Administrativo
	}
}