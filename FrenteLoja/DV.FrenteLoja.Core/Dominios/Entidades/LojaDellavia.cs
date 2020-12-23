using DV.FrenteLoja.Core.Contratos.Abstratos;
using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class LojaDellaVia : Entidade
    {
        public long? BancoId { get; set; }
        public virtual Banco Banco { get; set; }
        public virtual ICollection<LojaDellaViaProxima> LojasDellaViaProximas { get; set; }
        public virtual ICollection<Orcamento> OrcamentoList { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Telefone { get; set; }
    }
}