using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class OrcamentoPagamentoDto
    {
        /// <summary>
        /// Id + Descrição
        /// </summary>

        public OrcamentoPagamentoDto()
        {
            FormasPagamentos = new List<OrcamentoFormaPagamentoDto>();
        }
        public string CondicaoPagamento { get; set; }
        public List<OrcamentoFormaPagamentoDto> FormasPagamentos { get; set; }
        public decimal ValorRestante { get; set; }
    }
}