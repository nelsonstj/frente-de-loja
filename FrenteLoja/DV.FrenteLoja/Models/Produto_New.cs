using System;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Models
{
    public class Produto_New
    {
        public long Id { get; set; }
        public long IdGrupoProduto { get; set; }
        [Display(Name = "CÓD. FABRICANTE")]
        public string CodigoFabricante { get; set; }
        public string IdGrupoServicoAgregado { get; set; }
        public string Descricao { get; set; }
        public bool RegistroInativo { get; set; }
        [Display(Name = "CÓD. DELLAVIA")]
        public string CampoCodigo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
        [Display(Name = "FABRICANTE")]
        public string FabricantePeca { get; set; }
    }
}