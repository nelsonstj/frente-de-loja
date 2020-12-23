using System;
using System.ComponentModel.DataAnnotations;
using DV.FrenteLoja.Core.Contratos.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
	public class LogCargaCatalogoDto
	{
		public LogCargaCatalogoDto()
		{
			DataInicio = DateTime.Now;
			DataFim = DateTime.Now.AddDays(1);
		}

		public string NomeArquivo { get; set; }
		[JsonIgnore]
		public string LogImportacao { get; set; }
		[Display(Name = "Status da Integração")]
		[JsonConverter(typeof(StringEnumConverter))]
		public StatusIntegracao? StatusIntegracao { get; set; }

		[Display(Name = "Data Inicio")]
		public DateTime DataInicio { get; set; }
		[Display(Name = "Data Fim")]
		public DateTime DataFim { get; set; }
		public DateTime DataAtualizacao { get; set; }
		public long Id { get; set; }
	}
}
