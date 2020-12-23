using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Interfaces;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class vwVeiculos : IEntidade
    {
        public string id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Versao { get; set; }
        public string VersaoMotor { get; set; }
        public DateTime InicioProducao { get; set; }
        public DateTime FinalProducao { get; set; }

        // Ignorado - Não utilizar por ser uma View
        public long Id { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
        public string Descricao { get; set; }
        public bool RegistroInativo { get; set; }
    }
}
