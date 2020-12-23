using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Exceptions;

namespace DV.FrenteLoja.Core.ProtheusAPI
{
    class VeiculoApi : IVeiculoApi
    {
        public Task<List<VeiculoProtheusDto>> ObterVeiculos()
        {
            try
            {
                var baseAddress = System.Configuration.ConfigurationManager.AppSettings["ProtheusApiBaseAddress"];
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new ServicoIntegracaoException($"Erro no método {nameof(ObterVeiculos)}. Descrição: " + e.Message);
            }
            return null;
        }
    }
}
