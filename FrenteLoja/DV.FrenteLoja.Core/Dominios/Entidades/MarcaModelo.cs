using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class MarcaModelo : Entidade
    {
        public long IdMarca { get; set; }
        public virtual Marca Marca { get; set; }
	    public virtual ICollection<MarcaModeloVersao> MarcaModeloVersoes { get; set; }

		// tirar elasticsearch tipo veiculo, tirar na carga de catalogo, e em todos os lugares que são referenciados marca modelo.
		// tirar nos dto's
    }
}
