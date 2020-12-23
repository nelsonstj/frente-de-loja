using System;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class OrcamentoConsultaDto
    {
        [Display(Name = "ID")]
        public long Id { get; set; }
        [Display(Name = "Nº Protheus")]
        public string CampoCodigo { get; set; }
        [Display(Name = "Nome do cliente")]
        public string NomeCliente { get; set; }
        [Display(Name = "Loja Destino")]
        public string LojaDestino { get; set; }
        [Display(Name = "Vendedor")]
        public string Vendedor { get; set; }
        [Display(Name = "Emissão")]
        public String DataCriacao { get; set; }
        [Display(Name = "Vecto.")]
        public string DataValidade { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
