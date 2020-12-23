using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class CheckupTruck : Entidade
    {
        public long CheckupId { get; set; }
        public virtual Checkup Checkup { get; set; }

        public CheckupLegenda Embuchamento { get; set; }
        public CheckupLegenda TerminalBarraLonga { get; set; }
        public CheckupLegenda BarraCurtaDirecao { get; set; }
        public CheckupLegenda LonaPastilhaFreioDianteiro { get; set; }
        public CheckupLegenda Amortecedor { get; set; }
        public CheckupLegenda BarraEstabilizadora { get; set; }
        public CheckupLegenda TamborDiscoFreioDianteiro { get; set; }
        public CheckupLegenda MolejoCompleto { get; set; }
        public CheckupLegenda BracoTensor { get; set; }
        public CheckupLegenda BalancaPino { get; set; }
        public CheckupLegenda LonaPastilhaFreioTraseiro { get; set; }
        public CheckupLegenda CatracaFlexivel { get; set; }
        public CheckupLegenda MolejoSuporte { get; set; }
        public CheckupLegenda SuspensorTruck { get; set; }
        public CheckupLegenda TamborDiscoFreioTraseiro { get; set; }
        public CheckupLegenda AmortecedorTraseiro { get; set; }


        public bool EmbuchamentoExecutado { get; set; }
        public bool TerminalBarraLongaExecutado { get; set; }
        public bool BarraCurtaDirecaoExecutado { get; set; }
        public bool LonaPastilhaFreioDianteiroExecutado { get; set; }
        public bool AmortecedorExecutado { get; set; }
        public bool BarraEstabilizadoraExecutado { get; set; }
        public bool TamborDiscoFreioDianteiroExecutado { get; set; }
        public bool MolejoCompletoExecutado { get; set; }
        public bool BracoTensorExecutado { get; set; }
        public bool BalancaPinoExecutado { get; set; }
        public bool LonaPastilhaFreioTraseiroExecutado { get; set; }
        public bool CatracaFlexivelExecutado { get; set; }
        public bool MolejoSuporteExecutado { get; set; }
        public bool SuspensorTruckExecutado { get; set; }
        public bool TamborDiscoFreioTraseiroExecutado { get; set; }
        public bool AmortecedorTraseiroExecutado { get; set; }

        public string Observacoes { get; set; }
    }
}
