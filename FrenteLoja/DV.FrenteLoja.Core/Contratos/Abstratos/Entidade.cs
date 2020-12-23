using DV.FrenteLoja.Core.Contratos.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.Abstratos
{
    public abstract class Entidade : IEntidade
    {
        [Key]
        public long Id { get; set; }
        public string Descricao { get; set; }
        [Display(Name = "Inativo")]
        public bool RegistroInativo { get; set;}
        [Display(Name = "Código")]
        public string CampoCodigo { get; set; }
        private DateTime? _dataAtualizacao;
        [DataType(DataType.DateTime)]
        [Display(Name = "Data Atualização")]
        public DateTime DataAtualizacao
        {
            get { return _dataAtualizacao ?? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local); }
            set { _dataAtualizacao = value; }
        }
        [Display(Name = "Usuário Atualização")]
        public string UsuarioAtualizacao { get; set; }
    }
}
