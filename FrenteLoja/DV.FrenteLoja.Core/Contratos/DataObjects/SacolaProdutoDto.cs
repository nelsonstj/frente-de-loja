using DV.FrenteLoja.Core.Contratos.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class SacolaProdutoDto
    {
        public SacolaProdutoDto()
        {
            Servicos = new List<ServicoCorrelacionadoDto>();
            ProfissionaisMontagem = new List<ProfissionalMontagemDto>();
            SolicitacoesDescontoAlcada = new List<SolicitacaoDescontoVendaAlcadaDto>();
        }
        public long IdOrcamentoItem { get; set; }
        public string Descricao { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public List<ServicoCorrelacionadoDto> Servicos { get; set; }
        public List<ProfissionalMontagemDto> ProfissionaisMontagem { get; set; }
        public int NumeroItem { get; set; }
        public string CampoCodigoProduto { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal PercentualDesconto { get; set; }
        public decimal ValorTotal { get; set; }
        public List<SolicitacaoDescontoVendaAlcadaDto> SolicitacoesDescontoAlcada { get; set; }
        public TipoItemOrcamento TipoItem { get; set; }
    }
}
