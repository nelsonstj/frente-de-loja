using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class ClienteDto
	{
        public long Id { get; set; }
        public string IdCliente { get; set; }
        public string CNPJCPF { get; set; }
		public string Telefone { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string Email { get; set; }
		public string Loja { get; set; }
		public string Nome { get; set; }
		public StatusCliente StatusCliente { get; set; }
		public StatusCreditoCliente MotivoBloqueioCredito { get; set; }
		public string ClassificacaoCliente { get; set; }
		public string Score { get; set; }
		//public virtual ClienteVeiculoDto UltimoVeiculoUtilizado { get; set; }
        public string CampoCodigo { get; set; }
    }
}