using System;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class CheckupCar : Entidade
    {
        public long CheckupId { get; set; }
        public virtual Checkup Checkup { get; set; }

        public decimal? ConvergenciaTotalInicial { get; set; }
        public decimal? CamberEsquerdoDianteiroInicial { get; set; }
        public decimal? CasterEsquerdoInicial { get; set; }
        public decimal? CamberEsquerdoTraseiroInicial { get; set; }
        public decimal? ConvergenciaEsquerdoInicial { get; set; }
        public decimal? CamberDireitoDianteiroInicial { get; set; }
        public decimal? CasterDireitoInicial { get; set; }
        public decimal? CamberDireitoTraseiroInicial { get; set; }
        public decimal? ConvergenciaDireitoInicial { get; set; }

        public decimal? ConvergenciaTotalFinal { get; set; }
        public decimal? CamberEsquerdoDianteiroFinal { get; set; }
        public decimal? CasterEsquerdoFinal { get; set; }
        public decimal? CamberEsquerdoTraseiroFinal { get; set; }
        public decimal? ConvergenciaEsquerdoFinal { get; set; }
        public decimal? CamberDireitoDianteiroFinal { get; set; }
        public decimal? CasterDireitoFinal { get; set; }
        public decimal? CamberDireitoTraseiroFinal { get; set; }
        public decimal? ConvergenciaDireitoFinal { get; set; }
               
        public CheckupLegenda PneuDianteiro { get; set; }
        public CheckupLegenda PneuTraseiro { get; set; }
        public CheckupLegenda Estepe { get; set; }
        public CheckupLegenda Valvula { get; set; }
        public CheckupLegenda AmortecedorDianteiro { get; set; }
        public CheckupLegenda AmortecedorTraseiro { get; set; }
        public CheckupLegenda TerminaisDirecao { get; set; }
        public CheckupLegenda BracosAxiais { get; set; }
        public CheckupLegenda Pivos { get; set; }
        public CheckupLegenda Bandeja { get; set; }
        public CheckupLegenda Pastilhas { get; set; }
        public CheckupLegenda Discos { get; set; }
        public CheckupLegenda FreioTraseiro { get; set; }
        public CheckupLegenda FluidoFreio { get; set; }
        public CheckupLegenda Balanceamento { get; set; }
        public CheckupLegenda Alinhamento { get; set; }
        public CheckupLegenda ConsertoRoda { get; set; }
        public CheckupLegenda CambagemCaster { get; set; }
        public CheckupLegenda Palhetas { get; set; }
        public CheckupLegenda Calotas { get; set; }
        public CheckupLegenda Extintor { get; set; }
        public CheckupLegenda InjecaoEletronica { get; set; }
        public CheckupLegenda HigienizacaoAr { get; set; }
        public CheckupLegenda FiltroArCondicionado { get; set; }
        public bool PneuDianteiroExecutado { get; set; }
        public bool PneuTraseiroExecutado { get; set; }
        public bool EstepeExecutado { get; set; }
        public bool ValvulaExecutado { get; set; }
        public bool AmortecedorDianteiroExecutado { get; set; }
        public bool AmortecedorTraseiroExecutado { get; set; }
        public bool TerminaisDirecaoExecutado { get; set; }
        public bool BracosAxiaisExecutado { get; set; }
        public bool PivosExecutado { get; set; }
        public bool BandejaExecutado { get; set; }
        public bool PastilhasExecutado { get; set; }
        public bool DiscosExecutado { get; set; }
        public bool FreioTraseiroExecutado { get; set; }
        public bool FluidoFreioExecutado { get; set; }
        public bool BalanceamentoExecutado { get; set; }
        public bool AlinhamentoExecutado { get; set; }
        public bool ConsertoRodaExecutado { get; set; }
        public bool CambagemCasterExecutado { get; set; }
        public bool PalhetasExecutado { get; set; }
        public bool CalotasExecutado { get; set; }
        public bool ExtintorExecutado { get; set; }
        public bool InjecaoEletronicaExecutado { get; set; }
        public bool HigienizacaoArExecutado { get; set; }
        public bool FiltroArCondicionadoExecutado { get; set; }
        public DateTime? DataTrocaOleo { get; set; }
        public DateTime? DataTrocaFiltroOleo { get; set; }
        public TipoOleo TipoOleo { get; set; }
        public string Especificacao { get; set; }
        public string CodigoDellaVia { get; set; }
        public CheckupLegenda FiltroAr { get; set; }
        public CheckupLegenda FiltroCombustivel { get; set; }
        public bool FiltroArExecutado { get; set; }
        public bool FiltroCombustivelExecutado { get; set; }
        public string Observacoes { get; set; }
               
    }


}
