using System;

namespace DV.FrenteLoja.Models
{
    public class VeiculoMarcaModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public bool RegistroInativo { get; set; }
        public string CampoCodigo { get; set; }
	    public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
    }
}