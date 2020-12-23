namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class EstoqueDto
    {
        public string Filial { get; set; }
        public string CodigoDellaVia { get; set; }
        public int SaldoDisponivel { get; set; }
        public int SaldoAtual { get; set; }
        public string NomeFilial { get; set; }
    }
}
