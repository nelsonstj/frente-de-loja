using DV.FrenteLoja.Core.Contratos.Abstratos;
using System;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Veiculo : Entidade
    {
        public string IdFraga { get; set; }
        public long IdMarcaModeloVersao { get; set; }
        public DateTime AnoInicial { get; set; }
        public DateTime AnoFinal { get; set; }
        public long IdVersaoMotor { get; set; }

        public virtual MarcaModeloVersao MarcaModeloVersao { get; set; }
        public virtual VersaoMotor VersaoMotor { get; set; }
        public virtual ICollection<ClienteVeiculo> ClienteVeiculoList { get; set; }
        public virtual ICollection<VeiculoMedidasPneus> VeiculoMedidasPneusList { get; set; }
    }
}