using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class VeiculoMedidasPneus : Entidade
    {
        public long idVeiculo { get; set; }
        [ForeignKey(nameof(idVeiculo))]
        public virtual Veiculo Veiculo { get; set; }
        public decimal Aro { get; set; }
        public decimal Largura { get; set; }
        public decimal Perfil { get; set; }
        public decimal Carga { get; set; }
        public string Indice { get; set; }
        public VeiculoPosicaoPneuEnum Posicao { get; set; }
    }
}
