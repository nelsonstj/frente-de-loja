using System;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface IEntidade
    {
        long Id { get; set; }
        DateTime DataAtualizacao { get; set; }
        string Descricao { get; set; }
        string UsuarioAtualizacao { get; set; }
        bool RegistroInativo { get; set; }
    }
}