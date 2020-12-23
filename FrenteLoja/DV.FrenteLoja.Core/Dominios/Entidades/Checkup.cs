using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Checkup : Entidade
    {
        public long OrcamentoId { get; set; }
        public virtual Orcamento Orcamento { get; set; }
        public long? VendedorId { get; set; }
        public virtual Vendedor Vendedor { get; set; }
        public long? TecnicoResponsavelId { get; set; }
        public virtual Vendedor TecnicoResponsavel { get; set; }

        public bool IsCheckupCar { get; set; }

        public virtual ICollection<CheckupCar> CheckupCarList { get; set; }
        public virtual ICollection<CheckupTruck> CheckupTruckList { get; set; }
    }
}
