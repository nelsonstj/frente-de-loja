using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class OrcamentoItemEquipeMontagem: Entidade
	{
		public long IdOrcamentoItem { get; set; }
		public virtual OrcamentoItem OrcamentoItem { get; set; }
		public string IdVendedor { get; set; }
		public virtual Vendedor Vendedor { get; set; }
		public ProfissionalMontagemFuncao Funcao { get; set; }
        //public string Funcao { get; set; }
    }
}