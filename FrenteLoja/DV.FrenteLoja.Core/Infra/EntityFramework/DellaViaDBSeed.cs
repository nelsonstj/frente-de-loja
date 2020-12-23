using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace DV.FrenteLoja.Core.Infra.EntityFramework
{
    public class DellaViaDBSeed
    {
        public static void ExecutarSeed(DellaviaContexto context)
        {
            IRepositorio<Marca> marcaRepositorio = new Repositorio<Marca>(context);
            IRepositorio<MarcaModelo> marcaModeloRepositorio = new Repositorio<MarcaModelo>(context);
            IRepositorio<MarcaModeloVersao> marcaModeloVersaoRepositorio = new Repositorio<MarcaModeloVersao>(context);
            IRepositorio<Banco> bancoRepositorio = new Repositorio<Banco>(context);
            IRepositorio<TipoVenda> tipoVendaRepositorio = new Repositorio<TipoVenda>(context);
            IRepositorio<Vendedor> vendedorRepositorio = new Repositorio<Vendedor>(context);
            IRepositorio<Operador> operadorRepositorio = new Repositorio<Operador>(context);
            IRepositorio<Transportadora> transportadoraRepositorio = new Repositorio<Transportadora>(context);
            IRepositorio<LojaDellaVia> lojaDellaViaRepositorio = new Repositorio<LojaDellaVia>(context);
            IRepositorio<AdministradoraFinanceira> administradoraFinanceiraRepositorio = new Repositorio<AdministradoraFinanceira>(context);
            IRepositorio<CondicaoPagamento> condicaoPagamentoRepositorio = new Repositorio<CondicaoPagamento>(context);
            IRepositorio<GrupoProduto> grupoProdutoRepositorio = new Repositorio<GrupoProduto>(context);
            IRepositorio<Cliente> clienteRepositorio = new Repositorio<Cliente>(context);
            IRepositorio<ClienteVeiculo> clienteVeiculoRepositorio = new Repositorio<ClienteVeiculo>(context);
            IRepositorio<TabelaPreco> tabelaPrecoRepositorio = new Repositorio<TabelaPreco>(context);
            IRepositorio<TabelaPrecoItem> tabelaPrecoItemRepositorio = new Repositorio<TabelaPrecoItem>(context);
            IRepositorio<Produto> produtoRepositorio = new Repositorio<Produto>(context);
            IRepositorio<ProdutoComplemento> produtoComplementoRepositorio = new Repositorio<ProdutoComplemento>(context);
            IRepositorio<GrupoServicoAgregadoProduto> grupoServicoAgregadoProdutoRepositorio = new Repositorio<GrupoServicoAgregadoProduto>(context);
            //IRepositorio<Convenio> convenioRepositorio = new Repositorio<Convenio>(context);
            IRepositorio<CheckupCar> checkupCarRepositorio = new Repositorio<CheckupCar>(context);
            IRepositorio<CheckupTruck> checkupTruckRepositorio = new Repositorio<CheckupTruck>(context);

            #region Marca ALFA ROMEO

            var marca = new Marca { CampoCodigo = "001", Descricao = "ALFA ROMEO" };
            marca.Id = marcaRepositorio.Add(marca).Id;

            var marcaModelo = new MarcaModelo() { Descricao = "155 IMP", IdMarca = marca.Id,};
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            var marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);


            marcaModelo = new MarcaModelo() { Descricao = "156 IMP", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);

            marcaModelo = new MarcaModelo() { Descricao = "164 IMP", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);

            #endregion

            #region Marca VOLKSWAGEN
            marca = new Marca() { CampoCodigo = "002", Descricao = "VOLKSWAGEN" };
            marca.Id = marcaRepositorio.Add(marca).Id;

            marcaModelo = new MarcaModelo() { Descricao = "VOYAGE", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.6 8V TOTALFLEX", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.8 8V CL", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V TOTALFLEX", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);

            marcaModelo = new MarcaModelo() { Descricao = "GOL", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V POPULAR", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V SE", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V SPECIAL", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            #endregion

            #region Marca KIA
            marca = new Marca() { Descricao = "KIA" };
            marca.Id = marcaRepositorio.Add(marca).Id;

            marcaModelo = new MarcaModelo() { Descricao = "BONGO", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "2.7D K2700 CABINE DUPLA RS", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "3.6D K3600", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "2.5 TDCI K2500 RD", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);

            marcaModelo = new MarcaModelo() { Descricao = "PICANTO", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 EX", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 EX II", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V EX FLEX AUTO II", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            #endregion

            #region Marca CHEVROLET
            marca = new Marca() { Descricao = "CHEVROLET" };
            marca.Id = marcaRepositorio.Add(marca).Id;

            marcaModelo = new MarcaModelo() { Descricao = "CELTA", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V LIFE", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.4 8V SUPER", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);

            marcaModelo = new MarcaModelo() { Descricao = "CORSA", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V SEDAN CLASSIC SPIRIT", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V SEDAN", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V SEDAN JOY FLEXPOWER", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            #endregion

            #region Marca FIAT
            marca = new Marca() { Descricao = "FIAT" };
            marca.Id = marcaRepositorio.Add(marca).Id;

            marcaModelo = new MarcaModelo() { Descricao = "PALIO", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V ED", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V FIRE", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.4 8V YOUNG", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);

            marcaModelo = new MarcaModelo() { Descricao = "UNO", IdMarca = marca.Id,  };
            marcaModelo.Id = marcaModeloRepositorio.Add(marcaModelo).Id;
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V ELETRONIC", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0 8V MILLE FIRE", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "1.0A 8V EX", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            marcaModeloVersao = new MarcaModeloVersao { Descricao = "Todas as Versões", IdMarcaModelo = marcaModelo.Id };
            marcaModeloVersaoRepositorio.Add(marcaModeloVersao);
            #endregion

            #region Banco
            var bancos = new List<Banco>();
            bancos.Add(new Banco { Descricao = "BANCO ITAU S.A", CampoCodigo = "341" });
            bancos.Add(new Banco { Descricao = "BANCO SAFRA S.A", CampoCodigo = "422" });
            bancos.Add(new Banco { Descricao = "BRADESCO", CampoCodigo = "237" });
            bancos.Add(new Banco { Descricao = "BANCO BRASIL S.A", CampoCodigo = "001" });
            bancoRepositorio.AddRange(bancos);
            #endregion

            #region Tipo Venda
            var tiposVenda = new List<TipoVenda>();
            tiposVenda.Add(new TipoVenda { Descricao = "ATACADO", CampoCodigo = "A" });
            tiposVenda.Add(new TipoVenda { Descricao = "TRUCK", CampoCodigo = "T" });
            tiposVenda.Add(new TipoVenda { Descricao = "VAREJO", CampoCodigo = "V" });
            tiposVenda.Add(new TipoVenda { Descricao = "B2B", CampoCodigo = "B" });
            tiposVenda.Add(new TipoVenda { Descricao = "DIVERSAS", CampoCodigo = "Z" });
            tipoVendaRepositorio.AddRange(tiposVenda);
            #endregion

            #region Vendedor
            var vendedores = new List<Vendedor>();
            vendedores.Add(new Vendedor { Nome = "RENATA REGINA MARTINS DIAS", CampoCodigo = "TMK020", FilialOrigem = "01" });
            vendedores.Add(new Vendedor { Nome = "VENDEDOR PADRAO", CampoCodigo = "000001", FilialOrigem = "01" });
            vendedores.Add(new Vendedor { Nome = "JOAO APARECIDO GONZALEZ ROMERO", CampoCodigo = "002174", FilialOrigem = "02" });
            vendedores.Add(new Vendedor { Nome = "AIRTON CAGNIN", CampoCodigo = "021001", FilialOrigem = "21" });
            vendedores.Add(new Vendedor { Nome = "DANIEL CUNHA DE SOUZA", CampoCodigo = "021336", FilialOrigem = "21" });
            vendedores.Add(new Vendedor { Nome = "LOJA 82", CampoCodigo = "973224", FilialOrigem = "01" });
            vendedorRepositorio.AddRange(vendedores);
            #endregion

            #region Operador
            var operador = new Operador();
            var vendedor = vendedorRepositorio.GetSingle(x => x.CampoCodigo == "TMK020");
            if (vendedor != null)
            {
                operador.IdConsultor = vendedor.Id.ToString();
                operador.Descricao = "RENATA REGINA MARTINS DIAS";
                operador.PercLimiteDesconto = 99.99M;
                operador.CampoCodigo = "000598";
                operadorRepositorio.Add(operador);
            }

            vendedor = vendedorRepositorio.GetSingle(x => x.CampoCodigo == "973224");
            if (vendedor != null)
            {
                operador = new Operador();
                operador.IdConsultor = vendedor.Id.ToString();
                operador.Descricao = "ADMINISTRATIVO LOJA 09";
                operador.PercLimiteDesconto = 99.99M;
                operador.CampoCodigo = "000051";
                operadorRepositorio.Add(operador);
            }

            #endregion

            #region Transportadora
            var transportadoras = new List<Transportadora>();
            transportadoras.Add(new Transportadora { CampoCodigo = "000001", Descricao = "GP TRANSPORTES E SERVICOS LTDA RJ" });
            transportadoras.Add(new Transportadora { CampoCodigo = "000002", Descricao = "PROPRIO CLIENTE" });
            transportadoras.Add(new Transportadora { CampoCodigo = "000003", Descricao = "NOSSO CARRO" });
            transportadoras.Add(new Transportadora { CampoCodigo = "000004", Descricao = "TRANS TAVARES TRANSPORTES DE CARGAS LTDA" });
            transportadoraRepositorio.AddRange(transportadoras);
            #endregion

            #region Lojas Della Via (filial)
            var loja = new LojaDellaVia();
            var banco = bancoRepositorio.GetSingle(x => x.CampoCodigo == "341");
            if (vendedor != null)
            {
                loja.BancoId = banco.Id;
                loja.Descricao = "DELLA VIA IPIRANGA";
                loja.CampoCodigo = "000003";
                lojaDellaViaRepositorio.Add(loja);

                loja = new LojaDellaVia();
                loja.BancoId = banco.Id;
                loja.Descricao = "DELLA VIA SBC";
                loja.CampoCodigo = "000002";
                lojaDellaViaRepositorio.Add(loja);

                loja = new LojaDellaVia();
                loja.BancoId = banco.Id;
                loja.Descricao = "DELLA VIA MOGI";
                loja.CampoCodigo = "000006";
                lojaDellaViaRepositorio.Add(loja);
            }

            banco = bancoRepositorio.GetSingle(x => x.CampoCodigo == "001");
            if (vendedor != null)
            {
                loja = new LojaDellaVia();
                loja.BancoId = banco.Id;
                loja.Descricao = "DELLAVIA TATUI";
                loja.CampoCodigo = "000019";
                lojaDellaViaRepositorio.Add(loja);

                loja = new LojaDellaVia();
                loja.BancoId = banco.Id;
                loja.Descricao = "DELLAVIA ALPHAVILLE";
                loja.CampoCodigo = "000037";
                lojaDellaViaRepositorio.Add(loja);

                loja = new LojaDellaVia();
                loja.BancoId = banco.Id;
                loja.Descricao = "DELLAVIA SANTANA";
                loja.CampoCodigo = "0000A5";
                lojaDellaViaRepositorio.Add(loja);
            }
            #endregion

            #region Parametros Gerais
            //Ver com o mailson os valores
            #endregion

            #region Administradora Financeira
            var administradorasFinanceira = new List<AdministradoraFinanceira>();
            administradorasFinanceira.Add(new AdministradoraFinanceira { CampoCodigo = "001", Descricao = "MASTERCARD (CREDITO ATE 6X)", FormaPagamento = "CC" });
            administradorasFinanceira.Add(new AdministradoraFinanceira { CampoCodigo = "003", Descricao = "VISA (CREDITO ATE 6X)", FormaPagamento = "CC" });
            administradorasFinanceira.Add(new AdministradoraFinanceira { CampoCodigo = "004", Descricao = "MAESTRO DEBITO", FormaPagamento = "CD" });
            administradorasFinanceira.Add(new AdministradoraFinanceira { CampoCodigo = "006", Descricao = "ELECTRON DEBITO", FormaPagamento = "CD" });
            administradorasFinanceira.Add(new AdministradoraFinanceira { CampoCodigo = "011", Descricao = "FINASA CH ACIMA DE 3X", FormaPagamento = "FI" });
            administradoraFinanceiraRepositorio.AddRange(administradorasFinanceira);
            #endregion

            #region Condição pagamento

            var condicaoPagamento = new CondicaoPagamento();
            condicaoPagamento.Descricao = "30 DDL";
            condicaoPagamento.FormaCondicaoPagamento = "30";
            condicaoPagamento.FormaPagamento = "DP";
            condicaoPagamento.CampoCodigo = "001";
            condicaoPagamento.QtdParcelas = condicaoPagamento.FormaCondicaoPagamento.Split(',').Count() + 1;
            condicaoPagamento.ValorAcrescimo = 0;
            condicaoPagamentoRepositorio.Add(condicaoPagamento);

            condicaoPagamento = new CondicaoPagamento();
            condicaoPagamento.Descricao = "05,30,60,90 DD";
            condicaoPagamento.FormaCondicaoPagamento = "05,30,60,90";
            condicaoPagamento.FormaPagamento = "";
            condicaoPagamento.CampoCodigo = "116";
            condicaoPagamento.QtdParcelas = condicaoPagamento.FormaCondicaoPagamento.Split(',').Count() + 1;
            condicaoPagamento.ValorAcrescimo = 0;
            condicaoPagamentoRepositorio.Add(condicaoPagamento);

            condicaoPagamento = new CondicaoPagamento();
            condicaoPagamento.Descricao = "15,30,60..120";
            condicaoPagamento.FormaCondicaoPagamento = "15,30,60,90,120";
            condicaoPagamento.FormaPagamento = "";
            condicaoPagamento.CampoCodigo = "147";
            condicaoPagamento.QtdParcelas = condicaoPagamento.FormaCondicaoPagamento.Split(',').Count() + 1;
            condicaoPagamento.ValorAcrescimo = 0;
            condicaoPagamentoRepositorio.Add(condicaoPagamento);

            condicaoPagamento = new CondicaoPagamento();
            condicaoPagamento.Descricao = "30,60,90,120,150,180,210,240,270,300";
            condicaoPagamento.FormaCondicaoPagamento = "30,60...300 DD";
            condicaoPagamento.FormaPagamento = "";
            condicaoPagamento.CampoCodigo = "146";
            condicaoPagamento.QtdParcelas = condicaoPagamento.FormaCondicaoPagamento.Split(',').Count() + 1;
            condicaoPagamento.ValorAcrescimo = 0;
            condicaoPagamentoRepositorio.Add(condicaoPagamento);

            #endregion

            #region Grupo de Produto
            var gruposProduto = new List<GrupoProduto>();
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0001", Descricao = "PNEUS PIRELLI" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0004", Descricao = "PNEUS TOYO" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0395", Descricao = "AMORTECEDOR NAKATA" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0501", Descricao = "PASTILHA FREIO PASSEIO" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0003", Descricao = "PROTETORES PIRELLI" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0014", Descricao = "CAMARAS FIRESTONE" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0071", Descricao = "SERVICOS" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0078", Descricao = "SERVICOS REL.PNEU" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0650", Descricao = "VALVULAS E BICOS P/ PNEUS" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0082", Descricao = "PRODUTOS DE SEGURO" });
            gruposProduto.Add(new GrupoProduto { CampoCodigo = "0617", Descricao = "LUBRIFICANTES MOBIL" });
            grupoProdutoRepositorio.AddRange(gruposProduto);
            #endregion

            #region Cliente

            banco = bancoRepositorio.GetSingle(x => x.CampoCodigo == "341");

            var cliente = new Cliente();
            cliente.CampoCodigo = "03TGTL";
            cliente.Nome = "TOTAL PERFORMANCE IND COSM EIRELI ME";
            cliente.CNPJCPF = "05765886000188";
            cliente.Score = "72";
            cliente.BancoId = banco?.Id;
            cliente.StatusCliente = StatusCliente.Liberado;
            cliente.TipoCliente = "";
            cliente.Loja = "01";
            cliente.Telefone = "33418833";
            cliente.TelefoneCelular = "999552233";
            cliente.TelefoneComercial = "33004477";
            cliente.Email = "comercial@totalperformance.com.br";
            cliente.ClassificacaoCliente = "Gold";
            clienteRepositorio.Add(cliente);

            cliente = new Cliente();
            cliente.CampoCodigo = "0U1EK3";
            cliente.Nome = "JOSE RONALDO DE LACERDA";
            cliente.CNPJCPF = "04507947847";
            cliente.Score = "180";
            cliente.BancoId = banco?.Id;
            cliente.StatusCliente = StatusCliente.Liberado;
            cliente.TipoCliente = "";
            cliente.Loja = "99";
            cliente.Telefone = "36612233";
            cliente.TelefoneCelular = "87552233";
            cliente.TelefoneComercial = "23004477";
            cliente.Email = "jose.ronaldo@terra.com.br";
            cliente.ClassificacaoCliente = "Silver";
            clienteRepositorio.Add(cliente);

            cliente = new Cliente();
            cliente.CampoCodigo = "FV3X8I";
            cliente.Nome = "JOAQUIM";
            cliente.CNPJCPF = "83451684853";
            cliente.Score = "10";
            cliente.BancoId = banco?.Id;
            cliente.TipoCliente = "";
            cliente.Loja = "99";
            cliente.Telefone = "36712233";
            cliente.TelefoneCelular = "988952233";
            cliente.TelefoneComercial = "23004477";
            cliente.Email = "joaquim@terra.com.br";
            clienteRepositorio.Add(cliente);

            cliente = new Cliente();
            cliente.CampoCodigo = "000001";
            cliente.Nome = "CLIENTE PADRAO";
            cliente.CNPJCPF = "00000000000";
            cliente.Score = "0";
            cliente.StatusCliente = StatusCliente.Liberado;
            cliente.TipoCliente = "";
            cliente.Loja = "01";
            clienteRepositorio.Add(cliente);

            #endregion

            #region Cliente Veiculo

            var clienteVeiculo = new ClienteVeiculo();
            clienteVeiculo.ClienteId = "1";
            //clienteVeiculo.MarcaModeloVersaoId = 13; 
            clienteVeiculo.Placa = "DDA8212";
            clienteVeiculo.Ano = 2001;
            clienteVeiculo.Observacoes = "AC DH VE TE";
            clienteVeiculoRepositorio.Add(clienteVeiculo);

            clienteVeiculo = new ClienteVeiculo();
            clienteVeiculo.ClienteId = "1";
            //clienteVeiculo.MarcaModeloVersaoId = 10;
            clienteVeiculo.Placa = "MSK4201";
            clienteVeiculo.Ano = 2008;
            clienteVeiculoRepositorio.Add(clienteVeiculo);

            clienteVeiculo = new ClienteVeiculo();
            clienteVeiculo.ClienteId = "3";
            // clienteVeiculo.MarcaModeloVersaoId = 7; 
            clienteVeiculo.Placa = "YUA0140";
            clienteVeiculo.Ano = 2014;
            clienteVeiculoRepositorio.Add(clienteVeiculo);

            #endregion

            //#region Produto
            //var produto = new Produto();

            //var grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0001");
            //var agregadoProduto = grupoServicoAgregadoProdutoRepositorio.GetSingle(s => s.IdProduto == "100300100");
			

            //produto.CampoCodigo = "002963";
            //produto.Descricao = "PN 315/80R22.5 156/150 FR01-PI";
            //produto.CodigoFabricante = "2454900";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;


            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0501");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "110800400");

            //produto.CampoCodigo = "500001";
            //produto.Descricao = "PASTILHA FR 1001 SYL TRAS";
            //produto.CodigoFabricante = "1001SYL";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0078");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "901013";
            //produto.Descricao = "SERV SUBST PNEU";
            //produto.CodigoFabricante = "SERVICO PN";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0001");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "001556";
            //produto.Descricao = "PN 175/65R14 82T F.ENGY - PI";
            //produto.CodigoFabricante = "2695800";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0001");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "002990";
            //produto.Descricao = "PN 195/60R15 88H P 7 - PI";
            //produto.CodigoFabricante = "1477900";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0001");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "119118";
            //produto.Descricao = "PN 185/60R15 88H XL P1CINT -PI";
            //produto.CodigoFabricante = "2531700";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0078");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "901013";
            //produto.Descricao = "SERV SUBST PNEU A-13,14,15,16";
            //produto.CodigoFabricante = "Servico PN";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0078");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "903013";
            //produto.Descricao = "BALANC RODA A-13,14,15,16";
            //produto.CodigoFabricante = "BAL RODA A-13 A16";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0078");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "904001";
            //produto.Descricao = "ALINH DIR VEIC PASSEIO";
            //produto.CodigoFabricante = "ALINH PASSEIO";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0078");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "904038";
            //produto.Descricao = "SERV CASTER  CADA LADO";
            //produto.CodigoFabricante = "SERV CASTER";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;


            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0650");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "000901";
            //produto.Descricao = "VALVULA TR 414 SEGURANCA";
            //produto.CodigoFabricante = "";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0082");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "097003";
            //produto.Descricao = "GARANTIA ESTENDIDA PN A14 RD";
            //produto.CodigoFabricante = "";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0078");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "904026";
            //produto.Descricao = "SERV CAMBAGEM DIANT/TRAS LADO";
            //produto.CodigoFabricante = "SERV CAMBAGEM V";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0617");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "660060";
            //produto.Descricao = "M SUPER 2000X3 10W40 GRANEL";
            //produto.CodigoFabricante = "MOBIL 122628";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;

            //produto = new Produto();

            //grupo = grupoProdutoRepositorio.GetSingle(s => s.CampoCodigo == "0617");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(s => s.CampoCodigo == "100300100");

            //produto.CampoCodigo = "660063";
            //produto.Descricao = "M SUPER ECOPWER 5W30 GRANEL USAR COD 660069";
            //produto.CodigoFabricante = "MOBIL 121337";
            //produto.IdGrupoProduto = grupo.Id;
            //produto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //produto.Id = produtoRepositorio.Add(produto).Id;
            //#endregion

            //#region Produto Complemento

            //var produtoComplemento = new ProdutoComplemento();
            //produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "002963");
            //if (produto != null)
            //{
            //    produtoComplemento.IdProduto = produto.Id;

            //    produtoComplemento.Comprimento = 0;
            //    produtoComplemento.Espessura = 0;
            //    produtoComplemento.Largura = 0;
            //    produtoComplemento.VolumeM3 = 0;
            //    produtoComplemento.CampoHTML = "";
            //    produtoComplementoRepositorio.Add(produtoComplemento);
            //}

            //produtoComplemento = new ProdutoComplemento();
            //produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "500001");
            //if (produto != null)
            //{
            //    produtoComplemento.IdProduto = produto.Id;

            //    produtoComplemento.Comprimento = 0;
            //    produtoComplemento.Espessura = 0;
            //    produtoComplemento.Largura = 0;
            //    produtoComplemento.VolumeM3 = 0;
            //    produtoComplemento.CampoHTML = "";
            //    produtoComplementoRepositorio.Add(produtoComplemento);
            //}
            //#endregion

            //#region Grupo serviço agregado produtos

            //var grupoagregadoProduto = new GrupoServicoAgregadoProduto();
            //produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "901013");
            //agregadoProduto = grupoServicoAgregadoRepositorio.GetSingle(x => x.CampoCodigo == "100300100");

            //if (produto != null && agregadoProduto != null)
            //{
            //    grupoagregadoProduto.IdGrupoServicoAgregado = agregadoProduto.Id;
            //    grupoagregadoProduto.Item = "0001";
            //    grupoagregadoProduto.PermiteAlterarQuantidade = true;
            //    grupoagregadoProduto.Quantidade = 0;
            //    grupoagregadoProduto.IdProduto = produto.Id;
            //    grupoServicoAgregadoProdutoRepositorio.Add(grupoagregadoProduto);
            //}

            //#endregion

            //#region Tabela de Preço

            //var tabelaPreco = new TabelaPreco();

            //tabelaPreco.CampoCodigo = "CAV";
            //tabelaPreco.DataDe = new DateTime(2015, 04, 10);
            //tabelaPreco.DataAte = new DateTime(2047, 12, 31);
            //tabelaPreco.CodCondicaoPagamento = "";
            //tabelaPreco.Descricao = "CAPITAL E GRANDE SP VAREJO";
            //tabelaPreco.Id = tabelaPrecoRepositorio.Add(tabelaPreco).Id;

            //#endregion

   //         #region Tabela de Preço Item

   //         var tabelaPrecoItem = new TabelaPrecoItem();

   //         tabelaPreco = tabelaPrecoRepositorio.GetSingle(x => x.CampoCodigo == "CAV");
   //         produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "002963");
   //         if (!(produto is null) && !(tabelaPreco is null))
   //         {
   //             tabelaPrecoItem.CampoCodigo = "1372845";
   //             tabelaPrecoItem.PrecoVenda = 2726.39M;
   //             tabelaPrecoItem.ProdutoId = produto.Id;
   //             tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
   //             tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
   //         }

   //         produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "500001");
   //         if (!(produto is null) && !(tabelaPreco is null))
   //         {
   //             tabelaPrecoItem.CampoCodigo = "757346";
   //             tabelaPrecoItem.PrecoVenda = 101.91M;
   //             tabelaPrecoItem.ProdutoId = produto.Id;
   //             tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
   //             tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
   //         }

   //         produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "901013");
   //         if (!(produto is null) && !(tabelaPreco is null))
   //         {
   //             tabelaPrecoItem.CampoCodigo = "772956";
   //             tabelaPrecoItem.PrecoVenda = 30.8M;
   //             tabelaPrecoItem.ProdutoId = produto.Id;
   //             tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
   //             tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
   //         }


	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "001556");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "772356";
		 //       tabelaPrecoItem.PrecoVenda = 330.8M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "002990");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "771256";
		 //       tabelaPrecoItem.PrecoVenda = 299.99M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "901013");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "222256";
		 //       tabelaPrecoItem.PrecoVenda = 49.99M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "903013");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "222226";
		 //       tabelaPrecoItem.PrecoVenda = 29.99M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "904001");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "212226";
		 //       tabelaPrecoItem.PrecoVenda = 129.99M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }


	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "904038");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "25526";
		 //       tabelaPrecoItem.PrecoVenda = 79.99M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "000901");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "122385";
		 //       tabelaPrecoItem.PrecoVenda = 7.5M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "097003");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "122344";
		 //       tabelaPrecoItem.PrecoVenda = 40M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }

	  //      produto = produtoRepositorio.GetSingle(x => x.CampoCodigo == "660060");
	  //      if (!(produto is null) && !(tabelaPreco is null))
	  //      {
		 //       tabelaPrecoItem.CampoCodigo = "199344";
		 //       tabelaPrecoItem.PrecoVenda = 35M;
		 //       tabelaPrecoItem.ProdutoId = produto.Id;
		 //       tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id;
		 //       tabelaPrecoItemRepositorio.Add(tabelaPrecoItem);
	  //      }
			//#endregion

			//#region Convênio

			//var convenio = new Convenio();

   //         cliente = clienteRepositorio.GetSingle(x => x.CampoCodigo == "000001" && x.Loja == "01");
   //         tabelaPreco = tabelaPrecoRepositorio.GetSingle(x => x.CampoCodigo == "CAV");

   //         if (cliente != null && tabelaPreco != null)
   //         {
   //             convenio.IdCliente = cliente.Id;
   //             convenio.IdTabelaPreco = tabelaPreco.Id;

   //             convenio.Descricao = "CONVENIO PADRAO";
   //             convenio.Observacoes = @"Este é o convênio padrão, 
   //                                      orientações de uso.";
   //             convenio.DataInicioVigencia = new DateTime(2005, 01, 01);
   //             convenio.DataFimVigencia = new DateTime(2030, 01, 01);
   //             convenio.TrocaCliente = true;
   //             convenio.CampoCodigo = "000001";
   //             convenio.TrocaPreco = TrocaPrecoConvenio.Livre;
   //             convenio.TrocaProduto = true;
   //             convenio.TrocaTabelaPreco = true;
   //             convenioRepositorio.Add(convenio);
   //         }

   //         #endregion

            context.SaveChanges();
            context.Dispose();
        }
    }

}
