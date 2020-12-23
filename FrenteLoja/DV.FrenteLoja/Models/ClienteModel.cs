using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DV.FrenteLoja.Models
{
    public class ClienteModel
    {
        public long Id { get; set; }
        public string CNPJCPF { get; set; }
        public string Telefone { get; set; }
        public string TelefoneComercial { get; set; }
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        public string Loja { get; set; }
        public string Nome { get; set; }
        public long? BancoId { get; set; }
        public virtual Banco Banco { get; set; }
        public int StatusCliente { get; set; }
        public int MotivoBloqueioCredito { get; set; }
        public string TipoCliente { get; set; }
        public string ClassificacaoCliente { get; set; }
        public string Score { get; set; }
        //public virtual ICollection<ClienteVeiculo> Veiculos { get; set; }
        public virtual ICollection<Convenio> ConvenioList { get; set; }
        public virtual ICollection<Orcamento> OrcamentoList { get; set; }
    }
}