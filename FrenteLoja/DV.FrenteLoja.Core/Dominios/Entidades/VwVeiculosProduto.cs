using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using DV.FrenteLoja.Core.Contratos.Interfaces;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class VwVeiculoProdutos : IEntidade
    {
        [Key]
        public string Veiculo_Id { get; set; }
        public string PartNumber { get; set; }
        public string GrupoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string MarcaProduto { get; set; }

        // Ignorado - Não utilizar por ser uma View
        public long Id { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
        public string Descricao { get; set; }
        public bool RegistroInativo { get; set; }
    }
}
