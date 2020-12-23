using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Marca : Entidade
    {
        public virtual ICollection<MarcaModelo> MarcaModeloList { get; set; }
    }
}
