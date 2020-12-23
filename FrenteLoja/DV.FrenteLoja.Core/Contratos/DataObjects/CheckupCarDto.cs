using System;
using System.ComponentModel.DataAnnotations;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class CheckupCarDto
    {
        #region [ Inicio ]

        [Display(Name = "Nome")]
        public string Cliente { get; set; }
        [Display(Name = "CPF / CNPJ")]
        public string CPFCNPJ { get; set; }
        [Display(Name = "Telefone")]
        public string Phone { get; set; }
        [Display(Name = "Placa")]
        public string License { get; set; }
        [Display(Name = "Modelo")]
        public string Car { get; set; }
        [Display(Name = "Ano")]
        public int? Year { get; set; }
        [Display(Name = "KM Rodados")]
        public long? KM { get; set; }
        [Display(Name = "Marca")]
        public string Marca { get; set; }
        [Display(Name = "Versão")]
        public string Versao { get; set; }
        [Display(Name = "Técnico Responsável")]
        public string Tecnico { get; set; }
        public string TecnicoNome { get; set; }
        [Display(Name = "Vendedor Responsável")]
        public string Vendedor { get; set; }
        public string VendedorNome { get; set; }
        [Display(Name = "Serviço/Produto Solicitado")]
        public string ProdutoServico { get; set; }
        public bool IsCheckupCar { get; set; }
        #endregion

        #region [ Step 1 ]

        #region [ Frontal Inicio ]

        [Display(Name = "CONVERGÊNCIA TOTAL (MM)")]
        public string ConvergenciaTotalInicial { get; set; }
        [Display(Name = "CÂMBER ESQUERDO (º)")]
        public string CambarEsquerdoFrontalInicial { get; set; }
        [Display(Name = "CÁSTER ESQUERDO (º)")]
        public string CasterEsquerdoFrontalInicial { get; set; }
        [Display(Name = "CÂMBER DIREITO (º)")]
        public string CambarDireitoFrontalInicial { get; set; }
        [Display(Name = "CÁSTER DIREITO (º)")]
        public string CasterDireitoFrontalInicial { get; set; }

        #endregion

        #region [ Traseiro Inicio ]

        [Display(Name = "CÂMBER ESQUERDO (º)")]
        public string CambarEsquerdoTraseiroInicial { get; set; }
        [Display(Name = "CONVERGÊNCIA (MM)")]
        public string ConvergenciaEsquerdaTraseiroInicial { get; set; }
        [Display(Name = "CÂMBER DIREITO(º) ")]
        public string CambarDireitoTraseiroInicial { get; set; }
        [Display(Name = "CONVERGÊNCIA (MM)")]
        public string ConvergenciaDireitoTraseiroInicial { get; set; }

        #endregion

        #region [ Frontal Final ]
        [Display(Name = "CONVERGÊNCIA TOTAL (MM)")]
        public string ConvergenciaTotalFinal { get; set; }
        [Display(Name = "CÂMBER ESQUERDO (º)")]
        public string CambarEsquerdoFrontalFinal { get; set; }
        [Display(Name = "CÁSTER ESQUERDO (º)")]
        public string CasterEsquerdoFrontalFinal { get; set; }
        [Display(Name = "CÂMBER DIREITO (º)")]
        public string CambarDireitoFrontalFinal { get; set; }
        [Display(Name = "CÁSTER DIREITO (º)")]
        public string CasterDireitoFrontalFinal { get; set; }
        #endregion

        #region [ Traseiro Final ]
        [Display(Name = "CÂMBER ESQUERDO (º)")]
        public string CambarEsquerdoTraseiroFinal { get; set; }
        [Display(Name = "CONVERGÊNCIA (MM)")]
        public string ConvergenciaEsquerdaTraseiroFinal { get; set; }
        [Display(Name = "CÂMBER DIREITO(º) ")]
        public string CambarDireitoTraseiroFinal { get; set; }
        [Display(Name = "CONVERGÊNCIA (MM)")]
        public string ConvergenciaDireitoTraseiroFinal { get; set; }
        #endregion

        #endregion

        #region [ Step 2]
        [Display(Name = "Pneu Dianteiro")]
        public CheckListEnum PneuDianteiro { get; set; }
        public bool? PneuDianteiroExecutado { get; set; }

        [Display(Name = "Pneu Traseiro")]
        public CheckListEnum PneuTraseiro { get; set; }
        public bool? PneuTraseiroExecutado { get; set; }

        [Display(Name = "Estepe")]
        public CheckListEnum Estepe { get; set; }
        public bool? EstepeExecutado { get; set; }

        [Display(Name = "Valvula")]
        public CheckListEnum Valvula { get; set; }
        public bool? ValvulaExecutado { get; set; }

        #endregion

        #region [ Step 3]
        [Display(Name = "Amortecedor Dianteiro")]
        public CheckListEnum AmortecedorDianteiro { get; set; }
        public bool? AmortecedorDianteiroExecutado { get; set; }

        [Display(Name = "Amortecedor Traseiro")]
        public CheckListEnum AmortecedorTraseiro { get; set; }
        public bool? AmortecedorTraseiroExecutado { get; set; }

        [Display(Name = "Terminais de Direção")]
        public CheckListEnum TerminaisDeDirecao { get; set; }
        public bool? TerminaisDeDirecaoExecutado { get; set; }

        [Display(Name = "Braços Axiais")]
        public CheckListEnum BracosAxiais { get; set; }
        public bool? BracosAxiaisExecutado { get; set; }


        [Display(Name = "Barra Estabilizadora")]
        public CheckListEnum BarraEstabilizadora { get; set; }
        public bool? BarraEstabilizadoraExecutada { get; set; }

        [Display(Name = "Pivôs")]
        public CheckListEnum Pivos { get; set; }
        public bool? PivosExecutado { get; set; }

        [Display(Name = "Bandeja")]
        public CheckListEnum Bandeja { get; set; }
        public bool? BandejaExecutado { get; set; }
        #endregion

        #region [ Step 4]
        [Display(Name = "Pastilhas")]
        public CheckListEnum Pastilhas { get; set; }
        public bool? PastilhasExecutado { get; set; }

        [Display(Name = "Discos")]
        public CheckListEnum Discos { get; set; }
        public bool? DiscosExecutado { get; set; }

        [Display(Name = "Freio Traseiro")]
        public CheckListEnum FreioTraseiro { get; set; }
        public bool? FreioTraseiroExecutado { get; set; }

        [Display(Name = "Fluído de Freio")]
        public CheckListEnum FluidoFreio { get; set; }
        public bool? FluidoFreioExecutado { get; set; }
        #endregion

        #region [ Step 5 ]
        [Display(Name = "Balanceamento")]
        public CheckListEnum Balanceamento { get; set; }
        public bool? BalanceamentoExecutado { get; set; }

        [Display(Name = "Alinhamento")]
        public CheckListEnum Alinhamento { get; set; }
        public bool? AlinhamentoExecutado { get; set; }

        [Display(Name = "Conserto da Roda")]
        public CheckListEnum ConsertoRoda { get; set; }
        public bool? ConsertoRodaExecutado { get; set; }

        [Display(Name = "Cambagem/Cáster")]
        public CheckListEnum CambagemCáster { get; set; }
        public bool? CambagemCásterExecutado { get; set; }

        #endregion

        #region [ Step 6 ]
        [Display(Name = "Palhetas")]
        public CheckListEnum Palhetas { get; set; }
        public bool? PalhetasExecutado { get; set; }

        [Display(Name = "Calotas")]
        public CheckListEnum Calotas { get; set; }
        public bool? CalotasExecutado { get; set; }

        [Display(Name = "Extintor")]
        public CheckListEnum Extintor { get; set; }
        public bool? ExtintorExecutado { get; set; }

        [Display(Name = "Injeção eletrônica")]
        public CheckListEnum InjecaoEletronica { get; set; }
        public bool? InjecaoEletronicaExecutado { get; set; }

        [Display(Name = "Higienização de ar")]
        public CheckListEnum HigienizacaoDeAr { get; set; }
        public bool? HigienizacaoDeArExecutado { get; set; }

        [Display(Name = "Filtro de ar condicionado")]
        public CheckListEnum FiltroArCondicionado { get; set; }
        public bool? FiltroArCondicionadoExecutado { get; set; }

        #endregion

        #region [ Step 7]
        [Display(Name = "Tipo óleo")]
        public string TipoOleo { get; set; }
        [Display(Name = "Especificação")]
        public string Especificacao { get; set; }
        [Display(Name = "Óleo do motor ")]
        [DataType(DataType.Date)]
        public DateTime? OleoMotor { get; set; }
        [Display(Name = "Filtro de óleo ")]
        public DateTime? FiltroOleo { get; set; }
        [Display(Name = "Código Della Via (óleo)")]
        public string CodigoDellaVia { get; set; }
        [Display(Name = "Filtro de ar")]
        public CheckListEnum FiltroDeAr { get; set; }
        public bool? FiltroDeArExecutado { get; set; }

        [Display(Name = "Filtro de combustível")]
        public CheckListEnum FiltroCombustivel { get; set; }
        public bool? FiltroCombustivelExecutado { get; set; }
        #endregion

        #region [ Step 8]
        [Display(Name = "Observações Gerais")]
        public string Observacoes { get; set; }
        #endregion

        public long Id { get; set; }
        public long CheckupId { get; set; }
        [Required]
        public string OrcamentoId { get; set; }
        [Display(Name = "Email Cliente")]
        public string EmailCliente { get; set; }
    }
}