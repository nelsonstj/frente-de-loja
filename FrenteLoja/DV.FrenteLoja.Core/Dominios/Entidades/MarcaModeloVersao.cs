using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class MarcaModeloVersao:Entidade
	{
		public long IdMarcaModelo { get; set; }
		public virtual MarcaModelo MarcaModelo { get; set; }
        public virtual ICollection<Orcamento> OrcamentoList { get; set; }
    }
}