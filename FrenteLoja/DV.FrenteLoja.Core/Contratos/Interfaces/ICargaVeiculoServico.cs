using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICargaVeiculoServico
    {
        Task SyncVeiculo();
    }
}
