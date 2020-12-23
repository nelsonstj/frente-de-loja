using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum TipoItemOrcamento
	{
		[Description("Produto")]
		Produto = 0,
		[Description("Serviço")]
		Servico = 1
	}
}