using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class CatalogoCargaLog : Entidade
	{
		public string LogImportacao { get; set; }
		public string NomeArquivo { get; set; }
		public StatusIntegracao StatusIntegracao { get; set; }
	}
}
