using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DV.FrenteLoja.Models
{
    public class VeiculoClienteModel
    {
        public long Id { get; set; }
        public string Placa { get; set; }
        public long ClienteId { get; set; }
        public string VeiculoIdFraga { get; set; }
        public int Ano { get; set; }
        public string Observacoes { get; set; }
        public bool RegistroInativo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Versao { get; set; }
        public string Motor { get; set; }
        public string AnoInicial { get; set; }
        public string AnoFinal { get; set; }
    }
}