using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    interface IVeiculoApi
    {
		Task<List<VeiculoProtheusDto>> ObterVeiculos();
    }
}
