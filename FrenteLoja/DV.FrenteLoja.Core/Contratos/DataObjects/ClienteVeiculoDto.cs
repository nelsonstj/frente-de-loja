namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class ClienteVeiculoDto
	{
		public string ClienteId { get; set; }
		public string VeiculoIdFraga { get; set; }
		public string Placa { get; set; }
		public int Ano { get; set; }
		public string Observacoes { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Versao { get; set; }
        public string Motor { get; set; }
        public string AnoInicial { get; set; }
        public string AnoFinal { get; set; }

        public string SinespMarca { get; set; }
        public string SinespModelo { get; set; }
        public string SinespVersao { get; set; }
        public string SinespMotor { get; set; }
        public string SinespAno { get; set; }
        public string SinespAnoModelo { get; set; }

        public string Origem { get; set; }
    }
}