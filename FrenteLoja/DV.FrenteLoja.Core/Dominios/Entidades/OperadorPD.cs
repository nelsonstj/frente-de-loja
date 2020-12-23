using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class OperadorPD
	{
        public string IdOperador { get; set; }
        public string IdConsultor { get; set; }
        public string NomeOperador { get; set; }
        public decimal PercLimiteDesconto { get; set; }
    }
}