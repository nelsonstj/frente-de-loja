using System;

namespace DV.FrenteLoja.Models
{
    public class VeiculoModeloModel
    {
        public long Id { get; set; }
        public long IdMarca { get; set; }
        public string  Descricao { get; set; }
        public bool RegistroInativo { get; set; }
        public string CampoCodigo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
    }
}