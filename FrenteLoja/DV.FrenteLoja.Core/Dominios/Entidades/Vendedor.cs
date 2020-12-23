using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Vendedor: Entidade
    {
        public string IdConsultor { get; set; }
        public string Nome { get; set; }
	    public string FilialOrigem { get; set; }
        public string IdLoja { get; set; }
        public string IdRegional { get; set; }
        public string IdUser { get; set; }
        public virtual ICollection<Orcamento> OrcamentoList { get; set; }
	    public virtual ICollection<OrcamentoItemEquipeMontagem> OrcamentoItemEquipeMontagemList { get; set; }
        public virtual ICollection<Checkup> CheckupList { get; set; }
        public virtual ICollection<Checkup> CheckupTecnicoList { get; set; }
    }
}