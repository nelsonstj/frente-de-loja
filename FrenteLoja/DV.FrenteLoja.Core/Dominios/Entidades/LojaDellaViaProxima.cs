using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class LojaDellaViaProxima:Entidade
	{
		public long IdLojaDellaViaReferencia { get; set; }
		public virtual LojaDellaVia LojaDellaViaReferencia { get; set; }
		public int NrOrdem { get; set; }
		/// <summary>
		/// Id da loja indicada como uma das próximas da loja referencia.
		/// </summary>
		public long IdLojaDellaVia { get; set; }
	}
}