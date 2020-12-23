using System;

namespace DV.FrenteLoja.Models
{
    public class VeiculoVersaoModel
    {
        public long Id { get; set; }
        public long IdMarcaModelo { get; set; }
        public string Descricao { get; set; }
        public bool RegistroInativo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
    }
}