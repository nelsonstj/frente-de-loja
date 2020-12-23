using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    class VeiculoMedidasPneusDTO
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long idVeiculo { get; set; }
        public decimal Aro { get; set; }
        public decimal Largura { get; set; }
        public decimal Perfil { get; set; }
        public decimal Carga { get; set; }
        public string Indice { get; set; }
    }
}
