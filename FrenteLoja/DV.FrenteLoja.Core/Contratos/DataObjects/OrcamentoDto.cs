using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class OrcamentoDto
    {
        public OrcamentoDto()
        {
            OrcamentoProduto = new OrcamentoProdutoBuscaDto();
        }

        #region [ Inicio ]
        public long Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool StatusSomenteLeitura { get; set; }

        #region [ Veículo ]
        [Display(Name = "Placa")]
        public string PlacaVeiculo { get; set; }
        [Display(Name = "Marca")]
        public string MarcaVeiculo { get; set; }
        public string MarcaVeiculoDescricao { get; set; }
        public string SinespMarcaVeiculo { get; set; }
        [Display(Name = "Modelo")]
        public string ModeloVeiculo { get; set; }
        public string ModeloVeiculoDescricao { get; set; }
        public string SinespModeloVeiculo { get; set; }
        [Display(Name = "Versão")]
        public string VersaoVeiculo { get; set; }
        public string VersaoVeiculoDescricao { get; set; }
        public string SinespVersaoVeiculo { get; set; }
        [Display(Name = "Motor")]
        public string VersaoMotor { get; set; }
        public string VersaoMotorDescricao { get; set; }
        public string SinespMotorVeiculo { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Somente valores numéricos positivos")]
        [Display(Name = "Ano")]
        public int? AnoVeiculo { get; set; }
        public string AnoDescricao { get; set; }
        public string SinespAnoVeiculo { get; set; }
        public string SinespAnoModeloVeiculo { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Somente valores numéricos positivos")]
        [Display(Name = "KM Rodados")]
        public int? QuilometragemVeiculo { get; set; }
        public string VeiculoIdFraga { get; set; }
        #endregion

        #region [ Cliente ]
        public string IdCliente { get; set; }
        [Display(Name = "Código")]
        public string CodigoCliente { get; set; }
        [Display(Name = "CPF/CNPJ")]
        public string CPFCNPJCliente { get; set; }
        [Display(Name = "Nome")]
        public string NomeCliente { get; set; }
        [Display(Name = "Classificação")]
        public string ClassificacaoCliente { get; set; }
        [Display(Name = "Score")]
        public string ScoreCliente { get; set; }
        [Display(Name = "E-mail")]
        public string EmailCliente { get; set; }
        [Display(Name = "Celular")]
        public string CelularCliente { get; set; }
        [Display(Name = "Telefone")]
        public string TelefoneCliente { get; set; }
        [Display(Name = "Telefone Comercial")]
        public string TelefoneComercialCliente { get; set; }
        [RegularExpression("[-_,A-Za-z0-9]$")]
        [Display(Name = "Informações Cliente")]
        public string InformacoesCliente { get; set; }
        public string LojaCliente { get; set; }
        #endregion

        #endregion

        #region [ Complemento ]
        public string Vendedor { get; set; }
        public string VendedorDescricao { get; set; }
        [Display(Name = "Área de Negócio")]
        public string AreaNegocio { get; set; }
        public string AreaNegocioDescricao { get; set; }
        public bool TrocaCliente { get; set; }
        public bool TrocaProduto { get; set; }
        public bool TrocaTabelaPreco { get; set; }
        public TrocaPrecoConvenio TrocaPrecoConvenio { get; set; }
        public bool Voltar { get; set; }
        [Display(Name = "Tabela de preço")]
        public string TabelaPreco { get; set; }
        public string TabelaPrecoDescricao { get; set; }
        [Display(Name = "Convênio")]
        public string Convenio { get; set; }
        public string ConvenioDescricao { get; set; }
        public string IdClienteLogado { get; set; }
        [Display(Name = "Loja Destino")]
        public string LojaDestino { get; set; }
        public string LojaDestinoDescricao { get; set; }
        [RegularExpression("[-_,A-Za-z0-9]$")]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        [Display(Name = "Informações Convênio")]
        public string InformacaoConvenio { get; set; }
        [Display(Name = "Tipo de Orçamento")]
        public TipoOrcamento TipoOrcamento { get; set; }
        [Display(Name = "Status do Cliente")]
        public StatusCliente StatusCliente { get; set; }
        [Display(Name = "Data validade")]
        public DateTime DataValidade { get; set; }
        public long? Transportadora { get; set; }
        public string TransportadoraDescricao { get; set; }
        [Display(Name = "Pedido (xPed)")]
        public string Xped { get; set; }
        [Display(Name = "Mensagem NF")]
        public string MensagemNF { get; set; }
        #endregion

        #region [ Busca Produto ]
        public string GrupoProduto { get; set; }
        public string FabricantePeca { get; set; }
        public string Numero { get; set; }
        public string CampoCodigo { get; set; }
        public virtual OrcamentoProdutoBuscaDto OrcamentoProduto { get; set; }
        public int OrcamentoProdutoCount { get; set; }
        #endregion

        #region [ Negociação ]
        public bool ReservaEstoque { get; set; }
        public OrcamentoPagamentoDto FormaPagamento { get; set; }
        public decimal ValorRestante { get; set; }
        public decimal ValorImpostos { get; set; }
        #endregion

        #region [ Finalização ]
        public List<SolicitacaoAnaliseCreditoDto> SolicitacoesAnaliseCredito { get; set; }
        #endregion

        #region [ Tooltips ]
        public List<WizardDto> TooltipInformacoesVeiculo
        {
            get
            {
                var conteudoTooltip = new List<WizardDto>();
                var itemTooltip = new WizardDto()
                {
                    Titulo = "Orçamento",
                    informacoes = new List<string>() { ConvenioDescricao, TabelaPrecoDescricao, LojaDestinoDescricao }
                };
                conteudoTooltip.Add(itemTooltip);
                itemTooltip = new WizardDto()
                {
                    Titulo = "Veículo",
                    informacoes = new List<string> { "", MarcaVeiculoDescricao + " " + ModeloVeiculoDescricao + " " + VersaoVeiculoDescricao, VersaoMotorDescricao, QuilometragemVeiculo + " KM" }
                };
                if (!string.IsNullOrEmpty(PlacaVeiculo))
                    itemTooltip.informacoes[0] = PlacaVeiculo;
                conteudoTooltip.Add(itemTooltip);
                itemTooltip = new WizardDto()
                {
                    Titulo = "Cliente",
                    informacoes = new List<string>() { $"{CodigoCliente} {NomeCliente}", CPFCNPJCliente, EmailCliente, CelularCliente, TelefoneCliente}
                };
                conteudoTooltip.Add(itemTooltip);
                return conteudoTooltip;
            }
        }

        public List<WizardDto> TooltipInformacoesEquipeVenda
        {
            get
            {
                var conteudoTooltip = new List<WizardDto>();
                var itemTooltip = new WizardDto()
                {
                    Titulo = "Vendedor",
                    informacoes = new List<string>() { VendedorDescricao, AreaNegocioDescricao, LojaDestinoDescricao }
                };
                conteudoTooltip.Add(itemTooltip);
                itemTooltip = new WizardDto()
                {
                    Titulo = "Orçamento",
                    informacoes = new List<string>() { $"Válido até {DataValidade.ToShortDateString()}", TransportadoraDescricao, !string.IsNullOrEmpty(Xped) ? $"xped {Xped}" : "" }
                };
                conteudoTooltip.Add(itemTooltip);
                return conteudoTooltip;
            }
        }
        #endregion

        #region [ Dados Relatório ]
        public string LogradouroLoja { get; set; }
        public string BairroLoja { get; set; }
        public string CidadeLoja { get; set; }
        public string EstadoLoja { get; set; }
        public string CepLoja { get; set; }
        public string CnpjLoja { get; set; }
        public string InscricaoEstadualLoja { get; set; }
        public string TelefoneLoja { get; set; }

        public ObservacaoDto ObservacoesRelatorio { get; set; }
        public List<ObservacaoDto> AtividadesDellaViaRelatorio { get; set; }
	    public string LojaDestinoCampoCodigo { get; set; }
        public string Complemento {get; set;}
	    #endregion
    }
}
