using System;
using System.ComponentModel;

namespace DV.FrenteLoja.Core.Contratos.Enums
{
    public enum OrigemCarga
    {
        [Description("Carga Produto")]
        Produto = 1,
        [Description("Carga Catalogo")]
        Catalogo = 2,
    }
}
