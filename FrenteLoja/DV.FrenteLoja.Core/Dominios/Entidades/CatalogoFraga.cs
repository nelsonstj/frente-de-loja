using System;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class CatalogoFraga
    {
        public string ProdutoDescricao { get; set; }
        public string ProdutoMarca { get; set; }
        public string ProdutoCodDellavia { get; set; }
        public string ProdutoCodFabricante { get; set; }
        public string ProdutoFabricantePeca { get; set; }
        public string GrupoAplicacao { get; set; }
        public string InformacoesComplementares { get; set; }
        public string VeiculoIdFraga { get; set; }
        public string VeiculoMarca { get; set; }
        public string VeiculoModelo { get; set; }
        public string VeiculoVersao { get; set; }
        public string VersaoMotor { get; set; }
        public string VeiculoAnoInicial { get; set; }
        public string VeiculoAnoFinal { get; set; }
        public string IdGrupoServicoAgregado { get; set; }
        public string CodigoGrupo { get; set; }
        public string DescricaoGrupo { get; set; }
        public string CodigoSubGrupo { get; set; }
        public string DescricaoSubGrupo { get; set; }
        public int? Estoque { get; set; }
        public string Valor { get; set; }
        public string TabelaPreco { get; set; }
        public string IdLojaDestino { get; set; }
    }
}
