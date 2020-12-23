using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
	public class Cliente : Entidade
	{
        public string IdCliente { get; set; }
        public string CNPJCPF { get; set; }
		public string Telefone { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string Email { get; set; }
		public string Loja { get; set; }
		public string Nome { get; set; }
		public long? BancoId { get; set; }
		public virtual Banco Banco { get; set; }
		public StatusCliente StatusCliente { get; set; }
		public StatusCreditoCliente MotivoBloqueioCredito { get; set; }
		public string TipoCliente { get; set; }
		public string ClassificacaoCliente { get; set; }
		public string Score { get; set; }
		//public virtual ICollection<ClienteVeiculo> Veiculos { get; set; }
        public virtual ICollection<Convenio> ConvenioList { get; set; }
        public virtual ICollection<Orcamento> OrcamentoList { get; set; }

    }
}
