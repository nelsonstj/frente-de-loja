using DV.FrenteLoja.Core.Contratos.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class LojaConfig
    {
        [Display(Name = "Id Loja")]
        [MaxLength(2)]
        [Required(ErrorMessage = "Código da loja é obrigatório.")]
        public string idLoja { get; set; }

        [Display(Name = "Convenio Padrão")]
        [Required(ErrorMessage = "Convenio Padrão é obrigatório.")]
        [MaxLength(6)]
        public string ConvenioPadrao { get; set; }

        [Display(Name = "Desconto Max.")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float DescontoMax { get; set; }
    }
}