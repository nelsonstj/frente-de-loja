using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using DV.FrenteLoja.Core.Contratos.Enums;
using Stimulsoft.Report;

namespace DV.FrenteLoja.Reports
{
	public static class RelatorioHelper
	{
		public const string LOCALIZACAO = "~/bin/Localizacao/pt-BR.xml";

		public const string FONTE = "'Source Sans Pro', sans-serif";

		public static readonly Color CorFundo = ColorTranslator.FromHtml("#c2ccb7");

		public static readonly Color CorFonte = ColorTranslator.FromHtml("#495f32");

		public static StiReport ObterRelatorio(TipoRelatorio tipo)
		{
			var nomeRelatorio = tipo.GetDescription();
			var nomeTemplate = $"{nomeRelatorio}.mrt";
			var nomeAssembly = $"{nomeRelatorio}.dll";

			StiReport relatorio;

			if (!File.Exists(nomeAssembly))
			{
				relatorio = new StiReport();
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nomeTemplate))
				{
					relatorio.Load(stream);
					relatorio.Compile(nomeAssembly);
				}
			}
			else
			{
				using (var fileStream = new FileStream(nomeAssembly, FileMode.Open, FileAccess.Read))
				{
					relatorio = StiReport.GetReportFromAssembly(fileStream);
				}
			}

			return relatorio;
		}

		private static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		private static byte[] StreamByteArray(this Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}

		private static byte[] StreamToArray(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}
