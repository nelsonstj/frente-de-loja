using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Linq;

namespace DV.FrenteLoja.Core.Util
{
    public static class ConversorStatusOrcamento
    {
        public static string MapStatus(Orcamento obj)
        {
            //if (((obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva) && obj.DataValidade.Date < DateTime.Today) || obj.StatusOrcamento == StatusOrcamento.EmAbertoVencido)
            //{
            //    return StatusConsultaOrcamento.Vencidos.GetDescription();
            //}
            //if ((obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva) && obj.OrcamentoItens.Any(a => a.SolicitacaoDescontoVendaAlcadaList.Any(b => b.StatusSolicitacaoAlcada == StatusSolicitacao.PendenteRetorno)))
            //{
            //    return StatusConsultaOrcamento.AguardandoRetornoSolicitacaoAlcada.GetDescription();
            //}

            //if (!obj.IsOrigemProtheus && (obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva))
            //{
            //    return StatusConsultaOrcamento.EmAberto.GetDescription();
            //}

            //if (obj.StatusOrcamento != StatusOrcamento.EmAberto && obj.StatusOrcamento != StatusOrcamento.Reserva)
            //{
            //    return StatusConsultaOrcamento.Encerrados.GetDescription();
            //}

            //if (obj.IsOrigemProtheus && (obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva))
            //{
            //    return StatusConsultaOrcamento.OrigemProtheus.GetDescription();
            //}

            return "Não Definido";
        }

        public static IQueryable<Orcamento> AdicionaQueryStatus(IQueryable<Orcamento> query, StatusOrcamento? statusOrcamento)
        {
            switch (statusOrcamento)
            {
                //case StatusConsultaOrcamento.EmAberto:
                //    query = query.Where(obj => !obj.IsOrigemProtheus && (obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva) && obj.DataValidade.Date >= DateTime.Today && obj.StatusOrcamento != StatusOrcamento.EmAbertoVencido);
                //    return query;
                //case StatusConsultaOrcamento.Encerrados:
                //    query = query.Where(obj => obj.StatusOrcamento != StatusOrcamento.EmAberto && obj.StatusOrcamento != StatusOrcamento.Reserva);
                //    return query;
                //case StatusConsultaOrcamento.OrigemProtheus:
                //    query = query.Where(obj => obj.IsOrigemProtheus && (obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva));
                //    return query;
                //case StatusConsultaOrcamento.AguardandoRetornoSolicitacaoAlcada:
                //    query = query.Where(obj => (obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva) && obj.OrcamentoItens.Any(a => a.SolicitacaoDescontoVendaAlcadaList.Any(b => b.StatusSolicitacaoAlcada == StatusSolicitacao.PendenteRetorno)));
                //    return query;
                //case StatusConsultaOrcamento.Vencidos:
                //    query = query.Where(obj => ((obj.StatusOrcamento == StatusOrcamento.EmAberto || obj.StatusOrcamento == StatusOrcamento.Reserva) && obj.DataValidade.Date < DateTime.Today) || obj.StatusOrcamento == StatusOrcamento.EmAbertoVencido);
                //    return query;
                default:
                    return query;
            }
        }
    }
}
