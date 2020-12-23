using System;

namespace DV.FrenteLoja.Models
{
    public class VeiculoModel
    {
        public long Id { get; set; }
        public string VeiculoIdFraga { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Versao { get; set; }
        public string Motor { get; set; }
        public string AnoInicial { get; set; }
        public string AnoFinal { get; set; }
    }
}
