using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class CheckupDto
    {
        public static implicit operator CheckupCarDto(CheckupDto checkupDTO)
        {
            if (!checkupDTO.IsCheckupCar)
                return null;

            var checkupCar = new CheckupCarDto
            {
                Cliente = checkupDTO.Cliente,
                CPFCNPJ = checkupDTO.CPFCNPJ,
                Phone = checkupDTO.Phone,
                License = checkupDTO.License,
                Car = checkupDTO.Car,
                Year = checkupDTO.Year,
                KM = checkupDTO.KM,
                Marca = checkupDTO.Marca,
                Versao = checkupDTO.Versao,
                Tecnico = checkupDTO.Tecnico,
                Vendedor = checkupDTO.Vendedor,
                TecnicoNome = checkupDTO.TecnicoNome,
                VendedorNome = checkupDTO.VendedorNome,
                ProdutoServico = checkupDTO.ProdutoServico,
                OrcamentoId = checkupDTO.OrcamentoId,
                EmailCliente = checkupDTO.EmailCliente,
                CheckupId = checkupDTO.CheckupId,
                Id =checkupDTO.Id

            };

            return checkupCar;
        }

        public static implicit operator CheckupTruckDto(CheckupDto checkupDTO)
        {
            if (checkupDTO.IsCheckupCar)
                return null;

            var checkupTruck = new CheckupTruckDto
            {
                Cliente = checkupDTO.Cliente,
                CPFCNPJ = checkupDTO.CPFCNPJ,
                Phone = checkupDTO.Phone,
                License = checkupDTO.License,
                Car = checkupDTO.Car,
                Year = checkupDTO.Year,
                KM = checkupDTO.KM,
                Marca = checkupDTO.Marca,
                Versao = checkupDTO.Versao,
                Tecnico = checkupDTO.Tecnico,
                Vendedor = checkupDTO.Vendedor,
                TecnicoNome = checkupDTO.TecnicoNome,
                VendedorNome = checkupDTO.VendedorNome,
                ProdutoServico = checkupDTO.ProdutoServico,
                OrcamentoId = checkupDTO.OrcamentoId,
                EmailCliente = checkupDTO.EmailCliente,
                CheckupId = checkupDTO.CheckupId,
                Id = checkupDTO.Id

            };

            return checkupTruck;
        }

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

        public long CheckupId { get; set; }
        public long Id { get; set; }
        [Required]
        public string OrcamentoId { get; set; }
        [Display(Name = "Email Cliente")]
        public string EmailCliente { get; set; }
    }
}