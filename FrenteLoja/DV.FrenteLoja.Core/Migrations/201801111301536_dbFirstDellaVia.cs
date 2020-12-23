namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class dbFirstDellaVia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CHECKUP_CAR",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CheckupId = c.Long(nullable: false),
                    ConvergenciaTotalInicial = c.Decimal(precision: 18, scale: 1),
                    CamberEsquerdoDianteiroInicial = c.Decimal(precision: 18, scale: 2),
                    CasterEsquerdoInicial = c.Decimal(precision: 18, scale: 2),
                    CamberEsquerdoTraseiroInicial = c.Decimal(precision: 18, scale: 2),
                    ConvergenciaEsquerdoInicial = c.Decimal(precision: 18, scale: 1),
                    CamberDireitoDianteiroInicial = c.Decimal(precision: 18, scale: 2),
                    CasterDireitoInicial = c.Decimal(precision: 18, scale: 2),
                    CamberDireitoTraseiroInicial = c.Decimal(precision: 18, scale: 2),
                    ConvergenciaDireitoInicial = c.Decimal(precision: 18, scale: 1),
                    ConvergenciaTotalFinal = c.Decimal(precision: 18, scale: 1),
                    CamberEsquerdoDianteiroFinal = c.Decimal(precision: 18, scale: 2),
                    CasterEsquerdoFinal = c.Decimal(precision: 18, scale: 2),
                    CamberEsquerdoTraseiroFinal = c.Decimal(precision: 18, scale: 2),
                    ConvergenciaEsquerdoFinal = c.Decimal(precision: 18, scale: 1),
                    CamberDireitoDianteiroFinal = c.Decimal(precision: 18, scale: 2),
                    CasterDireitoFinal = c.Decimal(precision: 18, scale: 2),
                    CamberDireitoTraseiroFinal = c.Decimal(precision: 18, scale: 2),
                    ConvergenciaDireitoFinal = c.Decimal(precision: 18, scale: 1),
                    PneuDianteiro = c.Int(nullable: false),
                    PneuTraseiro = c.Int(nullable: false),
                    Estepe = c.Int(nullable: false),
                    Valvula = c.Int(nullable: false),
                    AmortecedorDianteiro = c.Int(nullable: false),
                    AmortecedorTraseiro = c.Int(nullable: false),
                    TerminaisDirecao = c.Int(nullable: false),
                    BracosAxiais = c.Int(nullable: false),
                    Pivos = c.Int(nullable: false),
                    Bandeja = c.Int(nullable: false),
                    Pastilhas = c.Int(nullable: false),
                    Discos = c.Int(nullable: false),
                    FreioTraseiro = c.Int(nullable: false),
                    FluidoFreio = c.Int(nullable: false),
                    Balanceamento = c.Int(nullable: false),
                    Alinhamento = c.Int(nullable: false),
                    ConsertoRoda = c.Int(nullable: false),
                    CambagemCaster = c.Int(nullable: false),
                    Palhetas = c.Int(nullable: false),
                    Calotas = c.Int(nullable: false),
                    Extintor = c.Int(nullable: false),
                    InjecaoEletronica = c.Int(nullable: false),
                    HigienizacaoAr = c.Int(nullable: false),
                    FiltroArCondicionado = c.Int(nullable: false),
                    PneuDianteiroExecutado = c.Boolean(nullable: false),
                    PneuTraseiroExecutado = c.Boolean(nullable: false),
                    EstepeExecutado = c.Boolean(nullable: false),
                    ValvulaExecutado = c.Boolean(nullable: false),
                    AmortecedorDianteiroExecutado = c.Boolean(nullable: false),
                    AmortecedorTraseiroExecutado = c.Boolean(nullable: false),
                    TerminaisDirecaoExecutado = c.Boolean(nullable: false),
                    BracosAxiaisExecutado = c.Boolean(nullable: false),
                    PivosExecutado = c.Boolean(nullable: false),
                    BandejaExecutado = c.Boolean(nullable: false),
                    PastilhasExecutado = c.Boolean(nullable: false),
                    DiscosExecutado = c.Boolean(nullable: false),
                    FreioTraseiroExecutado = c.Boolean(nullable: false),
                    FluidoFreioExecutado = c.Boolean(nullable: false),
                    BalanceamentoExecutado = c.Boolean(nullable: false),
                    AlinhamentoExecutado = c.Boolean(nullable: false),
                    ConsertoRodaExecutado = c.Boolean(nullable: false),
                    CambagemCasterExecutado = c.Boolean(nullable: false),
                    PalhetasExecutado = c.Boolean(nullable: false),
                    CalotasExecutado = c.Boolean(nullable: false),
                    ExtintorExecutado = c.Boolean(nullable: false),
                    InjecaoEletronicaExecutado = c.Boolean(nullable: false),
                    HigienizacaoArExecutado = c.Boolean(nullable: false),
                    FiltroArCondicionadoExecutado = c.Boolean(nullable: false),
                    DataTrocaOleo = c.DateTime(),
                    DataTrocaFiltroOleo = c.DateTime(),
                    TipoOleo = c.Int(nullable: false),
                    Especificacao = c.String(),
                    CodigoDellaVia = c.String(),
                    FiltroAr = c.Int(nullable: false),
                    FiltroCombustivel = c.Int(nullable: false),
                    FiltroArExecutado = c.Boolean(nullable: false),
                    FiltroCombustivelExecutado = c.Boolean(nullable: false),
                    Observacoes = c.String(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CHECKUP", t => t.CheckupId, cascadeDelete: true)
                .Index(t => t.CheckupId);

            CreateTable(
                "dbo.CHECKUP",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    OrcamentoId = c.Long(nullable: false),
                    VendedorId = c.Long(),
                    TecnicoResponsavelId = c.Long(),
                    IsCheckupCar = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ORCAMENTO", t => t.OrcamentoId, cascadeDelete: true)
                .ForeignKey("dbo.VENDEDOR", t => t.TecnicoResponsavelId)
                .ForeignKey("dbo.VENDEDOR", t => t.VendedorId)
                .Index(t => t.OrcamentoId)
                .Index(t => t.VendedorId)
                .Index(t => t.TecnicoResponsavelId);

            CreateTable(
                "dbo.CHECKUP_TRUCK",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CheckupId = c.Long(nullable: false),
                    Embuchamento = c.Int(nullable: false),
                    TerminalBarraLonga = c.Int(nullable: false),
                    BarraCurtaDirecao = c.Int(nullable: false),
                    LonaPastilhaFreioDianteiro = c.Int(nullable: false),
                    Amortecedor = c.Int(nullable: false),
                    BarraEstabilizadora = c.Int(nullable: false),
                    TamborDiscoFreioDianteiro = c.Int(nullable: false),
                    MolejoCompleto = c.Int(nullable: false),
                    BracoTensor = c.Int(nullable: false),
                    BalancaPino = c.Int(nullable: false),
                    LonaPastilhaFreioTraseiro = c.Int(nullable: false),
                    CatracaFlexivel = c.Int(nullable: false),
                    MolejoSuporte = c.Int(nullable: false),
                    SuspensorTruck = c.Int(nullable: false),
                    TamborDiscoFreioTraseiro = c.Int(nullable: false),
                    AmortecedorTraseiro = c.Int(nullable: false),
                    EmbuchamentoExecutado = c.Boolean(nullable: false),
                    TerminalBarraLongaExecutado = c.Boolean(nullable: false),
                    BarraCurtaDirecaoExecutado = c.Boolean(nullable: false),
                    LonaPastilhaFreioDianteiroExecutado = c.Boolean(nullable: false),
                    AmortecedorExecutado = c.Boolean(nullable: false),
                    BarraEstabilizadoraExecutado = c.Boolean(nullable: false),
                    TamborDiscoFreioDianteiroExecutado = c.Boolean(nullable: false),
                    MolejoCompletoExecutado = c.Boolean(nullable: false),
                    BracoTensorExecutado = c.Boolean(nullable: false),
                    BalancaPinoExecutado = c.Boolean(nullable: false),
                    LonaPastilhaFreioTraseiroExecutado = c.Boolean(nullable: false),
                    CatracaFlexivelExecutado = c.Boolean(nullable: false),
                    MolejoSuporteExecutado = c.Boolean(nullable: false),
                    SuspensorTruckExecutado = c.Boolean(nullable: false),
                    TamborDiscoFreioTraseiroExecutado = c.Boolean(nullable: false),
                    AmortecedorTraseiroExecutado = c.Boolean(nullable: false),
                    Observacoes = c.String(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CHECKUP", t => t.CheckupId, cascadeDelete: true)
                .Index(t => t.CheckupId);

            CreateTable(
                "dbo.ORCAMENTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsOrigemProtheus = c.Boolean(nullable: false),
                    IdConvenio = c.Long(nullable: false),
                    IdCliente = c.Long(nullable: false),
                    DataValidade = c.DateTime(nullable: false),
                    IdTabelaPreco = c.Long(nullable: false),
                    Complemento = c.String(),
                    Telefone = c.String(),
                    IdVendedor = c.Long(nullable: false),
                    InformacoesCliente = c.String(),
                    Placa = c.String(),
                    Ano = c.Int(nullable: false),
                    IdMarcaModeloVersao = c.Long(nullable: false),
                    TelefoneComercial = c.String(),
                    TelefoneCelular = c.String(),
                    IdLojaDellaVia = c.Long(nullable: false),
                    IdOperador = c.Long(nullable: false),
                    IdTransportadora = c.Long(),
                    PossuiReservaEstoque = c.Boolean(nullable: false),
                    StatusOrcamento = c.Int(nullable: false),
                    ExisteAlcadaPendente = c.Boolean(nullable: false),
                    KM = c.Int(nullable: false),
                    IdTipoVenda = c.Long(nullable: false),
                    TipoOrcamento = c.Int(nullable: false),
                    Xped = c.String(),
                    MensagemNF = c.String(),
                    IdBanco = c.Long(),
                    DataCriacao = c.DateTime(nullable: false),
                    ValorImpostos = c.Decimal(nullable: false, precision: 18, scale: 2),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                    VeiculoId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BANCO", t => t.IdBanco)
                .ForeignKey("dbo.CLIENTE", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.CONVENIO", t => t.IdConvenio)
                .ForeignKey("dbo.LOJA_DELLAVIA", t => t.IdLojaDellaVia, cascadeDelete: true)
                .ForeignKey("dbo.MARCA_MODELO_VERSAO", t => t.IdMarcaModeloVersao, cascadeDelete: true)
                .ForeignKey("dbo.OPERADOR", t => t.IdOperador, cascadeDelete: true)
                .ForeignKey("dbo.TABELA_PRECO", t => t.IdTabelaPreco, cascadeDelete: true)
                .ForeignKey("dbo.TIPO_VENDA", t => t.IdTipoVenda, cascadeDelete: true)
                .ForeignKey("dbo.TRANSPORTADORA", t => t.IdTransportadora)
                .ForeignKey("dbo.VENDEDOR", t => t.IdVendedor, cascadeDelete: true)
                .ForeignKey("dbo.VEICULO", t => t.VeiculoId, cascadeDelete: true)
                .Index(t => t.IdConvenio)
                .Index(t => t.IdCliente)
                .Index(t => t.IdTabelaPreco)
                .Index(t => t.IdVendedor)
                .Index(t => t.IdMarcaModeloVersao)
                .Index(t => t.IdLojaDellaVia)
                .Index(t => t.IdOperador)
                .Index(t => t.IdTransportadora)
                .Index(t => t.IdTipoVenda)
                .Index(t => t.IdBanco)
                .Index(t => t.CampoCodigo)
                .Index(t => t.VeiculoId);

            CreateTable(
                "dbo.BANCO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.ORCAMENTO_FORMA_PAGAMENTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdOrcamento = c.Long(nullable: false),
                    IdCondicaoPagamento = c.Long(nullable: false),
                    IdAdministradoraFinanceira = c.Long(),
                    IdBanco = c.Long(),
                    TotalValorForma = c.Decimal(nullable: false, precision: 18, scale: 2),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ADMINISTRACAO_FINANCEIRA", t => t.IdAdministradoraFinanceira)
                .ForeignKey("dbo.BANCO", t => t.IdBanco)
                .ForeignKey("dbo.CONDICAO_PAGAMENTO", t => t.IdCondicaoPagamento, cascadeDelete: true)
                .ForeignKey("dbo.ORCAMENTO", t => t.IdOrcamento, cascadeDelete: true)
                .Index(t => t.IdOrcamento)
                .Index(t => t.IdCondicaoPagamento)
                .Index(t => t.IdAdministradoraFinanceira)
                .Index(t => t.IdBanco);

            CreateTable(
                "dbo.ADMINISTRACAO_FINANCEIRA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FormaPagamento = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.CONDICAO_PAGAMENTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    QtdParcelas = c.Int(nullable: false),
                    ValorAcrescimo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    FormaPagamento = c.String(),
                    FormaCondicaoPagamento = c.String(),
                    ListaTipoVenda = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.CONVENIO_CONDICAO_PAGAMENTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdConvenio = c.Long(nullable: false),
                    IdCondicaoPagamento = c.Long(nullable: false),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CONDICAO_PAGAMENTO", t => t.IdCondicaoPagamento, cascadeDelete: true)
                .ForeignKey("dbo.CONVENIO", t => t.IdConvenio, cascadeDelete: true)
                .Index(t => t.IdConvenio)
                .Index(t => t.IdCondicaoPagamento)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.CONVENIO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Observacoes = c.String(),
                    IdTabelaPreco = c.Long(nullable: false),
                    DataInicioVigencia = c.DateTime(nullable: false),
                    DataFimVigencia = c.DateTime(nullable: false),
                    TrocaCliente = c.Boolean(nullable: false),
                    TrocaPreco = c.Byte(nullable: false),
                    TrocaProduto = c.Boolean(nullable: false),
                    IdCliente = c.Long(),
                    TrocaTabelaPreco = c.Boolean(nullable: false),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CLIENTE", t => t.IdCliente)
                .ForeignKey("dbo.TABELA_PRECO", t => t.IdTabelaPreco, cascadeDelete: true)
                .Index(t => t.IdTabelaPreco)
                .Index(t => t.IdCliente)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.CLIENTE",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CNPJCPF = c.String(),
                    Telefone = c.String(),
                    TelefoneComercial = c.String(),
                    TelefoneCelular = c.String(),
                    Email = c.String(),
                    Loja = c.String(maxLength: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "UK_CLienteLoja",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: UK_CLienteLoja, Order: 2, IsUnique: True }")
                                },
                            }),
                    Nome = c.String(),
                    BancoId = c.Long(),
                    StatusCliente = c.Int(nullable: false),
                    MotivoBloqueioCredito = c.Int(nullable: false),
                    TipoCliente = c.String(),
                    ClassificacaoCliente = c.String(),
                    Score = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "UK_CLienteLoja",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: UK_CLienteLoja, Order: 1, IsUnique: True }")
                                },
                            }),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BANCO", t => t.BancoId)
                .Index(t => t.BancoId);

            CreateTable(
                "dbo.CLIENTE_VEICULO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ClienteId = c.Long(nullable: false),
                    VeiculoId = c.Long(nullable: false),
                    Placa = c.String(),
                    VeiculoAno = c.Int(nullable: false),
                    Observacoes = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CLIENTE", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.VEICULO", t => t.VeiculoId, cascadeDelete: true)
                .Index(t => t.ClienteId)
                .Index(t => t.VeiculoId);

            CreateTable(
                "dbo.MARCA_MODELO_VERSAO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdMarcaModelo = c.Long(nullable: false),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MARCA_MODELO", t => t.IdMarcaModelo, cascadeDelete: true)
                .Index(t => t.IdMarcaModelo);

            CreateTable(
                "dbo.MARCA_MODELO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdMarca = c.Long(nullable: false),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MARCA", t => t.IdMarca, cascadeDelete: true)
                .Index(t => t.IdMarca)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.MARCA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.CONVENIO_CLIENTE",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdConvenio = c.Long(nullable: false),
                    IdCliente = c.Long(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CONVENIO", t => t.IdConvenio, cascadeDelete: true)
                .Index(t => t.IdConvenio)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.CONVENIO_PRODUTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdConvenio = c.Long(nullable: false),
                    IdProduto = c.Long(),
                    IdGrupoProduto = c.Long(),
                    TipoPreco = c.Int(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CONVENIO", t => t.IdConvenio, cascadeDelete: true)
                .Index(t => t.IdConvenio)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.TABELA_PRECO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataDe = c.DateTime(nullable: false),
                    DataAte = c.DateTime(nullable: false),
                    CodCondicaoPagamento = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.TABELA_PRECO_ITEM",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TabelaPrecoId = c.Long(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    PrecoVenda = c.Decimal(nullable: false, precision: 18, scale: 2),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PRODUTO", t => t.ProdutoId, cascadeDelete: true)
                .ForeignKey("dbo.TABELA_PRECO", t => t.TabelaPrecoId, cascadeDelete: true)
                .Index(t => t.TabelaPrecoId)
                .Index(t => t.ProdutoId)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.PRODUTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdGrupoProduto = c.Long(nullable: false),
                    CodigoFabricante = c.String(),
                    IdGrupoServicoAgregado = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GRUPO_PRODUTO", t => t.IdGrupoProduto, cascadeDelete: true)
                .Index(t => t.IdGrupoProduto)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.GRUPO_PRODUTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    GrupoProdutoId = c.Long(nullable: false),
                    PercentualDescontoVendedor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercentualDescontoGerente = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TipoVendaId = c.Long(nullable: false),
                    LojaDellaviaId = c.Long(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GRUPO_PRODUTO", t => t.GrupoProdutoId, cascadeDelete: true)
                .ForeignKey("dbo.LOJA_DELLAVIA", t => t.LojaDellaviaId, cascadeDelete: true)
                .ForeignKey("dbo.TIPO_VENDA", t => t.TipoVendaId, cascadeDelete: true)
                .Index(t => t.GrupoProdutoId)
                .Index(t => t.TipoVendaId)
                .Index(t => t.LojaDellaviaId)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.LOJA_DELLAVIA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    BancoId = c.Long(),
                    Logradouro = c.String(),
                    Bairro = c.String(),
                    Cidade = c.String(),
                    Estado = c.String(),
                    Cep = c.String(),
                    Cnpj = c.String(),
                    InscricaoEstadual = c.String(),
                    Telefone = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BANCO", t => t.BancoId)
                .Index(t => t.BancoId)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.LOJA_DELLAVIA_PROXIMA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdLojaDellaViaReferencia = c.Long(nullable: false),
                    NrOrdem = c.Int(nullable: false),
                    IdLojaDellaVia = c.Long(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LOJA_DELLAVIA", t => t.IdLojaDellaViaReferencia, cascadeDelete: true)
                .Index(t => t.IdLojaDellaViaReferencia);

            CreateTable(
                "dbo.TIPO_VENDA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.PRODUTO_COMPLEMENTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdProduto = c.Long(nullable: false),
                    Comprimento = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Espessura = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Largura = c.Decimal(nullable: false, precision: 18, scale: 2),
                    VolumeM3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CampoHTML = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PRODUTO", t => t.IdProduto, cascadeDelete: true)
                .Index(t => t.IdProduto)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.OPERADOR",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    VendedorId = c.Long(),
                    PercLimiteDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VENDEDOR", t => t.VendedorId)
                .Index(t => t.VendedorId);

            CreateTable(
                "dbo.VENDEDOR",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                    FilialOrigem = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdOrcamentoItem = c.Long(nullable: false),
                    IdVendedor = c.Long(nullable: false),
                    Funcao = c.Int(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ORCAMENTO_ITEM", t => t.IdOrcamentoItem)
                .ForeignKey("dbo.VENDEDOR", t => t.IdVendedor)
                .Index(t => t.IdOrcamentoItem)
                .Index(t => t.IdVendedor);

            CreateTable(
                "dbo.ORCAMENTO_ITEM",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    OrcamentoId = c.Long(nullable: false),
                    IdDescontoModeloVenda = c.Long(),
                    NrItem = c.Int(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PrecoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TotalItem = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ProdutoId = c.Long(nullable: false),
                    NrItemProdutoPaiId = c.Int(),
                    ValorDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercDescon = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TipoOperacao = c.String(),
                    ReservaEstoque = c.Boolean(nullable: false),
                    DescontoModeloVendaUtilizado = c.Int(),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DESCONTO_MODELO_VENDA", t => t.IdDescontoModeloVenda)
                .ForeignKey("dbo.ORCAMENTO", t => t.OrcamentoId, cascadeDelete: true)
                .ForeignKey("dbo.PRODUTO", t => t.ProdutoId, cascadeDelete: true)
                .Index(t => t.OrcamentoId)
                .Index(t => t.IdDescontoModeloVenda)
                .Index(t => t.ProdutoId);

            CreateTable(
                "dbo.DESCONTO_MODELO_VENDA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TabelasDePrecoAssociadas = c.String(),
                    CodigosDeProdutoLiberados = c.String(),
                    PercentualDesconto1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercentualDesconto2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercentualDesconto3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercentualDesconto4 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TipoVendaId = c.Long(nullable: false),
                    Bloqueado = c.Boolean(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TIPO_VENDA", t => t.TipoVendaId, cascadeDelete: true)
                .Index(t => t.TipoVendaId);

            CreateTable(
                "dbo.SOLICITACAO_DESCONTO_VENDA_ALCADA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdOrcamentoItem = c.Long(nullable: false),
                    DataSolicitacao = c.DateTime(nullable: false),
                    ObservacaoItem = c.String(),
                    ObservacaoGeral = c.String(),
                    RespostaSolicitacao = c.String(),
                    DataResposta = c.DateTime(),
                    StatusSolicitacaoAlcada = c.Int(nullable: false),
                    ValorDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercentualDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Situacao = c.Int(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ORCAMENTO_ITEM", t => t.IdOrcamentoItem, cascadeDelete: true)
                .Index(t => t.IdOrcamentoItem);

            CreateTable(
                "dbo.SOLICITACAO_ANALISE_CREDITO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdOrcamento = c.Long(nullable: false),
                    DataSolicitacao = c.DateTime(nullable: false),
                    StatusSolicitacaoAnaliseCredito = c.Int(nullable: false),
                    SituacaoAnaliseCredito = c.Int(nullable: false),
                    DataResposta = c.DateTime(),
                    RespostaSolicitacao = c.String(),
                    NumeroContrato = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ORCAMENTO", t => t.IdOrcamento, cascadeDelete: true)
                .Index(t => t.IdOrcamento);

            CreateTable(
                "dbo.TRANSPORTADORA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.ORCAMENTO_OBSERVACAO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Titulo = c.String(),
                    Conteudo = c.String(),
                    TipoObservacao = c.Int(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CATALOGO_ARQUIVO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                    Arquivo = c.Binary(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CATALOGO_CARGA_LOG",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    LogImportacao = c.String(),
                    NomeArquivo = c.String(),
                    StatusIntegracao = c.Int(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CATALOGO_PRODUTO_CORRELACIONADO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdCatalogo = c.Long(nullable: false),
                    CodigoFabricante = c.String(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CATALOGO", t => t.IdCatalogo, cascadeDelete: true)
                .Index(t => t.IdCatalogo);

            CreateTable(
                "dbo.CATALOGO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CodigoFabricante = c.String(),
                    FabricantePeca = c.String(),
                    InformacoesComplementares = c.String(),
                    MarcaVeiculo = c.String(),
                    ModeloVeiculo = c.String(),
                    VersaoVeiculo = c.String(),
                    AnoInicial = c.Int(nullable: false),
                    AnoFinal = c.Int(nullable: false),
                    CodigoDellavia = c.String(),
                    Descricao = c.String(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.DESCONTO_VENDA_ALCADA",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    LojaDellaviaId = c.Long(nullable: false),
                    PercentualDescontoVendedor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PercentualDescontoGerente = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TipoVendaId = c.Long(nullable: false),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LOJA_DELLAVIA", t => t.LojaDellaviaId, cascadeDelete: true)
                .ForeignKey("dbo.TIPO_VENDA", t => t.TipoVendaId, cascadeDelete: true)
                .Index(t => t.LojaDellaviaId)
                .Index(t => t.TipoVendaId)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.GRUPO_SERVICO_AGREGADO_PRODUTO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdProduto = c.Long(nullable: false),
                    Item = c.String(),
                    PermiteAlterarQuantidade = c.Boolean(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IdGrupoServicoAgregado = c.String(),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

            CreateTable(
                "dbo.LOG_INTEGRACAO",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TipoTabelaProtheus = c.Int(nullable: false),
                    LogErro = c.String(),
                    StatusIntegracao = c.Int(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.PARAMETRO_GERAL",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    RegistroInativo = c.Boolean(nullable: false),
                    CampoCodigo = c.String(maxLength: 12),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAtualizacao = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CampoCodigo, unique: true);

        }

        public override void Down()
        {
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA", "TipoVendaId", "dbo.TIPO_VENDA");
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaviaId", "dbo.LOJA_DELLAVIA");
            DropForeignKey("dbo.CATALOGO_PRODUTO_CORRELACIONADO", "IdCatalogo", "dbo.CATALOGO");
            DropForeignKey("dbo.CHECKUP_CAR", "CheckupId", "dbo.CHECKUP");
            DropForeignKey("dbo.CHECKUP", "VendedorId", "dbo.VENDEDOR");
            DropForeignKey("dbo.CHECKUP", "TecnicoResponsavelId", "dbo.VENDEDOR");
            DropForeignKey("dbo.CHECKUP", "OrcamentoId", "dbo.ORCAMENTO");
            DropForeignKey("dbo.ORCAMENTO", "IdVendedor", "dbo.VENDEDOR");
            DropForeignKey("dbo.ORCAMENTO", "IdTransportadora", "dbo.TRANSPORTADORA");
            DropForeignKey("dbo.ORCAMENTO", "IdTipoVenda", "dbo.TIPO_VENDA");
            DropForeignKey("dbo.ORCAMENTO", "IdTabelaPreco", "dbo.TABELA_PRECO");
            DropForeignKey("dbo.SOLICITACAO_ANALISE_CREDITO", "IdOrcamento", "dbo.ORCAMENTO");
            DropForeignKey("dbo.ORCAMENTO", "IdOperador", "dbo.OPERADOR");
            DropForeignKey("dbo.OPERADOR", "VendedorId", "dbo.VENDEDOR");
            DropForeignKey("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdVendedor", "dbo.VENDEDOR");
            DropForeignKey("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdOrcamentoItem", "dbo.ORCAMENTO_ITEM");
            DropForeignKey("dbo.SOLICITACAO_DESCONTO_VENDA_ALCADA", "IdOrcamentoItem", "dbo.ORCAMENTO_ITEM");
            DropForeignKey("dbo.ORCAMENTO_ITEM", "ProdutoId", "dbo.PRODUTO");
            DropForeignKey("dbo.ORCAMENTO_ITEM", "OrcamentoId", "dbo.ORCAMENTO");
            DropForeignKey("dbo.ORCAMENTO_ITEM", "IdDescontoModeloVenda", "dbo.DESCONTO_MODELO_VENDA");
            DropForeignKey("dbo.DESCONTO_MODELO_VENDA", "TipoVendaId", "dbo.TIPO_VENDA");
            DropForeignKey("dbo.ORCAMENTO", "IdMarcaModeloVersao", "dbo.MARCA_MODELO_VERSAO");
            DropForeignKey("dbo.ORCAMENTO", "IdLojaDellaVia", "dbo.LOJA_DELLAVIA");
            DropForeignKey("dbo.ORCAMENTO", "IdConvenio", "dbo.CONVENIO");
            DropForeignKey("dbo.ORCAMENTO", "IdCliente", "dbo.CLIENTE");
            DropForeignKey("dbo.ORCAMENTO", "IdBanco", "dbo.BANCO");
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdOrcamento", "dbo.ORCAMENTO");
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO");
            DropForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdConvenio", "dbo.CONVENIO");
            DropForeignKey("dbo.CONVENIO", "IdTabelaPreco", "dbo.TABELA_PRECO");
            DropForeignKey("dbo.TABELA_PRECO_ITEM", "TabelaPrecoId", "dbo.TABELA_PRECO");
            DropForeignKey("dbo.TABELA_PRECO_ITEM", "ProdutoId", "dbo.PRODUTO");
            DropForeignKey("dbo.PRODUTO_COMPLEMENTO", "IdProduto", "dbo.PRODUTO");
            DropForeignKey("dbo.PRODUTO", "IdGrupoProduto", "dbo.GRUPO_PRODUTO");
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "TipoVendaId", "dbo.TIPO_VENDA");
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaviaId", "dbo.LOJA_DELLAVIA");
            DropForeignKey("dbo.LOJA_DELLAVIA_PROXIMA", "IdLojaDellaViaReferencia", "dbo.LOJA_DELLAVIA");
            DropForeignKey("dbo.LOJA_DELLAVIA", "BancoId", "dbo.BANCO");
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "GrupoProdutoId", "dbo.GRUPO_PRODUTO");
            DropForeignKey("dbo.CONVENIO_PRODUTO", "IdConvenio", "dbo.CONVENIO");
            DropForeignKey("dbo.CONVENIO_CLIENTE", "IdConvenio", "dbo.CONVENIO");
            DropForeignKey("dbo.CONVENIO", "IdCliente", "dbo.CLIENTE");
            DropForeignKey("dbo.CLIENTE_VEICULO", "MarcaModeloVersaoId", "dbo.MARCA_MODELO_VERSAO");
            DropForeignKey("dbo.MARCA_MODELO_VERSAO", "IdMarcaModelo", "dbo.MARCA_MODELO");
            DropForeignKey("dbo.MARCA_MODELO", "IdMarca", "dbo.MARCA");
            DropForeignKey("dbo.CLIENTE_VEICULO", "ClienteId", "dbo.CLIENTE");
            DropForeignKey("dbo.CLIENTE", "BancoId", "dbo.BANCO");
            DropForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO");
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdBanco", "dbo.BANCO");
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdAdministradoraFinanceira", "dbo.ADMINISTRACAO_FINANCEIRA");
            DropForeignKey("dbo.CHECKUP_TRUCK", "CheckupId", "dbo.CHECKUP");
            DropIndex("dbo.PARAMETRO_GERAL", new[] { "CampoCodigo" });
            DropIndex("dbo.GRUPO_SERVICO_AGREGADO_PRODUTO", new[] { "CampoCodigo" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA", new[] { "CampoCodigo" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA", new[] { "TipoVendaId" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA", new[] { "LojaDellaviaId" });
            DropIndex("dbo.CATALOGO_PRODUTO_CORRELACIONADO", new[] { "IdCatalogo" });
            DropIndex("dbo.TRANSPORTADORA", new[] { "CampoCodigo" });
            DropIndex("dbo.SOLICITACAO_ANALISE_CREDITO", new[] { "IdOrcamento" });
            DropIndex("dbo.SOLICITACAO_DESCONTO_VENDA_ALCADA", new[] { "IdOrcamentoItem" });
            DropIndex("dbo.DESCONTO_MODELO_VENDA", new[] { "TipoVendaId" });
            DropIndex("dbo.ORCAMENTO_ITEM", new[] { "ProdutoId" });
            DropIndex("dbo.ORCAMENTO_ITEM", new[] { "IdDescontoModeloVenda" });
            DropIndex("dbo.ORCAMENTO_ITEM", new[] { "OrcamentoId" });
            DropIndex("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", new[] { "IdVendedor" });
            DropIndex("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", new[] { "IdOrcamentoItem" });
            DropIndex("dbo.VENDEDOR", new[] { "CampoCodigo" });
            DropIndex("dbo.OPERADOR", new[] { "VendedorId" });
            DropIndex("dbo.PRODUTO_COMPLEMENTO", new[] { "CampoCodigo" });
            DropIndex("dbo.PRODUTO_COMPLEMENTO", new[] { "IdProduto" });
            DropIndex("dbo.TIPO_VENDA", new[] { "CampoCodigo" });
            DropIndex("dbo.LOJA_DELLAVIA_PROXIMA", new[] { "IdLojaDellaViaReferencia" });
            DropIndex("dbo.LOJA_DELLAVIA", new[] { "CampoCodigo" });
            DropIndex("dbo.LOJA_DELLAVIA", new[] { "BancoId" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "CampoCodigo" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "LojaDellaviaId" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "TipoVendaId" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "GrupoProdutoId" });
            DropIndex("dbo.GRUPO_PRODUTO", new[] { "CampoCodigo" });
            DropIndex("dbo.PRODUTO", new[] { "CampoCodigo" });
            DropIndex("dbo.PRODUTO", new[] { "IdGrupoProduto" });
            DropIndex("dbo.TABELA_PRECO_ITEM", new[] { "CampoCodigo" });
            DropIndex("dbo.TABELA_PRECO_ITEM", new[] { "ProdutoId" });
            DropIndex("dbo.TABELA_PRECO_ITEM", new[] { "TabelaPrecoId" });
            DropIndex("dbo.TABELA_PRECO", new[] { "CampoCodigo" });
            DropIndex("dbo.CONVENIO_PRODUTO", new[] { "CampoCodigo" });
            DropIndex("dbo.CONVENIO_PRODUTO", new[] { "IdConvenio" });
            DropIndex("dbo.CONVENIO_CLIENTE", new[] { "CampoCodigo" });
            DropIndex("dbo.CONVENIO_CLIENTE", new[] { "IdConvenio" });
            DropIndex("dbo.MARCA", new[] { "CampoCodigo" });
            DropIndex("dbo.MARCA_MODELO", new[] { "CampoCodigo" });
            DropIndex("dbo.MARCA_MODELO", new[] { "IdMarca" });
            DropIndex("dbo.MARCA_MODELO_VERSAO", new[] { "IdMarcaModelo" });
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "MarcaModeloVersaoId" });
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "ClienteId" });
            DropIndex("dbo.CLIENTE", new[] { "BancoId" });
            DropIndex("dbo.CONVENIO", new[] { "CampoCodigo" });
            DropIndex("dbo.CONVENIO", new[] { "IdCliente" });
            DropIndex("dbo.CONVENIO", new[] { "IdTabelaPreco" });
            DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "CampoCodigo" });
            DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "IdCondicaoPagamento" });
            DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "IdConvenio" });
            DropIndex("dbo.CONDICAO_PAGAMENTO", new[] { "CampoCodigo" });
            DropIndex("dbo.ADMINISTRACAO_FINANCEIRA", new[] { "CampoCodigo" });
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdBanco" });
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdAdministradoraFinanceira" });
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdCondicaoPagamento" });
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdOrcamento" });
            DropIndex("dbo.BANCO", new[] { "CampoCodigo" });
            DropIndex("dbo.ORCAMENTO", new[] { "CampoCodigo" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdBanco" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdTipoVenda" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdTransportadora" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdOperador" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdLojaDellaVia" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdMarcaModeloVersao" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdVendedor" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdTabelaPreco" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdCliente" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdConvenio" });
            DropIndex("dbo.CHECKUP_TRUCK", new[] { "CheckupId" });
            DropIndex("dbo.CHECKUP", new[] { "TecnicoResponsavelId" });
            DropIndex("dbo.CHECKUP", new[] { "VendedorId" });
            DropIndex("dbo.CHECKUP", new[] { "OrcamentoId" });
            DropIndex("dbo.CHECKUP_CAR", new[] { "CheckupId" });
            DropTable("dbo.PARAMETRO_GERAL");
            DropTable("dbo.LOG_INTEGRACAO");
            DropTable("dbo.GRUPO_SERVICO_AGREGADO_PRODUTO");
            DropTable("dbo.DESCONTO_VENDA_ALCADA");
            DropTable("dbo.CATALOGO");
            DropTable("dbo.CATALOGO_PRODUTO_CORRELACIONADO");
            DropTable("dbo.CATALOGO_CARGA_LOG");
            DropTable("dbo.CATALOGO_ARQUIVO");
            DropTable("dbo.ORCAMENTO_OBSERVACAO");
            DropTable("dbo.TRANSPORTADORA");
            DropTable("dbo.SOLICITACAO_ANALISE_CREDITO");
            DropTable("dbo.SOLICITACAO_DESCONTO_VENDA_ALCADA");
            DropTable("dbo.DESCONTO_MODELO_VENDA");
            DropTable("dbo.ORCAMENTO_ITEM");
            DropTable("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM");
            DropTable("dbo.VENDEDOR");
            DropTable("dbo.OPERADOR");
            DropTable("dbo.PRODUTO_COMPLEMENTO");
            DropTable("dbo.TIPO_VENDA");
            DropTable("dbo.LOJA_DELLAVIA_PROXIMA");
            DropTable("dbo.LOJA_DELLAVIA");
            DropTable("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO");
            DropTable("dbo.GRUPO_PRODUTO");
            DropTable("dbo.PRODUTO");
            DropTable("dbo.TABELA_PRECO_ITEM");
            DropTable("dbo.TABELA_PRECO");
            DropTable("dbo.CONVENIO_PRODUTO");
            DropTable("dbo.CONVENIO_CLIENTE");
            DropTable("dbo.MARCA");
            DropTable("dbo.MARCA_MODELO");
            DropTable("dbo.MARCA_MODELO_VERSAO");
            DropTable("dbo.CLIENTE_VEICULO");
            DropTable("dbo.CLIENTE",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CampoCodigo",
                        new Dictionary<string, object>
                        {
                            { "UK_CLienteLoja", "IndexAnnotation: { Name: UK_CLienteLoja, Order: 1, IsUnique: True }" },
                        }
                    },
                    {
                        "Loja",
                        new Dictionary<string, object>
                        {
                            { "UK_CLienteLoja", "IndexAnnotation: { Name: UK_CLienteLoja, Order: 2, IsUnique: True }" },
                        }
                    },
                });
            DropTable("dbo.CONVENIO");
            DropTable("dbo.CONVENIO_CONDICAO_PAGAMENTO");
            DropTable("dbo.CONDICAO_PAGAMENTO");
            DropTable("dbo.ADMINISTRACAO_FINANCEIRA");
            DropTable("dbo.ORCAMENTO_FORMA_PAGAMENTO");
            DropTable("dbo.BANCO");
            DropTable("dbo.ORCAMENTO");
            DropTable("dbo.CHECKUP_TRUCK");
            DropTable("dbo.CHECKUP");
            DropTable("dbo.CHECKUP_CAR");
        }
    }
}
