using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    class VeiculoProtheusDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string IdFraga { get; set; }
        public long IdMarcaModeloVersao { get; set; }
        public DateTime AnoInicial { get; set; }
        public DateTime AnoFinal { get; set; }
        public long IdVersaoMotor { get; set; }
    }
}
