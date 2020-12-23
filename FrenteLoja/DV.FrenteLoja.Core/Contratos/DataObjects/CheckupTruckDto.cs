using DV.FrenteLoja.Core.Contratos.Enums;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class CheckupTruckDto
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

        #region [ Step 1]
        [Display(Name = "Embuchamento manga de eixo")]
        public CheckListEnum Embuchamento { get; set; }
       public bool? EmbuchamentoExecutado { get; set; }

        [Display(Name = "Terminal barra longa")]
        public CheckListEnum TerminalBarraLonga { get; set; }
       public bool? TerminalBarraLongaExecutado { get; set; }

        [Display(Name = "Barra curta direção")]
        public CheckListEnum BarraCurtaDireção { get; set; }
       public bool? BarraCurtaDireçãoExecutado { get; set; }

        [Display(Name = "Lona ou pastilha de freio")]
        public CheckListEnum LonaPastilhaFreioDianteiro { get; set; }
       public bool? LonaPastilhaFreioDianteiroExecutado { get; set; }

        [Display(Name = "Amortecedor")]
        public CheckListEnum Amortecedor { get; set; }
       public bool? AmortecedorExecutado { get; set; }

        [Display(Name = "Barra estabilizadora")]
        public CheckListEnum BarraEstabilozadora { get; set; }
       public bool? BarraEstabilozadoraExecutado { get; set; }
     
        [Display(Name = "Tambor ou disco de freio")]
        public CheckListEnum TamborDiscoFreio { get; set; }
       public bool? TamborDiscoFreioExecutado { get; set; }

        [Display(Name = "Molejo completo")]
        public CheckListEnum MolejoCompleto { get; set; }
       public bool? MolejoCompletoExecutado { get; set; }
        #endregion

        #region [Step 2]
        [Display(Name = "Braço tensor")]
        public CheckListEnum BracoTensor { get; set; }
       public bool? BracoTensorExecutado { get; set; }

        [Display(Name = "Balança e pino")]
        public CheckListEnum BalancaPino { get; set; }
       public bool? BalancaPinoExecutado { get; set; }

        [Display(Name = "Lona e pastinha de freio")]
        public CheckListEnum LonaPastilhaFreioTraseiro { get; set; }
       public bool? LonaPastilhaFreioTraseiroExecutado { get; set; }

        [Display(Name = "Catraca flexível")]
        public CheckListEnum CatracaFlexivel { get; set; }
       public bool? CatracaFlexivelExecutado { get; set; }

        [Display(Name = "Molejo e suporte")]
        public CheckListEnum MolejoSuporte { get; set; }
       public bool? MolejoSuporteExecutado { get; set; }

        [Display(Name = "Suspensor Truck")]
        public CheckListEnum SuspensorTruck { get; set; }
       public bool? SuspensorTruckExecutado { get; set; }

        [Display(Name = "Tambor ou disco de freio")]
        public CheckListEnum TamborDiscoFreioTraseiro { get; set; }
       public bool? TamborDiscoFreioTraseiroTruckExecutado { get; set; }

        [Display(Name = "Amortecedor")]
        public CheckListEnum AmortecedorTraseiro { get; set; }
       public bool? AmortecedorTraseiroExecutado { get; set; }



        #endregion

        #region [ Step 3]
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