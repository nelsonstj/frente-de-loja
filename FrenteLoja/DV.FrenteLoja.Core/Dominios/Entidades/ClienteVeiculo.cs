using System.ComponentModel.DataAnnotations.Schema;
using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class ClienteVeiculo : Entidade
    {
        public string ClienteId { get; set; }
        //[ForeignKey(nameof(ClienteId))]
        public virtual Cliente Cliente { get; set; }
        public string VeiculoIdFraga { get; set; }
        //public virtual Veiculo Veiculo { get; set; } 
		public string Placa { get; set; }
		public int Ano { get; set; }
		public string Observacoes { get; set; }

	}
}