namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class PDCondicaoPagamento
    {
        public string IdCondicaoPagamento { get; set; }
        public string NomeCondicaoPagamento { get; set; }
        public string DescricaoCondicaoPagamento { get; set; }
        public string IdFormaPagamento { get; set; }
        public string PrazoMedio { get; set; }
        public int QtdParcelas { get; set; }
        public decimal ValorAcrescimo { get; set; }

        /*
                public string FormaPagamento { get; set; }
                public string FormaCondicaoPagamento { get; set; }
                public string ListaTipoVenda { get; set; }
                public string TipoCondicaoPagamento { get; set; }
                public bool CondicaoDeVenda { get; set; }
        */
    }
}
