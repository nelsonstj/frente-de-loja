using System.Collections.Generic;
using DV.FrenteLoja.Core.Contratos.Abstratos;
using Newtonsoft.Json;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class Catalogo : Entidade
    {
        public string CodigoFabricante { get; set; }
        public string FabricantePeca { get; set; }
        public string InformacoesComplementares { get; set; }
        public string MarcaVeiculo { get; set; }
        public string ModeloVeiculo { get; set; }
        public string VersaoVeiculo { get; set; }
        public int AnoInicial { get; set; }
        public int AnoFinal { get; set; }       
        public string CodigoDellavia { get; set; }
        [JsonIgnore]
        public virtual ICollection<CatalogoProdutosCorrelacionados> CatalogoProdutoRelacionadoList { get; set; }
        public string VersaoMotor { get; set; }
    }
}
