using DV.FrenteLoja.Core.Contratos.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class LogIntegracaoDto
    {
        public LogIntegracaoDto()
        {
            DataInicio = DateTime.Now;
            DataFim = DateTime.Now.AddDays(1);
        }

        [Display(Name ="Tipo Tabela Protheus")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TipoTabelaProtheus? TipoTabelaProtheus { get; set; }
        [JsonIgnoreAttribute]
        public string DadosIntegracaoJson { get; set; }
        [JsonIgnoreAttribute]
        public string Log { get; set; }
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
