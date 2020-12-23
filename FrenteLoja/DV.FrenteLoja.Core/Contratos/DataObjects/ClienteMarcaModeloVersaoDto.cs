using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class ClienteMarcaModeloVersaoDto
	{
		public long Id { get; set; }
        public string Descricao { get; set; }
        public long IdMarca { get; set; }
		public string Marca { get; set; }
		public long IdModelo { get; set; }
		public string Modelo { get; set; }
		public long IdVersao { get; set; }
		public string Versao { get; set; }
		public int Ano { get; set; }
		public long IdCliente { get; set; }
        public string Placa { get; set; }
        public long IdVersaoMotor { get; set; }
        public string VersaoMotor { get; set; }
    }
}