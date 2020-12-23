using System;
using System.IO;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using Stimulsoft.Report;
using System.Collections.Generic;

namespace DV.FrenteLoja.Reports
{
	public class RelatoriosServico: IRelatoriosServico
	{
		private readonly IRepositorioEscopo _escopo;
		private readonly IRepositorio<Orcamento> _orcamentoRepositorio;

		public RelatoriosServico(IRepositorioEscopo escopo)
		{
			_escopo = escopo;
			_orcamentoRepositorio = escopo.GetRepositorio<Orcamento>();
		}
		public byte[] ObterRelatorioOrcamento(long idOrcamento)
		{
				
			var orcamento = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento);
			if(orcamento == null)
				throw new NegocioException("Orçamento não encontrado.");

			
			var report = RelatorioHelper.ObterRelatorio(TipoRelatorio.Orcamento);
			report.RegBusinessObject("orcamento",new List<Orcamento> { orcamento } );
			report.Render();

			StiReport stiPdfExportService = new StiReport();

			var configuracoes = new Stimulsoft.Report.Export.StiExcel2007ExportSettings();
			configuracoes.ExportPageBreaks = false;
			configuracoes.UseOnePageHeaderAndFooter = true;

			var diretorioTemporario = Path.Combine(Path.GetTempPath(), "relatorio_" + Guid.NewGuid().ToString().Replace("-", "") + ".pdf");


			stiPdfExportService.ExportDocument(StiExportFormat.Pdf,diretorioTemporario);

			var service = new Stimulsoft.Report.Export.StiPdfExportService();
			MemoryStream stream = new MemoryStream();
			service.ExportPdf(new StiReport(), stream,StiPagesRange.All);
			return stream.ToArray();
		}
	}
}