using AutoMapper;
using System.Linq;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Util;
using System;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.MapperConfig
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<VeiculoMedidasPneus, VeiculoMedidasPneusDTO>();
                cfg.CreateMap<VersaoMotor, VersaoMotorDTO>();
                cfg.CreateMap<Veiculo, VeiculoDTO>();
                cfg.CreateMap<Cliente, ClienteDto>()
                    .ForMember(dto => dto.Telefone, op => op.MapFrom(entidade => FormatarTelefone.Formatar(entidade.Telefone)))
                    .ForMember(dto => dto.TelefoneCelular, op => op.MapFrom(entidade => FormatarTelefone.Formatar(entidade.TelefoneCelular)))
                    .ForMember(dto => dto.TelefoneComercial, op => op.MapFrom(entidade => FormatarTelefone.Formatar(entidade.TelefoneComercial)));
                cfg.CreateMap<Catalogo, CatalogoDto>()
                    .ForMember(dto => dto.CatalogoProdutoRelacionadoList, op => op.MapFrom(entidade => entidade.CatalogoProdutoRelacionadoList.Select(s => s.CodigoFabricante)));
                cfg.CreateMap<CatalogoProdutosCorrelacionados, CatalogoProdutosCorrelacionadosDto>();
                cfg.CreateMap<Marca, MarcaDto>();
                cfg.CreateMap<MarcaModelo, MarcaModeloDto>();
                cfg.CreateMap<MarcaModeloVersao, ClienteMarcaModeloVersaoDto>();
                cfg.CreateMap<Convenio, ConvenioDto>();
                cfg.CreateMap<ConvenioDto, Convenio>();
                cfg.CreateMap<TipoVenda, AreaNegocioDto>();
                cfg.CreateMap<LogIntegracao, LogIntegracaoDto>();
                cfg.CreateMap<CatalogoCargaLog, LogCargaCatalogoDto>();
                cfg.CreateMap<ClienteVeiculo, ClienteVeiculoDto>();
                cfg.CreateMap<Vendedor, VendedorDto>()
                    .ForMember(dto => dto.Nome, op => op.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Nome));
                cfg.CreateMap<VendedorDto, Vendedor>();
                //cfg.CreateMap<Convenio, ConvenioDto>()
                //    .ForMember(dto => dto.Descricao, op => op.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Descricao));
                cfg.CreateMap<TipoVenda, AreaNegocioDto>()
                    .ForMember(dto => dto.Descricao, op => op.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Descricao));
                cfg.CreateMap<LojaDellaVia, LojaDellaViaDto>()
                    .ForMember(dto => dto.Descricao, op => op.MapFrom(entidade => entidade.Descricao.Substring(0, 2).Equals(entidade.CampoCodigo) ? entidade.Descricao : entidade.CampoCodigo + " - " + entidade.Descricao));
                cfg.CreateMap<Transportadora, TransportadoraDto>()
                    .ForMember(dto => dto.Descricao, op => op.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Descricao));
                cfg.CreateMap<Orcamento, OrcamentoDto>()
                    //.ForMember(dto => dto.ConvenioDescricao, dto => dto.MapFrom(entidade => entidade.Convenio.Descricao))
                    //.ForMember(dto => dto.Convenio, dto => dto.MapFrom(entidade => entidade.Convenio.Id))
                    //.ForMember(dto => dto.TabelaPreco, dto => dto.MapFrom(entidade => entidade.TabelaPreco.Id))
                    //.ForMember(dto => dto.TabelaPrecoDescricao, dto => dto.MapFrom(entidade => entidade.TabelaPreco.Descricao))
                    //.ForMember(dto => dto.Transportadora, dto => dto.MapFrom(entidade => entidade.Transportadora.Id))
                    //.ForMember(dto => dto.TransportadoraDescricao, dto => dto.MapFrom(entidade => entidade.Transportadora.Descricao))
                    //.ForMember(dto => dto.TipoVenda, dto => dto.MapFrom(entidade => entidade.TipoVenda.Id))
                    //.ForMember(dto => dto.TipoVendaDescricao, dto => dto.MapFrom(entidade => entidade.TipoVenda.Descricao))
                    .ForMember(dto => dto.NomeCliente, dto => dto.MapFrom(entidade => entidade.Cliente.Nome))
                    .ForMember(dto => dto.OrcamentoProduto, dto => dto.MapFrom(model => model));
                cfg.CreateMap<Orcamento, OrcamentoProdutoBuscaDto>()
                    .ForMember(dto => dto.Total, dto => dto.MapFrom(entidade => entidade.OrcamentoItens.Count > 0 ? entidade.OrcamentoItens.Sum(a => a.TotalItem) : decimal.Zero));
                cfg.CreateMap<OrcamentoDto, Orcamento>();
                    //.ForMember(dto => dto.Convenio, op => op.Ignore());
                    //.ForMember(dto => dto.TabelaPreco, op => op.Ignore());
                    //.ForMember(dto => dto.Transportadora, op => op.Ignore())
                    //.ForMember(dto => dto.TipoVenda, op => op.Ignore());
                cfg.CreateMap<OrcamentoItem, OrcamentoItemDto>();
                cfg.CreateMap<OrcamentoItemDto, OrcamentoItem>();
                cfg.CreateMap<TabelaPreco, TabelaPrecoDto>();
                cfg.CreateMap<Produto, ProdutoDto>();
                cfg.CreateMap<GrupoServicoAgregadoProduto, GrupoServicoAgregadoProdutoDto>();
                cfg.CreateMap<ProdutoComplemento, ProdutoComplementoDto>();
                cfg.CreateMap<Vendedor, ProfissionalMontagemDto>()
                    .ForMember(dto => dto.ProfissionalNome, dto => dto.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Nome));
                cfg.CreateMap<OrcamentoItemEquipeMontagem, ProfissionalMontagemDto>()
                    .ForMember(dto => dto.ProfissionalNome, dto => dto.MapFrom(entidade => entidade.Vendedor.Nome))
                    .ForMember(dto => dto.Id, dto => dto.MapFrom(entidade => entidade.Vendedor.Id));
                cfg.CreateMap<CondicaoPagamento, FormaPagamentoDto>()
                    .ForMember(dto => dto.Descricao, dto => dto.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Descricao));
                cfg.CreateMap<Banco, BancoDto>()
                    .ForMember(dto => dto.Descricao, dto => dto.MapFrom(entidade => entidade.CampoCodigo + " - " + entidade.Descricao));
                cfg.CreateMap<AdministradoraFinanceira, AdministradoraFinanceiraDto>();
                cfg.CreateMap<OrcamentoFormaPagamento, OrcamentoFormaPagamentoDto>()
                    //.ForMember(dto => dto.CondicaoPagamento, dto => dto.MapFrom(entidade => entidade.CondicaoPagamento.Descricao))
                    .ForMember(dto => dto.QtdParcelas, dto => dto.MapFrom(entidade => entidade.CondicaoPagamento.QtdParcelas))
                    .ForMember(dto => dto.ValorTotal, dto => dto.MapFrom(entidade => entidade.TotalValorForma))
                    .ForMember(dto => dto.ValorParcela, dto => dto.MapFrom(entidade => entidade.CondicaoPagamento.QtdParcelas > 1 ? entidade.TotalValorForma / entidade.CondicaoPagamento.QtdParcelas : 0));
                    //.ForMember(dto => dto.TemAcrescimo, dto => dto.MapFrom(entidade => entidade.CondicaoPagamento.ValorAcrescimo > 0));

                #region [CheckUp Car-Truck]

                cfg.CreateMap<CheckupCar, CheckupCarDto>()
                    .ForMember(dto => dto.Vendedor, dto => dto.MapFrom(entidade => (entidade.Checkup.Vendedor == null ? string.Empty : entidade.Checkup.Vendedor.Id.ToString())))
                    .ForMember(dto => dto.Tecnico, dto => dto.MapFrom(entidade => (entidade.Checkup.TecnicoResponsavel == null ? string.Empty : entidade.Checkup.TecnicoResponsavel.Id.ToString())))
                    .ForMember(dto => dto.VendedorNome, dto => dto.MapFrom(entidade => entidade.Checkup.Vendedor.Nome))
                    .ForMember(dto => dto.TecnicoNome, dto => dto.MapFrom(entidade => entidade.Checkup.TecnicoResponsavel.Nome))
                    .ForMember(dto => dto.OrcamentoId, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.Id))
                    .ForMember(dto => dto.CheckupId, dto => dto.MapFrom(entidade => entidade.Checkup.Id))
                    .ForMember(dto => dto.EmailCliente, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.Cliente.Email))
                    .ForMember(dto => dto.Cliente, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.Cliente.Nome))
                    //.ForMember(dto => dto.Car, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.MarcaModeloVersao.MarcaModelo.Descricao))
                    .ForMember(dto => dto.TipoOleo, dto => dto.MapFrom(entidade => entidade.TipoOleo.ToString()))
                    .ForMember(dto => dto.CambarEsquerdoFrontalInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberEsquerdoDianteiroInicial)))
                    .ForMember(dto => dto.CasterEsquerdoFrontalInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CasterEsquerdoInicial)))
                    .ForMember(dto => dto.CambarEsquerdoTraseiroInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberEsquerdoTraseiroInicial)))
                    .ForMember(dto => dto.ConvergenciaEsquerdaTraseiroInicial, dto => dto.MapFrom(entidade => entidade.ConvergenciaEsquerdoInicial))
                    .ForMember(dto => dto.CambarDireitoFrontalInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberDireitoDianteiroInicial)))
                    .ForMember(dto => dto.CasterDireitoFrontalInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CasterDireitoInicial)))
                    .ForMember(dto => dto.CambarDireitoTraseiroInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberDireitoTraseiroInicial)))
                    .ForMember(dto => dto.ConvergenciaDireitoTraseiroInicial, dto => dto.MapFrom(entidade => entidade.ConvergenciaDireitoInicial))
                    .ForMember(dto => dto.CambarEsquerdoFrontalFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberEsquerdoDianteiroFinal)))
                    .ForMember(dto => dto.CasterEsquerdoFrontalFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CasterEsquerdoFinal)))
                    .ForMember(dto => dto.CambarEsquerdoTraseiroFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberEsquerdoTraseiroFinal)))
                    .ForMember(dto => dto.ConvergenciaEsquerdaTraseiroFinal, dto => dto.MapFrom(entidade => entidade.ConvergenciaEsquerdoFinal))
                    .ForMember(dto => dto.CambarDireitoFrontalFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberDireitoDianteiroFinal)))
                    .ForMember(dto => dto.CasterDireitoFrontalFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CasterDireitoFinal)))
                    .ForMember(dto => dto.CambarDireitoTraseiroFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.ReverterSimbolosGraus(entidade.CamberDireitoTraseiroFinal)))
                    .ForMember(dto => dto.ConvergenciaDireitoTraseiroFinal, dto => dto.MapFrom(entidade => entidade.ConvergenciaDireitoFinal))
                    .ForMember(dto => dto.CambagemCáster, dto => dto.MapFrom(entidade => entidade.CambagemCaster))
                    .ForMember(dto => dto.CambagemCásterExecutado, dto => dto.MapFrom(entidade => entidade.CambagemCasterExecutado))
                    .ForMember(dto => dto.HigienizacaoDeAr, dto => dto.MapFrom(entidade => entidade.HigienizacaoAr))
                    .ForMember(dto => dto.HigienizacaoDeArExecutado, dto => dto.MapFrom(entidade => entidade.HigienizacaoArExecutado))
                    .ForMember(dto => dto.TerminaisDeDirecao, dto => dto.MapFrom(entidade => entidade.TerminaisDirecao))
                    .ForMember(dto => dto.TerminaisDeDirecaoExecutado, dto => dto.MapFrom(entidade => entidade.TerminaisDirecaoExecutado))
                    .ForMember(dto => dto.FiltroDeAr, dto => dto.MapFrom(entidade => entidade.FiltroAr))
                    .ForMember(dto => dto.FiltroDeArExecutado, dto => dto.MapFrom(entidade => entidade.FiltroArExecutado))
                    .ForMember(dto => dto.OleoMotor, dto => dto.MapFrom(entidade => entidade.DataTrocaOleo))
                    .ForMember(dto => dto.FiltroOleo, dto => dto.MapFrom(entidade => entidade.DataTrocaFiltroOleo));

                long tempVal;
                cfg.CreateMap<CheckupCarDto, CheckupCar>()
                    .ForMember(x => x.Checkup, opt => opt.ResolveUsing(model => new Checkup()
                    {
                        OrcamentoId = Int64.Parse(model.OrcamentoId),
                        VendedorId = Int64.TryParse(model.Vendedor, out tempVal) ? Int64.Parse(model.Vendedor) : (long?)null,
                        TecnicoResponsavelId = Int64.TryParse(model.Tecnico, out tempVal) ? Int64.Parse(model.Tecnico) : (long?)null
                    }))
                    .ForMember(dto => dto.TipoOleo, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirTipoOleo(entidade.TipoOleo)))
                    .ForMember(dto => dto.ConvergenciaTotalInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.ConvergenciaTotalInicial)))
                    .ForMember(dto => dto.CamberEsquerdoDianteiroInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarEsquerdoFrontalInicial)))
                    .ForMember(dto => dto.CasterEsquerdoInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CasterEsquerdoFrontalInicial)))
                    .ForMember(dto => dto.CamberEsquerdoTraseiroInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarEsquerdoTraseiroInicial)))
                    .ForMember(dto => dto.ConvergenciaEsquerdoInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.ConvergenciaEsquerdaTraseiroInicial)))
                    .ForMember(dto => dto.CamberDireitoDianteiroInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarDireitoFrontalInicial)))
                    .ForMember(dto => dto.CasterDireitoInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CasterDireitoFrontalInicial)))
                    .ForMember(dto => dto.CamberDireitoTraseiroInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarDireitoTraseiroInicial)))
                    .ForMember(dto => dto.ConvergenciaDireitoInicial, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.ConvergenciaDireitoTraseiroInicial)))
                    .ForMember(dto => dto.ConvergenciaTotalFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.ConvergenciaTotalFinal)))
                    .ForMember(dto => dto.CamberEsquerdoDianteiroFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarEsquerdoFrontalFinal)))
                    .ForMember(dto => dto.CasterEsquerdoFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CasterEsquerdoFrontalFinal)))
                    .ForMember(dto => dto.CamberEsquerdoTraseiroFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarEsquerdoTraseiroFinal)))
                    .ForMember(dto => dto.ConvergenciaEsquerdoFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.ConvergenciaEsquerdaTraseiroFinal)))
                    .ForMember(dto => dto.CamberDireitoDianteiroFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarDireitoFrontalFinal)))
                    .ForMember(dto => dto.CasterDireitoFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CasterDireitoFrontalFinal)))
                    .ForMember(dto => dto.CamberDireitoTraseiroFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.CambarDireitoTraseiroFinal)))
                    .ForMember(dto => dto.ConvergenciaDireitoFinal, dto => dto.MapFrom(entidade => ConversoesCheckup.SubstituirSimbolosGraus(entidade.ConvergenciaDireitoTraseiroFinal)))
                    .ForMember(dto => dto.TerminaisDirecao, dto => dto.MapFrom(entidade => entidade.TerminaisDeDirecao))
                    .ForMember(dto => dto.TerminaisDirecaoExecutado, dto => dto.MapFrom(entidade => entidade.TerminaisDeDirecaoExecutado))
                    .ForMember(dto => dto.CambagemCaster, dto => dto.MapFrom(entidade => entidade.CambagemCáster))
                    .ForMember(dto => dto.CambagemCasterExecutado, dto => dto.MapFrom(entidade => entidade.CambagemCásterExecutado))
                    .ForMember(dto => dto.HigienizacaoAr, dto => dto.MapFrom(entidade => entidade.HigienizacaoDeAr))
                    .ForMember(dto => dto.HigienizacaoArExecutado, dto => dto.MapFrom(entidade => entidade.HigienizacaoDeArExecutado))
                    .ForMember(dto => dto.FiltroAr, dto => dto.MapFrom(entidade => entidade.FiltroDeAr))
                    .ForMember(dto => dto.FiltroArExecutado, dto => dto.MapFrom(entidade => entidade.FiltroDeArExecutado))
                    .ForMember(dto => dto.DataTrocaOleo, dto => dto.MapFrom(entidade => entidade.OleoMotor))
                    .ForMember(dto => dto.DataTrocaFiltroOleo, dto => dto.MapFrom(entidade => entidade.FiltroOleo));

                cfg.CreateMap<CheckupDto, Checkup>()
                    .ForMember(dto => dto.Vendedor, opt => opt.MapFrom(model => model))
                    .ForMember(dto => dto.TecnicoResponsavel, opt => opt.MapFrom(model => model))
                    .ForMember(dto => dto.Orcamento, opt => opt.MapFrom(model => model));
                cfg.CreateMap<Checkup, CheckupDto>()
                    .ForMember(dto => dto.CheckupId, dto => dto.MapFrom(entidade => entidade.Id))
                    .ForMember(dto => dto.OrcamentoId, dto => dto.MapFrom(entidade => entidade.Orcamento.Id))
                    .ForMember(dto => dto.Vendedor, dto => dto.MapFrom(entidade => (entidade.Vendedor == null ? string.Empty : entidade.Vendedor.Id.ToString())))
                    .ForMember(dto => dto.Tecnico, dto => dto.MapFrom(entidade => (entidade.TecnicoResponsavel == null ? string.Empty : entidade.TecnicoResponsavel.Id.ToString())))
                    .ForMember(dto => dto.VendedorNome, dto => dto.MapFrom(entidade => entidade.Vendedor.Nome))
                    .ForMember(dto => dto.TecnicoNome, dto => dto.MapFrom(entidade => entidade.TecnicoResponsavel.Nome))
                    .ForMember(dto => dto.Cliente, dto => dto.MapFrom(entidade => entidade.Orcamento.Cliente.Nome))
                    .ForMember(dto => dto.CPFCNPJ, dto => dto.MapFrom(entidade => entidade.Orcamento.Cliente.CNPJCPF))
                    .ForMember(dto => dto.Phone, dto => dto.MapFrom(entidade => entidade.Orcamento.Cliente.Telefone))
                    .ForMember(dto => dto.License, dto => dto.MapFrom(entidade => entidade.Orcamento.Placa))
                    //.ForMember(dto => dto.Marca, dto => dto.MapFrom(entidade => entidade.Orcamento.MarcaModeloVersao.MarcaModelo.Marca.Descricao))
                    //.ForMember(dto => dto.Car, dto => dto.MapFrom(entidade => entidade.Orcamento.MarcaModeloVersao.MarcaModelo.Descricao))
                    //.ForMember(dto => dto.Versao, dto => dto.MapFrom(entidade => entidade.Orcamento.MarcaModeloVersao.Descricao))
                    .ForMember(dto => dto.Year, dto => dto.MapFrom(entidade => entidade.Orcamento.Ano))
                    .ForMember(dto => dto.KM, dto => dto.MapFrom(entidade => (long)entidade.Orcamento.KM))
                    .ForMember(dto => dto.ProdutoServico, dto => dto.MapFrom(entidade => entidade.Orcamento.InformacoesCliente))
                    .ForMember(dto => dto.EmailCliente, dto => dto.MapFrom(entidade => entidade.Orcamento.Cliente.Email));

                cfg.CreateMap<Checkup, CheckupCarDto>()
                    .ForMember(dto => dto.Vendedor, dto => dto.MapFrom(entidade => (entidade.Vendedor == null ? string.Empty : entidade.Vendedor.Id.ToString())))
                    .ForMember(dto => dto.Tecnico, dto => dto.MapFrom(entidade => (entidade.TecnicoResponsavel == null ? string.Empty : entidade.TecnicoResponsavel.Id.ToString())))
                    .ForMember(dto => dto.OrcamentoId, dto => dto.MapFrom(entidade => entidade.Orcamento.Id))
                    .ForMember(dto => dto.CheckupId, dto => dto.MapFrom(entidade => entidade.Id))
                    .ForMember(dto => dto.EmailCliente, dto => dto.MapFrom(entidade => entidade.Orcamento.Cliente.Email));


                cfg.CreateMap<CheckupCarDto, Vendedor>()
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.Vendedor))
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.Tecnico))
                    .ForMember(dto => dto.Nome, opt => opt.MapFrom(entidade => entidade.VendedorNome))
                    .ForMember(dto => dto.Nome, opt => opt.MapFrom(entidade => entidade.TecnicoNome));

                cfg.CreateMap<CheckupDto, Vendedor>()
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.Vendedor))
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.Tecnico))
                    .ForMember(dto => dto.Nome, opt => opt.MapFrom(entidade => entidade.VendedorNome))
                    .ForMember(dto => dto.Nome, opt => opt.MapFrom(entidade => entidade.TecnicoNome));

                cfg.CreateMap<CheckupTruck, CheckupTruckDto>()
                    .ForMember(dto => dto.Vendedor, dto => dto.MapFrom(entidade => (entidade.Checkup.Vendedor == null ? string.Empty : entidade.Checkup.Vendedor.Id.ToString())))
                    .ForMember(dto => dto.Tecnico, dto => dto.MapFrom(entidade => (entidade.Checkup.TecnicoResponsavel == null ? string.Empty : entidade.Checkup.TecnicoResponsavel.Id.ToString())))
                    .ForMember(dto => dto.VendedorNome, dto => dto.MapFrom(entidade => entidade.Checkup.Vendedor.Nome))
                    .ForMember(dto => dto.TecnicoNome, dto => dto.MapFrom(entidade => entidade.Checkup.TecnicoResponsavel.Nome))
                    .ForMember(dto => dto.OrcamentoId, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.Id))
                    .ForMember(dto => dto.EmailCliente, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.Cliente.Email))
                    .ForMember(dto => dto.Cliente, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.Cliente.Nome))
                    //.ForMember(dto => dto.Car, dto => dto.MapFrom(entidade => entidade.Checkup.Orcamento.MarcaModeloVersao.MarcaModelo.Descricao))
                    .ForMember(dto => dto.CheckupId, dto => dto.MapFrom(entidade => entidade.Checkup.Id))
                    .ForMember(dto => dto.BarraCurtaDireção, dto => dto.MapFrom(entidade => entidade.BarraCurtaDirecao))
                    .ForMember(dto => dto.BarraCurtaDireçãoExecutado, dto => dto.MapFrom(entidade => entidade.BarraCurtaDirecaoExecutado))
                    .ForMember(dto => dto.BarraEstabilozadora, dto => dto.MapFrom(entidade => entidade.BarraEstabilizadora))
                    .ForMember(dto => dto.BarraEstabilozadoraExecutado, dto => dto.MapFrom(entidade => entidade.BarraEstabilizadoraExecutado))
                    .ForMember(dto => dto.TamborDiscoFreio, dto => dto.MapFrom(entidade => entidade.TamborDiscoFreioDianteiro))
                    .ForMember(dto => dto.TamborDiscoFreioExecutado, dto => dto.MapFrom(entidade => entidade.TamborDiscoFreioDianteiroExecutado))
                    .ForMember(dto => dto.TamborDiscoFreioTraseiroTruckExecutado, dto => dto.MapFrom(entidade => entidade.TamborDiscoFreioTraseiroExecutado));

                cfg.CreateMap<CheckupTruckDto, CheckupTruck>()
                    .ForMember(x => x.Checkup, opt => opt.ResolveUsing(model => new Checkup()
                    {
                        OrcamentoId = Int64.Parse(model.OrcamentoId),
                        VendedorId = Int64.TryParse(model.Vendedor, out tempVal) ? Int64.Parse(model.Vendedor) : (long?)null,
                        TecnicoResponsavelId = Int64.TryParse(model.Tecnico, out tempVal) ? Int64.Parse(model.Tecnico) : (long?)null
                    }))
                    .ForMember(dto => dto.Checkup, opt => opt.MapFrom(model => model))
                    .ForMember(dto => dto.BarraCurtaDirecao, dto => dto.MapFrom(entidade => entidade.BarraCurtaDireção))
                    .ForMember(dto => dto.BarraCurtaDirecaoExecutado, dto => dto.MapFrom(entidade => entidade.BarraCurtaDireçãoExecutado))
                    .ForMember(dto => dto.BarraEstabilizadora, dto => dto.MapFrom(entidade => entidade.BarraEstabilozadora))
                    .ForMember(dto => dto.BarraEstabilizadoraExecutado, dto => dto.MapFrom(entidade => entidade.BarraEstabilozadoraExecutado))
                    .ForMember(dto => dto.TamborDiscoFreioDianteiro, dto => dto.MapFrom(entidade => entidade.TamborDiscoFreio))
                    .ForMember(dto => dto.TamborDiscoFreioDianteiroExecutado, dto => dto.MapFrom(entidade => entidade.TamborDiscoFreioExecutado))
                    .ForMember(dto => dto.TamborDiscoFreioTraseiroExecutado, dto => dto.MapFrom(entidade => entidade.TamborDiscoFreioTraseiroTruckExecutado));

                cfg.CreateMap<Checkup, CheckupTruckDto>()
                    .ForMember(dto => dto.Vendedor, dto => dto.MapFrom(entidade => (entidade.Vendedor == null ? string.Empty : entidade.Vendedor.Id.ToString())))
                    .ForMember(dto => dto.Tecnico, dto => dto.MapFrom(entidade => (entidade.TecnicoResponsavel == null ? string.Empty : entidade.TecnicoResponsavel.Id.ToString())))
                    .ForMember(dto => dto.OrcamentoId, dto => dto.MapFrom(entidade => entidade.Orcamento.Id))
                    .ForMember(dto => dto.CheckupId, dto => dto.MapFrom(entidade => entidade.Id))
                    .ForMember(dto => dto.EmailCliente, dto => dto.MapFrom(entidade => entidade.Orcamento.Cliente.Email));
                cfg.CreateMap<CheckupTruckDto, Checkup>()
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.CheckupId));

                cfg.CreateMap<CheckupTruckDto, Vendedor>()
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.Vendedor))
                    .ForMember(dto => dto.Id, opt => opt.MapFrom(entidade => entidade.Tecnico))
                    .ForMember(dto => dto.Nome, opt => opt.MapFrom(entidade => entidade.VendedorNome))
                    .ForMember(dto => dto.Nome, opt => opt.MapFrom(entidade => entidade.TecnicoNome));
                #endregion

                cfg.CreateMap<SolicitacaoDescontoVendaAlcada, SolicitacaoDescontoVendaAlcadaDto>();
                cfg.CreateMap<SolicitacaoDescontoVendaAlcadaDto, SolicitacaoDescontoVendaAlcada>();
                cfg.CreateMap<SolicitacaoAnaliseCredito, SolicitacaoAnaliseCreditoDto>();
                cfg.CreateMap<SolicitacaoAnaliseCreditoDto, SolicitacaoAnaliseCredito>();

                cfg.CreateMap<Orcamento, OrcamentoConsultaDto>()
                    .ForMember(dto => dto.Id, dto => dto.MapFrom(entidade => entidade.Id))
                    .ForMember(dto => dto.CampoCodigo, dto => dto.MapFrom(entidade => entidade.CampoCodigo))
                    .ForMember(dto => dto.NomeCliente, dto => dto.MapFrom(entidade => entidade.Cliente.Nome))
                    .ForMember(dto => dto.LojaDestino, dto => dto.MapFrom(entidade => entidade.LojaDellaVia.Descricao.Substring(0, 2).Equals(entidade.LojaDellaVia.CampoCodigo) ? entidade.LojaDellaVia.Descricao : entidade.LojaDellaVia.CampoCodigo + " - " + entidade.LojaDellaVia.Descricao))
                    .ForMember(dto => dto.DataCriacao, dto => dto.MapFrom(entidade => entidade.DataCriacao.ToString("dd'/'MM'/'yyyy HH:mm")))
                    .ForMember(dto => dto.DataValidade, dto => dto.MapFrom(entidade => entidade.DataValidade.ToString("dd'/'MM'/'yyyy")))
                    .ForMember(dto => dto.Status, dto => dto.MapFrom(entidade => (entidade.StatusOrcamento == StatusOrcamento.EmAberto &&
                                   entidade.DataValidade.Date < DateTime.Now.Date) ? StatusOrcamento.EmAbertoVencido.GetDescription() : entidade.StatusOrcamento.GetDescription()));
            });
        }
    }
}
